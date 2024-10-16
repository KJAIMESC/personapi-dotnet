using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;

public class ProfesionRepository : IProfesionRepository
{
    private readonly PersonaDbContext _context;

    public ProfesionRepository(PersonaDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Profesion> GetAllProfesions()
    {
        return _context.Profesions.Include(p => p.Estudios).ToList();
    }

    public Profesion GetProfesionById(int id)
    {
        return _context.Profesions.FirstOrDefault(p => p.Id == id);
    }

    public void AddProfesion(Profesion profesion)
    {
        _context.Profesions.Add(profesion);
    }

    public void UpdateProfesion(Profesion profesion)
    {
        _context.Profesions.Update(profesion);
    }

    public void DeleteProfesion(int id)
    {
        var profesion = GetProfesionById(id);
        if (profesion != null)
        {
            _context.Profesions.Remove(profesion);
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
