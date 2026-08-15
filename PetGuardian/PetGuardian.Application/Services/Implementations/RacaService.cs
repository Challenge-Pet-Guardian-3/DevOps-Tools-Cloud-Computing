using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class RacaService(IRepository<Raca> racaRepository) : IRacaService
{
    public IReadOnlyList<RacaResponse> GetAll() =>
        racaRepository.GetAll().Select(RacaResponse.FromDomain).ToList();

    public RacaResponse? GetById(Guid id)
    {
        var raca = racaRepository.GetById(id);
        return raca is null ? null : RacaResponse.FromDomain(raca);
    }

    public RacaResponse Create(RacaRequest request)
    {
        var raca = request.ToDomain();
        racaRepository.Add(raca);
        return RacaResponse.FromDomain(raca);
    }

    public bool Delete(Guid id) => racaRepository.Delete(id);
}