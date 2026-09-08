package fiap.com.br.petguardian.usuariopet;

import com.fasterxml.jackson.databind.ObjectMapper;
import fiap.com.br.petguardian.usuariopet.dto.CoCuidadorRequest;
import fiap.com.br.petguardian.usuariopet.dto.CoCuidadorResponse;
import fiap.com.br.petguardian.usuariopet.dto.TransferirResponsabilidadeRequest;
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
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@SpringBootTest
@AutoConfigureMockMvc
class UsuarioPetControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockitoBean
    private UsuarioPetService usuarioPetService;

    @Test
    @WithMockUser
    @DisplayName("GET /pets/{petId}/cuidadores - Deve retornar lista de cuidadores")
    void deveListarCuidadores() throws Exception {
        var cuidador = new CoCuidadorResponse(1L, "Enzo", "enzo@fiap.com.br", 10L, "Thor", true);
        when(usuarioPetService.listarCuidadoresDoPet(10L)).thenReturn(List.of(cuidador));

        mockMvc.perform(get("/pets/10/cuidadores"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$[0].usuarioId").value(1))
                .andExpect(jsonPath("$[0].nome").value("Enzo"))
                .andExpect(jsonPath("$[0].email").value("enzo@fiap.com.br"))
                .andExpect(jsonPath("$[0].responsavelPrincipal").value(true));
    }

    @Test
    @WithMockUser
    @DisplayName("POST /pets/{petId}/cuidadores - Deve convidar co-cuidador")
    void deveConvidarCuidador() throws Exception {
        var request = new CoCuidadorRequest(1L, "familiar@fiap.com.br");
        var response = new CoCuidadorResponse(2L, "Familiar", "familiar@fiap.com.br", 10L, "Thor", false);

        when(usuarioPetService.convidarCoCuidador(eq(10L), any(CoCuidadorRequest.class))).thenReturn(response);

        mockMvc.perform(post("/pets/10/cuidadores")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.usuarioId").value(2))
                .andExpect(jsonPath("$.email").value("familiar@fiap.com.br"));
    }

    @Test
    @WithMockUser
    @DisplayName("DELETE /pets/{petId}/cuidadores/{usuarioId} - Deve desvincular co-cuidador")
    void deveDesvincularCuidador() throws Exception {
        mockMvc.perform(delete("/pets/10/cuidadores/2")
                        .param("solicitanteId", "1"))
                .andExpect(status().isNoContent());

        verify(usuarioPetService).desvincularCuidador(10L, 2L, 1L);
    }

    @Test
    @WithMockUser
    @DisplayName("PATCH /pets/{petId}/responsavel-principal - Deve transferir titularidade")
    void deveTransferirResponsabilidade() throws Exception {
        var request = new TransferirResponsabilidadeRequest(1L, 2L);

        mockMvc.perform(patch("/pets/10/responsavel-principal")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isNoContent());

        verify(usuarioPetService).transferirResponsabilidadePrincipal(eq(10L), any(TransferirResponsabilidadeRequest.class));
    }
}
