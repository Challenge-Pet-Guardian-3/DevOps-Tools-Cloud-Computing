using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Tarefas de cuidado. Usuario é opcional; Veterinario é obrigatório.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class TarefaController(ITarefaService tarefaService) : ControllerBase
{
    /// <summary>Lista todos os registros de tarefas de cuidado cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TarefaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(tarefaService.GetAll());

    /// <summary>Obtém um registro de tarefa de cuidado pelo seu identificador único (ID).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var t = tarefaService.GetById(id);
        return t is null ? NotFound() : Ok(t);
    }

    /// <summary>Lista todos os registros de tarefas de cuidado associados a um pet específico.</summary>
    [HttpGet("by-pet/{petId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<TarefaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByPet(Guid petId) => Ok(tarefaService.GetByPetId(petId));

    /// <summary>Lista todos os registros de tarefas de cuidado associados a um usuário específico.</summary>
    [HttpGet("by-usuario/{usuarioId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<TarefaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByUsuario(Guid usuarioId) =>
        Ok(tarefaService.GetByUsuarioId(usuarioId));

    /// <summary>Lista todos os registros de tarefas de cuidado associados a um veterinário específico.</summary>
    [HttpGet("by-veterinario/{veterinarioId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<TarefaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByVeterinario(Guid veterinarioId) =>
        Ok(tarefaService.GetByVeterinarioId(veterinarioId));

    /// <summary>Lista todos os registros de tarefas de cuidado associados a um status específico.</summary>
    [HttpGet("by-status/{statusId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<TarefaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByStatus(Guid statusId) =>
        Ok(tarefaService.GetByStatusId(statusId));

    /// <summary>Cadastra um novo registro de tarefa de cuidado na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] TarefaRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = tarefaService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Registra a conclusão de uma tarefa de cuidado por um usuário, somando os pontos ao seu score.</summary>
    [HttpPost("{id:guid}/concluir")]
    [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Concluir(Guid id, [FromBody] TarefaConcluirRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(tarefaService.Concluir(id, request.UsuarioId));
    }

    /// <summary>Exclui um registro de tarefa de cuidado cadastrado pelo seu ID.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => tarefaService.Delete(id) ? NoContent() : NotFound();
}
