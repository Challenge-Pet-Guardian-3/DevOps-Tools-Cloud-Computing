using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Repositories;

public interface ITarefaRepository : IRepository<Tarefa>
{
    IReadOnlyList<Tarefa> GetByPetId(Guid petId);
    IReadOnlyList<Tarefa> GetByUsuarioId(Guid usuarioId);
    IReadOnlyList<Tarefa> GetByVeterinarioId(Guid veterinarioId);
    IReadOnlyList<Tarefa> GetByStatusId(Guid statusId);
}