using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class EstudioRepository : IEstudioRepository
    {
        private readonly PersonaDbContext _context;

        public EstudioRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estudio>> GetAllAsync()
        {
            return await _context.Estudios.ToListAsync();
        }

        public async Task<Estudio> GetByIdAsync(int idProf, long ccPer)
        {
            var estudio = await _context.Estudios.FindAsync(idProf, ccPer);
            if (estudio == null)
            {
                throw new KeyNotFoundException($"Estudio with Id {idProf} and CcPer {ccPer} not found.");
            }
            return estudio;
        }

        public async Task AddAsync(Estudio estudio)
        {
            var persona = await _context.Personas.FindAsync(estudio.CcPer);
            var profesion = await _context.Profesions.FindAsync(estudio.IdProf);

            if (persona == null)
            {
                throw new KeyNotFoundException($"Persona with Cc {estudio.CcPer} not found.");
            }

            if (profesion == null)
            {
                throw new KeyNotFoundException($"Profesion with Id {estudio.IdProf} not found.");
            }

            // Assigning navigation properties
            estudio.CcPerNavigation = persona;
            estudio.IdProfNavigation = profesion;

            await _context.Estudios.AddAsync(estudio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Estudio estudio)
        {
            var persona = await _context.Personas.FindAsync(estudio.CcPer);
            var profesion = await _context.Profesions.FindAsync(estudio.IdProf);

            if (persona == null)
            {
                throw new KeyNotFoundException($"Persona with Cc {estudio.CcPer} not found.");
            }

            if (profesion == null)
            {
                throw new KeyNotFoundException($"Profesion with Id {estudio.IdProf} not found.");
            }

            // Assigning navigation properties
            estudio.CcPerNavigation = persona;
            estudio.IdProfNavigation = profesion;

            _context.Estudios.Update(estudio);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int idProf, long ccPer)
        {
            var estudio = await GetByIdAsync(idProf, ccPer);
            if (estudio != null)
            {
                _context.Estudios.Remove(estudio);
                await _context.SaveChangesAsync();
            }
        }
    }
}
