using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

/// <summary>
/// IAtendimentoRepository removido (Atendimento não existe mais); GetHistorico agora combina
/// Historico + Tarefas concluídas. Adicionado Update.
/// </summary>
public sealed class PetService(
    IPetRepository       petRepository,
    IRepository<Raca>    racaRepository,
    ITarefaRepository    tarefaRepository,
    IHistoricoRepository historicoRepository) : IPetService
{
    public IReadOnlyList<PetResponse> GetAll() =>
        petRepository.GetAll().Select(PetResponse.FromDomain).ToList();

    public PetResponse? GetById(Guid id)
    {
        var pet = petRepository.GetById(id);
        return pet is null ? null : PetResponse.FromDomain(pet);
    }

    public IReadOnlyList<PetResponse> GetByRacaId(Guid racaId) =>
        petRepository.GetAll()
            .Where(p => p.RacaId == racaId)
            .Select(PetResponse.FromDomain)
            .ToList();

    public IReadOnlyList<PetHistoricoItemResponse> GetHistorico(Guid petId)
    {
        if (!petRepository.ExistsById(petId))
            throw new InvalidOperationException("Pet não encontrado.");

        var historico = new List<PetHistoricoItemResponse>();

        historico.AddRange(historicoRepository.GetByPetId(petId)
            .Select(h => new PetHistoricoItemResponse(
                h.DataHist,
                h.TipoHist,
                h.Id,
                h.TipoHist,
                null,
                h.PetId,
                null,
                null)));

        historico.AddRange(tarefaRepository.GetByPetId(petId)
            .Where(t => t.Conclusao.HasValue)
            .Select(t => new PetHistoricoItemResponse(
                t.Conclusao!.Value,
                "TAREFA_CONCLUIDA",
                t.Id,
                t.Titulo,
                t.Descricao,
                t.PetId,
                t.UsuarioId,
                t.PontosTarefa)));

        return historico
            .OrderByDescending(i => i.DataEvento)
            .ToList();
    }

    public PetResponse Create(PetRequest request)
    {
        if (!racaRepository.ExistsById(request.RacaId))
            throw new InvalidOperationException("Raça não encontrada.");

        var pet = request.ToDomain();
        petRepository.Add(pet);
        return PetResponse.FromDomain(pet);
    }
    
    public PetResponse? Update(Guid id, PetRequest request)
    {
        var pet = petRepository.GetById(id);
        if (pet is null) return null;

        if (!racaRepository.ExistsById(request.RacaId))
            throw new InvalidOperationException("Raça não encontrada.");

        pet.Atualizar(request.Nome, request.DataNascimento, request.Sexo, request.Porte, request.Castrado, request.RacaId);
        petRepository.Update(pet);
        return PetResponse.FromDomain(pet);
    }

    public bool Delete(Guid id) => petRepository.Delete(id);
}
