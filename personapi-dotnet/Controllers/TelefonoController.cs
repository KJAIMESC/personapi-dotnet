using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;

[ApiController]
[Route("api/[controller]")]
public class TelefonoController : ControllerBase
{
    private readonly ITelefonoRepository _telefonoRepository;

    public TelefonoController(ITelefonoRepository telefonoRepository)
    {
        _telefonoRepository = telefonoRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var telefonos = _telefonoRepository.GetAllTelefonos();
        return Ok(telefonos);
    }

    [HttpGet("{num}")]
    public IActionResult GetById(string num)
    {
        var telefono = _telefonoRepository.GetTelefonoById(num);
        if (telefono == null)
        {
            return NotFound();
        }
        return Ok(telefono);
    }

    [HttpPost]
    public IActionResult Add(Telefono telefono)
    {
        _telefonoRepository.AddTelefono(telefono);
        _telefonoRepository.Save();
        return CreatedAtAction(nameof(GetById), new { num = telefono.Num }, telefono);
    }

    [HttpPut("{num}")]
    public IActionResult Update(string num, Telefono telefono)
    {
        if (num != telefono.Num)
        {
            return BadRequest();
        }
        _telefonoRepository.UpdateTelefono(telefono);
        _telefonoRepository.Save();
        return NoContent();
    }

    [HttpDelete("{num}")]
    public IActionResult Delete(string num)
    {
        var telefono = _telefonoRepository.GetTelefonoById(num);
        if (telefono == null)
        {
            return NotFound();
        }
        _telefonoRepository.DeleteTelefono(num);
        _telefonoRepository.Save();
        return NoContent();
    }
}
