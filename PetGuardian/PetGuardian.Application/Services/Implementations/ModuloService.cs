using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.Application.Services.Implementations;

public sealed class ModuloService(
    IModuloRepository moduloRepository,
    ITrilhaRepository trilhaRepository) : IModuloService
{
    public IReadOnlyList<ModuloResponse> GetAll() =>
        moduloRepository.GetAll().Select(ModuloResponse.FromDomain).ToList();

    public ModuloResponse? GetById(Guid id)
    {
        var modulo = moduloRepository.GetById(id);
        return modulo is null ? null : ModuloResponse.FromDomain(modulo);
    }

    public IReadOnlyList<ModuloResponse> GetByTrilhaId(Guid trilhaId) =>
        moduloRepository.GetByTrilhaId(trilhaId).Select(ModuloResponse.FromDomain).ToList();

    public ModuloResponse Create(ModuloRequest request)
    {
        if (!trilhaRepository.ExistsById(request.TrilhaId))
            throw new InvalidOperationException("Trilha não encontrada.");

        var modulo = request.ToDomain();
        moduloRepository.Add(modulo);
        return ModuloResponse.FromDomain(modulo);
    }

    public ModuloResponse? Update(Guid id, ModuloUpdateRequest request)
    {
        var modulo = moduloRepository.GetById(id);
        if (modulo is null) return null;

        modulo.Atualizar(request.Nome, request.TempoConclusao, request.Descricao);
        moduloRepository.Update(modulo);
        return ModuloResponse.FromDomain(modulo);
    }

    public bool Delete(Guid id) => moduloRepository.Delete(id);
}