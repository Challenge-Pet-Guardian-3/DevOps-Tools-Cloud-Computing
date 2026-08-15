using PetGuardian.Domain.Entities;
using PetGuardian.Domain.Enums;

namespace PetGuardian.Application.DTOs;

public record PetResponse(Guid Id, string Nome, int Idade, SexoPet Sexo, PortePet Porte, bool Castrado, Guid RacaId)
{
    public static PetResponse FromDomain(Pet p) => new(p.Id, p.Nome, p.Idade, p.Sexo, p.Porte, p.Castrado, p.RacaId);
}