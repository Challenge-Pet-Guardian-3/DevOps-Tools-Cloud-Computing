package fiap.com.br.petguardian.pet;

import com.fasterxml.jackson.databind.ObjectMapper;
import fiap.com.br.petguardian.pet.dto.PetHistoryResponse;
import fiap.com.br.petguardian.pet.dto.PetPontuacaoResponse;
import fiap.com.br.petguardian.pet.dto.PetRequest;
import fiap.com.br.petguardian.pet.raca.Raca;
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

import java.time.LocalDate;
import java.util.List;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@SpringBootTest
@AutoConfigureMockMvc
class PetControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockitoBean
    private PetService petService;

    private Pet criarPetMock() {
        Raca raca = Raca.builder().id(1L).nome("Golden Retriever").build();
        return Pet.builder()
                .id(1L)
                .nome("Thor")
                .dataNasc(LocalDate.now().minusYears(2))
                .raca(raca)
                .porte(PetPorte.GRANDE)
                .sexo('M')
                .castrado(true)
                .build();
    }

    @Test
    @WithMockUser
    @DisplayName("GET /pets - Deve listar pets com paginação")
    void deveListarPets() throws Exception {
        Pet pet = criarPetMock();
        when(petService.findAll(any())).thenReturn(new PageImpl<>(List.of(pet)));

        mockMvc.perform(get("/pets"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.content[0].nome").value("Thor"))
                .andExpect(jsonPath("$.content[0].raca").value("Golden Retriever"));
    }

    @Test
    @WithMockUser
    @DisplayName("GET /pets/{id} - Deve retornar pet por ID")
    void deveBuscarPetPorId() throws Exception {
        Pet pet = criarPetMock();
        when(petService.findById(1L)).thenReturn(pet);

        mockMvc.perform(get("/pets/1"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id").value(1))
                .andExpect(jsonPath("$.nome").value("Thor"));
    }

    @Test
    @WithMockUser
    @DisplayName("GET /pets/{id}/historico - Deve retornar histórico consolidado")
    void deveRetornarHistorico() throws Exception {
        var historico = new PetHistoryResponse(1L, "Thor", List.of());
        when(petService.getConsolidatedHistory(1L)).thenReturn(historico);

        mockMvc.perform(get("/pets/1/historico"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.petId").value(1))
                .andExpect(jsonPath("$.nomePet").value("Thor"));
    }

    @Test
    @WithMockUser
    @DisplayName("GET /pets/{id}/pontos - Deve retornar pontuação consolidada")
    void deveRetornarPontos() throws Exception {
        var pontos = new PetPontuacaoResponse(1L, "Thor", 45, 30, 75);
        when(petService.calcularPontuacaoTotalPet(1L)).thenReturn(pontos);

        mockMvc.perform(get("/pets/1/pontos"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.pontosTarefas").value(45))
                .andExpect(jsonPath("$.pontosAulas").value(30))
                .andExpect(jsonPath("$.pontosTotais").value(75));
    }

    @Test
    @WithMockUser
    @DisplayName("POST /pets - Deve criar novo pet")
    void deveCriarPet() throws Exception {
        var request = new PetRequest("Thor", LocalDate.now().minusYears(2), "Golden Retriever", "GRANDE", 'M', true, 1L);
        Pet pet = criarPetMock();

        when(petService.create(any(PetRequest.class))).thenReturn(pet);

        mockMvc.perform(post("/pets")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.nome").value("Thor"));
    }

    @Test
    @WithMockUser
    @DisplayName("DELETE /pets/{id} - Deve excluir pet")
    void deveExcluirPet() throws Exception {
        mockMvc.perform(delete("/pets/1").param("usuarioId", "1"))
                .andExpect(status().isNoContent());

        verify(petService).delete(1L, 1L);
    }
}
