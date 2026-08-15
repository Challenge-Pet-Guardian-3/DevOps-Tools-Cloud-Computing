using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class UsuarioEnderecoService(
    IUsuarioEnderecoRepository usuarioEnderecoRepository,
    IUsuarioRepository         usuarioRepository,
    IRepository<Endereco>      enderecoRepository) : IUsuarioEnderecoService
{
    public IReadOnlyList<UsuarioEnderecoResponse> GetAll() =>
        usuarioEnderecoRepository.GetAll().Select(UsuarioEnderecoResponse.FromDomain).ToList();

    public IReadOnlyList<UsuarioEnderecoResponse> GetByUsuarioId(Guid usuarioId) =>
        usuarioEnderecoRepository.GetByUsuarioId(usuarioId)
            .Select(UsuarioEnderecoResponse.FromDomain).ToList();

    public IReadOnlyList<UsuarioEnderecoResponse> GetByEnderecoId(Guid enderecoId) =>
        usuarioEnderecoRepository.GetByEnderecoId(enderecoId)
            .Select(UsuarioEnderecoResponse.FromDomain).ToList();

    public UsuarioEnderecoResponse Create(UsuarioEnderecoRequest request)
    {
        if (!usuarioRepository.ExistsById(request.UsuarioId))
            throw new InvalidOperationException("Usuário não encontrado.");

        if (!enderecoRepository.ExistsById(request.EnderecoId))
            throw new InvalidOperationException("Endereço não encontrado.");

        if (usuarioEnderecoRepository.Exists(request.UsuarioId, request.EnderecoId))
            throw new InvalidOperationException("Este endereço já está vinculado a este usuário.");

        var vinculo = request.ToDomain();
        usuarioEnderecoRepository.Add(vinculo);
        return UsuarioEnderecoResponse.FromDomain(vinculo);
    }

    public bool Delete(Guid usuarioId, Guid enderecoId) =>
        usuarioEnderecoRepository.Delete(usuarioId, enderecoId);
}