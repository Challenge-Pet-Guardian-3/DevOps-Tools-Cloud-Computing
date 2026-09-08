package fiap.com.br.petguardian.tarefa.dto;

import fiap.com.br.petguardian.tarefa.Tarefa;
import fiap.com.br.petguardian.tarefa.status.Status;
import fiap.com.br.petguardian.usuario.Usuario;
import jakarta.validation.constraints.NotNull;

import java.time.LocalDateTime;

public record TarefaConclusaoRequest(
        @NotNull
        Long concluinteId
) {
    public void aplicarEm(Tarefa tarefa, Usuario concluinte, Status statusConcluido) {
        tarefa.setUsuario(concluinte);
        tarefa.setStatus(statusConcluido);
        tarefa.setConclusao(LocalDateTime.now());
    }
}
