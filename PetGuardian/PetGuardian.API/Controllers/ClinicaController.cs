using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Clínicas veterinárias. Possuem endereço e telefone exclusivos.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ClinicaController(IClinicaService clinicaService) : ControllerBase
{
    /// <summary>Lista todos os registros de clínicas cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClinicaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(clinicaService.GetAll());

    /// <summary>Obtém um registro de clínica pelo seu identificador único (ID).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClinicaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var c = clinicaService.GetById(id);
        return c is null ? NotFound() : Ok(c);
    }

    /// <summary>Cadastra um novo registro de clínica na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClinicaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ClinicaRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = clinicaService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Exclui um registro de clínica cadastrado pelo seu ID.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => clinicaService.Delete(id) ? NoContent() : NotFound();
}