using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class ClinicaService(
    IRepository<Clinica>  clinicaRepository,
    IRepository<Telefone> telefoneRepository,
    IRepository<Endereco> enderecoRepository) : IClinicaService
{
    public IReadOnlyList<ClinicaResponse> GetAll() =>
        clinicaRepository.GetAll().Select(ClinicaResponse.FromDomain).ToList();

    public ClinicaResponse? GetById(Guid id)
    {
        var c = clinicaRepository.GetById(id);
        return c is null ? null : ClinicaResponse.FromDomain(c);
    }

    public ClinicaResponse Create(ClinicaRequest request)
    {
        if (!telefoneRepository.ExistsById(request.TelefoneId))
            throw new InvalidOperationException("Telefone não encontrado.");

        if (!enderecoRepository.ExistsById(request.EnderecoId))
            throw new InvalidOperationException("Endereço não encontrado.");

        var clinica = request.ToDomain();
        clinicaRepository.Add(clinica);
        return ClinicaResponse.FromDomain(clinica);
    }

    public bool Delete(Guid id) => clinicaRepository.Delete(id);
}