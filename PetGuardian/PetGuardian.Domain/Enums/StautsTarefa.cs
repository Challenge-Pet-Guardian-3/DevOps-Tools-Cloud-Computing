namespace PetGuardian.Domain.Enums;

/// <summary>
/// Ciclo de vida de uma tarefa associada a um pet.
/// Persiste como string no banco (HasConversion) para legibilidade das queries Oracle.
/// </summary>
public enum StatusTarefa
{
    Pendente   = 0,
    EmAndamento = 1,
    Concluida  = 2,
    Cancelada  = 3
}