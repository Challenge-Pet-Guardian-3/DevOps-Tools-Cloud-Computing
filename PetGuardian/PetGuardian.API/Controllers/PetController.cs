using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Gerenciamento de pets no contexto de rede de cuidado.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class PetController(IPetService petService) : ControllerBase
{
    /// <summary>Lista todos os pets.</summary>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(petService.GetAll());

    /// <summary>Obtém um pet pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var pet = petService.GetById(id);
        if (pet is null)
            return NotFound();
        return Ok(pet);
    }

    /// <summary>Lista todos os pets de uma raça.</summary>
    [HttpGet("by-raca/{racaId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByRaca(Guid racaId) =>
        Ok(petService.GetByRacaId(racaId));

    /// <summary>
    /// Retorna a linha do tempo histórica unificada de um pet.
    /// Passou de Atendimentos+Tarefas para Historico+Tarefas concluídas.
    /// </summary>
    [HttpGet("{id:guid}/historico")]
    [ProducesResponseType(typeof(IReadOnlyList<PetHistoricoItemResponse>), StatusCodes.Status200OK)]
    public IActionResult GetHistorico(Guid id) => Ok(petService.GetHistorico(id));

    /// <summary>Cadastra um pet.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] PetRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var created = petService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza um pet existente.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] PetRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var updated = petService.Update(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Remove um pet pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) =>
        petService.Delete(id) ? NoContent() : NotFound();
}
