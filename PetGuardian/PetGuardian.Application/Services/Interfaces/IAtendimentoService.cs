using PetGuardian.Application.DTOs;

namespace PetGuardian.Application.Services.Interfaces;

public interface IAtendimentoService
{
    IReadOnlyList<AtendimentoResponse> GetAll();
    AtendimentoResponse? GetById(Guid id);
    IReadOnlyList<AtendimentoResponse> GetByPetId(Guid petId);
    IReadOnlyList<AtendimentoResponse> GetByVeterinarioId(Guid veterinarioId);
    AtendimentoResponse Create(AtendimentoRequest request);
    bool Delete(Guid id);
}