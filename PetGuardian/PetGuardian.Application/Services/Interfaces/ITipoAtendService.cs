using PetGuardian.Application.DTOs;

namespace PetGuardian.Application.Services.Interfaces;

public interface ITipoAtendService
{
    IReadOnlyList<TipoAtendResponse> GetAll();
    TipoAtendResponse? GetById(Guid id);
    TipoAtendResponse Create(TipoAtendRequest request);
    bool Delete(Guid id);
}