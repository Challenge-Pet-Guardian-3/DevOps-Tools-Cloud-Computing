using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

/// <summary>
/// IVeterinarioRepository removido; UsuarioId agora é obrigatório já na criação
/// (era atribuído somente na conclusão). Adicionado Update. A conclusão agora também grava
/// um Historico do pet ("TAREFA_CONCLUIDA").
/// </summary>
public sealed class TarefaService(
    ITarefaRepository     tarefaRepository,
    IPetRepository        petRepository,
    IRepository<Status>   statusRepository,
    IUsuarioRepository    usuarioRepository,
    IUsuarioPetRepository usuarioPetRepository,
    IHistoricoRepository  historicoRepository) : ITarefaService
{
    public IReadOnlyList<TarefaResponse> GetAll() =>
        tarefaRepository.GetAll().Select(TarefaResponse.FromDomain).ToList();

    public TarefaResponse? GetById(Guid id)
    {
        var t = tarefaRepository.GetById(id);
        return t is null ? null : TarefaResponse.FromDomain(t);
    }

    public IReadOnlyList<TarefaResponse> GetByPetId(Guid petId) =>
        tarefaRepository.GetByPetId(petId).Select(TarefaResponse.FromDomain).ToList();

    public IReadOnlyList<TarefaResponse> GetByUsuarioId(Guid usuarioId) =>
        tarefaRepository.GetByUsuarioId(usuarioId).Select(TarefaResponse.FromDomain).ToList();

    public IReadOnlyList<TarefaResponse> GetByStatusId(Guid statusId) =>
        tarefaRepository.GetByStatusId(statusId).Select(TarefaResponse.FromDomain).ToList();

    public TarefaResponse Create(TarefaRequest request)
    {
        if (!petRepository.ExistsById(request.PetId))
            throw new InvalidOperationException("Pet não encontrado.");
        if (!usuarioRepository.ExistsById(request.UsuarioId))
            throw new InvalidOperationException("Usuário não encontrado.");
        if (!usuarioPetRepository.Exists(request.UsuarioId, request.PetId))
            throw new InvalidOperationException("Somente cuidadores vinculados ao pet podem receber tarefas.");

        var statusPendente = BuscarStatusObrigatorio("PENDENTE");
        var tarefa = request.ToDomain(statusPendente.Id);
        tarefaRepository.Add(tarefa);
        return TarefaResponse.FromDomain(tarefa);
    }

    /// <summary>Pet/Usuario da tarefa não são reatribuíveis; tarefa concluída não pode ser editada.</summary>
    public TarefaResponse? Update(Guid id, TarefaUpdateRequest request)
    {
        var tarefa = tarefaRepository.GetById(id);
        if (tarefa is null) return null;

        tarefa.Atualizar(request.Titulo, request.PontosTarefa, request.Descricao, request.Prazo);
        tarefaRepository.Update(tarefa);
        return TarefaResponse.FromDomain(tarefa);
    }

    public TarefaResponse Concluir(Guid tarefaId, Guid usuarioId)
    {
        var tarefa = tarefaRepository.GetById(tarefaId)
            ?? throw new InvalidOperationException("Tarefa não encontrada.");

        if (!usuarioPetRepository.Exists(usuarioId, tarefa.PetId))
            throw new InvalidOperationException("Somente cuidadores vinculados ao pet podem concluir a tarefa.");

        if (tarefa.Conclusao.HasValue)
            throw new InvalidOperationException("A tarefa já foi concluída.");

        var statusConcluido = BuscarStatusObrigatorio("CONCLUIDO");
        tarefa.AtualizarStatus(statusConcluido.Id);
        tarefa.Concluir();
        tarefaRepository.Update(tarefa);

        historicoRepository.Add(Historico.Registrar("TAREFA_CONCLUIDA", tarefa.PetId));

        return TarefaResponse.FromDomain(tarefa);
    }

    public bool Delete(Guid id) => tarefaRepository.Delete(id);

    private Status BuscarStatusObrigatorio(string nomeStatus)
    {
        return statusRepository.GetAll()
            .FirstOrDefault(s => s.NomeStatus.Equals(nomeStatus, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Status obrigatório não encontrado: {nomeStatus}.");
    }
}
