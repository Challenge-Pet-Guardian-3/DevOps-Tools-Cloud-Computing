using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class EstadoService(IRepository<Estado> estadoRepository) : IEstadoService
{
    public IReadOnlyList<EstadoResponse> GetAll() =>
        estadoRepository.GetAll().Select(EstadoResponse.FromDomain).ToList();

    public EstadoResponse? GetById(Guid id)
    {
        var estado = estadoRepository.GetById(id);
        return estado is null ? null : EstadoResponse.FromDomain(estado);
    }

    public EstadoResponse Create(EstadoRequest request)
    {
        var estado = request.ToDomain();
        estadoRepository.Add(estado);
        return EstadoResponse.FromDomain(estado);
    }

    public bool Delete(Guid id) => estadoRepository.Delete(id);
}