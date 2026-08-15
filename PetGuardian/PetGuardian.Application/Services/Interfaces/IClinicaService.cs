using PetGuardian.Application.DTOs;

namespace PetGuardian.Application.Services.Interfaces;

public interface IClinicaService
{
    IReadOnlyList<ClinicaResponse> GetAll();
    ClinicaResponse? GetById(Guid id);
    ClinicaResponse Create(ClinicaRequest request);
    bool Delete(Guid id);
}