using System.ComponentModel.DataAnnotations;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.DTOs;

public record TipoAtendRequest(
    [Required][StringLength(30, MinimumLength = 2)] string Tipo)
{
    public TipoAtend ToDomain() => new(Tipo);
}