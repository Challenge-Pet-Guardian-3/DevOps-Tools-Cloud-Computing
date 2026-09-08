package fiap.com.br.petguardian.trilha.modulo.dto;

import fiap.com.br.petguardian.trilha.Trilha;
import fiap.com.br.petguardian.trilha.modulo.Modulo;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Size;

public record ModuloRequest(
        @NotBlank
        @Size(max = 50)
        String nome,

        @NotBlank
        @Size(max = 10)
        String tempoConclusao,

        @NotBlank
        @Size(max = 100)
        String descricao,

        @NotNull
        Long trilhaId
) {
    public Modulo toEntity(Trilha trilha) {
        return Modulo.builder()
                .nome(nome)
                .tempoConclusao(tempoConclusao)
                .descricao(descricao)
                .trilha(trilha)
                .build();
    }

    public Modulo aplicarEm(Modulo modulo, Trilha trilha) {
        modulo.setNome(nome);
        modulo.setTempoConclusao(tempoConclusao);
        modulo.setDescricao(descricao);
        modulo.setTrilha(trilha);
        return modulo;
    }
}
