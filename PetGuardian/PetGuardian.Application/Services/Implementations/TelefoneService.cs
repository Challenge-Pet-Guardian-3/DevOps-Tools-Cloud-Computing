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
    
    public TelefoneResponse? Update(Guid id, TelefoneRequest request)
    {
        var telefone = telefoneRepository.GetById(id);
        if (telefone is null) return null;

        telefone.Atualizar(request.NumDdd, request.NumTel);
        telefoneRepository.Update(telefone);
        return TelefoneResponse.FromDomain(telefone);
    }

    public bool Delete(Guid id) => telefoneRepository.Delete(id);
}