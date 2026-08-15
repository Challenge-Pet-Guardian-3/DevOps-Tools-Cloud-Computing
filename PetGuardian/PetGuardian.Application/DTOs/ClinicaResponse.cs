using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record ClinicaResponse(Guid Id, string Nome, Guid TelefoneId, Guid EnderecoId)
{
    public static ClinicaResponse FromDomain(Clinica c) =>
        new(c.Id, c.Nome, c.TelefoneId, c.EnderecoId);
}