using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.Application.Services.Implementations;

public sealed class HistoricoService(
    IHistoricoRepository historicoRepository,
    IPetRepository       petRepository) : IHistoricoService
{
    public IReadOnlyList<HistoricoResponse> GetAll() =>
        historicoRepository.GetAll().Select(HistoricoResponse.FromDomain).ToList();

    public HistoricoResponse? GetById(Guid id)
    {
        var h = historicoRepository.GetById(id);
        return h is null ? null : HistoricoResponse.FromDomain(h);
    }

    public IReadOnlyList<HistoricoResponse> GetByPetId(Guid petId) =>
        historicoRepository.GetByPetId(petId).Select(HistoricoResponse.FromDomain).ToList();

    public HistoricoResponse Create(HistoricoRequest request)
    {
        if (!petRepository.ExistsById(request.PetId))
            throw new InvalidOperationException("Pet não encontrado.");

        var historico = request.ToDomain();
        historicoRepository.Add(historico);
        return HistoricoResponse.FromDomain(historico);
    }

    public HistoricoResponse? Update(Guid id, HistoricoUpdateRequest request)
    {
        var historico = historicoRepository.GetById(id);
        if (historico is null) return null;

        historico.Atualizar(request.TipoHist, request.DataHist);
        historicoRepository.Update(historico);
        return HistoricoResponse.FromDomain(historico);
    }

    public bool Delete(Guid id) => historicoRepository.Delete(id);
}