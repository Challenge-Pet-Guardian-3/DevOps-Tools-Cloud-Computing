using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.Application.Services.Implementations;

public sealed class AulaService(
    IAulaRepository   aulaRepository,
    IModuloRepository moduloRepository) : IAulaService
{
    public IReadOnlyList<AulaResponse> GetAll() =>
        aulaRepository.GetAll().Select(AulaResponse.FromDomain).ToList();

    public AulaResponse? GetById(Guid id)
    {
        var aula = aulaRepository.GetById(id);
        return aula is null ? null : AulaResponse.FromDomain(aula);
    }

    public IReadOnlyList<AulaResponse> GetByModuloId(Guid moduloId) =>
        aulaRepository.GetByModuloId(moduloId).Select(AulaResponse.FromDomain).ToList();

    public AulaResponse Create(AulaRequest request)
    {
        if (!moduloRepository.ExistsById(request.ModuloId))
            throw new InvalidOperationException("Módulo não encontrado.");

        var aula = request.ToDomain();
        aulaRepository.Add(aula);
        return AulaResponse.FromDomain(aula);
    }

    public AulaResponse? Update(Guid id, AulaUpdateRequest request)
    {
        var aula = aulaRepository.GetById(id);
        if (aula is null) return null;

        aula.Atualizar(request.Nome, request.Descricao, request.PontosAula, request.Dificuldade, request.Conteudo, request.Concluida);
        aulaRepository.Update(aula);
        return AulaResponse.FromDomain(aula);
    }

    public bool Delete(Guid id) => aulaRepository.Delete(id);
}