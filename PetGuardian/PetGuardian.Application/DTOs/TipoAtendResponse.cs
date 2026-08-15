using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record TipoAtendResponse(Guid Id, string Tipo)
{
    public static TipoAtendResponse FromDomain(TipoAtend t) => new(t.Id, t.Tipo);
}