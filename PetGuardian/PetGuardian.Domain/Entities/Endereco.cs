using PetGuardian.Domain.Common;
using PetGuardian.Domain.Exceptions;

namespace PetGuardian.Domain.Entities;

/// <summary>
/// Endereço físico. Hierarquia: Endereco → Bairro → Cidade → Estado.
/// Relacionamento 1:1 com Usuario e com Veterinaria.
/// </summary>
public sealed class Endereco : BaseEntity
{
    public string Cep    { get; private set; } = string.Empty;
    public string Rua    { get; private set; } = string.Empty;
    public string Numero { get; private set; } = string.Empty;

    public Guid    BairroId { get; private set; }
    public Bairro? Bairro   { get; private set; }

    private Endereco() { }

    public Endereco(string cep, string rua, string numero, Guid bairroId)
    {
        if (string.IsNullOrWhiteSpace(cep))
            throw new DomainException("O CEP não pode ser vazio.");

        cep = cep.Trim().Replace("-", "");

        if (cep.Length != 8)
            throw new DomainException("O CEP deve ter 8 dígitos.");

        if (string.IsNullOrWhiteSpace(rua))
            throw new DomainException("A rua não pode ser vazia.");

        if (string.IsNullOrWhiteSpace(numero))
            throw new DomainException("O número não pode ser vazio.");

        numero = numero.Trim();

        if (numero.Length > 5)
            throw new DomainException("O número deve ter no máximo 5 caracteres.");

        if (bairroId == Guid.Empty)
            throw new DomainException("O endereço deve estar associado a um bairro válido.");

        Cep      = cep;
        Rua      = rua.Trim();
        Numero   = numero;
        BairroId = bairroId;
    }
}