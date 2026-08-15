namespace PetGuardian.Domain.Enums;

/// <summary>
/// Tipo do atendimento realizado na veterinária.
/// Persiste como string no banco (HasConversion) para legibilidade das queries Oracle.
/// </summary>
public enum TipoAtendimento
{
    Consulta   = 0,
    Vacina     = 1,
    Cirurgia   = 2,
    Exame      = 3,
    BanhoETosa = 4,
    Retorno    = 5
}