package fiap.com.br.petguardian.endereco;

import com.fasterxml.jackson.databind.ObjectMapper;
import fiap.com.br.petguardian.endereco.bairro.Bairro;
import fiap.com.br.petguardian.endereco.cidade.Cidade;
import fiap.com.br.petguardian.endereco.dto.EnderecoRequest;
import fiap.com.br.petguardian.endereco.estado.Estado;
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

import java.util.List;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@SpringBootTest
@AutoConfigureMockMvc
class EnderecoControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockitoBean
    private EnderecoService enderecoService;

    private Endereco criarEnderecoMock() {
        Estado estado = Estado.builder().id(1L).nome("São Paulo").build();
        Cidade cidade = Cidade.builder().id(1L).nome("São Paulo").estado(estado).build();
        Bairro bairro = Bairro.builder().id(1L).nome("Bela Vista").cidade(cidade).build();
        return Endereco.builder()
                .id(1L)
                .cep("01310100")
                .numero("100")
                .rua("Avenida Paulista")
                .bairro(bairro)
                .build();
    }

    @Test
    @WithMockUser
    @DisplayName("GET /enderecos - Deve listar enderecos")
    void deveListarEnderecos() throws Exception {
        Endereco endereco = criarEnderecoMock();
        when(enderecoService.findAll(any())).thenReturn(new PageImpl<>(List.of(endereco)));

        mockMvc.perform(get("/enderecos"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.content[0].cep").value("01310100"))
                .andExpect(jsonPath("$.content[0].rua").value("Avenida Paulista"));
    }

    @Test
    @WithMockUser
    @DisplayName("GET /enderecos/{id} - Deve buscar endereco por ID")
    void deveBuscarEnderecoPorId() throws Exception {
        Endereco endereco = criarEnderecoMock();
        when(enderecoService.findById(1L)).thenReturn(endereco);

        mockMvc.perform(get("/enderecos/1"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id").value(1))
                .andExpect(jsonPath("$.bairro").value("Bela Vista"));
    }

    @Test
    @WithMockUser
    @DisplayName("POST /enderecos - Deve criar endereco")
    void deveCriarEndereco() throws Exception {
        var request = new EnderecoRequest("01310100", "100");
        Endereco endereco = criarEnderecoMock();

        when(enderecoService.create(any(EnderecoRequest.class))).thenReturn(endereco);

        mockMvc.perform(post("/enderecos")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.cep").value("01310100"));
    }

    @Test
    @WithMockUser
    @DisplayName("DELETE /enderecos/{id} - Deve deletar endereco")
    void deveDeletarEndereco() throws Exception {
        mockMvc.perform(delete("/enderecos/1"))
                .andExpect(status().isNoContent());

        verify(enderecoService).delete(1L);
    }
}
