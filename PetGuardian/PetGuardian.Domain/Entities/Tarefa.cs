using PetGuardian.Domain.Common;
using PetGuardian.Domain.Exceptions;

namespace PetGuardian.Domain.Entities;

/// <summary>
/// Tarefa de cuidado vinculada a um Pet e a um Veterinario.
/// Usuario responsável é nullable (tarefa pode ser criada sem usuário ainda atribuído).
/// </summary>
public sealed class Tarefa : BaseEntity
{
    public string    Titulo       { get; private set; } = string.Empty;
    public int       PontosTarefa { get; private set; }
    public string    Descricao    { get; private set; } = string.Empty;
    public DateTime  Criacao      { get; private set; }
    public DateTime  Prazo        { get; private set; }
    public DateTime? Conclusao    { get; private set; }

    // Nullable — tarefa pode existir sem usuário atribuído
    public Guid?    UsuarioId { get; private set; }
    public Usuario? Usuario   { get; private set; }

    public Guid         PetId         { get; private set; }
    public Pet?         Pet           { get; private set; }

    public Guid    StatusId { get; private set; }
    public Status? Status   { get; private set; }

    public Guid         VeterinarioId { get; private set; }
    public Veterinario? Veterinario   { get; private set; }

    private Tarefa() { }

    public Tarefa(
        string   titulo,
        int      pontosTarefa,
        string   descricao,
        DateTime prazo,
        Guid     petId,
        Guid     statusId,
        Guid     veterinarioId,
        Guid?    usuarioId = null)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("O título não pode ser vazio.");

        titulo = titulo.Trim();

        if (titulo.Length > 30)
            throw new DomainException("O título deve ter no máximo 30 caracteres.");

        if (pontosTarefa < 0)
            throw new DomainException("Os pontos não podem ser negativos.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("A descrição não pode ser vazia.");

        descricao = descricao.Trim();

        if (descricao.Length > 200)
            throw new DomainException("A descrição deve ter no máximo 200 caracteres.");

        if (prazo <= DateTime.UtcNow)
            throw new DomainException("O prazo deve ser uma data/hora futura.");

        if (petId == Guid.Empty)
            throw new DomainException("A tarefa deve estar associada a um pet válido.");

        if (statusId == Guid.Empty)
            throw new DomainException("O status da tarefa é obrigatório.");

        if (veterinarioId == Guid.Empty)
            throw new DomainException("O veterinário da tarefa é obrigatório.");

        Titulo        = titulo;
        PontosTarefa  = pontosTarefa;
        Descricao     = descricao;
        Criacao       = DateTime.UtcNow;
        Prazo         = prazo;
        Conclusao     = null;
        PetId         = petId;
        StatusId      = statusId;
        VeterinarioId = veterinarioId;
        UsuarioId     = usuarioId;
    }

    public void DefinirExecutorConclusao(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new DomainException("Id do usuário inválido.");

        UsuarioId = usuarioId;
    }

    public void AtualizarStatus(Guid statusId)
    {
        if (statusId == Guid.Empty)
            throw new DomainException("Id do status inválido.");

        StatusId = statusId;
    }

    public void Concluir()
    {
        if (Conclusao is not null)
            throw new DomainException("A tarefa já foi concluída.");

        Conclusao = DateTime.UtcNow;
    }

    public void EstenderPrazo(DateTime novoPrazo)
    {
        if (novoPrazo <= DateTime.UtcNow)
            throw new DomainException("O novo prazo deve ser futuro.");

        if (novoPrazo <= Prazo)
            throw new DomainException("O novo prazo deve ser posterior ao prazo atual.");

        Prazo = novoPrazo;
    }
}
