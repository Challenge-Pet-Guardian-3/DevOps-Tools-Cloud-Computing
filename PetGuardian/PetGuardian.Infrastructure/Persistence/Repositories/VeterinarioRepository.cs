using Microsoft.EntityFrameworkCore;
using PetGuardian.Application.Repositories;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Infrastructure.Persistence.Repositories;

public sealed class VeterinarioRepository(PetGuardianContext context)
    : Repository<Veterinario>(context), IVeterinarioRepository
{
    public Veterinario? GetByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;
        var normalizado = email.Trim().ToLowerInvariant();
        return Context.Veterinarios.AsNoTracking()
            .FirstOrDefault(v => v.Email.ToLower() == normalizado);
    }

    public bool ExistsByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        var normalizado = email.Trim().ToLowerInvariant();
        return Context.Veterinarios.Any(v => v.Email.ToLower() == normalizado);
    }

    public IReadOnlyList<Veterinario> GetByClinicaId(Guid clinicaId) =>
        Context.Veterinarios.AsNoTracking()
            .Where(v => v.ClinicaId == clinicaId)
            .OrderBy(v => v.Nome)
            .ToList();
}