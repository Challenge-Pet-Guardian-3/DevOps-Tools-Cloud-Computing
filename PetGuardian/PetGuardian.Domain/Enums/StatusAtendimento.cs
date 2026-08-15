namespace PetGuardian.Domain.Enums;

/// <summary>
/// Ciclo de vida de um atendimento veterinário.
/// Persiste como string no banco (HasConversion) para legibilidade das queries Oracle.
/// </summary>
public enum StatusAtendimento
{
    Agendado   = 0,
    EmAndamento = 1,
    Concluido  = 2,
    Cancelado  = 3
}