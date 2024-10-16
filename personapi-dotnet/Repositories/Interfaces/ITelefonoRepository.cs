using personapi_dotnet.Models.Entities;

public interface ITelefonoRepository
{
    IEnumerable<Telefono> GetAllTelefonos();
    Telefono GetTelefonoById(string num);
    void AddTelefono(Telefono telefono);
    void UpdateTelefono(Telefono telefono);
    void DeleteTelefono(string num);
    void Save();
}
