using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Status de ciclo de vida compartilhado por tarefas e atendimentos.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class StatusController(IStatusService statusService) : ControllerBase
{
    /// <summary>Lista todos os registros de status cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StatusResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(statusService.GetAll());

    /// <summary>Obtém um registro de status pelo seu identificador único (ID).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var status = statusService.GetById(id);
        return status is null ? NotFound() : Ok(status);
    }

    /// <summary>Cadastra um novo registro de status na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] StatusRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = statusService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Exclui um registro de status cadastrado pelo seu ID.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => statusService.Delete(id) ? NoContent() : NotFound();
}