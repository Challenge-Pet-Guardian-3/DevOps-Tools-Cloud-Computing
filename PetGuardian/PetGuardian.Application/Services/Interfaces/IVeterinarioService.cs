using PetGuardian.Application.DTOs;

namespace PetGuardian.Application.Services.Interfaces;

public interface IVeterinarioService
{
    IReadOnlyList<VeterinarioResponse> GetAll();
    VeterinarioResponse? GetById(Guid id);
    VeterinarioResponse? GetByEmail(string email);
    IReadOnlyList<VeterinarioResponse> GetByClinicaId(Guid clinicaId);
    VeterinarioResponse Create(VeterinarioRequest request);
    bool Delete(Guid id);
}