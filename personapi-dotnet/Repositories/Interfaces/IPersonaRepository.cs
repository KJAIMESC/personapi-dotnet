using personapi_dotnet.Models.Entities;

public interface IPersonaRepository
{
    IEnumerable<Persona> GetAllPersonas();
    Persona GetPersonaById(long cc);
    void AddPersona(Persona persona);
    void UpdatePersona(Persona persona);
    void DeletePersona(long cc);
    void Save();
}
