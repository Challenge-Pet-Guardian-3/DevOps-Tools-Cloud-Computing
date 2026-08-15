using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Repositories;

/// <summary>Repositório dedicado para a entidade de junção <see cref="UsuarioPet"/> (chave composta).</summary>
public interface IUsuarioPetRepository
{
    IReadOnlyList<UsuarioPet> GetAll();
    IReadOnlyList<UsuarioPet> GetByUsuarioId(Guid usuarioId);
    IReadOnlyList<UsuarioPet> GetByPetId(Guid petId);
    UsuarioPet? GetByUsuarioAndPet(Guid usuarioId, Guid petId);
    UsuarioPet Add(UsuarioPet entity);
    bool Delete(Guid usuarioId, Guid petId);
    bool Exists(Guid usuarioId, Guid petId);
}