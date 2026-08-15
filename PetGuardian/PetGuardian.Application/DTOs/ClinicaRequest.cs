using System.ComponentModel.DataAnnotations;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record ClinicaRequest(
    [Required][StringLength(30, MinimumLength = 2)] string Nome,
    [Required] Guid TelefoneId,
    [Required] Guid EnderecoId)
{
    public Clinica ToDomain() => new(Nome, TelefoneId, EnderecoId);
}