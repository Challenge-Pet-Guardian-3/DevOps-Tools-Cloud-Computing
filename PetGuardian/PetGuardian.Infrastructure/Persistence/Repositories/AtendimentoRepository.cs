using Microsoft.EntityFrameworkCore;
using PetGuardian.Application.Repositories;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Infrastructure.Persistence.Repositories;

public sealed class AtendimentoRepository(PetGuardianContext context)
    : Repository<Atendimento>(context), IAtendimentoRepository
{
    public IReadOnlyList<Atendimento> GetByPetId(Guid petId) =>
        Context.Atendimentos.AsNoTracking()
            .Where(a => a.PetId == petId).OrderByDescending(a => a.Data).ToList();

    public IReadOnlyList<Atendimento> GetByVeterinarioId(Guid veterinarioId) =>
        Context.Atendimentos.AsNoTracking()
            .Where(a => a.VeterinarioId == veterinarioId).OrderByDescending(a => a.Data).ToList();
}