namespace PetGuardian.Application.DTOs;

public record PetHistoricoItemResponse(
    DateTime DataEvento,
    string TipoEvento,
    Guid ReferenciaId,
    string Titulo,
    string Descricao,
    Guid PetId,
    Guid? UsuarioExecutorId,
    Guid? VeterinarioId,
    decimal? ValorAtendimento,
    int? PontosTarefa
);
