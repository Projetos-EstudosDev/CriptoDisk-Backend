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
        [HttpGet]
        public ActionResult<List<Cadastro>> PegarCadastros()
        {
            var cadas = _context.usuarios.ToList();
            return Ok(cadas);
        }
        
        [HttpGet("{username}")]
        public async Task<ActionResult<List<Username>>>  BuscarUsername(string username)
        {
    
            var result  =  _context.usuarios.Where(u=>u.username == username);
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
            if (cadastro == null)
            {
                return BadRequest("Dados invalidos");
            }

            _context.usuarios.Add(cadastro);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(BuscarUsername), new { username = cadastro.username }, cadastro);
        }

     

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
