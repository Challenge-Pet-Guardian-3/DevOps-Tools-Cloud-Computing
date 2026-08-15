using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Repositories;

public interface IVeterinarioRepository : IRepository<Veterinario>
{
    Veterinario? GetByEmail(string email);
    bool ExistsByEmail(string email);
    IReadOnlyList<Veterinario> GetByClinicaId(Guid clinicaId);
}