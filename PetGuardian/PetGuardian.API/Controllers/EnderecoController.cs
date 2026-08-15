using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Endereços. Devem ser criados após Bairro. Usados por Usuario e Veterinaria.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class EnderecoController(IEnderecoService enderecoService) : ControllerBase
{
    /// <summary>Lista todos os endereços.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EnderecoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(enderecoService.GetAll());

    /// <summary>Obtém um endereço pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EnderecoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var endereco = enderecoService.GetById(id);
        return endereco is null ? NotFound() : Ok(endereco);
    }

    /// <summary>Cria um endereço.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EnderecoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] EnderecoRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = enderecoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Remove um endereço pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => enderecoService.Delete(id) ? NoContent() : NotFound();
}