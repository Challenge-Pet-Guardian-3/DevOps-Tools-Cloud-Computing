using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Tipos de atendimento veterinário.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class TipoAtendController(ITipoAtendService tipoAtendService) : ControllerBase
{
    /// <summary>Lista todos os registros de tipos de atendimento clínico cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TipoAtendResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(tipoAtendService.GetAll());

    /// <summary>Obtém um registro de tipo de atendimento clínico pelo seu identificador único (ID).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TipoAtendResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var tipo = tipoAtendService.GetById(id);
        return tipo is null ? NotFound() : Ok(tipo);
    }

    /// <summary>Cadastra um novo registro de tipo de atendimento clínico na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TipoAtendResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] TipoAtendRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = tipoAtendService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Exclui um registro de tipo de atendimento clínico cadastrado pelo seu ID.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => tipoAtendService.Delete(id) ? NoContent() : NotFound();
}