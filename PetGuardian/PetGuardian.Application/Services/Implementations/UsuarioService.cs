using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class UsuarioService(
    IUsuarioRepository    usuarioRepository,
    IRepository<Telefone> telefoneRepository,
    ITarefaRepository tarefaRepository) : IUsuarioService
{
    public IReadOnlyList<UsuarioResponse> GetAll() =>
        usuarioRepository.GetAll().Select(UsuarioResponse.FromDomain).ToList();

    public UsuarioResponse? GetById(Guid id)
    {
        var u = usuarioRepository.GetById(id);
        return u is null ? null : UsuarioResponse.FromDomain(u);
    }

    public UsuarioResponse? GetByEmail(string email)
    {
        var u = usuarioRepository.GetByEmail(email);
        return u is null ? null : UsuarioResponse.FromDomain(u);
    }

    public UsuarioScoreResponse GetScore(Guid usuarioId)
    {
        if (!usuarioRepository.ExistsById(usuarioId))
            throw new InvalidOperationException("Usuário não encontrado.");

        var pontosTotais = tarefaRepository.GetByUsuarioId(usuarioId)
            .Where(t => t.Conclusao.HasValue)
            .Sum(t => t.PontosTarefa);

        return new UsuarioScoreResponse(usuarioId, pontosTotais);
    }

    public UsuarioResponse Create(UsuarioRequest request)
    {
        if (usuarioRepository.ExistsByEmail(request.Email))
            throw new InvalidOperationException("Já existe um usuário com este e-mail.");

        if (!telefoneRepository.ExistsById(request.TelefoneId))
            throw new InvalidOperationException("Telefone não encontrado.");

        var usuario = request.ToDomain();
        usuarioRepository.Add(usuario);
        return UsuarioResponse.FromDomain(usuario);
    }

    public bool Delete(Guid id) => usuarioRepository.Delete(id);
}
