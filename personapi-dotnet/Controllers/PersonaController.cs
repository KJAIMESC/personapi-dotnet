using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;

[ApiController]
[Route("api/[controller]")]
public class PersonaController : ControllerBase
{
    private readonly IPersonaRepository _personaRepository;

    public PersonaController(IPersonaRepository personaRepository)
    {
        _personaRepository = personaRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var personas = _personaRepository.GetAllPersonas();
        return Ok(personas);
    }

    [HttpGet("{cc}")]
    public IActionResult GetById(long cc)
    {
        var persona = _personaRepository.GetPersonaById(cc);
        if (persona == null)
        {
            return NotFound();
        }
        return Ok(persona);
    }

    [HttpPost]
    public IActionResult Add(Persona persona)
    {
        _personaRepository.AddPersona(persona);
        _personaRepository.Save();
        return CreatedAtAction(nameof(GetById), new { cc = persona.Cc }, persona);
    }

    [HttpPut("{cc}")]
    public IActionResult Update(long cc, Persona persona)
    {
        if (cc != persona.Cc)
        {
            return BadRequest();
        }
        _personaRepository.UpdatePersona(persona);
        _personaRepository.Save();
        return NoContent();
    }

    [HttpDelete("{cc}")]
    public IActionResult Delete(long cc)
    {
        var persona = _personaRepository.GetPersonaById(cc);
        if (persona == null)
        {
            return NotFound();
        }
        _personaRepository.DeletePersona(cc);
        _personaRepository.Save();
        return NoContent();
    }
}
