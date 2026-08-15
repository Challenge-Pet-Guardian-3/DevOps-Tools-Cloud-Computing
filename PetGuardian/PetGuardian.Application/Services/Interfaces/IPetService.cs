using PetGuardian.Application.DTOs;

namespace PetGuardian.Application.Services.Interfaces;

public interface IPetService
{
    IReadOnlyList<PetResponse> GetAll();
    PetResponse? GetById(Guid id);
    IReadOnlyList<PetResponse> GetByRacaId(Guid racaId);
    IReadOnlyList<PetHistoricoItemResponse> GetHistorico(Guid petId);
    PetResponse Create(PetRequest request);
    bool Delete(Guid id);
}
