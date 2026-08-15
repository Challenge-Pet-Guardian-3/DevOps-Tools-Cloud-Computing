using PetGuardian.Application.DTOs;

namespace PetGuardian.Application.Services.Interfaces;

public interface ITelefoneService
{
    IReadOnlyList<TelefoneResponse> GetAll();
    TelefoneResponse? GetById(Guid id);
    TelefoneResponse Create(TelefoneRequest request);
    bool Delete(Guid id);
}