using System.ComponentModel.DataAnnotations;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record UsuarioRequest(
    [Required][StringLength(100, MinimumLength = 2)] string Nome,
    [Required][EmailAddress][StringLength(50)] string Email,
    [Required][StringLength(20, MinimumLength = 6)] string Senha,
    [Required] Guid TelefoneId)
{
    public Usuario ToDomain() => new(Nome, Email, Senha, TelefoneId);
}