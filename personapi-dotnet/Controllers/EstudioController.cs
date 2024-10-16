using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;

[ApiController]
[Route("api/[controller]")]
public class EstudioController : ControllerBase
{
    private readonly IEstudioRepository _estudioRepository;

    public EstudioController(IEstudioRepository estudioRepository)
    {
        _estudioRepository = estudioRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var estudios = _estudioRepository.GetAllEstudios();
        return Ok(estudios);
    }

    [HttpGet("{idProf}/{ccPer}")]
    public IActionResult GetById(int idProf, long ccPer)
    {
        var estudio = _estudioRepository.GetEstudioById(idProf, ccPer);
        if (estudio == null)
        {
            return NotFound();
        }
        return Ok(estudio);
    }

    [HttpPost]
    public IActionResult Add(Estudio estudio)
    {
        _estudioRepository.AddEstudio(estudio);
        _estudioRepository.Save();
        return CreatedAtAction(nameof(GetById), new { idProf = estudio.IdProf, ccPer = estudio.CcPer }, estudio);
    }

    [HttpPut("{idProf}/{ccPer}")]
    public IActionResult Update(int idProf, long ccPer, Estudio estudio)
    {
        if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
        {
            return BadRequest();
        }
        _estudioRepository.UpdateEstudio(estudio);
        _estudioRepository.Save();
        return NoContent();
    }

    [HttpDelete("{idProf}/{ccPer}")]
    public IActionResult Delete(int idProf, long ccPer)
    {
        var estudio = _estudioRepository.GetEstudioById(idProf, ccPer);
        if (estudio == null)
        {
            return NotFound();
        }
        _estudioRepository.DeleteEstudio(idProf, ccPer);
        _estudioRepository.Save();
        return NoContent();
    }
}
