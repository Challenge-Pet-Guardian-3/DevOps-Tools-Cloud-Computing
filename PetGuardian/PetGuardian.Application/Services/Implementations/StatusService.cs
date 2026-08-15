using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class StatusService(IRepository<Status> statusRepository) : IStatusService
{
    public IReadOnlyList<StatusResponse> GetAll() =>
        statusRepository.GetAll().Select(StatusResponse.FromDomain).ToList();

    public StatusResponse? GetById(Guid id)
    {
        var status = statusRepository.GetById(id);
        return status is null ? null : StatusResponse.FromDomain(status);
    }

    public StatusResponse Create(StatusRequest request)
    {
        var status = request.ToDomain();
        statusRepository.Add(status);
        return StatusResponse.FromDomain(status);
    }

    public bool Delete(Guid id) => statusRepository.Delete(id);
}