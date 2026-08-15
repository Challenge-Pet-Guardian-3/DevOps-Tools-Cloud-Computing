using PetGuardian.Domain.Common;
using PetGuardian.Domain.Exceptions;

namespace PetGuardian.Domain.Entities;

/// <summary>
/// Tipo de atendimento veterinário (tabela de domínio configurável).
/// Exemplos: Consulta, Vacinação, Cirurgia, Exame, Banho e Tosa.
/// </summary>
public sealed class TipoAtend : BaseEntity
{
    public string Tipo { get; private set; } = string.Empty;

    public List<Atendimento> Atendimentos { get; private set; } = [];

    private TipoAtend() { }

    public TipoAtend(string tipo)
    {
        if (string.IsNullOrWhiteSpace(tipo))
            throw new DomainException("O tipo de atendimento não pode ser vazio.");

        tipo = tipo.Trim();

        if (tipo.Length > 30)
            throw new DomainException("O tipo de atendimento deve ter no máximo 30 caracteres.");

        Tipo = tipo;
    }
}