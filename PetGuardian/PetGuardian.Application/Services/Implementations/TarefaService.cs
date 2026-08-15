﻿using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class TarefaService(
    ITarefaRepository      tarefaRepository,
    IPetRepository         petRepository,
    IVeterinarioRepository veterinarioRepository,
    IRepository<Status>    statusRepository,
    IUsuarioRepository     usuarioRepository,
    IUsuarioPetRepository  usuarioPetRepository) : ITarefaService
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

    public IReadOnlyList<TarefaResponse> GetByVeterinarioId(Guid veterinarioId) =>
        tarefaRepository.GetByVeterinarioId(veterinarioId).Select(TarefaResponse.FromDomain).ToList();

    public IReadOnlyList<TarefaResponse> GetByStatusId(Guid statusId) =>
        tarefaRepository.GetByStatusId(statusId).Select(TarefaResponse.FromDomain).ToList();

    public TarefaResponse Create(TarefaRequest request)
    {
        if (!petRepository.ExistsById(request.PetId))
            throw new InvalidOperationException("Pet não encontrado.");

        if (!veterinarioRepository.ExistsById(request.VeterinarioId))
            throw new InvalidOperationException("Veterinário não encontrado.");

        var statusPendente = BuscarStatusObrigatorio("PENDENTE");

        var tarefa = request.ToDomain(statusPendente.Id);
        tarefaRepository.Add(tarefa);
        return TarefaResponse.FromDomain(tarefa);
    }

    public TarefaResponse Concluir(Guid tarefaId, Guid usuarioId)
    {
        if (usuarioId == Guid.Empty || !usuarioRepository.ExistsById(usuarioId))
            throw new InvalidOperationException("Usuário não encontrado.");

        var tarefa = tarefaRepository.GetById(tarefaId)
            ?? throw new InvalidOperationException("Tarefa não encontrada.");

        if (!usuarioPetRepository.Exists(usuarioId, tarefa.PetId))
            throw new InvalidOperationException("Somente cuidadores vinculados ao pet podem concluir a tarefa.");

        if (tarefa.Conclusao.HasValue)
            throw new InvalidOperationException("A tarefa já foi concluída.");

        if (tarefa.UsuarioId.HasValue && tarefa.UsuarioId.Value != usuarioId)
            throw new InvalidOperationException("A tarefa já está vinculada à conclusão por outro cuidador.");

        if (!tarefa.UsuarioId.HasValue)
            tarefa.DefinirExecutorConclusao(usuarioId);

        var statusConcluido = BuscarStatusObrigatorio("CONCLUIDO");
        tarefa.AtualizarStatus(statusConcluido.Id);
        tarefa.Concluir();

        tarefaRepository.Update(tarefa);
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
