using Microsoft.AspNetCore.Mvc;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.API.Controllers;

/// <summary>Módulos de uma Trilha. Agrupam Aulas.</summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ModuloController(IModuloService moduloService) : ControllerBase
{
    /// <summary>Lista todos os módulos cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ModuloResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(moduloService.GetAll());

    /// <summary>Obtém um módulo pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ModuloResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var modulo = moduloService.GetById(id);
        return modulo is null ? NotFound() : Ok(modulo);
    }

    /// <summary>Lista módulos de uma trilha.</summary>
    [HttpGet("by-trilha/{trilhaId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<ModuloResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByTrilha(Guid trilhaId) => Ok(moduloService.GetByTrilhaId(trilhaId));

    /// <summary>Cadastra um novo módulo na base de dados.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ModuloResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ModuloRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = moduloService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza um módulo existente (a trilha vinculada não é reatribuível por aqui).</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ModuloResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] ModuloUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = moduloService.Update(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Remove um módulo pelo Id.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) => moduloService.Delete(id) ? NoContent() : NotFound();
}