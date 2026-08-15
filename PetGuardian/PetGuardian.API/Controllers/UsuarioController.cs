using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Usuários. Senha nunca é exposta nas respostas.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
{
    /// <summary>Lista todos os registros de usuários cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UsuarioResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(usuarioService.GetAll());

    /// <summary>Obtém um registro de usuário pelo seu identificador único (ID).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var u = usuarioService.GetById(id);
        return u is null ? NotFound() : Ok(u);
    }

    /// <summary>Obtém um registro de usuário buscando pelo e-mail informado.</summary>
    [HttpGet("by-email")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetByEmail([FromQuery] string email)
    {
        var u = usuarioService.GetByEmail(email);
        return u is null ? NotFound() : Ok(u);
    }

    /// <summary>Retorna o score cumulativo e as tarefas concluídas de um usuário.</summary>
    [HttpGet("{id:guid}/score")]
    [ProducesResponseType(typeof(UsuarioScoreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetScore(Guid id) => Ok(usuarioService.GetScore(id));

    /// <summary>Cadastra um novo registro de usuário na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] UsuarioRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = usuarioService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Exclui um registro de usuário cadastrado pelo seu ID.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => usuarioService.Delete(id) ? NoContent() : NotFound();
}
