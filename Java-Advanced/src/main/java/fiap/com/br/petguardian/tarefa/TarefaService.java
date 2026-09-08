package fiap.com.br.petguardian.tarefa;

import fiap.com.br.petguardian.exception.ResourceNotFoundException;
import fiap.com.br.petguardian.pet.Pet;
import fiap.com.br.petguardian.pet.PetRepository;
import fiap.com.br.petguardian.tarefa.status.EnumStatus;
import fiap.com.br.petguardian.tarefa.status.Status;
import fiap.com.br.petguardian.tarefa.status.StatusService;
import fiap.com.br.petguardian.tarefa.dto.TarefaConclusaoRequest;
import fiap.com.br.petguardian.tarefa.dto.TarefaRequest;
import fiap.com.br.petguardian.usuario.Usuario;
import fiap.com.br.petguardian.usuario.UsuarioRepository;
import fiap.com.br.petguardian.validation.TarefaValidator;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;

@Service
@RequiredArgsConstructor
public class TarefaService {
    private final TarefaRepository tarefaRepository;
    private final UsuarioRepository usuarioRepository;
    private final PetRepository petRepository;
    private final StatusService statusService;
    private final TarefaValidator tarefaValidator;

    public Page<Tarefa> findAll(Pageable pageable) {
        expirarTarefasPendentesAtrasadas();
        return tarefaRepository.findAll(pageable);
    }

    public Page<Tarefa> findAllByUsuario(Long usuarioId, String statusFiltro, Pageable pageable) {
        expirarTarefasPendentesAtrasadas();
        if ("ALL".equals(statusFiltro)) {
            return tarefaRepository.findAllDoCuidador(usuarioId, pageable);
        }
        return tarefaRepository.findAllDoCuidadorByStatus(usuarioId, EnumStatus.valueOf(statusFiltro), pageable);
    }

    public Page<Tarefa> findAllByPetId(Long petId, Pageable pageable) {
        expirarTarefasPendentesAtrasadas();
        findPetById(petId);
        return tarefaRepository.findAllByPetId(petId, pageable);
    }

    public Tarefa findById(Long id) {
        expirarTarefasPendentesAtrasadas();
        return findTarefaById(id);
    }

    public Tarefa findByUsuarioIdAndTarefaId(Long usuarioId, Long tarefaId) {
        expirarTarefasPendentesAtrasadas();
        return tarefaRepository.findByIdAndUsuarioId(tarefaId, usuarioId)
                .orElseThrow(() -> new ResourceNotFoundException("Tarefa com id " + tarefaId + " nao encontrada para o usuario informado."));
    }

    @Transactional
    public Tarefa create(TarefaRequest request) {
        Pet pet = findPetById(request.petId());
        Usuario usuario = findUsuarioById(request.usuarioId());
        tarefaValidator.validarCuidadorDoPet(usuario.getId(), pet.getId());

        Tarefa tarefa = request.toEntity(usuario, pet, LocalDateTime.now());
        tarefa.setStatus(statusService.findStatus(EnumStatus.PENDENTE));
        return tarefaRepository.save(tarefa);
    }

    @Transactional
    public Tarefa update(Long id, TarefaRequest request) {
        Tarefa tarefa = findTarefaById(id);
        Pet pet = findPetById(request.petId());
        Usuario usuario = findUsuarioById(request.usuarioId());
        tarefaValidator.validarCuidadorDoPet(usuario.getId(), pet.getId());
        EnumStatus status = EnumStatus.valueOf(request.status());

        LocalDateTime conclusao = definirConclusao(tarefa, status, request.conclusao(), LocalDateTime.now());
        request.aplicarEm(tarefa, usuario, pet, statusService.findStatus(status), conclusao);
        return tarefaRepository.save(tarefa);
    }

    @Transactional
    public Tarefa concluir(Long id, TarefaConclusaoRequest request) {
        Tarefa tarefa = findTarefaById(id);
        tarefaValidator.validarPendenteParaConclusao(tarefa);

        Usuario usuario = findUsuarioById(request.concluinteId());
        tarefaValidator.validarCuidadorDoPet(usuario.getId(), tarefa.getPet().getId());

        request.aplicarEm(tarefa, usuario, statusService.findStatus(EnumStatus.CONCLUIDO));
        return tarefaRepository.save(tarefa);
    }

    @Transactional
    public Tarefa desmarcar(Long id, Long usuarioId) {
        Tarefa tarefa = findTarefaById(id);
        Usuario usuario = findUsuarioById(usuarioId);
        tarefaValidator.validarCuidadorDoPet(usuario.getId(), tarefa.getPet().getId());
        tarefaValidator.validarConcluidaParaDesmarcar(tarefa);

        tarefa.setStatus(statusService.findStatus(EnumStatus.PENDENTE));
        tarefa.setConclusao(null);
        return tarefaRepository.save(tarefa);
    }

    @Transactional(readOnly = true)
    public Integer calcularPontosTotaisUsuario(Long usuarioId) {
        findUsuarioById(usuarioId);
        return tarefaRepository.calcularPontosTotaisUsuario(usuarioId, EnumStatus.CONCLUIDO);
    }

    @Transactional
    public void delete(Long id) {
        findTarefaById(id);
        tarefaRepository.deleteById(id);
    }

    private Tarefa findTarefaById(Long id) {
        return tarefaRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Tarefa com id " + id + " nao encontrada."));
    }

    private LocalDateTime definirConclusao(Tarefa tarefa, EnumStatus status, LocalDateTime conclusaoInformada, LocalDateTime agora) {
        if (status != EnumStatus.CONCLUIDO) return null;
        return conclusaoInformada != null ? conclusaoInformada : (tarefa.getConclusao() != null ? tarefa.getConclusao() : agora);
    }

    private Pet findPetById(Long id) {
        return petRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Pet com id " + id + " nao encontrado."));
    }

    private Usuario findUsuarioById(Long id) {
        return usuarioRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Usuario com id " + id + " nao encontrado."));
    }

    private void expirarTarefasPendentesAtrasadas() {
        Status pendente = statusService.findStatus(EnumStatus.PENDENTE);
        Status expirado = statusService.findStatus(EnumStatus.EXPIRADO);
        tarefaRepository.expirarTarefasPendentesAtrasadas(LocalDateTime.now(), pendente, expirado);
    }
}
