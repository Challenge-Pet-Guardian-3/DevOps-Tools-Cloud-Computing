using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Vínculo N:N entre usuário e endereço.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UsuarioEnderecoController(IUsuarioEnderecoService usuarioEnderecoService) : ControllerBase
{
    /// <summary>Lista todos os registros de vínculos de endereços de usuários cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UsuarioEnderecoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(usuarioEnderecoService.GetAll());

    /// <summary>Lista todos os registros de vínculos de endereços de usuários associados a um usuário específico.</summary>
    [HttpGet("by-usuario/{usuarioId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<UsuarioEnderecoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByUsuario(Guid usuarioId) =>
        Ok(usuarioEnderecoService.GetByUsuarioId(usuarioId));

    /// <summary>Lista todos os registros de vínculos de endereços de usuários associados a um endereço específico.</summary>
    [HttpGet("by-endereco/{enderecoId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<UsuarioEnderecoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByEndereco(Guid enderecoId) =>
        Ok(usuarioEnderecoService.GetByEnderecoId(enderecoId));

    /// <summary>Cadastra um novo registro de vínculo de endereço de usuário na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioEnderecoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] UsuarioEnderecoRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = usuarioEnderecoService.Create(request);
        return CreatedAtAction(nameof(GetByUsuario), new { usuarioId = created.UsuarioId }, created);
    }

    /// <summary>Remove um vínculo pela chave composta.</summary>
    [HttpDelete("{usuarioId:guid}/{enderecoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid usuarioId, Guid enderecoId) =>
        usuarioEnderecoService.Delete(usuarioId, enderecoId) ? NoContent() : NotFound();
}