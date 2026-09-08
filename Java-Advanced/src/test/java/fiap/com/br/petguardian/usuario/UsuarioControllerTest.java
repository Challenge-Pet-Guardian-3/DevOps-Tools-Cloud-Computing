package fiap.com.br.petguardian.usuario;

import com.fasterxml.jackson.databind.ObjectMapper;
import fiap.com.br.petguardian.endereco.Endereco;
import fiap.com.br.petguardian.endereco.bairro.Bairro;
import fiap.com.br.petguardian.endereco.cidade.Cidade;
import fiap.com.br.petguardian.endereco.dto.EnderecoRequest;
import fiap.com.br.petguardian.endereco.estado.Estado;
import fiap.com.br.petguardian.telefone.Telefone;
import fiap.com.br.petguardian.usuario.dto.RedeCuidadoResponse;
import fiap.com.br.petguardian.usuario.dto.UsuarioRequest;
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
import java.util.Set;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@SpringBootTest
@AutoConfigureMockMvc
class UsuarioControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockitoBean
    private UsuarioService usuarioService;

    private Usuario criarUsuarioMock() {
        Estado estado = Estado.builder().id(1L).nome("São Paulo").build();
        Cidade cidade = Cidade.builder().id(1L).nome("São Paulo").estado(estado).build();
        Bairro bairro = Bairro.builder().id(1L).nome("Bela Vista").cidade(cidade).build();
        Endereco endereco = Endereco.builder().id(1L).cep("01310100").numero("100").rua("Av Paulista").bairro(bairro).build();
        Telefone telefone = Telefone.builder().id(1L).ddd("11").numero("987654321").build();

        return Usuario.builder()
                .id(1L)
                .nome("Enzo Silva")
                .email("enzo@fiap.com.br")
                .senha("hash")
                .role(UsuarioRole.PREMIUM)
                .telefone(telefone)
                .enderecos(Set.of(endereco))
                .build();
    }

    @Test
    @WithMockUser
    @DisplayName("GET /usuarios - Deve retornar lista paginada de usuarios")
    void deveListarUsuarios() throws Exception {
        Usuario usuario = criarUsuarioMock();
        when(usuarioService.findAll(any())).thenReturn(new PageImpl<>(List.of(usuario)));

        mockMvc.perform(get("/usuarios"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.content[0].nome").value("Enzo Silva"))
                .andExpect(jsonPath("$.content[0].email").value("enzo@fiap.com.br"));
    }

    @Test
    @WithMockUser
    @DisplayName("GET /usuarios/{id} - Deve retornar dados do usuario por ID")
    void deveBuscarUsuarioPorId() throws Exception {
        Usuario usuario = criarUsuarioMock();
        when(usuarioService.findById(1L)).thenReturn(usuario);

        mockMvc.perform(get("/usuarios/1"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id").value(1))
                .andExpect(jsonPath("$.nome").value("Enzo Silva"));
    }

    @Test
    @WithMockUser
    @DisplayName("GET /usuarios/{id}/rede-cuidado - Deve retornar Care Circle")
    void deveBuscarRedeCuidado() throws Exception {
        var rede = new RedeCuidadoResponse(1L, "Enzo Silva", List.of(), List.of(), 2, 5, 150);
        when(usuarioService.getRedeCuidado(1L)).thenReturn(rede);

        mockMvc.perform(get("/usuarios/1/rede-cuidado"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.usuarioId").value(1))
                .andExpect(jsonPath("$.nomeUsuario").value("Enzo Silva"))
                .andExpect(jsonPath("$.pontosAcumulados").value(150));
    }

    @Test
    @DisplayName("POST /usuarios - Deve cadastrar usuario (rota publica permitAll)")
    void deveCadastrarNovoUsuario() throws Exception {
        var enderecoReq = new EnderecoRequest("01310100", "100");
        var request = new UsuarioRequest("Enzo Silva", "enzo@fiap.com.br", "123456", "11", "987654321", "PREMIUM", enderecoReq);
        Usuario usuario = criarUsuarioMock();

        when(usuarioService.create(any(UsuarioRequest.class))).thenReturn(usuario);

        mockMvc.perform(post("/usuarios")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.nome").value("Enzo Silva"));
    }

    @Test
    @WithMockUser
    @DisplayName("DELETE /usuarios/{id} - Deve remover usuario com 204 No Content")
    void deveDeletarUsuario() throws Exception {
        mockMvc.perform(delete("/usuarios/1"))
                .andExpect(status().isNoContent());

        verify(usuarioService).delete(1L);
    }
}
