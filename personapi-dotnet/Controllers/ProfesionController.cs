using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;

[ApiController]
[Route("api/[controller]")]
public class ProfesionController : ControllerBase
{
    private readonly IProfesionRepository _profesionRepository;

    public ProfesionController(IProfesionRepository profesionRepository)
    {
        _profesionRepository = profesionRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var profesions = _profesionRepository.GetAllProfesions();
        return Ok(profesions);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var profesion = _profesionRepository.GetProfesionById(id);
        if (profesion == null)
        {
            return NotFound();
        }
        return Ok(profesion);
    }

    [HttpPost]
    public IActionResult Add(Profesion profesion)
    {
        _profesionRepository.AddProfesion(profesion);
        _profesionRepository.Save();
        return CreatedAtAction(nameof(GetById), new { id = profesion.Id }, profesion);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Profesion profesion)
    {
        if (id != profesion.Id)
        {
            return BadRequest();
        }
        _profesionRepository.UpdateProfesion(profesion);
        _profesionRepository.Save();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var profesion = _profesionRepository.GetProfesionById(id);
        if (profesion == null)
        {
            return NotFound();
        }
        _profesionRepository.DeleteProfesion(id);
        _profesionRepository.Save();
        return NoContent();
    }
}
