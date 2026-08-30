using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Trilhas de cuidado/aprendizado vinculadas a um Pet. Agrupam Módulos.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class TrilhaController(ITrilhaService trilhaService) : ControllerBase
{
    /// <summary>Lista todas as trilhas cadastradas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TrilhaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(trilhaService.GetAll());

    /// <summary>Obtém uma trilha pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TrilhaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var trilha = trilhaService.GetById(id);
        return trilha is null ? NotFound() : Ok(trilha);
    }

    /// <summary>Lista trilhas de um pet.</summary>
    [HttpGet("by-pet/{petId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<TrilhaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByPet(Guid petId) => Ok(trilhaService.GetByPetId(petId));

    /// <summary>Cadastra uma nova trilha na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TrilhaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] TrilhaRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = trilhaService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza uma trilha existente (o pet vinculado não é reatribuível por aqui).</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TrilhaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] TrilhaUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = trilhaService.Update(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Remove uma trilha pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => trilhaService.Delete(id) ? NoContent() : NotFound();
}