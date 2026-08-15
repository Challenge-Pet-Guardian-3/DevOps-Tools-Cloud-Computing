using PetGuardian.Domain.Common;
using PetGuardian.Domain.Exceptions;

namespace PetGuardian.Domain.Entities;

/// <summary>
/// Status de ciclo de vida compartilhado por Tarefa e Atendimento.
/// Valores: CONCLUIDO | EXPIRADO | PENDENTE.
/// </summary>
public sealed class Status : BaseEntity
{
    private static readonly string[] ValoresValidos = ["CONCLUIDO", "EXPIRADO", "PENDENTE"];

    public string NomeStatus { get; private set; } = string.Empty;

    public List<Tarefa>      Tarefas      { get; private set; } = [];
    public List<Atendimento> Atendimentos { get; private set; } = [];

    private Status() { }

    public Status(string nomeStatus)
    {
        if (string.IsNullOrWhiteSpace(nomeStatus))
            throw new DomainException("O status não pode ser vazio.");

        nomeStatus = nomeStatus.Trim().ToUpperInvariant();

        if (!ValoresValidos.Contains(nomeStatus))
            throw new DomainException($"Status inválido. Valores aceitos: {string.Join(", ", ValoresValidos)}.");

        NomeStatus = nomeStatus;
    }
}