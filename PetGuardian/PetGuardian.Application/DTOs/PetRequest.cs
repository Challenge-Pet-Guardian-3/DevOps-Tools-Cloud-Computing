using System.ComponentModel.DataAnnotations;
using PetGuardian.Domain.Entities;
using PetGuardian.Domain.Enums;

namespace PetGuardian.Application.DTOs;

public record PetRequest(
    [Required][StringLength(30, MinimumLength = 2)] string Nome,
    [Range(0, 99)] int Idade,
    [Required] SexoPet Sexo,
    [Required] PortePet Porte,
    bool Castrado,
    [Required] Guid RacaId)
{
    public Pet ToDomain() => new(Nome, Idade, Sexo, Porte, Castrado, RacaId);
}