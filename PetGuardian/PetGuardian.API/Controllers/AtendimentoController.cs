using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Atendimentos veterinários. Liga Pet + Veterinario + Tipo + Status.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AtendimentoController(IAtendimentoService atendimentoService) : ControllerBase
{
    /// <summary>Lista todos os registros de atendimentos clínicos cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AtendimentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(atendimentoService.GetAll());

    /// <summary>Obtém um registro de atendimento clínico pelo seu identificador único (ID).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AtendimentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var a = atendimentoService.GetById(id);
        return a is null ? NotFound() : Ok(a);
    }

    /// <summary>Lista todos os registros de atendimentos clínicos associados a um pet específico.</summary>
    [HttpGet("by-pet/{petId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<AtendimentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByPet(Guid petId) => Ok(atendimentoService.GetByPetId(petId));

    /// <summary>Lista todos os registros de atendimentos clínicos associados a um veterinário específico.</summary>
    [HttpGet("by-veterinario/{veterinarioId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<AtendimentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByVeterinario(Guid veterinarioId) =>
        Ok(atendimentoService.GetByVeterinarioId(veterinarioId));

    /// <summary>Cadastra um novo registro de atendimento clínico na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AtendimentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] AtendimentoRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = atendimentoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Exclui um registro de atendimento clínico cadastrado pelo seu ID.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => atendimentoService.Delete(id) ? NoContent() : NotFound();
}