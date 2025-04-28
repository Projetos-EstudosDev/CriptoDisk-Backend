using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackCriptoDisk2.Models;
using BackCriptoDisk2.Data;


namespace BackCriptoDisk2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<AuthController>
        [HttpGet("Usuarios")]
        public ActionResult<List<Username>> PegarUsuarios()
        {
            List<Username> usernames = _context.usuarios.Select(u => new Username { username = u.username }).ToList();
            return Ok(usernames);
        }
        
        [HttpGet("username")]
        public ActionResult<List<Username>> BuscarUsername(string username)
        {
    
            var result  =  _context.usuarios.Select(u => new Username { username = u.username }).FirstOrDefault(u => u.username == username);
            if (result == null)
            {
                return NotFound("Usuario nao encontrado");
            }
            return Ok(result);
        }

        // POST api/<AuthController>
        [HttpPost("Cadastro")]
        public async Task<ActionResult<Cadastro>> CriarConta(Cadastro cadastro)
        {
            if (string.IsNullOrWhiteSpace(cadastro.nome) || string.IsNullOrWhiteSpace(cadastro.username) || string.IsNullOrWhiteSpace(cadastro.senha) || string.IsNullOrWhiteSpace(cadastro.email))
            {
                return BadRequest("Dados invalidos");
            } 
            else if (_context.usuarios.Any(u => u.username == cadastro.username))
            {
                return BadRequest("Usuario ja existe");
            }
            _context.usuarios.Add(cadastro);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(BuscarUsername), new { username = cadastro.username }, cadastro);
        }

        [HttpPost("Login")]
        public ActionResult<Cadastro>  logar(Login login)
        {
            var result = _context.usuarios.FirstOrDefault(l => l.username == login.username && l.senha == login.senha);
            if (result == null)
            {
                return BadRequest("Dados invalidos, Usuario ou senha incorretos");
            } 
            return Ok("Seja bem vindo " + result.username);
        }
    }
}
