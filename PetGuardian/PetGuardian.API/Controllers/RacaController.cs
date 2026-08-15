using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Raças de pets.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class RacaController(IRacaService racaService) : ControllerBase
{
    /// <summary>Lista todos os registros de raças cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RacaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(racaService.GetAll());

    /// <summary>Obtém um registro de raça pelo seu identificador único (ID).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RacaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var raca = racaService.GetById(id);
        return raca is null ? NotFound() : Ok(raca);
    }

    /// <summary>Cadastra um novo registro de raça na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(RacaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] RacaRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = racaService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Exclui um registro de raça cadastrado pelo seu ID.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => racaService.Delete(id) ? NoContent() : NotFound();
}