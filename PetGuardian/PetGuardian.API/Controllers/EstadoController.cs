using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Estados da federação. Topo da hierarquia de endereço.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class EstadoController(IEstadoService estadoService) : ControllerBase
{
    /// <summary>Lista todos os estados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EstadoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(estadoService.GetAll());

    /// <summary>Obtém um estado pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EstadoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var estado = estadoService.GetById(id);
        return estado is null ? NotFound() : Ok(estado);
    }

    /// <summary>Cria um estado.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EstadoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] EstadoRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = estadoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Remove um estado pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => estadoService.Delete(id) ? NoContent() : NotFound();
}