using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;

public class EstudioRepository : IEstudioRepository
{
    private readonly PersonaDbContext _context;

    public EstudioRepository(PersonaDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Estudio> GetAllEstudios()
    {
        return _context.Estudios.Include(e => e.CcPerNavigation).Include(e => e.IdProfNavigation).ToList();
    }

    public Estudio GetEstudioById(int idProf, long ccPer)
    {
        return _context.Estudios.FirstOrDefault(e => e.IdProf == idProf && e.CcPer == ccPer);
    }

    public void AddEstudio(Estudio estudio)
    {
        _context.Estudios.Add(estudio);
    }

    public void UpdateEstudio(Estudio estudio)
    {
        _context.Estudios.Update(estudio);
    }

    public void DeleteEstudio(int idProf, long ccPer)
    {
        var estudio = GetEstudioById(idProf, ccPer);
        if (estudio != null)
        {
            _context.Estudios.Remove(estudio);
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
