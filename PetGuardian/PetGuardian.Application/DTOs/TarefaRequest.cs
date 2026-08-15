using System.ComponentModel.DataAnnotations;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record TarefaRequest(
    [Required][StringLength(30, MinimumLength = 2)] string Titulo,
    [Range(0, 999)] int PontosTarefa,
    [Required][StringLength(200, MinimumLength = 2)] string Descricao,
    [Required] DateTime Prazo,
    [Required] Guid PetId,
    [Required] Guid VeterinarioId)
{
    public Tarefa ToDomain(Guid statusId) =>
        new(Titulo, PontosTarefa, Descricao, Prazo, PetId, statusId, VeterinarioId, usuarioId: null);
}
