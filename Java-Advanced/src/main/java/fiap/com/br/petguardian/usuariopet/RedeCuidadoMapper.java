package fiap.com.br.petguardian.usuariopet;

import fiap.com.br.petguardian.usuario.Usuario;
import fiap.com.br.petguardian.usuario.dto.RedeCuidadoResponse;
import fiap.com.br.petguardian.usuario.dto.RedeCuidadoResponse.CuidadorResumo;
import fiap.com.br.petguardian.usuario.dto.RedeCuidadoResponse.PetResumo;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.stream.Collectors;

@Component
public class RedeCuidadoMapper {

    public RedeCuidadoResponse toEmptyResponse(Usuario usuario) {
        return new RedeCuidadoResponse(
                usuario.getId(),
                usuario.getNome(),
                List.of(),
                List.of(),
                0,
                0,
                0
        );
    }

    public List<PetResumo> toPetResumoList(
            List<UsuarioPet> vinculos,
            Map<Long, List<Long>> tarefasPorPet
    ) {
        return vinculos.stream()
                .map(vinculo -> {
                    Long petId = vinculo.getPet().getId();
                    List<Long> tarefaIds = tarefasPorPet.getOrDefault(petId, List.of());
                    return new PetResumo(
                            petId,
                            vinculo.getPet().getNome(),
                            vinculo.getPet().getRaca().getNome(),
                            vinculo.isResponsavelPrincipal(),
                            tarefaIds
                    );
                })
                .toList();
    }

    public List<CuidadorResumo> toCuidadorResumoList(
            List<UsuarioPet> todosVinculosDosPets,
            Long usuarioLogadoId
    ) {
        return toCuidadorResumoList(todosVinculosDosPets, usuarioLogadoId, List.of());
    }

    public List<CuidadorResumo> toCuidadorResumoList(
            List<UsuarioPet> todosVinculosDosPets,
            Long usuarioLogadoId,
            List<Long> petsOndeUsuarioEPrincipal
    ) {
        Map<Usuario, List<UsuarioPet>> vinculosPorCuidador = todosVinculosDosPets.stream()
                .filter(vinculo -> !Objects.equals(vinculo.getUsuario().getId(), usuarioLogadoId))
                .collect(Collectors.groupingBy(UsuarioPet::getUsuario));

        return vinculosPorCuidador.entrySet().stream()
                .map(entry -> {
                    Usuario cuidador = entry.getKey();
                    List<UsuarioPet> vinculosDoCuidador = entry.getValue();

                    boolean auxiliaEmPetsDoUsuario = vinculosDoCuidador.stream()
                            .anyMatch(v -> petsOndeUsuarioEPrincipal.contains(v.getPet().getId()));

                    boolean responsavelPrincipal = !auxiliaEmPetsDoUsuario && vinculosDoCuidador.stream()
                            .anyMatch(UsuarioPet::isResponsavelPrincipal);

                    List<Long> petIds = vinculosDoCuidador.stream().map(v -> v.getPet().getId()).toList();
                    List<String> petNomes = vinculosDoCuidador.stream().map(v -> v.getPet().getNome()).toList();

                    return new CuidadorResumo(
                            cuidador.getId(),
                            cuidador.getNome(),
                            cuidador.getEmail(),
                            responsavelPrincipal,
                            petIds,
                            petNomes
                    );
                })
                .toList();
    }
}
