using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Cidades. Pertencem a um estado.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CidadeController(ICidadeService cidadeService) : ControllerBase
{
    /// <summary>Lista todas as cidades.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CidadeResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(cidadeService.GetAll());

    /// <summary>Obtém uma cidade pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CidadeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var cidade = cidadeService.GetById(id);
        return cidade is null ? NotFound() : Ok(cidade);
    }

    /// <summary>Lista cidades de um estado.</summary>
    [HttpGet("by-estado/{estadoId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<CidadeResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByEstado(Guid estadoId) => Ok(cidadeService.GetByEstadoId(estadoId));

    /// <summary>Cria uma cidade.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CidadeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CidadeRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = cidadeService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Remove uma cidade pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => cidadeService.Delete(id) ? NoContent() : NotFound();
}