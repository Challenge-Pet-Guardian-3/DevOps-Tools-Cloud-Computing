using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Linha do tempo de eventos de um Pet (ex.: tarefas concluídas, marcos de saúde).</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class HistoricoController(IHistoricoService historicoService) : ControllerBase
{
    /// <summary>Lista todos os registros de histórico cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<HistoricoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(historicoService.GetAll());

    /// <summary>Obtém um registro de histórico pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HistoricoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var h = historicoService.GetById(id);
        return h is null ? NotFound() : Ok(h);
    }

    /// <summary>Lista o histórico de um pet específico.</summary>
    [HttpGet("by-pet/{petId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<HistoricoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByPet(Guid petId) => Ok(historicoService.GetByPetId(petId));

    /// <summary>Cadastra um novo registro de histórico na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(HistoricoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] HistoricoRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = historicoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza um registro de histórico existente (o pet vinculado não é reatribuível por aqui).</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(HistoricoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] HistoricoUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = historicoService.Update(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Remove um registro de histórico pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => historicoService.Delete(id) ? NoContent() : NotFound();
}