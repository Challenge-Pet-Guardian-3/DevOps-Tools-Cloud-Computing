using PetGuardian.Domain.Common;
using PetGuardian.Domain.Exceptions;

namespace PetGuardian.Domain.Entities;

/// <summary>
/// Clínica veterinária. Possui telefone e endereço exclusivos (1:1).
/// Agrupa Veterinarios.
/// </summary>
public sealed class Clinica : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;

    public Guid      TelefoneId { get; private set; }
    public Telefone? Telefone   { get; private set; }

    public Guid      EnderecoId { get; private set; }
    public Endereco? Endereco   { get; private set; }

    public List<Veterinario> Veterinarios { get; private set; } = [];

    private Clinica() { }

    public Clinica(string nome, Guid telefoneId, Guid enderecoId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome da clínica não pode ser vazio.");

        nome = nome.Trim();

        if (nome.Length > 30)
            throw new DomainException("O nome da clínica deve ter no máximo 30 caracteres.");

        if (telefoneId == Guid.Empty)
            throw new DomainException("A clínica deve ter um telefone válido.");

        if (enderecoId == Guid.Empty)
            throw new DomainException("A clínica deve ter um endereço válido.");

        Nome       = nome;
        TelefoneId = telefoneId;
        EnderecoId = enderecoId;
    }
}