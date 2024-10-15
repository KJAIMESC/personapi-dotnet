using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelefonoController : ControllerBase
    {
        private readonly ITelefonoRepository _telefonoRepository;

        public TelefonoController(ITelefonoRepository telefonoRepository)
        {
            _telefonoRepository = telefonoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Telefono>>> GetAll()
        {
            var telefonos = await _telefonoRepository.GetAllAsync();
            return Ok(telefonos);
        }

        [HttpGet("{num}")]
        public async Task<ActionResult<Telefono>> GetById(string num)
        {
            var telefono = await _telefonoRepository.GetByIdAsync(num);
            if (telefono == null) return NotFound();
            return Ok(telefono);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Telefono telefono)
        {
            await _telefonoRepository.AddAsync(telefono);
            return CreatedAtAction(nameof(GetById), new { num = telefono.Num }, telefono);
        }

        [HttpPut("{num}")]
        public async Task<ActionResult> Update(string num, [FromBody] Telefono telefono)
        {
            if (num != telefono.Num) return BadRequest();
            await _telefonoRepository.UpdateAsync(telefono);
            return NoContent();
        }

        [HttpDelete("{num}")]
        public async Task<ActionResult> Delete(string num)
        {
            await _telefonoRepository.DeleteAsync(num);
            return NoContent();
        }
    }
}
