using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class AtendimentoService(
    IAtendimentoRepository  atendimentoRepository,
    IPetRepository          petRepository,
    IVeterinarioRepository  veterinarioRepository,
    IRepository<TipoAtend>  tipoAtendRepository,
    IRepository<Status>     statusRepository) : IAtendimentoService
{
    public IReadOnlyList<AtendimentoResponse> GetAll() =>
        atendimentoRepository.GetAll().Select(AtendimentoResponse.FromDomain).ToList();

    public AtendimentoResponse? GetById(Guid id)
    {
        var a = atendimentoRepository.GetById(id);
        return a is null ? null : AtendimentoResponse.FromDomain(a);
    }

    public IReadOnlyList<AtendimentoResponse> GetByPetId(Guid petId) =>
        atendimentoRepository.GetByPetId(petId).Select(AtendimentoResponse.FromDomain).ToList();

    public IReadOnlyList<AtendimentoResponse> GetByVeterinarioId(Guid veterinarioId) =>
        atendimentoRepository.GetByVeterinarioId(veterinarioId)
            .Select(AtendimentoResponse.FromDomain).ToList();

    public AtendimentoResponse Create(AtendimentoRequest request)
    {
        if (!petRepository.ExistsById(request.PetId))
            throw new InvalidOperationException("Pet não encontrado.");

        if (!veterinarioRepository.ExistsById(request.VeterinarioId))
            throw new InvalidOperationException("Veterinário não encontrado.");

        if (!tipoAtendRepository.ExistsById(request.TipoAtendId))
            throw new InvalidOperationException("Tipo de atendimento não encontrado.");

        if (!statusRepository.ExistsById(request.StatusId))
            throw new InvalidOperationException("Status não encontrado.");

        var atendimento = request.ToDomain();
        atendimentoRepository.Add(atendimento);
        return AtendimentoResponse.FromDomain(atendimento);
    }

    public bool Delete(Guid id) => atendimentoRepository.Delete(id);
}