using PetGuardian.Domain.Common;
using PetGuardian.Domain.Exceptions;

namespace PetGuardian.Domain.Entities;

/// <summary>
/// Veterinário (pessoa física). Possui telefone exclusivo (1:1).
/// Pode ou não estar vinculado a uma Clinica (nullable).
/// </summary>
public sealed class Veterinario : BaseEntity
{
    public string Nome  { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Senha { get; private set; } = string.Empty;

    public Guid      TelefoneId { get; private set; }
    public Telefone? Telefone   { get; private set; }

    // Nullable — veterinário pode não estar vinculado a uma clínica
    public Guid?    ClinicaId { get; private set; }
    public Clinica? Clinica   { get; private set; }

    public List<Atendimento> Atendimentos { get; private set; } = [];
    public List<Tarefa>      Tarefas      { get; private set; } = [];

    private Veterinario() { }

    public Veterinario(string nome, string email, string senha, Guid telefoneId, Guid? clinicaId = null)
    {
        AtualizarNome(nome);
        AtualizarEmail(email);
        AtualizarSenha(senha);

        if (telefoneId == Guid.Empty)
            throw new DomainException("O veterinário deve ter um telefone válido.");

        TelefoneId = telefoneId;
        ClinicaId  = clinicaId;
    }

    public void AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new DomainException("O nome não pode ser vazio.");

        novoNome = novoNome.Trim();

        if (novoNome.Length > 100)
            throw new DomainException("O nome deve ter no máximo 100 caracteres.");

        Nome = novoNome;
    }

    public void AtualizarEmail(string novoEmail)
    {
        if (string.IsNullOrWhiteSpace(novoEmail) || !novoEmail.Contains('@'))
            throw new DomainException("O e-mail informado é inválido.");

        novoEmail = novoEmail.Trim();

        if (novoEmail.Length > 50)
            throw new DomainException("O e-mail deve ter no máximo 50 caracteres.");

        Email = novoEmail;
    }

    public void AtualizarSenha(string novaSenha)
    {
        if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 6)
            throw new DomainException("A senha deve ter pelo menos 6 caracteres.");

        if (novaSenha.Length > 20)
            throw new DomainException("A senha deve ter no máximo 20 caracteres.");

        Senha = novaSenha;
    }

    public void VincularClinica(Guid clinicaId)
    {
        if (clinicaId == Guid.Empty)
            throw new DomainException("Id da clínica inválido.");

        ClinicaId = clinicaId;
    }
}