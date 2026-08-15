using PetGuardian.Application.DTOs;

namespace PetGuardian.Application.Services.Interfaces;

public interface IUsuarioService
{
    IReadOnlyList<UsuarioResponse> GetAll();
    UsuarioResponse? GetById(Guid id);
    UsuarioResponse? GetByEmail(string email);
    UsuarioScoreResponse GetScore(Guid usuarioId);
    UsuarioResponse Create(UsuarioRequest request);
    bool Delete(Guid id);
}
