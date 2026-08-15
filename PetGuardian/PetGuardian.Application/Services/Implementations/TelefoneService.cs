using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class TelefoneService(IRepository<Telefone> telefoneRepository) : ITelefoneService
{
    public IReadOnlyList<TelefoneResponse> GetAll() =>
        telefoneRepository.GetAll().Select(TelefoneResponse.FromDomain).ToList();

    public TelefoneResponse? GetById(Guid id)
    {
        var telefone = telefoneRepository.GetById(id);
        return telefone is null ? null : TelefoneResponse.FromDomain(telefone);
    }

    public TelefoneResponse Create(TelefoneRequest request)
    {
        var telefone = request.ToDomain();
        telefoneRepository.Add(telefone);
        return TelefoneResponse.FromDomain(telefone);
    }

    public bool Delete(Guid id) => telefoneRepository.Delete(id);
}