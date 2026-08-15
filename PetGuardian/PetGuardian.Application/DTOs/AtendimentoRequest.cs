using System.ComponentModel.DataAnnotations;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record AtendimentoRequest(
    [Required] DateTime Data,
    [Required][StringLength(300, MinimumLength = 2)] string Anotacoes,
    [Range(0, 99999.99)] decimal Valor,
    [Required] Guid PetId,
    [Required] Guid VeterinarioId,
    [Required] Guid TipoAtendId,
    [Required] Guid StatusId)
{
    public Atendimento ToDomain() =>
        new(Data, Anotacoes, Valor, PetId, VeterinarioId, TipoAtendId, StatusId);
}