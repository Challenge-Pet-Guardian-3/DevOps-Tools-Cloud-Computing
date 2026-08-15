namespace PetGuardian.Application.DTOs;

public record RedeCuidadoResponse(
    Guid UsuarioId,
    IReadOnlyList<RedeCuidadoPetResponse> Pets,
    IReadOnlyList<RedeCuidadoCoCuidadorResponse> CoCuidadores
);

public record RedeCuidadoPetResponse(
    Guid PetId,
    string Nome,
    IReadOnlyList<RedeCuidadoTarefaResponse> Tarefas,
    IReadOnlyList<RedeCuidadoAtendimentoResponse> Atendimentos
);

public record RedeCuidadoTarefaResponse(
    Guid TarefaId,
    string Titulo,
    DateTime Prazo,
    DateTime? Conclusao,
    Guid? UsuarioExecutorId,
    Guid StatusId,
    int PontosTarefa
);

public record RedeCuidadoAtendimentoResponse(
    Guid AtendimentoId,
    DateTime Data,
    string Anotacoes,
    decimal Valor,
    Guid StatusId,
    Guid VeterinarioId
);

public record RedeCuidadoCoCuidadorResponse(
    Guid UsuarioId,
    string Nome,
    string Email
);
