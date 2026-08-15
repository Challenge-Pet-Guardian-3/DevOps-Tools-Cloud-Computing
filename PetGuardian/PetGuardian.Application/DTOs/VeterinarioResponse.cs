using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record VeterinarioResponse(Guid Id, string Nome, string Email, Guid TelefoneId, Guid? ClinicaId)
{
    public static VeterinarioResponse FromDomain(Veterinario v) =>
        new(v.Id, v.Nome, v.Email, v.TelefoneId, v.ClinicaId);
}