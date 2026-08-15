using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Telefones de contato. Devem ser criados antes de Usuario e Veterinaria.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class TelefoneController(ITelefoneService telefoneService) : ControllerBase
{
    /// <summary>Lista todos os registros de telefones cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TelefoneResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(telefoneService.GetAll());

    /// <summary>Obtém um registro de telefone pelo seu identificador único (ID).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TelefoneResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var telefone = telefoneService.GetById(id);
        return telefone is null ? NotFound() : Ok(telefone);
    }

    /// <summary>Cadastra um novo registro de telefone na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TelefoneResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] TelefoneRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = telefoneService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Exclui um registro de telefone cadastrado pelo seu ID.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => telefoneService.Delete(id) ? NoContent() : NotFound();
}