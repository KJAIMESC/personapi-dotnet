using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly PersonaDbContext _context;

        public PersonaRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas
                .Include(p => p.Estudios) 
                .Include(p => p.Telefonos)
                .ToListAsync();
        }

        public async Task<Persona> GetByIdAsync(long cc)
        {
            var persona = await _context.Personas
                .Include(p => p.Estudios)
                .Include(p => p.Telefonos)
                .FirstOrDefaultAsync(p => p.Cc == cc); 

            if (persona == null)
            {
                throw new KeyNotFoundException($"Persona with Cc {cc} not found.");
            }
            return persona;
        }

        public async Task AddAsync(Persona persona)
        {
            await _context.Personas.AddAsync(persona);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Persona persona)
        {
            var existingPersona = await _context.Personas.FindAsync(persona.Cc);

            if (existingPersona != null)
            {
                existingPersona.Nombre = persona.Nombre;
                existingPersona.Apellido = persona.Apellido;
                existingPersona.Genero = persona.Genero;
                existingPersona.Edad = persona.Edad;

                _context.Personas.Update(existingPersona);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(long cc)
        {
            var persona = await GetByIdAsync(cc);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
                await _context.SaveChangesAsync();
            }
        }
    }
}
