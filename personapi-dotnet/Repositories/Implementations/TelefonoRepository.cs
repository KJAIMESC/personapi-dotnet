using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;

public class TelefonoRepository : ITelefonoRepository
{
    private readonly PersonaDbContext _context;

    public TelefonoRepository(PersonaDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Telefono> GetAllTelefonos()
    {
        return _context.Telefonos.Include(t => t.DuenoNavigation).ToList();
    }

    public Telefono GetTelefonoById(string num)
    {
        return _context.Telefonos.FirstOrDefault(t => t.Num == num);
    }

    public void AddTelefono(Telefono telefono)
    {
        _context.Telefonos.Add(telefono);
    }

    public void UpdateTelefono(Telefono telefono)
    {
        _context.Telefonos.Update(telefono);
    }

    public void DeleteTelefono(string num)
    {
        var telefono = GetTelefonoById(num);
        if (telefono != null)
        {
            _context.Telefonos.Remove(telefono);
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
