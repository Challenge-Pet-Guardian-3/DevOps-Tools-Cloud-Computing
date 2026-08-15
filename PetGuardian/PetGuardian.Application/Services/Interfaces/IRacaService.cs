using PetGuardian.Application.DTOs;

namespace PetGuardian.Application.Services.Interfaces;

public interface IRacaService
{
    IReadOnlyList<RacaResponse> GetAll();
    RacaResponse? GetById(Guid id);
    RacaResponse Create(RacaRequest request);
    bool Delete(Guid id);
}