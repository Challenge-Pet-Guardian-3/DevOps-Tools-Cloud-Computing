using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record StatusResponse(Guid Id, string NomeStatus)
{
    public static StatusResponse FromDomain(Status s) => new(s.Id, s.NomeStatus);
}