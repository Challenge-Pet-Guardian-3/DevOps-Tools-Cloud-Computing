package fiap.com.br.petguardian.pet.historico;

import com.fasterxml.jackson.databind.ObjectMapper;
import fiap.com.br.petguardian.pet.Pet;
import fiap.com.br.petguardian.pet.historico.dto.HistoricoRequest;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.webmvc.test.autoconfigure.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.MediaType;
import org.springframework.security.test.context.support.WithMockUser;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;

import java.time.LocalDateTime;
import java.util.List;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@SpringBootTest
@AutoConfigureMockMvc
class HistoricoControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockitoBean
    private HistoricoService historicoService;

    @Test
    @WithMockUser
    @DisplayName("GET /historicos/pet/{petId} - Deve listar eventos de histórico do pet")
    void deveListarHistoricoPet() throws Exception {
        Pet pet = Pet.builder().id(10L).nome("Thor").build();
        Historico hist = Historico.builder().id(1L).tipoHist("Vacina Raiva").dataHist(LocalDateTime.now()).pet(pet).build();

        when(historicoService.findAllByPetId(10L)).thenReturn(List.of(hist));

        mockMvc.perform(get("/historicos/pet/10"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$[0].tipoHist").value("Vacina Raiva"))
                .andExpect(jsonPath("$[0].nomePet").value("Thor"));
    }

    @Test
    @WithMockUser
    @DisplayName("POST /historicos - Deve criar evento de histórico")
    void deveCriarHistorico() throws Exception {
        var request = new HistoricoRequest("Exame de Sangue", LocalDateTime.now(), 10L);
        Pet pet = Pet.builder().id(10L).nome("Thor").build();
        Historico hist = Historico.builder().id(1L).tipoHist("Exame de Sangue").dataHist(LocalDateTime.now()).pet(pet).build();

        when(historicoService.create(any(HistoricoRequest.class))).thenReturn(hist);

        mockMvc.perform(post("/historicos")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.tipoHist").value("Exame de Sangue"));
    }

    @Test
    @WithMockUser
    @DisplayName("DELETE /historicos/{id} - Deve remover registro de histórico")
    void deveDeletarHistorico() throws Exception {
        mockMvc.perform(delete("/historicos/1"))
                .andExpect(status().isNoContent());

        verify(historicoService).delete(1L);
    }
}
