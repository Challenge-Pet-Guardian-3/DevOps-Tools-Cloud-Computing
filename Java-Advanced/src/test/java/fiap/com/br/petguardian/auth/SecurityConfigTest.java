package fiap.com.br.petguardian.auth;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.webmvc.test.autoconfigure.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.security.test.context.support.WithMockUser;
import org.springframework.test.web.servlet.MockMvc;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.patch;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@SpringBootTest
@AutoConfigureMockMvc
class SecurityConfigTest {

    @Autowired
    private MockMvc mockMvc;

    @Test
    @DisplayName("Endpoint do Swagger e docs devem ser públicos")
    void swaggerEndpointPublico() throws Exception {
        mockMvc.perform(get("/v3/api-docs"))
                .andExpect(status().isOk());
    }

    @Test
    @DisplayName("Endpoint de saúde Actuator deve ser público")
    void actuatorHealthPublico() throws Exception {
        mockMvc.perform(get("/actuator/health"))
                .andExpect(status().isOk());
    }

    @Test
    @DisplayName("Acesso anônimo a rotas protegidas deve retornar 401 Unauthorized")
    void acessoAnonimoRetorna401() throws Exception {
        mockMvc.perform(get("/pets"))
                .andExpect(status().isUnauthorized());
    }

    @Test
    @WithMockUser(username = "tutor_comum@fiap.com.br", roles = {"COMUM"})
    @DisplayName("Usuário com ROLE_COMUM deve ter acesso a /pets")
    void usuarioComumPodeAcessarPets() throws Exception {
        mockMvc.perform(get("/pets"))
                .andExpect(status().isOk());
    }

    @Test
    @WithMockUser(username = "tutor_comum@fiap.com.br", roles = {"COMUM"})
    @DisplayName("Usuário com ROLE_COMUM deve receber 403 Forbidden ao tentar acessar /trilhas/pet/1")
    void usuarioComumBloqueadoEmTrilhas() throws Exception {
        mockMvc.perform(get("/trilhas/pet/1"))
                .andExpect(status().isForbidden());
    }

    @Test
    @WithMockUser(username = "tutor_premium@fiap.com.br", roles = {"PREMIUM"})
    @DisplayName("Usuário com ROLE_PREMIUM não deve ser bloqueado com 403 em /trilhas/pet/1")
    void usuarioPremiumPodeAcessarTrilhas() throws Exception {
        mockMvc.perform(get("/trilhas/pet/1"))
                .andExpect(status().isNotFound()); // Passou da camada de segurança (não é 403)
    }

    @Test
    @WithMockUser(username = "tutor_premium@fiap.com.br", roles = {"PREMIUM"})
    @DisplayName("Usuário com ROLE_PREMIUM deve receber 403 Forbidden em POST /trilhas")
    void usuarioPremiumBloqueadoEmCriarTrilhas() throws Exception {
        mockMvc.perform(post("/trilhas"))
                .andExpect(status().isForbidden());
    }

    @Test
    @WithMockUser(username = "admin@fiap.com.br", roles = {"ADMIN"})
    @DisplayName("Usuário com ROLE_ADMIN deve ter autorização para POST /trilhas")
    void adminPodeAcessarCriarTrilhas() throws Exception {
        mockMvc.perform(post("/trilhas"))
                .andExpect(status().isBadRequest()); // Passou da segurança (não é 403)
    }

    @Test
    @WithMockUser(username = "admin@fiap.com.br", roles = {"ADMIN"})
    @DisplayName("Usuário com ROLE_ADMIN deve ter autorização para GET /trilhas/pet/1")
    void adminPodeAcessarTrilhas() throws Exception {
        mockMvc.perform(get("/trilhas/pet/1"))
                .andExpect(status().isNotFound()); // Passou da segurança (não é 403)
    }

    @Test
    @WithMockUser(username = "tutor_comum@fiap.com.br", roles = {"COMUM"})
    @DisplayName("Usuário com ROLE_COMUM deve receber 403 Forbidden em PATCH /aulas/1/concluir")
    void usuarioComumBloqueadoEmConcluirAula() throws Exception {
        mockMvc.perform(patch("/aulas/1/concluir"))
                .andExpect(status().isForbidden());
    }

    @Test
    @WithMockUser(username = "tutor_premium@fiap.com.br", roles = {"PREMIUM"})
    @DisplayName("Usuário com ROLE_PREMIUM deve ter autorização para PATCH /aulas/1/concluir")
    void usuarioPremiumPodeConcluirAula() throws Exception {
        mockMvc.perform(patch("/aulas/1/concluir"))
                .andExpect(status().isNotFound()); // Passou da segurança (não é 403)
    }
}
