package fiap.com.br.petguardian.config;

import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import io.swagger.v3.oas.annotations.enums.SecuritySchemeType;
import io.swagger.v3.oas.annotations.info.Info;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.security.SecurityScheme;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.context.annotation.Configuration;

@Configuration
@OpenAPIDefinition(
        info = @Info(
                title = "PetGuardian API",
                description = "API da plataforma de cuidado colaborativo centrada no Pet. Tutores compartilham responsabilidades, gerenciam tarefas da rotina do animal e acompanham o histórico consolidado de cuidados em um único lugar.",
                version = "1.0"
        ),
        security = @SecurityRequirement(name = "bearerAuth"),
        tags = {
                @Tag(name = "Autenticacao", description = "Autenticacao de usuarios e geracao de token JWT"),
                @Tag(name = "Usuarios", description = "Gerenciamento de usuarios (tutores/cuidadores)"),
                @Tag(name = "Pets", description = "Gerenciamento de pets e historico clinico"),
                @Tag(name = "Care Circle (Rede de Cuidados)", description = "Gestao colaborativa de tutores e co-cuidadores do pet"),
                @Tag(name = "Tarefas", description = "Gerenciamento de tarefas e rotinas de cuidados do pet"),
                @Tag(name = "Historico", description = "Historico de saude e eventos do pet"),
                @Tag(name = "Trilhas", description = "Gerenciamento de trilhas de aprendizado e adestramento do pet"),
                @Tag(name = "Modulos", description = "Gerenciamento de modulos das trilhas de aprendizado"),
                @Tag(name = "Aulas", description = "Gerenciamento de aulas e conteudos educativos das trilhas"),
                @Tag(name = "Enderecos", description = "Gerenciamento de enderecos")
        }
)
@SecurityScheme(
        name = "bearerAuth",
        type = SecuritySchemeType.HTTP,
        scheme = "bearer",
        bearerFormat = "JWT"
)
public class SwaggerConfig {
}
