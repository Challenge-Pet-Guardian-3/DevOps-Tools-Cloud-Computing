using PetGuardian.Domain.Common;
using PetGuardian.Domain.Enums;
using PetGuardian.Domain.Exceptions;

namespace PetGuardian.Domain.Entities;

/// <summary>
/// Usuário do sistema. Possui telefone exclusivo (1:1).
/// Endereços via N:N (UsuarioEndereco). Relacionamento N:N com Pet via UsuarioPet.
/// </summary>
/// <remarks>
/// <c>Role</c> (COMUM/PREMIUM) e a coluna de senha cresceu para 60
/// caracteres — tamanho compatível com um hash bcrypt. Isso é só o espaço no banco; o hashing em
/// si (BCrypt.Net, Identity etc.) fica para a Sprint 4, junto com JWT (Não será implementado AINDA)
/// </remarks>
public sealed class Usuario : BaseEntity
{
    public string      Nome  { get; private set; } = string.Empty;
    public string      Email { get; private set; } = string.Empty;
    public string      Senha { get; private set; } = string.Empty;
    public RoleUsuario Role  { get; private set; }

    public Guid      TelefoneId { get; private set; }
    public Telefone? Telefone   { get; private set; }

    public List<UsuarioEndereco> Enderecos      { get; private set; } = [];
    public List<UsuarioPet>      PetsVinculados { get; private set; } = [];
    public List<Tarefa>          Tarefas        { get; private set; } = [];

    private Usuario() { }

    public Usuario(string nome, string email, string senha, RoleUsuario role, Guid telefoneId)
    {
        AtualizarNome(nome);
        AtualizarEmail(email);
        AtualizarSenha(senha);
        Role = role;
        if (telefoneId == Guid.Empty)
            throw new DomainException("O usuário deve ter um telefone válido.");
        TelefoneId = telefoneId;
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
        // 60 = espaço para acomodar um hash bcrypt quando o hashing for implementado (Sprint 4).
        if (novaSenha.Length > 60)
            throw new DomainException("A senha deve ter no máximo 60 caracteres.");
        Senha = novaSenha;
    }

    /// <summary>NOVO (schema 2026-08-29).</summary>
    public void AtualizarRole(RoleUsuario novaRole) => Role = novaRole;
}