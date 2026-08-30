using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Aulas de um Módulo. Concedem pontos de gamificação ao ser concluídas.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AulaController(IAulaService aulaService) : ControllerBase
{
    /// <summary>Lista todas as aulas cadastradas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AulaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(aulaService.GetAll());

    /// <summary>Obtém uma aula pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AulaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var aula = aulaService.GetById(id);
        return aula is null ? NotFound() : Ok(aula);
    }

    /// <summary>Lista aulas de um módulo.</summary>
    [HttpGet("by-modulo/{moduloId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<AulaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByModulo(Guid moduloId) => Ok(aulaService.GetByModuloId(moduloId));

    /// <summary>Cadastra uma nova aula na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AulaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] AulaRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = aulaService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza uma aula existente (o módulo vinculado não é reatribuível por aqui).</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AulaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] AulaUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = aulaService.Update(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Remove uma aula pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => aulaService.Delete(id) ? NoContent() : NotFound();
}