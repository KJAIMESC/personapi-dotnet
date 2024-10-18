using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Dto;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EstudiosController : Controller
    {
        private readonly PersonaDbContext _context;

        public EstudiosController(PersonaDbContext context)
        {
            _context = context;
        }

        // GET: Estudios
        public async Task<IActionResult> Index()
        {
            var personaDbContext = _context.Estudios.Include(e => e.CcPerNavigation).Include(e => e.IdProfNavigation);
            return View(await personaDbContext.ToListAsync());
        }

        // GET: Estudios/Details/{idProf}/{ccPer}
        [HttpGet("Estudios/Details/{idProf:int}/{ccPer:long}")]
        public async Task<IActionResult> Details(int idProf, long ccPer)
        {
            var estudio = await _context.Estudios
                .Include(e => e.CcPerNavigation)
                .Include(e => e.IdProfNavigation)
                .FirstOrDefaultAsync(m => m.IdProf == idProf && m.CcPer == ccPer);

            if (estudio == null)
            {
                return NotFound();
            }

            // Map the Estudio entity to the EstudioDto
            var estudioDto = new EstudioDto
            {
                IdProf = estudio.IdProf,
                CcPer = estudio.CcPer,
                Fecha = estudio.Fecha,
                Univer = estudio.Univer,
                CcPerName = estudio.CcPerNavigation.Nombre,
                IdProfName = estudio.IdProfNavigation.Nom
            };

            return View(estudioDto);
        }

        // GET: Estudios/Create
        public IActionResult Create()
        {
            ViewData["CcPer"] = new SelectList(_context.Personas, "Cc", "Cc");
            ViewData["IdProf"] = new SelectList(_context.Profesions, "Id", "Id");
            return View();
        }

        // POST: Estudios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProf,CcPer,Fecha,Univer")] EstudioDto estudioDto)
        {
            if (ModelState.IsValid)
            {
                var persona = await _context.Personas.FindAsync(estudioDto.CcPer);
                var profesion = await _context.Profesions.FindAsync(estudioDto.IdProf);

                if (persona == null || profesion == null)
                {
                    return BadRequest("Invalid Persona or Profesion.");
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

                _context.Add(estudio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CcPer"] = new SelectList(_context.Personas, "Cc", "Cc", estudioDto.CcPer);
            ViewData["IdProf"] = new SelectList(_context.Profesions, "Id", "Id", estudioDto.IdProf);
            return View(estudioDto);
        }

        // GET: Estudios/Edit/{idProf}/{ccPer}
        [HttpGet("Estudios/Edit/{idProf:int}/{ccPer:long}")]
        public async Task<IActionResult> Edit(int idProf, long ccPer)
        {
            var estudio = await _context.Estudios
                .Include(e => e.CcPerNavigation)
                .Include(e => e.IdProfNavigation)
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);

            if (estudio == null)
            {
                return NotFound();
            }

            var estudioDto = new EstudioDto
            {
                IdProf = estudio.IdProf,
                CcPer = estudio.CcPer,
                Fecha = estudio.Fecha,
                Univer = estudio.Univer,
                CcPerName = estudio.CcPerNavigation.Nombre,
                IdProfName = estudio.IdProfNavigation.Nom
            };

            ViewData["CcPer"] = new SelectList(_context.Personas, "Cc", "Cc", estudioDto.CcPer);
            ViewData["IdProf"] = new SelectList(_context.Profesions, "Id", "Id", estudioDto.IdProf);
            return View(estudioDto);
        }

        // POST: Estudios/Edit/{idProf}/{ccPer}
        [HttpPost("Estudios/Edit/{idProf:int}/{ccPer:long}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idProf, long ccPer, [Bind("IdProf,CcPer,Fecha,Univer")] EstudioDto estudioDto)
        {
            if (idProf != estudioDto.IdProf || ccPer != estudioDto.CcPer)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingEstudio = await _context.Estudios.FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
                if (existingEstudio == null)
                {
                    return NotFound();
                }

                var persona = await _context.Personas.FindAsync(estudioDto.CcPer);
                var profesion = await _context.Profesions.FindAsync(estudioDto.IdProf);

                if (persona == null || profesion == null)
                {
                    return BadRequest("Invalid Persona or Profesion.");
                }

                existingEstudio.Fecha = estudioDto.Fecha;
                existingEstudio.Univer = estudioDto.Univer;
                existingEstudio.CcPerNavigation = persona;
                existingEstudio.IdProfNavigation = profesion;

                try
                {
                    _context.Update(existingEstudio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudioExists(estudioDto.IdProf))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CcPer"] = new SelectList(_context.Personas, "Cc", "Cc", estudioDto.CcPer);
            ViewData["IdProf"] = new SelectList(_context.Profesions, "Id", "Id", estudioDto.IdProf);
            return View(estudioDto);
        }

        // GET: Estudios/Delete/{idProf}/{ccPer}
        [HttpGet("Estudios/Delete/{idProf:int}/{ccPer:long}")]
        public async Task<IActionResult> Delete(int idProf, long ccPer)
        {
            var estudio = await _context.Estudios
                .Include(e => e.CcPerNavigation)
                .Include(e => e.IdProfNavigation)
                .FirstOrDefaultAsync(m => m.IdProf == idProf && m.CcPer == ccPer);

            if (estudio == null)
            {
                return NotFound();
            }

            var estudioDto = new EstudioDto
            {
                IdProf = estudio.IdProf,
                CcPer = estudio.CcPer,
                Fecha = estudio.Fecha,
                Univer = estudio.Univer,
                CcPerName = estudio.CcPerNavigation.Nombre,
                IdProfName = estudio.IdProfNavigation.Nom
            };

            return View(estudioDto);
        }

        // POST: Estudios/Delete/{idProf}/{ccPer}
        [HttpPost("Estudios/Delete/{idProf:int}/{ccPer:long}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idProf, long ccPer)
        {
            var estudio = await _context.Estudios.FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
            if (estudio != null)
            {
                _context.Estudios.Remove(estudio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EstudioExists(int idProf)
        {
            return _context.Estudios.Any(e => e.IdProf == idProf);
        }
    }
}
