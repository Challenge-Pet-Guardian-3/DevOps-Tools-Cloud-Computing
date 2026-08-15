using System.ComponentModel.DataAnnotations;

namespace PetGuardian.Application.DTOs;

public record EnderecoRequest(
    [Required][StringLength(8, MinimumLength = 8, ErrorMessage = "O CEP deve ter 8 dígitos")]
    string Cep,
    [Required][StringLength(5, MinimumLength = 1)]
    string Numero
);