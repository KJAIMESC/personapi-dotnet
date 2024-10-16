using personapi_dotnet.Models.Entities;

public interface IEstudioRepository
{
    IEnumerable<Estudio> GetAllEstudios();
    Estudio GetEstudioById(int idProf, long ccPer);
    void AddEstudio(Estudio estudio);
    void UpdateEstudio(Estudio estudio);
    void DeleteEstudio(int idProf, long ccPer);
    void Save();
}
