using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record UsuarioResponse(Guid Id, string Nome, string Email, Guid TelefoneId)
{
    public static UsuarioResponse FromDomain(Usuario u) =>
        new(u.Id, u.Nome, u.Email, u.TelefoneId);
}