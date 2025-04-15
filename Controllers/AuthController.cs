using BackCriptoDisk2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackCriptoDisk2.Models;

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
        
        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public ActionResult<Cadastro> PegarCadastroID(int id)
        {
            var buscar = _context.usuarios.Find(id);
            if (buscar == null)
            {
                return NotFound();
            }
            return Ok(buscar);
        }

        // POST api/<AuthController>
        [HttpPost]
        public  ActionResult<Cadastro> CriarConta([FromBody] Cadastro cadastro)
        {
            if (cadastro == null)
            {
                return BadRequest("Dados invalidos");
            }
            _context.usuarios.Add(cadastro);
            _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PegarCadastroID), new {id = cadastro.id}, cadastro);
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
