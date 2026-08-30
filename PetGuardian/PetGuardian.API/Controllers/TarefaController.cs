using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>
/// Tarefas de cuidado. SPRINT 3: sempre vinculadas a um Pet e a um Usuario responsável
/// (Veterinario, que antes era obrigatório, não existe mais).
/// </summary>
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

    /// <summary>Atualiza título/pontos/descrição/prazo de uma tarefa ainda não concluída.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] TarefaUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = tarefaService.Update(id, request);
        return updated is null ? NotFound() : Ok(updated);
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
