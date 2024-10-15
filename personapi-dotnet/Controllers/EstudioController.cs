using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudioController : ControllerBase
    {
        private readonly IEstudioRepository _estudioRepository;

        public EstudioController(IEstudioRepository estudioRepository)
        {
            _estudioRepository = estudioRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudio>>> GetAll()
        {
            var estudios = await _estudioRepository.GetAllAsync();
            return Ok(estudios);
        }

        [HttpGet("{idProf}/{ccPer}")]
        public async Task<ActionResult<Estudio>> GetById(int idProf, long ccPer)
        {
            var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
            if (estudio == null) return NotFound();
            return Ok(estudio);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Estudio estudio)
        {
            await _estudioRepository.AddAsync(estudio);
            return CreatedAtAction(nameof(GetById), new { idProf = estudio.IdProf, ccPer = estudio.CcPer }, estudio);
        }

        [HttpPut("{idProf}/{ccPer}")]
        public async Task<ActionResult> Update(int idProf, long ccPer, [FromBody] Estudio estudio)
        {
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer) return BadRequest();
            await _estudioRepository.UpdateAsync(estudio);
            return NoContent();
        }

        [HttpDelete("{idProf}/{ccPer}")]
        public async Task<ActionResult> Delete(int idProf, long ccPer)
        {
            await _estudioRepository.DeleteAsync(idProf, ccPer);
            return NoContent();
        }
    }
}
