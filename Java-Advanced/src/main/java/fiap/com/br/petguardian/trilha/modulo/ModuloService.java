package fiap.com.br.petguardian.trilha.modulo;

import fiap.com.br.petguardian.exception.ResourceNotFoundException;
import fiap.com.br.petguardian.trilha.Trilha;
import fiap.com.br.petguardian.trilha.TrilhaRepository;
import fiap.com.br.petguardian.trilha.modulo.dto.ModuloRequest;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
@RequiredArgsConstructor
public class ModuloService {

    private final ModuloRepository moduloRepository;
    private final TrilhaRepository trilhaRepository;

    @Transactional
    public Modulo create(ModuloRequest request) {
        Trilha trilha = findTrilhaById(request.trilhaId());
        return moduloRepository.save(request.toEntity(trilha));
    }

    public List<Modulo> findAllByTrilhaId(Long trilhaId) {
        findTrilhaById(trilhaId);
        return moduloRepository.findAllByTrilhaId(trilhaId);
    }

    public Modulo findById(Long id) {
        return findModuloById(id);
    }

    @Transactional
    public Modulo update(Long id, ModuloRequest request) {
        Modulo modulo = findModuloById(id);
        Trilha trilha = findTrilhaById(request.trilhaId());
        request.aplicarEm(modulo, trilha);
        return moduloRepository.save(modulo);
    }

    @Transactional
    public void delete(Long id) {
        findModuloById(id);
        moduloRepository.deleteById(id);
    }

    private Modulo findModuloById(Long id) {
        return moduloRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Modulo com id " + id + " nao encontrado."));
    }

    private Trilha findTrilhaById(Long trilhaId) {
        return trilhaRepository.findById(trilhaId)
                .orElseThrow(() -> new ResourceNotFoundException("Trilha com id " + trilhaId + " nao encontrada."));
    }
}
