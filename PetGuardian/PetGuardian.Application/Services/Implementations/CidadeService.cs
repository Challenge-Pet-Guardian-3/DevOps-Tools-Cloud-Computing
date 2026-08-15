using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class CidadeService(
    IRepository<Cidade> cidadeRepository,
    IRepository<Estado> estadoRepository) : ICidadeService
{
    public IReadOnlyList<CidadeResponse> GetAll() =>
        cidadeRepository.GetAll().Select(CidadeResponse.FromDomain).ToList();

    public CidadeResponse? GetById(Guid id)
    {
        var cidade = cidadeRepository.GetById(id);
        return cidade is null ? null : CidadeResponse.FromDomain(cidade);
    }

    public IReadOnlyList<CidadeResponse> GetByEstadoId(Guid estadoId) =>
        cidadeRepository.GetAll()
            .Where(c => c.EstadoId == estadoId)
            .Select(CidadeResponse.FromDomain)
            .ToList();

    public CidadeResponse Create(CidadeRequest request)
    {
        if (!estadoRepository.ExistsById(request.EstadoId))
            throw new InvalidOperationException("Estado não encontrado.");

        var cidade = request.ToDomain();
        cidadeRepository.Add(cidade);
        return CidadeResponse.FromDomain(cidade);
    }

    public bool Delete(Guid id) => cidadeRepository.Delete(id);
}