using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Bairros. Pertencem a uma cidade.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class BairroController(IBairroService bairroService) : ControllerBase
{
    /// <summary>Lista todos os bairros.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BairroResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(bairroService.GetAll());

    /// <summary>Obtém um bairro pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BairroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var bairro = bairroService.GetById(id);
        return bairro is null ? NotFound() : Ok(bairro);
    }

    /// <summary>Lista bairros de uma cidade.</summary>
    [HttpGet("by-cidade/{cidadeId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<BairroResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByCidade(Guid cidadeId) => Ok(bairroService.GetByCidadeId(cidadeId));

    /// <summary>Cria um bairro.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BairroResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] BairroRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = bairroService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Remove um bairro pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => bairroService.Delete(id) ? NoContent() : NotFound();
}