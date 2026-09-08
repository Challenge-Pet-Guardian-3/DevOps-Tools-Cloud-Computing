package fiap.com.br.petguardian.trilha.aula;

import fiap.com.br.petguardian.exception.ResourceNotFoundException;
import fiap.com.br.petguardian.trilha.aula.dto.AulaRequest;
import fiap.com.br.petguardian.trilha.modulo.Modulo;
import fiap.com.br.petguardian.trilha.modulo.ModuloRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
@RequiredArgsConstructor
public class AulaService {

    private final AulaRepository aulaRepository;
    private final ModuloRepository moduloRepository;

    @Transactional
    public Aula create(AulaRequest request) {
        Modulo modulo = findModuloById(request.moduloId());
        return aulaRepository.save(request.toEntity(modulo));
    }

    public List<Aula> findAllByModuloId(Long moduloId) {
        findModuloById(moduloId);
        return aulaRepository.findAllByModuloId(moduloId);
    }

    public Aula findById(Long id) {
        return findAulaById(id);
    }

    @Transactional
    public Aula update(Long id, AulaRequest request) {
        Aula aula = findAulaById(id);
        Modulo modulo = findModuloById(request.moduloId());
        request.aplicarEm(aula, modulo);
        return aulaRepository.save(aula);
    }

    @Transactional
    public Aula concluir(Long id) {
        Aula aula = findAulaById(id);
        aula.setConcluida(true);
        return aulaRepository.save(aula);
    }

    @Transactional
    public Aula desmarcar(Long id) {
        Aula aula = findAulaById(id);
        aula.setConcluida(false);
        return aulaRepository.save(aula);
    }

    @Transactional
    public void delete(Long id) {
        findAulaById(id);
        aulaRepository.deleteById(id);
    }

    private Aula findAulaById(Long id) {
        return aulaRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Aula com id " + id + " nao encontrada."));
    }

    private Modulo findModuloById(Long moduloId) {
        return moduloRepository.findById(moduloId)
                .orElseThrow(() -> new ResourceNotFoundException("Modulo com id " + moduloId + " nao encontrado."));
    }
}
