package fiap.com.br.petguardian.exception;

import fiap.com.br.petguardian.usuario.UsuarioService;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.webmvc.test.autoconfigure.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.security.test.context.support.WithMockUser;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;

import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@SpringBootTest
@AutoConfigureMockMvc
class GlobalExceptionHandlerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockitoBean
    private UsuarioService usuarioService;

    @Test
    @WithMockUser
    @DisplayName("Deve tratar ResourceNotFoundException e retornar 404 com payload padronizado")
    void deveTratarResourceNotFoundException() throws Exception {
        when(usuarioService.findById(999L)).thenThrow(new ResourceNotFoundException("Usuario com id 999 nao encontrado."));

        mockMvc.perform(get("/usuarios/999"))
                .andExpect(status().isNotFound())
                .andExpect(jsonPath("$.status").value(404))
                .andExpect(jsonPath("$.error").value("Not Found"))
                .andExpect(jsonPath("$.message").value("Usuario com id 999 nao encontrado."))
                .andExpect(jsonPath("$.path").value("/usuarios/999"));
    }

    @Test
    @WithMockUser
    @DisplayName("Deve tratar IllegalArgumentException e retornar 400")
    void deveTratarIllegalArgumentException() throws Exception {
        when(usuarioService.findById(1L)).thenThrow(new IllegalArgumentException("Parametro invalido informado."));

        mockMvc.perform(get("/usuarios/1"))
                .andExpect(status().isBadRequest())
                .andExpect(jsonPath("$.status").value(400))
                .andExpect(jsonPath("$.message").value("Parametro invalido informado."));
    }
}
