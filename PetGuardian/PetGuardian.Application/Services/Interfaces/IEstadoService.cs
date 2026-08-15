using PetGuardian.Application.DTOs;

namespace PetGuardian.Application.Services.Interfaces;

public interface IEstadoService
{
    IReadOnlyList<EstadoResponse> GetAll();
    EstadoResponse? GetById(Guid id);
    EstadoResponse Create(EstadoRequest request);
    bool Delete(Guid id);
}