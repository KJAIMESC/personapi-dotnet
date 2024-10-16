using personapi_dotnet.Models.Entities;

public interface IProfesionRepository
{
    IEnumerable<Profesion> GetAllProfesions();
    Profesion GetProfesionById(int id);
    void AddProfesion(Profesion profesion);
    void UpdateProfesion(Profesion profesion);
    void DeleteProfesion(int id);
    void Save();
}
