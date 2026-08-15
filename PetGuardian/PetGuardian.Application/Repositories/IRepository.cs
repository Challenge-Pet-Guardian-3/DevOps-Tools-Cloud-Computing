using PetGuardian.Domain.Common;

namespace PetGuardian.Application.Repositories;

/// <summary>
/// Contrato genérico de persistência para entidades que derivam de <see cref="BaseEntity"/>.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio.</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    IReadOnlyList<T> GetAll();

    T? GetById(Guid id);

    T Add(T entity);
    
    T Update(T entity);

    bool Delete(Guid id);

    bool ExistsById(Guid id);

    /// <summary>
    /// Verifica existência pelo campo <c>Nome</c>. Lança <see cref="InvalidOperationException"/>
    /// se a entidade não possuir essa propriedade mapeada.
    /// </summary>
    bool ExistsByNome(string valor);

}
