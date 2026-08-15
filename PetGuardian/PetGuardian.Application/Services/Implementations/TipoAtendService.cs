using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class TipoAtendService(IRepository<TipoAtend> tipoAtendRepository) : ITipoAtendService
{
    public IReadOnlyList<TipoAtendResponse> GetAll() =>
        tipoAtendRepository.GetAll().Select(TipoAtendResponse.FromDomain).ToList();

    public TipoAtendResponse? GetById(Guid id)
    {
        var tipo = tipoAtendRepository.GetById(id);
        return tipo is null ? null : TipoAtendResponse.FromDomain(tipo);
    }

    public TipoAtendResponse Create(TipoAtendRequest request)
    {
        var tipo = request.ToDomain();
        tipoAtendRepository.Add(tipo);
        return TipoAtendResponse.FromDomain(tipo);
    }

    public bool Delete(Guid id) => tipoAtendRepository.Delete(id);
}