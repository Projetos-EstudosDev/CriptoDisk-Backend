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
        [HttpGet("GetUsuarios")]
        public ActionResult<List<Usuarios>> GetUsuarios()
        {
            List<Usuarios> usernames = _context.usuarios.Select(g => new Usuarios { Username = g.Username }).ToList();
            return Ok(usernames);
        }
        
        [HttpGet("FindUsername")]
        public ActionResult<List<Usuarios>> FindUsername(string username)
        {
    
            var result  =  _context.usuarios.Select(f => new Usuarios {Username = f.Username}).FirstOrDefault(f => f.Username == username);
            if (result == null)
            {
                return NotFound("Usuario nao encontrado");
            }
            return Ok(result);
        }

        // POST api/<AuthController>
        [HttpPost("CreateAccount")]
        public async Task<ActionResult<Usuarios>> CreateAccount([FromBody] Usuarios usuarios)
        {
            if (string.IsNullOrWhiteSpace(usuarios.Nome) || string.IsNullOrWhiteSpace(usuarios.Username) || string.IsNullOrWhiteSpace(usuarios.Senha) || string.IsNullOrWhiteSpace(usuarios.Email))
            {
                return BadRequest("Dados invalidos");
            } 
            else if (_context.usuarios.Any(u => u.Username == usuarios.Username))
            {
                return BadRequest("Usuario ja existe");
            }
            _context.usuarios.Add(usuarios);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(FindUsername), new { username = usuarios.Id }, usuarios);
        }

        [HttpPost("Login")]
        public ActionResult<Usuarios> Login([FromBody] Usuarios usuarios)
        {
            var result = _context.usuarios.FirstOrDefault(l => l.Username == usuarios.Username && l.Senha == usuarios.Senha);
            if (result == null)
            {
                return Unauthorized("Usuario nao encontrado");
            }
            return Ok("Seja bem vindo(a) " + result.Nome);
        }
        
        [HttpPut("ChangePassword")]
        public ActionResult<Usuarios> ChangePassword(Usuarios usuarios)
        {
            var result = _context.usuarios.FirstOrDefault(l => l.Username == usuarios.Username && l.Email == usuarios.Email);
            if (result == null)
            {
                return Unauthorized("Usuario nao encontrado");
            }
            result.Senha = usuarios.Senha;
            _context.usuarios.Update(result);
            _context.SaveChanges();
            return Ok("Senha alterada com sucesso");
        }
        
        [HttpDelete]
        public ActionResult<Usuarios> Delete(Usuarios usuarios)
        {
            var result = _context.usuarios.FirstOrDefault(l => l.Username == usuarios.Username);
            if (result == null)
            {
                return Unauthorized("Usuario nao encontrado");
            }
            _context.usuarios.Remove(result);
            _context.SaveChanges();
            return Ok("Usuario removido com sucesso");
        }
    }
}
