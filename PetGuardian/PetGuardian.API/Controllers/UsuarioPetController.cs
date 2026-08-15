using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Vínculo N:N entre usuário e pet, com flag de responsável principal.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UsuarioPetController(IUsuarioPetService usuarioPetService) : ControllerBase
{
    /// <summary>Lista todos os registros de vínculos de rede de cuidado de usuários e pets cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UsuarioPetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(usuarioPetService.GetAll());

    /// <summary>Lista todos os registros de vínculos de rede de cuidado de usuários e pets associados a um usuário específico.</summary>
    [HttpGet("by-usuario/{usuarioId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<UsuarioPetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByUsuario(Guid usuarioId) => Ok(usuarioPetService.GetByUsuarioId(usuarioId));

    /// <summary>Lista todos os registros de vínculos de rede de cuidado de usuários e pets associados a um pet específico.</summary>
    [HttpGet("by-pet/{petId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<UsuarioPetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByPet(Guid petId) => Ok(usuarioPetService.GetByPetId(petId));

    /// <summary>Obtém a rede de cuidado colaborativo (co-cuidadores e pets vinculados) de um usuário.</summary>
    [HttpGet("rede-cuidado/{usuarioId:guid}")]
    [ProducesResponseType(typeof(RedeCuidadoResponse), StatusCodes.Status200OK)]
    public IActionResult GetRedeCuidado(Guid usuarioId) => Ok(usuarioPetService.GetRedeCuidadoByUsuarioId(usuarioId));

    /// <summary>Cadastra um novo registro de vínculo de rede de cuidado de usuário e pet na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioPetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] UsuarioPetRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = usuarioPetService.Create(request);
        return CreatedAtAction(nameof(GetByUsuario), new { usuarioId = created.UsuarioId }, created);
    }

    /// <summary>Envia um convite de participação na rede de cuidado por ID (Exclusivo para Responsável Principal).</summary>
    [HttpPost("invite/by-usuario")]
    [ProducesResponseType(typeof(UsuarioPetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult InviteByUsuario([FromBody] UsuarioPetInviteByUsuarioRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = usuarioPetService.InviteByUsuario(request);
        return CreatedAtAction(nameof(GetByPet), new { petId = created.PetId }, created);
    }

    /// <summary>Envia um convite de participação na rede de cuidado buscando por E-mail (Exclusivo para Responsável Principal).</summary>
    [HttpPost("invite/by-email")]
    [ProducesResponseType(typeof(UsuarioPetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult InviteByEmail([FromBody] UsuarioPetInviteByEmailRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = usuarioPetService.InviteByEmail(request);
        return CreatedAtAction(nameof(GetByPet), new { petId = created.PetId }, created);
    }

    /// <summary>Remove um vínculo pela chave composta (usuarioId + petId).</summary>
    [HttpDelete("{usuarioId:guid}/{petId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid usuarioId, Guid petId) =>
        usuarioPetService.Delete(usuarioId, petId) ? NoContent() : NotFound();
}
