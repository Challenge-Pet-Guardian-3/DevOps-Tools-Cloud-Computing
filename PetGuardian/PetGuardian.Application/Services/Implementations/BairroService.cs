using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class BairroService(
    IRepository<Bairro> bairroRepository,
    IRepository<Cidade> cidadeRepository) : IBairroService
{
    public IReadOnlyList<BairroResponse> GetAll() =>
        bairroRepository.GetAll().Select(BairroResponse.FromDomain).ToList();

    public BairroResponse? GetById(Guid id)
    {
        var bairro = bairroRepository.GetById(id);
        return bairro is null ? null : BairroResponse.FromDomain(bairro);
    }

    public IReadOnlyList<BairroResponse> GetByCidadeId(Guid cidadeId) =>
        bairroRepository.GetAll()
            .Where(b => b.CidadeId == cidadeId)
            .Select(BairroResponse.FromDomain)
            .ToList();

    public BairroResponse Create(BairroRequest request)
    {
        if (!cidadeRepository.ExistsById(request.CidadeId))
            throw new InvalidOperationException("Cidade não encontrada.");

        var bairro = request.ToDomain();
        bairroRepository.Add(bairro);
        return BairroResponse.FromDomain(bairro);
    }

    public bool Delete(Guid id) => bairroRepository.Delete(id);
}