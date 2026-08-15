using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Repositories;

public interface IAtendimentoRepository : IRepository<Atendimento>
{
    IReadOnlyList<Atendimento> GetByPetId(Guid petId);
    IReadOnlyList<Atendimento> GetByVeterinarioId(Guid veterinarioId);
}