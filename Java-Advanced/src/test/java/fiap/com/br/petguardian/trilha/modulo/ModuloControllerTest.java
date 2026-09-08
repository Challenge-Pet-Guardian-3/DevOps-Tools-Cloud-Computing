package fiap.com.br.petguardian.trilha.modulo;

import com.fasterxml.jackson.databind.ObjectMapper;
import fiap.com.br.petguardian.trilha.Trilha;
import fiap.com.br.petguardian.trilha.modulo.dto.ModuloRequest;
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
class ModuloControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockitoBean
    private ModuloService moduloService;

    @Test
    @WithMockUser(roles = "PREMIUM")
    @DisplayName("GET /modulos/trilha/{trilhaId} - Deve listar modulos da trilha")
    void deveListarModulosPremium() throws Exception {
        Trilha trilha = Trilha.builder().id(1L).nome("Obediência").build();
        Modulo modulo = Modulo.builder().id(10L).nome("Primeiros Comandos").tempoConclusao("5").descricao("Desc").trilha(trilha).build();

        when(moduloService.findAllByTrilhaId(1L)).thenReturn(List.of(modulo));

        mockMvc.perform(get("/modulos/trilha/1"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$[0].nome").value("Primeiros Comandos"))
                .andExpect(jsonPath("$[0].tempoConclusao").value("5"));
    }

    @Test
    @WithMockUser(roles = "ADMIN")
    @DisplayName("POST /modulos - Deve criar modulo para trilha como ADMIN")
    void deveCriarModulo() throws Exception {
        var request = new ModuloRequest("Primeiros Comandos", "5", "Desc", 1L);
        Trilha trilha = Trilha.builder().id(1L).nome("Obediência").build();
        Modulo modulo = Modulo.builder().id(10L).nome("Primeiros Comandos").tempoConclusao("5").descricao("Desc").trilha(trilha).build();

        when(moduloService.create(any(ModuloRequest.class))).thenReturn(modulo);

        mockMvc.perform(post("/modulos")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.nome").value("Primeiros Comandos"));
    }

    @Test
    @WithMockUser(roles = "ADMIN")
    @DisplayName("DELETE /modulos/{id} - Deve deletar modulo como ADMIN")
    void deveDeletarModulo() throws Exception {
        mockMvc.perform(delete("/modulos/10"))
                .andExpect(status().isNoContent());

        verify(moduloService).delete(10L);
    }
}
