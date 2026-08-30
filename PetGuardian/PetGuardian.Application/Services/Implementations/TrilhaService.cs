using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.Application.Services.Implementations;

public sealed class TrilhaService(
    ITrilhaRepository trilhaRepository,
    IPetRepository    petRepository) : ITrilhaService
{
    public IReadOnlyList<TrilhaResponse> GetAll() =>
        trilhaRepository.GetAll().Select(TrilhaResponse.FromDomain).ToList();

    public TrilhaResponse? GetById(Guid id)
    {
        var trilha = trilhaRepository.GetById(id);
        return trilha is null ? null : TrilhaResponse.FromDomain(trilha);
    }

    public IReadOnlyList<TrilhaResponse> GetByPetId(Guid petId) =>
        trilhaRepository.GetByPetId(petId).Select(TrilhaResponse.FromDomain).ToList();

    public TrilhaResponse Create(TrilhaRequest request)
    {
        if (!petRepository.ExistsById(request.PetId))
            throw new InvalidOperationException("Pet não encontrado.");

        var trilha = request.ToDomain();
        trilhaRepository.Add(trilha);
        return TrilhaResponse.FromDomain(trilha);
    }

    public TrilhaResponse? Update(Guid id, TrilhaUpdateRequest request)
    {
        var trilha = trilhaRepository.GetById(id);
        if (trilha is null) return null;

        trilha.Atualizar(request.Nome, request.Descricao);
        trilhaRepository.Update(trilha);
        return TrilhaResponse.FromDomain(trilha);
    }

    public bool Delete(Guid id) => trilhaRepository.Delete(id);
}