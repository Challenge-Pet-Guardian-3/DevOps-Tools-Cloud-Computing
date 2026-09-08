package fiap.com.br.petguardian.trilha;

import fiap.com.br.petguardian.exception.ResourceNotFoundException;
import fiap.com.br.petguardian.pet.Pet;
import fiap.com.br.petguardian.pet.PetRepository;
import fiap.com.br.petguardian.trilha.dto.TrilhaRequest;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
@RequiredArgsConstructor
public class TrilhaService {

    private final TrilhaRepository trilhaRepository;
    private final PetRepository petRepository;

    @Transactional
    public Trilha create(TrilhaRequest request) {
        Pet pet = findPetById(request.petId());
        return trilhaRepository.save(request.toEntity(pet));
    }

    public List<Trilha> findAllByPetId(Long petId) {
        findPetById(petId);
        return trilhaRepository.findAllByPetId(petId);
    }

    public Trilha findById(Long id) {
        return findTrilhaById(id);
    }

    @Transactional
    public Trilha update(Long id, TrilhaRequest request) {
        Trilha trilha = findTrilhaById(id);
        Pet pet = findPetById(request.petId());
        request.aplicarEm(trilha, pet);
        return trilhaRepository.save(trilha);
    }

    @Transactional
    public void delete(Long id) {
        findTrilhaById(id);
        trilhaRepository.deleteById(id);
    }

    private Trilha findTrilhaById(Long id) {
        return trilhaRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Trilha com id " + id + " nao encontrada."));
    }

    private Pet findPetById(Long petId) {
        return petRepository.findById(petId)
                .orElseThrow(() -> new ResourceNotFoundException("Pet com id " + petId + " nao encontrado."));
    }
}
