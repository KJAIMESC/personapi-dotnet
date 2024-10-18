using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Dto;
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
        [HttpGet("{idProf:int}/{ccPer:long}")]
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
        [HttpPut("{idProf:int}/{ccPer:long}")]
        public async Task<IActionResult> PutEstudio(int idProf, long ccPer, EstudioCreateUpdateDto estudioDto)
        {
            if (idProf != estudioDto.IdProf || ccPer != estudioDto.CcPer)
            {
                return BadRequest("The ID and CC provided in the URL do not match the DTO.");
            }

            var existingEstudio = await _context.Estudios.FindAsync(idProf, ccPer);
            if (existingEstudio == null)
            {
                return NotFound("The Estudio was not found.");
            }

            var persona = await _context.Personas.FindAsync(ccPer);
            var profesion = await _context.Profesions.FindAsync(idProf);

            if (persona == null || profesion == null)
            {
                return BadRequest("Related Persona or Profesion not found.");
            }

            existingEstudio.Fecha = estudioDto.Fecha;
            existingEstudio.Univer = estudioDto.Univer;
            existingEstudio.CcPerNavigation = persona;
            existingEstudio.IdProfNavigation = profesion;

            _context.Entry(existingEstudio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudioExists(idProf, ccPer))
                {
                    return NotFound("The Estudio was not found during save.");
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
        public async Task<ActionResult<Estudio>> PostEstudio(EstudioCreateUpdateDto estudioDto)
        {
            var persona = await _context.Personas.FindAsync(estudioDto.CcPer);
            var profesion = await _context.Profesions.FindAsync(estudioDto.IdProf);

            if (persona == null || profesion == null)
            {
                return BadRequest("Invalid CcPer or IdProf provided.");
            }

            var existingEstudio = await _context.Estudios
                .FirstOrDefaultAsync(e => e.IdProf == estudioDto.IdProf && e.CcPer == estudioDto.CcPer);

            if (existingEstudio != null)
            {
                return Conflict("An Estudio with the provided IdProf and CcPer already exists.");
            }

            var estudio = new Estudio
            {
                IdProf = estudioDto.IdProf,
                CcPer = estudioDto.CcPer,
                Fecha = estudioDto.Fecha,
                Univer = estudioDto.Univer,
                CcPerNavigation = persona,
                IdProfNavigation = profesion
            };

            _context.Estudios.Add(estudio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstudio", new { idProf = estudio.IdProf, ccPer = estudio.CcPer }, estudio);
        }

        // DELETE: api/Estudios/{idProf}/{ccPer}
        [HttpDelete("{idProf:int}/{ccPer:long}")]
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
