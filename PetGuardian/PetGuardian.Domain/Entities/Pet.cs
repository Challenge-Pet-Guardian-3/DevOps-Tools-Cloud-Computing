using PetGuardian.Domain.Common;
using PetGuardian.Domain.Enums;
using PetGuardian.Domain.Exceptions;

namespace PetGuardian.Domain.Entities;

/// <summary>
/// Pet cadastrado no sistema. Pertence a uma <see cref="Raca"/>.
/// Relacionamento N:N com <see cref="Usuario"/> via <see cref="UsuarioPet"/>.
/// </summary>
public sealed class Pet : BaseEntity
{
    public string   Nome     { get; private set; } = string.Empty;
    public int      Idade    { get; private set; }
    public SexoPet  Sexo     { get; private set; }
    public PortePet Porte    { get; private set; }
    public bool     Castrado { get; private set; }

    public Guid  RacaId { get; private set; }
    public Raca? Raca   { get; private set; }

    // N:N
    public List<UsuarioPet>  UsuariosPet  { get; private set; } = [];
    public List<Atendimento> Atendimentos { get; private set; } = [];
    public List<Tarefa>      Tarefas      { get; private set; } = [];

    private Pet() { }

    public Pet(string nome, int idade, SexoPet sexo, PortePet porte, bool castrado, Guid racaId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do pet não pode ser vazio.");

        if (nome.Trim().Length > 30)
            throw new DomainException("O nome do pet deve ter no máximo 30 caracteres.");

        if (idade is < 0 or > 99)
            throw new DomainException("A idade do pet deve estar entre 0 e 99 anos.");

        if (racaId == Guid.Empty)
            throw new DomainException("O pet deve estar associado a uma raça válida.");

        Nome     = nome.Trim();
        Idade    = idade;
        Sexo     = sexo;
        Porte    = porte;
        Castrado = castrado;
        RacaId   = racaId;
    }

    public void Castrar()
    {
        if (Castrado)
            throw new DomainException("O pet já foi castrado.");
        Castrado = true;
    }

    public void AtualizarIdade(int novaIdade)
    {
        if (novaIdade < Idade)
            throw new DomainException("A nova idade não pode ser menor que a atual.");
        if (novaIdade > 99)
            throw new DomainException("A idade deve ser no máximo 99 anos.");
        Idade = novaIdade;
    }
}