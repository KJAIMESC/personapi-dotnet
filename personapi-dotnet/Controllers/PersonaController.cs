using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaController(IPersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetAll()
        {
            var personas = await _personaRepository.GetAllAsync();
            return Ok(personas);
        }

        [HttpGet("{cc}")]
        public async Task<ActionResult<Persona>> GetById(long cc)
        {
            var persona = await _personaRepository.GetByIdAsync(cc);
            if (persona == null) return NotFound();
            return Ok(persona);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Persona persona)
        {
            await _personaRepository.AddAsync(persona);
            return CreatedAtAction(nameof(GetById), new { cc = persona.Cc }, persona);
        }

        [HttpPut("{cc}")]
        public async Task<ActionResult> Update(long cc, [FromBody] Persona persona)
        {
            if (cc != persona.Cc) return BadRequest();
            await _personaRepository.UpdateAsync(persona);
            return NoContent();
        }

        [HttpDelete("{cc}")]
        public async Task<ActionResult> Delete(long cc)
        {
            await _personaRepository.DeleteAsync(cc);
            return NoContent();
        }
    }
}
