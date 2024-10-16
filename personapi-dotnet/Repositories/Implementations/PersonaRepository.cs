using personapi_dotnet.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class PersonaRepository : IPersonaRepository
{
    private readonly PersonaDbContext _context;

    public PersonaRepository(PersonaDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Persona> GetAllPersonas()
    {
        return _context.Personas.Include(p => p.Estudios).Include(p => p.Telefonos).ToList();
    }

    public Persona GetPersonaById(long cc)
    {
        return _context.Personas.Include(p => p.Estudios).Include(p => p.Telefonos).FirstOrDefault(p => p.Cc == cc);
    }

    public void AddPersona(Persona persona)
    {
        _context.Personas.Add(persona);
    }

    public void UpdatePersona(Persona persona)
    {
        _context.Personas.Update(persona);
    }

    public void DeletePersona(long cc)
    {
        var persona = GetPersonaById(cc);
        if (persona != null)
        {
            _context.Personas.Remove(persona);
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
