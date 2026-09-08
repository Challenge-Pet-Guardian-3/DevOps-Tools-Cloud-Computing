package fiap.com.br.petguardian.trilha.aula;

import com.fasterxml.jackson.databind.ObjectMapper;
import fiap.com.br.petguardian.trilha.aula.dto.AulaRequest;
import fiap.com.br.petguardian.trilha.modulo.Modulo;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.webmvc.test.autoconfigure.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.MediaType;
import org.springframework.security.test.context.support.WithMockUser;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;

import java.util.List;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@SpringBootTest
@AutoConfigureMockMvc
class AulaControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockitoBean
    private AulaService aulaService;

    @Test
    @WithMockUser(roles = "PREMIUM")
    @DisplayName("GET /aulas/modulo/{moduloId} - Deve listar aulas do modulo")
    void deveListarAulasPremium() throws Exception {
        Modulo modulo = Modulo.builder().id(10L).nome("Módulo 1").build();
        Aula aula = Aula.builder().id(100L).nome("Comando Senta!").descricao("Desc").pontosAula(25).dificuldade("FACIL").conteudo("Passo a passo").concluida(false).modulo(modulo).build();

        when(aulaService.findAllByModuloId(10L)).thenReturn(List.of(aula));

        mockMvc.perform(get("/aulas/modulo/10"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$[0].nome").value("Comando Senta!"))
                .andExpect(jsonPath("$[0].pontosAula").value(25))
                .andExpect(jsonPath("$[0].concluida").value(false));
    }

    @Test
    @WithMockUser(roles = "ADMIN")
    @DisplayName("POST /aulas - Deve criar aula para modulo como ADMIN")
    void deveCriarAula() throws Exception {
        var request = new AulaRequest("Comando Senta!", "Desc", 25, "FACIL", "Passo a passo", false, 10L);
        Modulo modulo = Modulo.builder().id(10L).nome("Módulo 1").build();
        Aula aula = Aula.builder().id(100L).nome("Comando Senta!").descricao("Desc").pontosAula(25).dificuldade("FACIL").conteudo("Passo a passo").concluida(false).modulo(modulo).build();

        when(aulaService.create(any(AulaRequest.class))).thenReturn(aula);

        mockMvc.perform(post("/aulas")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.nome").value("Comando Senta!"));
    }

    @Test
    @WithMockUser(roles = "PREMIUM")
    @DisplayName("PATCH /aulas/{id}/concluir - Deve concluir aula como PREMIUM")
    void deveConcluirAulaPremium() throws Exception {
        Modulo modulo = Modulo.builder().id(10L).nome("Módulo 1").build();
        Aula aula = Aula.builder().id(100L).nome("Comando Senta!").descricao("Desc").pontosAula(25).dificuldade("FACIL").conteudo("Passo a passo").concluida(true).modulo(modulo).build();

        when(aulaService.concluir(100L)).thenReturn(aula);

        mockMvc.perform(patch("/aulas/100/concluir"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.concluida").value(true));
    }

    @Test
    @WithMockUser(roles = "ADMIN")
    @DisplayName("DELETE /aulas/{id} - Deve deletar aula como ADMIN")
    void deveDeletarAula() throws Exception {
        mockMvc.perform(delete("/aulas/100"))
                .andExpect(status().isNoContent());

        verify(aulaService).delete(100L);
    }
}
