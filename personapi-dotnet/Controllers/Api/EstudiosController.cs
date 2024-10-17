using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiosController : ControllerBase
    {
        private readonly PersonaDbContext _context;

        public EstudiosController(PersonaDbContext context)
        {
            _context = context;
        }

        // GET: api/Estudios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudio>>> GetEstudios()
        {
            return await _context.Estudios.ToListAsync();
        }

        // GET: api/Estudios/{idProf}/{ccPer}
        [HttpGet("{idProf}/{ccPer}")]
        public async Task<ActionResult<Estudio>> GetEstudio(int idProf, long ccPer)
        {
            var estudio = await _context.Estudios.FindAsync(idProf, ccPer);

            if (estudio == null)
            {
                return NotFound();
            }

            return estudio;
        }

        // PUT: api/Estudios/{idProf}/{ccPer}
        [HttpPut("{idProf}/{ccPer}")]
        public async Task<IActionResult> PutEstudio(int idProf, long ccPer, Estudio estudio)
        {
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
            {
                return BadRequest();
            }

            // Check if related entities exist
            var persona = await _context.Personas.FindAsync(ccPer);
            var profesion = await _context.Profesions.FindAsync(idProf);

            if (persona == null || profesion == null)
            {
                return BadRequest("Related Persona or Profesion not found.");
            }

            // Set navigation properties
            estudio.CcPerNavigation = persona;
            estudio.IdProfNavigation = profesion;

            _context.Entry(estudio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudioExists(idProf, ccPer))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Estudios
        [HttpPost]
        public async Task<ActionResult<Estudio>> PostEstudio(Estudio estudio)
        {
            // Fetch the related entities based on the provided identifiers
            var persona = await _context.Personas.FindAsync(estudio.CcPer);
            var profesion = await _context.Profesions.FindAsync(estudio.IdProf);

            if (persona == null || profesion == null)
            {
                return BadRequest("Invalid CcPer or IdProf provided.");
            }

            // Set the navigation properties
            estudio.CcPerNavigation = persona;
            estudio.IdProfNavigation = profesion;

            _context.Estudios.Add(estudio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstudio", new { id = estudio.IdProf }, estudio);
        }


        // DELETE: api/Estudios/{idProf}/{ccPer}
        [HttpDelete("{idProf}/{ccPer}")]
        public async Task<IActionResult> DeleteEstudio(int idProf, long ccPer)
        {
            var estudio = await _context.Estudios.FindAsync(idProf, ccPer);
            if (estudio == null)
            {
                return NotFound();
            }

            _context.Estudios.Remove(estudio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstudioExists(int idProf, long ccPer)
        {
            return _context.Estudios.Any(e => e.IdProf == idProf && e.CcPer == ccPer);
        }
    }
}
