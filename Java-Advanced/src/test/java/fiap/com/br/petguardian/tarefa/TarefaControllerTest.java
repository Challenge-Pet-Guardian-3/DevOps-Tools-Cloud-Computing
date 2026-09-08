package fiap.com.br.petguardian.tarefa;

import com.fasterxml.jackson.databind.ObjectMapper;
import fiap.com.br.petguardian.pet.Pet;
import fiap.com.br.petguardian.tarefa.dto.TarefaConclusaoRequest;
import fiap.com.br.petguardian.tarefa.dto.TarefaRequest;
import fiap.com.br.petguardian.tarefa.status.EnumStatus;
import fiap.com.br.petguardian.tarefa.status.Status;
import fiap.com.br.petguardian.usuario.Usuario;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.webmvc.test.autoconfigure.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.data.domain.PageImpl;
import org.springframework.http.MediaType;
import org.springframework.security.test.context.support.WithMockUser;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;

import java.time.LocalDateTime;
import java.util.List;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

@SpringBootTest
@AutoConfigureMockMvc
class TarefaControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockitoBean
    private TarefaService tarefaService;

    private Tarefa criarTarefaMock() {
        Usuario usuario = Usuario.builder().id(1L).nome("Enzo").build();
        Pet pet = Pet.builder().id(10L).nome("Thor").build();
        Status status = Status.builder().id(1L).nomeStatus(EnumStatus.PENDENTE).build();

        return Tarefa.builder()
                .id(100L)
                .titulo("Ração matinal")
                .pontosTarefa(15)
                .descricao("Colocar 200g de ração")
                .criacao(LocalDateTime.now())
                .prazo(LocalDateTime.now().plusHours(4))
                .status(status)
                .usuario(usuario)
                .pet(pet)
                .build();
    }

    @Test
    @WithMockUser
    @DisplayName("GET /tarefas - Deve listar tarefas paginadas")
    void deveListarTarefas() throws Exception {
        Tarefa tarefa = criarTarefaMock();
        when(tarefaService.findAll(any())).thenReturn(new PageImpl<>(List.of(tarefa)));

        mockMvc.perform(get("/tarefas"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.content[0].id").value(100))
                .andExpect(jsonPath("$.content[0].titulo").value("Ração matinal"))
                .andExpect(jsonPath("$.content[0].status").value("PENDENTE"));
    }

    @Test
    @WithMockUser
    @DisplayName("GET /tarefas/by-usuario - Deve listar tarefas pendentes de um usuario")
    void deveListarTarefasPorUsuario() throws Exception {
        Tarefa tarefa = criarTarefaMock();
        when(tarefaService.findAllByUsuario(eq(1L), any(), any())).thenReturn(new PageImpl<>(List.of(tarefa)));

        mockMvc.perform(get("/tarefas/by-usuario").param("usuarioId", "1"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.content[0].titulo").value("Ração matinal"));
    }

    @Test
    @WithMockUser
    @DisplayName("POST /tarefas - Deve criar tarefa")
    void deveCriarTarefa() throws Exception {
        var request = new TarefaRequest("Ração matinal", 15, "Colocar 200g", LocalDateTime.now().plusHours(4), 1L, 10L, "PENDENTE", null);
        Tarefa tarefa = criarTarefaMock();

        when(tarefaService.create(any(TarefaRequest.class))).thenReturn(tarefa);

        mockMvc.perform(post("/tarefas")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.id").value(100))
                .andExpect(jsonPath("$.titulo").value("Ração matinal"));
    }

    @Test
    @WithMockUser
    @DisplayName("PATCH /tarefas/{id}/concluir - Deve concluir tarefa")
    void deveConcluirTarefa() throws Exception {
        var conclusaoReq = new TarefaConclusaoRequest(1L);
        Tarefa tarefa = criarTarefaMock();
        tarefa.setStatus(Status.builder().id(2L).nomeStatus(EnumStatus.CONCLUIDO).build());
        tarefa.setConclusao(LocalDateTime.now());

        when(tarefaService.concluir(eq(100L), any(TarefaConclusaoRequest.class))).thenReturn(tarefa);

        mockMvc.perform(patch("/tarefas/100/concluir")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(conclusaoReq)))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.status").value("CONCLUIDO"));
    }

    @Test
    @WithMockUser
    @DisplayName("PATCH /tarefas/{id}/desmarcar - Deve desmarcar tarefa")
    void deveDesmarcarTarefa() throws Exception {
        Tarefa tarefa = criarTarefaMock();

        when(tarefaService.desmarcar(100L, 1L)).thenReturn(tarefa);

        mockMvc.perform(patch("/tarefas/100/desmarcar").param("usuarioId", "1"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.status").value("PENDENTE"));
    }

    @Test
    @WithMockUser
    @DisplayName("GET /tarefas/by-usuario/pontos - Deve consultar total de pontos do usuario")
    void deveConsultarPontosUsuario() throws Exception {
        when(tarefaService.calcularPontosTotaisUsuario(1L)).thenReturn(85);

        mockMvc.perform(get("/tarefas/by-usuario/pontos").param("usuarioId", "1"))
                .andExpect(status().isOk())
                .andExpect(content().string("85"));
    }

    @Test
    @WithMockUser
    @DisplayName("DELETE /tarefas/{id} - Deve excluir tarefa")
    void deveDeletarTarefa() throws Exception {
        mockMvc.perform(delete("/tarefas/100"))
                .andExpect(status().isNoContent());

        verify(tarefaService).delete(100L);
    }
}
