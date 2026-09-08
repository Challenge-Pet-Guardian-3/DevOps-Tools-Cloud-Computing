package fiap.com.br.petguardian.trilha.aula;

import fiap.com.br.petguardian.trilha.aula.dto.AulaRequest;
import fiap.com.br.petguardian.trilha.aula.dto.AulaResponse;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/aulas")
@RequiredArgsConstructor
@Tag(name = "Aulas", description = "Gerenciamento de aulas e conteudos educativos das trilhas")
public class AulaController {

    private final AulaService aulaService;

    @GetMapping("/modulo/{moduloId}")
    @ResponseStatus(HttpStatus.OK)
    @Operation(summary = "Listar aulas associadas a um modulo")
    public List<AulaResponse> findByModuloId(@PathVariable Long moduloId) {
        return aulaService.findAllByModuloId(moduloId)
                .stream()
                .map(AulaResponse::fromEntity)
                .toList();
    }

    @GetMapping("/{id}")
    @ResponseStatus(HttpStatus.OK)
    @Operation(summary = "Buscar aula por ID")
    public AulaResponse findById(@PathVariable Long id) {
        return AulaResponse.fromEntity(aulaService.findById(id));
    }

    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    @Operation(summary = "Criar nova aula para um modulo")
    public AulaResponse create(@Valid @RequestBody AulaRequest request) {
        return AulaResponse.fromEntity(aulaService.create(request));
    }

    @PutMapping("/{id}")
    @ResponseStatus(HttpStatus.OK)
    @Operation(summary = "Atualizar aula")
    public AulaResponse update(@PathVariable Long id, @Valid @RequestBody AulaRequest request) {
        return AulaResponse.fromEntity(aulaService.update(id, request));
    }

    @PatchMapping("/{id}/concluir")
    @ResponseStatus(HttpStatus.OK)
    @Operation(summary = "Concluir aula")
    public AulaResponse concluir(@PathVariable Long id) {
        return AulaResponse.fromEntity(aulaService.concluir(id));
    }

    @PatchMapping("/{id}/desmarcar")
    @ResponseStatus(HttpStatus.OK)
    @Operation(summary = "Desmarcar aula concluida retornando ao estado nao concluido")
    public AulaResponse desmarcar(@PathVariable Long id) {
        return AulaResponse.fromEntity(aulaService.desmarcar(id));
    }

    @DeleteMapping("/{id}")
    @ResponseStatus(HttpStatus.NO_CONTENT)
    @Operation(summary = "Deletar aula")
    public void delete(@PathVariable Long id) {
        aulaService.delete(id);
    }
}
