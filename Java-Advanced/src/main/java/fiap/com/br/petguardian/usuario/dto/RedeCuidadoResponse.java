package fiap.com.br.petguardian.usuario.dto;

import java.util.List;

public record RedeCuidadoResponse(
        Long usuarioId,
        String nomeUsuario,
        List<PetResumo> pets,
        List<CuidadorResumo> coCuidadores,
        int totalTarefasPendentes,
        int totalTarefasConcluidas,
        int pontosAcumulados
) {
    public record PetResumo(
            Long id,
            String nome,
            String raca,
            boolean responsavelPrincipal,
            List<Long> tarefaIds
    ) {}

    public record CuidadorResumo(
            Long id,
            String nome,
            String email,
            boolean responsavelPrincipal,
            List<Long> petIds,
            List<String> petNomes
    ) {}
}
