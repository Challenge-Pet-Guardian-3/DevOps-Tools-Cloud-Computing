using PetGuardian.Domain.Common;
using PetGuardian.Domain.Exceptions;

namespace PetGuardian.Domain.Entities;

/// <summary>
/// Atendimento veterinário. Liga Pet + Veterinario + TipoAtend + Status.
/// </summary>
public sealed class Atendimento : BaseEntity
{
    public DateTime Data      { get; private set; }
    public string   Anotacoes { get; private set; } = string.Empty;
    public decimal  Valor     { get; private set; }

    public Guid         PetId         { get; private set; }
    public Pet?         Pet           { get; private set; }

    public Guid         VeterinarioId { get; private set; }
    public Veterinario? Veterinario   { get; private set; }

    public Guid      TipoAtendId { get; private set; }
    public TipoAtend? TipoAtend  { get; private set; }

    public Guid    StatusId { get; private set; }
    public Status? Status   { get; private set; }

    private Atendimento() { }

    public Atendimento(
        DateTime data,
        string   anotacoes,
        decimal  valor,
        Guid     petId,
        Guid     veterinarioId,
        Guid     tipoAtendId,
        Guid     statusId)
    {
        if (string.IsNullOrWhiteSpace(anotacoes))
            throw new DomainException("As anotações não podem ser vazias.");

        anotacoes = anotacoes.Trim();

        if (anotacoes.Length > 300)
            throw new DomainException("As anotações devem ter no máximo 300 caracteres.");

        if (valor < 0)
            throw new DomainException("O valor não pode ser negativo.");

        if (petId == Guid.Empty)
            throw new DomainException("O atendimento deve estar associado a um pet válido.");

        if (veterinarioId == Guid.Empty)
            throw new DomainException("O atendimento deve estar associado a um veterinário válido.");

        if (tipoAtendId == Guid.Empty)
            throw new DomainException("O tipo de atendimento é obrigatório.");

        if (statusId == Guid.Empty)
            throw new DomainException("O status do atendimento é obrigatório.");

        Data          = data;
        Anotacoes     = anotacoes;
        Valor         = valor;
        PetId         = petId;
        VeterinarioId = veterinarioId;
        TipoAtendId   = tipoAtendId;
        StatusId      = statusId;
    }

    public void AtualizarValor(decimal novoValor)
    {
        if (novoValor < 0)
            throw new DomainException("O valor não pode ser negativo.");

        Valor = novoValor;
    }
}