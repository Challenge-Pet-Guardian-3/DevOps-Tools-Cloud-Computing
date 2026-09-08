package fiap.com.br.petguardian.trilha;

import com.fasterxml.jackson.databind.ObjectMapper;
import fiap.com.br.petguardian.pet.Pet;
import fiap.com.br.petguardian.trilha.dto.TrilhaRequest;
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
class TrilhaControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockitoBean
    private TrilhaService trilhaService;

    @Test
    @WithMockUser(roles = "PREMIUM")
    @DisplayName("GET /trilhas/pet/{petId} - Deve listar trilhas do pet para usuario PREMIUM")
    void deveListarTrilhasPremium() throws Exception {
        Pet pet = Pet.builder().id(10L).nome("Thor").build();
        Trilha trilha = Trilha.builder().id(1L).nome("Obediência").descricao("Comandos").pet(pet).build();

        when(trilhaService.findAllByPetId(10L)).thenReturn(List.of(trilha));

        mockMvc.perform(get("/trilhas/pet/10"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$[0].nome").value("Obediência"))
                .andExpect(jsonPath("$[0].petId").value(10));
    }

    @Test
    @WithMockUser(roles = "ADMIN")
    @DisplayName("POST /trilhas - Deve criar trilha para usuario ADMIN")
    void deveCriarTrilha() throws Exception {
        var request = new TrilhaRequest("Obediência", "Comandos básicos", 10L);
        Pet pet = Pet.builder().id(10L).nome("Thor").build();
        Trilha trilha = Trilha.builder().id(1L).nome("Obediência").descricao("Comandos básicos").pet(pet).build();

        when(trilhaService.create(any(TrilhaRequest.class))).thenReturn(trilha);

        mockMvc.perform(post("/trilhas")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.nome").value("Obediência"));
    }

    @Test
    @WithMockUser(roles = "ADMIN")
    @DisplayName("DELETE /trilhas/{id} - Deve deletar trilha para usuario ADMIN")
    void deveDeletarTrilha() throws Exception {
        mockMvc.perform(delete("/trilhas/1"))
                .andExpect(status().isNoContent());

        verify(trilhaService).delete(1L);
    }
}
