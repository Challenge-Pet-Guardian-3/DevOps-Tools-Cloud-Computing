using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class VeterinarioService(
    IVeterinarioRepository veterinarioRepository,
    IRepository<Telefone>  telefoneRepository,
    IRepository<Clinica>   clinicaRepository) : IVeterinarioService
{
    public IReadOnlyList<VeterinarioResponse> GetAll() =>
        veterinarioRepository.GetAll().Select(VeterinarioResponse.FromDomain).ToList();

    public VeterinarioResponse? GetById(Guid id)
    {
        var v = veterinarioRepository.GetById(id);
        return v is null ? null : VeterinarioResponse.FromDomain(v);
    }

    public VeterinarioResponse? GetByEmail(string email)
    {
        var v = veterinarioRepository.GetByEmail(email);
        return v is null ? null : VeterinarioResponse.FromDomain(v);
    }

    public IReadOnlyList<VeterinarioResponse> GetByClinicaId(Guid clinicaId) =>
        veterinarioRepository.GetByClinicaId(clinicaId)
            .Select(VeterinarioResponse.FromDomain).ToList();

    public VeterinarioResponse Create(VeterinarioRequest request)
    {
        if (veterinarioRepository.ExistsByEmail(request.Email))
            throw new InvalidOperationException("Já existe um veterinário com este e-mail.");

        if (!telefoneRepository.ExistsById(request.TelefoneId))
            throw new InvalidOperationException("Telefone não encontrado.");

        if (request.ClinicaId.HasValue && !clinicaRepository.ExistsById(request.ClinicaId.Value))
            throw new InvalidOperationException("Clínica não encontrada.");

        var veterinario = request.ToDomain();
        veterinarioRepository.Add(veterinario);
        return VeterinarioResponse.FromDomain(veterinario);
    }

    public bool Delete(Guid id) => veterinarioRepository.Delete(id);
}