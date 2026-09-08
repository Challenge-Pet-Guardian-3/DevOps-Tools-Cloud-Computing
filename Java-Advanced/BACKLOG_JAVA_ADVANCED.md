# 📋 Backlog Master Azure Boards — Sprint 3: Java Advanced (Spring Boot)

> **Projeto Integrado:** PetGuardian / Clyvo Care (Challenge FIAP 2026 - 2º Ano ADS / 2TDSPG)  
> **Disciplina:** Advanced Business Development with Java (Java Advanced — FIAP 2TDSPG)  
> **Epic Principal:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`  
> **Start Date:** `2026-08-24`  
> **Target Date:** `2026-08-29`  
> **Padrão:** Azure Boards (Scrum Process: Epic ➔ Feature ➔ PBI ➔ Task)  
> **Diretrizes Estratégicas:** 1º Refatoração & Qualidade ➔ 2º Segurança & JWT ➔ 3º Versionamento de Banco (Flyway) ➔ 4º Fluxos Especializados de Negócio (Rede de Co-Cuidadores N:N e Rotina com Expiração Atômica) ➔ 5º Documentação & Entrega.

---

## 🎯 1. Matriz de Requisitos & Critérios de Avaliação Oficiais

| Requisito Oficial | Peso / Pontos | Status no Backlog | Destaque Técnico & Mentoria Clyvo |
| :--- | :---: | :---: | :--- |
| **1. Refatoração SOLID, DRY & Clean Code** | **Penalidades (-10 a -15 pts)** | **Concluído (PBI-01 a PBI-04)** | Injeção por construtor (`@RequiredArgsConstructor`), eliminação total de `ResponseStatusException`, Global Exception Handler, DTOs desacoplados com fábrica `toEntity()`/`fromEntity()`, `@Transactional` seletivo, tipos primitivos `boolean` e SpringDoc OpenAPI/Swagger. |
| **2. Spring Security & Controle de Acesso** | **30 pts** | **Crítico (PBI-05 a PBI-08)** | Autenticação via Spring Security OAuth2 Resource Server com par de chaves assimétricas **RSA 2048-bit** (`NimbusJwtEncoder` / `NimbusJwtDecoder`), BCrypt, tokens JWT Stateless para consumo Mobile (React Native), `CorsConfig` dedicado e proteção centralizada via `SecurityFilterChain`. |
| **3. Flyway (Controle de Versão de BD)** | **20 pts** | **Crítico (PBI-09 e PBI-10)** | Migrações versionadas (`V1`, `V2`, `V3`) com tabelas Pet-Centric: `usuario`, `pet`, `usuario_pet` (gestão N:N familiar com PK composta `@EmbeddedId`), `tarefa`, `status`, `endereco`, `bairro`, `cidade`, `estado`, `telefone`. |
| **4. Funcionalidades Completas (Fluxos Não-CRUD)** | **20 pts** | **Crítico (PBI-11 a PBI-14)** | **Dois fluxos ponta a ponta complexos:**<br>1) *Rede Familiar & Co-Cuidadores:* Vínculo N:N entre tutores e pets com `UsuarioPetController` (`/pets/{petId}/cuidadores`), gestão de Responsável Principal, convite de co-cuidadores exclusivamente por **e-mail**, transferência de titularidade, agregação performática da Rede de Cuidado (`RedeCuidadoMapper` com batch query anti-N+1) e histórico consolidado do pet.<br>2) *Rotina Familiar, Expiração Automática & Gamificação:* Tutores criam rotinas com prazo e pontuação, mecanismo atômico de expiração automática de tarefas pendentes vencidas via query `@Modifying`, conclusão com atribuição de cuidador executor e cálculo de ranking/pontuação. |
| **5. Documentação, Vídeo (10 min) & Avaliação Oral** | **Obrigatório** | **Alto (PBI-15 a PBI-17)** | README detalhado, gravação com demonstração das rotas/segurança/fluxos e guia de estudo para defesa individual com foco em decisões de arquitetura e Clean Code. |

---

## 🌳 2. Estrutura Hierárquica no Azure Boards

```text
[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric
│
├── 🏆 [FEATURE 01] Refatoração Arquitetural, SOLID & Tratamento de Exceções
│   ├── 📄 [PBI-01] Arquitetura de Exceções de Domínio e Global Exception Handler (1 pt)
│   └── 📄 [PBI-02] Refatoração dos Serviços de Negócio com Princípios SOLID (SRP, DIP & Injeção) (1 pt)
│
├── 🏆 [FEATURE 02] Validação de Contratos (Bean Validation) & Documentação OpenAPI
│   ├── 📄 [PBI-03] Padronização de DTOs e Bean Validation Avançado (@Valid) (1 pt)
│   └── 📄 [PBI-04] Configuração e Documentação Interativa com SpringDoc OpenAPI 3 / Swagger (1 pt)
│
├── 🏆 [FEATURE 03] Autenticação e Gestão de Credenciais com JWT (Spring Security Core)
│   ├── 📄 [PBI-05] Arquitetura de Autenticação, UserDetails, BCrypt & Serviço de Tokens JWT RSA (2 pts)
│   └── 📄 [PBI-06] Endpoints de Autenticação (/login) e Registro (/usuarios) com Emissão de JWT (1 pt)
│
├── 🏆 [FEATURE 04] Autorização, Filtro JWT Stateless e Proteção de Endpoints (RBAC)
│   ├── 📄 [PBI-07] Configuração do SecurityFilterChain Stateless, CORS Dedicado e Proteção Centralizada (2 pts)
│   └── 📄 [PBI-08] Segurança Granular por Método (@EnableMethodSecurity) e Handlers 401/403 (1 pt)
│
├── 🏆 [FEATURE 05] Versionamento de Banco de Dados & Governança de Esquema (Flyway)
│   ├── 📄 [PBI-09] Configuração do Flyway e Migração Inicial DDL Pet-Centric (V1) (2 pts)
│   └── 📄 [PBI-10] Migrações Incrementais de Domínio, Status e Seeds de Segurança (V2 e V3) (1 pt)
│
├── 🏆 [FEATURE 06] Fluxo 1: Gestão de Pets, Rede Familiar e Co-Cuidadores (N:N)
│   ├── 📄 [PBI-11] [Fluxo 1.1] Gestão de Vínculos N:N, Responsável Principal e Convite de Co-Cuidadores por E-mail (2 pts)
│   └── 📄 [PBI-12] [Fluxo 1.2] Visualização Agregada da Rede de Cuidado (RedeCuidadoMapper) e Histórico do Pet (2 pts)
│
├── 🏆 [FEATURE 07] Fluxo 2: Gestão da Rotina do Pet, Expiração Atômica & Gamificação
│   ├── 📄 [PBI-13] [Fluxo 2.1] Criação de Rotina Familiar e Mecanismo Atômico de Expiração Automática (2 pts)
│   └── 📄 [PBI-14] [Fluxo 2.2] Conclusão de Tarefas, Registro de Executor e Sistema de Pontuação (2 pts)
│
└── 🏆 [FEATURE 08] Documentação Técnica, Demonstração em Vídeo & Preparação para Banca Oral
    ├── 📄 [PBI-15] Atualização Completa do README.md e Guia de Execução (Gradle) (1 pt)
    ├── 📄 [PBI-16] Roteiro e Gravação do Vídeo Demonstrativo da Aplicação (Máx. 10 min) (1 pt)
    └── 📄 [PBI-17] Matriz de Argumentação e Preparação para a Avaliação Oral Individual (1 pt)
```

---

## 📊 3. Tabela Resumo do Backlog

| Feature Pai | ID do PBI | Título do Item de Backlog (PBI) | Story Points | Prioridade | Horas Estimadas |
| :--- | :--- | :--- | :---: | :---: | :---: |
| **[FEATURE 01] SOLID & Qualidade** | **PBI-01** | Arquitetura de Exceções de Domínio e Global Exception Handler | 1 pts | 1 - Critical | 4.5h |
| | **PBI-02** | Refatoração dos Serviços de Negócio com Princípios SOLID | 1 pts | 1 - Critical | 6.5h |
| **[FEATURE 02] Validação & Swagger** | **PBI-03** | Padronização de DTOs e Bean Validation Avançado (@Valid) | 1 pts | 2 - High | 3.5h |
| | **PBI-04** | Configuração e Documentação com SpringDoc OpenAPI 3 / Swagger | 1 pts | 2 - High | 4.5h |
| **[FEATURE 03] Security Core JWT** | **PBI-05** | Arquitetura de Autenticação, UserDetails, BCrypt & JWT Service RSA | 2 pts | 1 - Critical | 5.5h |
| | **PBI-06** | Endpoints de Autenticação (/login) e Registro (/usuarios) com JWT | 1 pts | 1 - Critical | 4.0h |
| **[FEATURE 04] Autorização RBAC** | **PBI-07** | SecurityFilterChain Stateless, CORS Dedicado e Proteção Centralizada | 2 pts | 1 - Critical | 5.0h |
| | **PBI-08** | Segurança Granular por Método (@EnableMethodSecurity) e Handlers | 1 pts | 1 - Critical | 5.0h |
| **[FEATURE 05] Flyway & Banco** | **PBI-09** | Configuração do Flyway e Migração Inicial DDL Pet-Centric (V1) | 2 pts | 1 - Critical | 6.0h |
| | **PBI-10** | Migrações Incrementais, Status e Seeds de Segurança (V2/V3) | 1 pts | 2 - High | 4.5h |
| **[FEATURE 06] Fluxo 1 Rede Cuidado** | **PBI-11** | [Fluxo 1.1] Gestão Vínculos N:N, Responsável Principal e Convite E-mail | 2 pts | 1 - Critical | 6.0h |
| | **PBI-12** | [Fluxo 1.2] Rede Cuidado Agregada (RedeCuidadoMapper) e Histórico Pet | 2 pts | 1 - Critical | 6.0h |
| **[FEATURE 07] Fluxo 2 Gamificação** | **PBI-13** | [Fluxo 2.1] Criação Rotina Familiar e Expiração Automática de Tarefas | 2 pts | 1 - Critical | 5.5h |
| | **PBI-14** | [Fluxo 2.2] Conclusão de Tarefas, Registro de Executor e Pontuação | 2 pts | 1 - Critical | 5.5h |
| **[FEATURE 08] Docs, Vídeo & Banca** | **PBI-15** | Atualização Completa do README.md e Guia de Execução (Gradle) | 1 pts | 2 - High | 3.0h |
| | **PBI-16** | Roteiro e Gravação do Vídeo Demonstrativo (Máx. 10 min) | 1 pts | 1 - Critical | 5.5h |
| | **PBI-17** | Matriz de Argumentação e Preparação para a Avaliação Oral | 1 pts | 2 - High | 3.5h |
| **TOTAL CONSOLIDADO** | **8 Features** | **17 PBIs / 43 Child Tasks Técnicas** | **24 pts** | — | **74.5h** |

---

## 📦 4. Detalhamento dos Itens de Trabalho (Épico, Features, PBIs e Tasks)

---

### 🏛️ ÉPICO
* **Work Item Type:** `Epic`
* **Title:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`
* **Tags:** `Sprint3, JavaAdvanced, SpringBoot, Security, Flyway, SOLID`
* **Start Date:** `2026-08-24`
* **Target Date:** `2026-08-29`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `24`
* **Business Value:** `100`
* **Description:** Evolução corporativa do backend Spring Boot aplicando governança de código SOLID/Clean Code, autenticação Stateless via OAuth2 Resource Server com par de chaves assimétricas RSA 2048-bit, migrações versionadas com Flyway e implementação de 2 fluxos de negócio complexos não-CRUD (Care Circle familiar N:N e rotina com expiração atômica e gamificação).

---

### 🏆 [FEATURE 01] Refatoração Arquitetural, SOLID & Tratamento de Exceções
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`
* **Title:** `[FEATURE 01] Refatoração Arquitetural, SOLID & Tratamento de Exceções`
* **Tags:** `Sprint3, JavaAdvanced, SOLID, CleanCode, Refactor, Exceptions`
* **Start Date:** `2026-08-24`
* **Target Date:** `2026-08-25`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Description:** Refatoração da camada de serviços e tratamento de erros para cumprir os princípios de Clean Code, SRP e DIP, eliminando ResponseStatusException solta, unificando o tratamento de exceções de domínio no GlobalExceptionHandler e aplicando @Transactional seletivo.

#### 🔹 [PBI-01] Arquitetura de Exceções de Domínio e Global Exception Handler
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 01] Refatoração Arquitetural, SOLID & Tratamento de Exceções`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, Exceptions, GlobalExceptionHandler, ControllerAdvice`

##### Descrição (História de Usuário)
> **Como** Desenvolvedor Backend,  
> **Eu quero** criar uma hierarquia de exceções de domínio expressivas e unificar sua captura em um `GlobalExceptionHandler` (`@RestControllerAdvice`),  
> **Para que** todas as falhas de negócio, validação e integridade retornem respostas HTTP estruturadas e previsíveis.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Exceção de domínio `ResourceNotFoundException` para recursos inexistentes (404 Not Found).
- [ ] DTO padrão `ApiErrorResponse` contendo `timestamp`, `status`, `error`, `message` e `path`.
- [ ] DTO `ValidationErrorDetail` com lista de erros de validação de formulários (`@Valid`).
- [ ] `GlobalExceptionHandler` intercepta `MethodArgumentNotValidException`, `ResourceNotFoundException`, `IllegalArgumentException`, `HttpMessageNotReadableException` e `DataIntegrityViolationException` sem expor stacktrace interno.

##### Tarefas Técnicas (Child Tasks)
* **Task 1.1:** [TASK-01] Criar classes de exceção de domínio e DTOs de resposta padronizada de erro (`ApiErrorResponse`). *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Estruturar payload RFC-7807/ProblemDetails para respostas de erro.
* **Task 1.2:** [TASK-02] Implementar `@RestControllerAdvice` no `GlobalExceptionHandler` mapeando todos os tipos de erro HTTP (400, 404, 500). *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Centralizar captura de exceções de validação e regras de negócio.
* **Task 1.3:** [TASK-03] Testar o tratamento de erros para garantir consistência de resposta em todos os endpoints. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Executar requisições com dados inválidos validando status codes e corpo da resposta.

---

#### 🔹 [PBI-02] Refatoração dos Serviços de Negócio com Princípios SOLID (SRP, DIP & Injeção)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 01] Refatoração Arquitetural, SOLID & Tratamento de Exceções`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, SOLID, CleanCode, SRP, DIP, Lombok`

##### Descrição (História de Usuário)
> **Como** Desenvolvedor Backend,  
> **Eu quero** refatorar os Services para eliminar acoplamento indevido, métodos longos e anotações redundantes,  
> **Para que** o código respeite os princípios SOLID (especialmente SRP e DIP), injeção por construtor com Lombok e utilize transações de forma atômica e seletiva.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Injeção de dependências estritamente por construtor utilizando `@RequiredArgsConstructor` (Lombok) com atributos `private final` nos Services (`PetService`, `UsuarioService`, `UsuarioPetService`, `TarefaService`, `EnderecoService`, `StatusService`).
- [ ] Eliminação de indireções e métodos privados duplicados de busca (`findById` direto).
- [ ] Padronização de tipos primitivos `boolean` (`responsavelPrincipal`, `castrado`) para evitar verificações defensivas redundantes de `null`.
- [ ] Anotação `@Transactional` mantida estritamente em métodos de escrita e `@Transactional(readOnly = true)` em consultas.

##### Tarefas Técnicas (Child Tasks)
* **Task 2.1:** [TASK-04] Refatorar `UsuarioService` e `PetService` aplicando `@RequiredArgsConstructor` e regras atômicas de transação. *(Activity: Development, Est: 2.5h)*
  * *Descrição:* Limpar injeção de dependência e garantir transações atômicas de escrita.
* **Task 2.2:** [TASK-05] Refatorar `TarefaService` e `EnderecoService` aplicando modularização de regras, integração ViaCEP e `@Transactional` seletivo. *(Activity: Development, Est: 2.5h)*
  * *Descrição:* Isolar chamadas externas e padronizar leitura somente-leitura.
* **Task 2.3:** [TASK-06] Auditar o código-fonte eliminando queries não utilizadas, `if` redundantes e classes mortas. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Remover código obsoleto da Sprint 1 e simplificar fluxo de execução.

---

### 🏆 [FEATURE 02] Validação de Contratos (Bean Validation) & Documentação OpenAPI
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`
* **Title:** `[FEATURE 02] Validação de Contratos (Bean Validation) & Documentação OpenAPI`
* **Tags:** `Sprint3, JavaAdvanced, BeanValidation, OpenAPI, Swagger, Contracts`
* **Start Date:** `2026-08-24`
* **Target Date:** `2026-08-25`
* **Priority:** `2 - High`
* **Effort (Story Points):** `2`
* **Description:** Padronização de DTOs de entrada e saída com Bean Validation avançado, isolamento das entidades JPA e documentação interativa completa da API utilizando SpringDoc OpenAPI 3 / Swagger UI.

#### 🔹 [PBI-03] Padronização de DTOs e Bean Validation Avançado (@Valid)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 02] Validação de Contratos (Bean Validation) & Documentação OpenAPI`
* **State:** `Approved`
* **Priority:** `2 - High`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, BeanValidation, DTO, Validation`

##### Descrição (História de Usuário)
> **Como** Desenvolvedor Mobile consumidor da API,  
> **Eu quero** que todas as requisições de entrada sejam estritamente validadas via Bean Validation e os Responses usem DTOs desacoplados,  
> **Para que** dados incorretos sejam rejeitados no Controller com mensagens amigáveis e não ocorra vazamento de entidades JPA.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] DTOs de Request anotados com constraints precisas (`@NotBlank`, `@NotNull`, `@Size`, `@Email`, `@FutureOrPresent`, `@Positive`, `@Pattern`).
- [ ] Validadores customizados: `@CepValidation`, `@DddValidation`, `@EnumValidation` e `@DiferentesUsuariosValidation`.
- [ ] Métodos de fábrica `toEntity()` embutidos nos DTOs de Request e `fromEntity()` nos DTOs de Response.
- [ ] DTOs de Response desacoplados das entidades (`UsuarioResponse`, `PetResponse`, `TarefaResponse`, `EnderecoResponse`, `RedeCuidadoResponse`, `CoCuidadorResponse`).
- [ ] Anotação `@Valid` aplicada em todos os parâmetros `@RequestBody` nos métodos dos Controllers.

##### Tarefas Técnicas (Child Tasks)
* **Task 3.1:** [TASK-07] Revisar e aplicar Bean Validation completo em `PetRequest`, `UsuarioRequest`, `TarefaRequest`, `TarefaConclusaoRequest`, `CoCuidadorRequest` e `EnderecoRequest`. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Aplicar regras de restrição de entrada em todos os DTOs.
* **Task 3.2:** [TASK-08] Garantir que todos os Responses utilizem DTOs Records imutáveis e testar respostas 400 Bad Request. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Desacoplar modelos de banco das respostas HTTP.

---

#### 🔹 [PBI-04] Configuração e Documentação Interativa com SpringDoc OpenAPI 3 / Swagger
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 02] Validação de Contratos (Bean Validation) & Documentação OpenAPI`
* **State:** `Approved`
* **Priority:** `2 - High`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, Swagger, OpenAPI, SpringDoc, Documentation`

##### Descrição (História de Usuário)
> **Como** Desenvolvedor e Integrador da API,  
> **Eu quero** acessar a documentação interativa Swagger UI da aplicação,  
> **Para que** eu possa testar todos os endpoints, parâmetros e validar os contratos de dados diretamente pelo navegador.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Dependência `org.springdoc:springdoc-openapi-starter-webmvc-ui` configurada no `build.gradle`.
- [ ] `SwaggerConfig` configurada com metadados do projeto PetGuardian.
- [ ] Controllers documentados com anotações `@Tag`, `@Operation` e `@ResponseStatus`.
- [ ] Swagger UI acessível e funcional em `/swagger-ui/index.html` e `/v3/api-docs`.

##### Tarefas Técnicas (Child Tasks)
* **Task 4.1:** [TASK-09] Configurar metadados do OpenAPI em `SwaggerConfig`. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Definir títulos, descrições, versões e esquemas de segurança JWT no Swagger.
* **Task 4.2:** [TASK-10] Enriquecer `PetController`, `UsuarioController`, `UsuarioPetController`, `TarefaController` e `EnderecoController` com `@Tag` e `@Operation`. *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Descrever operações e códigos de retorno de cada rota.
* **Task 4.3:** [TASK-11] Validar documentação interativa e execução dos endpoints no Swagger UI. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Testar chamadas no navegador verificando esquemas de entrada e saída.

---

### 🏆 [FEATURE 03] Autenticação e Gestão de Credenciais com JWT (Spring Security Core)
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`
* **Title:** `[FEATURE 03] Autenticação e Gestão de Credenciais com JWT (Spring Security Core)`
* **Tags:** `Sprint3, JavaAdvanced, Security, Authentication, BCrypt, JWT, RSA2048`
* **Start Date:** `2026-08-25`
* **Target Date:** `2026-08-26`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Description:** Estruturação da arquitetura de autenticação com Spring Security e OAuth2 Resource Server com par de chaves assimétricas RSA 2048-bit, criptografia BCrypt, UserDetailsService e emissão de tokens JWT para consumo pelo aplicativo Mobile (React Native).

#### 🔹 [PBI-05] Arquitetura de Autenticação, UserDetails, BCrypt & Serviço de Tokens JWT RSA
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 03] Autenticação e Gestão de Credenciais com JWT (Spring Security Core)`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Tags:** `Sprint3, JavaAdvanced, SpringSecurity, BCrypt, UserDetails, JWT, RSA`

##### Descrição (História de Usuário)
> **Como** Arquiteto de Software Backend,  
> **Eu quero** configurar o Spring Security com `BCryptPasswordEncoder`, par de chaves RSA 2048-bit (`NimbusJwtEncoder`/`NimbusJwtDecoder`), `UserDetailsService` e um serviço emissor de tokens JWT (`TokenService`),  
> **Para que** as credenciais sejam salvas com hash seguro e a aplicação emita tokens JWT assinados digitalmente para autenticar o app Mobile.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Dependências `spring-boot-starter-security` e `spring-boot-starter-oauth2-resource-server` configuradas no `build.gradle`.
- [ ] Par de chaves RSA de 2048 bits (`private_key.pem`, `public_key.pem`) gerado em `src/main/resources/keys/` e injetado via `@ConfigurationProperties(prefix = "rsa")`.
- [ ] Bean `PasswordEncoder` configurado com `BCryptPasswordEncoder` e coluna de senha no `Usuario` dimensionada com segurança.
- [ ] `TokenService` implementado gerando tokens JWT assinados com tempo de expiração, claims (`role`, `sub: email`) e issuer `petguardian-api`.
- [ ] Serviço `AuthService` implementando `UserDetailsService` para busca de usuário por e-mail no banco de dados.

##### Tarefas Técnicas (Child Tasks)
* **Task 5.1:** [TASK-12] Configurar Gradle com dependências de segurança e gerar par de chaves RSA 2048-bit. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Criar par de chaves assimétricas PEM e injetar beans no contexto Spring.
* **Task 5.2:** [TASK-13] Implementar `AuthService` (`UserDetailsService`) e configurar `BCryptPasswordEncoder`. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Carregar usuário pelo e-mail e aplicar hashing seguro de senhas.
* **Task 5.3:** [TASK-14] Desenvolver `TokenService` com `JwtEncoder` baseado em chaves assimétricas RSA. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Emitir tokens JWT contendo claims de autorização e expiração.

---

#### 🔹 [PBI-06] Endpoints de Autenticação (/login) e Registro (/usuarios) com Emissão de JWT
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 03] Autenticação e Gestão de Credenciais com JWT (Spring Security Core)`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, AuthEndpoints, Login, Register, JWT, MobileAPI`

##### Descrição (História de Usuário)
> **Como** Usuário do aplicativo Mobile (Tutor ou Cuidador),  
> **Eu quero** realizar login através do endpoint `/login` e cadastro via `POST /usuarios`,  
> **Para que** eu receba um token JWT válido e os dados do meu perfil (`user`) no payload para manter a sessão ativa no aplicativo.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Endpoint `POST /login` no `AuthController` recebendo `LoginRequest` (`email`, `senha`) com `@NotBlank` e `@Email`.
- [ ] `LoginResponse` contendo `token` e o objeto `user` (`UsuarioResponse`) para armazenamento direto no `AsyncStorage` do Mobile.
- [ ] `POST /usuarios` criptografa a senha com BCrypt via `UsuarioService` e persiste o usuário com seus dados de endereço e telefone.
- [ ] Retorno de status HTTP 401 para credenciais inválidas e HTTP 400 para erros de validação de payload.

##### Tarefas Técnicas (Child Tasks)
* **Task 6.1:** [TASK-15] Desenvolver DTOs `LoginRequest` e `LoginResponse` no `AuthController`. *(Activity: Development, Est: 1.0h)*
  * *Descrição:* Mapear payload de autenticação com validação estrita.
* **Task 6.2:** [TASK-16] Implementar autenticação via `AuthenticationManager` e delegar busca de dados do usuário para `UsuarioService`. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Validar credenciais contra banco e emitir token no login.
* **Task 6.3:** [TASK-17] Testar cenários de login válido, senha incorreta e persistência de senhas com hash BCrypt. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Validar respostas de sucesso (200) e erro de autenticação (401).

---

### 🏆 [FEATURE 04] Autorização, Filtro JWT Stateless e Proteção de Endpoints (RBAC)
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`
* **Title:** `[FEATURE 04] Autorização, Filtro JWT Stateless e Proteção de Endpoints (RBAC)`
* **Tags:** `Sprint3, JavaAdvanced, Security, RBAC, SecurityFilterChain, JWT, CorsConfig`
* **Start Date:** `2026-08-25`
* **Target Date:** `2026-08-26`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Description:** Configuração da cadeia de filtros SecurityFilterChain em modo STATELESS com decodificação automática de JWT via OAuth2 Resource Server, suporte a CORS isolado no CorsConfig para o aplicativo Mobile e política Secure by Default.

#### 🔹 [PBI-07] Configuração do SecurityFilterChain Stateless, CORS Dedicado e Proteção Centralizada
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 04] Autorização, Filtro JWT Stateless e Proteção de Endpoints (RBAC)`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Tags:** `Sprint3, JavaAdvanced, SecurityFilterChain, Stateless, OAuth2ResourceServer, CORS`

##### Descrição (História de Usuário)
> **Como** Arquiteto de Software e Administrador de Segurança,  
> **Eu quero** configurar o `SecurityFilterChain` em modo Stateless com decodificação JWT e classe `CorsConfig` dedicada,  
> **Para que** o aplicativo React Native consiga consumir a API enviando o header `Authorization: Bearer <token>` sem bloqueios de CORS ou sessão no servidor.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] `SecurityConfig` configurado com `SessionCreationPolicy.STATELESS`, CSRF desabilitado e `headers.frameOptions.sameOrigin` para o console H2.
- [ ] `CorsConfig` dedicado liberando métodos (`GET`, `POST`, `PUT`, `PATCH`, `DELETE`, `OPTIONS`) e todos os headers.
- [ ] Configuração de Resource Server `.oauth2ResourceServer(oauth2 -> oauth2.jwt(...))` com conversão de claims para `ROLE_`.
- [ ] Whitelist de rotas públicas: `POST /login`, `POST /usuarios`, `/swagger-ui/**`, `/v3/api-docs/**`, `/h2-console/**`, `/actuator/**`.
- [ ] Todos os demais endpoints protegidos por padrão (`anyRequest().authenticated()`).

##### Tarefas Técnicas (Child Tasks)
* **Task 7.1:** [TASK-18] Implementar classe `CorsConfig` no pacote `config`. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Liberar origens e cabeçalhos para consumo pelo aplicativo Mobile e Swagger.
* **Task 7.2:** [TASK-19] Configurar `SecurityConfig` com `SecurityFilterChain` Stateless e OAuth2 Resource Server JWT. *(Activity: Development, Est: 2.5h)*
  * *Descrição:* Montar cadeia de filtros do Spring Security com autenticação Bearer.
* **Task 7.3:** [TASK-20] Validar requisições autenticadas com Bearer Token e bloqueio 401 para requisições não autenticadas. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Testar acesso a rotas privadas com e sem header Authorization.

---

#### 🔹 [PBI-08] Segurança Granular por Método (@EnableMethodSecurity) e Handlers 401/403
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 04] Autorização, Filtro JWT Stateless e Proteção de Endpoints (RBAC)`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, MethodSecurity, EnableMethodSecurity, RBAC`

##### Descrição (História de Usuário)
> **Como** Desenvolvedor Backend,  
> **Eu quero** habilitar segurança por método com `@EnableMethodSecurity`,  
> **Para que** a aplicação suporte autorizações granulares e retorne payloads JSON claros quando requisições não autorizadas forem recebidas.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] `@EnableMethodSecurity` habilitado no `SecurityConfig`.
- [ ] `JwtAuthenticationConverter` mapeia o claim `role` para a autoridade `ROLE_USER` / `ROLE_ADMIN`.
- [ ] Tratamento padronizado de 401 Unauthorized e 403 Forbidden sem stacktrace exposto.

##### Tarefas Técnicas (Child Tasks)
* **Task 8.1:** [TASK-21] Habilitar `@EnableMethodSecurity` e configurar `JwtAuthenticationConverter`. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Integrar conversão de roles do JWT para authorities do Spring Security.
* **Task 8.2:** [TASK-22] Validar respostas de erro 401 e 403 no Swagger e Postman. *(Activity: Testing, Est: 1.5h)*
  * *Descrição:* Testar tentativas de acesso com token expirado ou permissão insuficiente.

---

### 🏆 [FEATURE 05] Versionamento de Banco de Dados & Governança de Esquema (Flyway)
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`
* **Title:** `[FEATURE 05] Versionamento de Banco de Dados & Governança de Esquema (Flyway)`
* **Tags:** `Sprint3, JavaAdvanced, Flyway, Database, Governance, PetCentric`
* **Start Date:** `2026-08-26`
* **Target Date:** `2026-08-27`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Description:** Implementação de controle de versão de banco de dados relacional através do Flyway, estruturando tabelas Pet-Centric limpas (usuario, pet, usuario_pet, tarefa, status, endereco, bairro, cidade, estado, telefone).

#### 🔹 [PBI-09] Configuração do Flyway e Migração Inicial DDL Pet-Centric (V1)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 05] Versionamento de Banco de Dados & Governança de Esquema (Flyway)`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Tags:** `Sprint3, JavaAdvanced, Spring-Boot, Flyway, DDL, PetCentric`

##### Descrição (História de Usuário)
> **Como** Desenvolvedor Backend da equipe PetGuardian,  
> **Eu quero** configurar o Flyway no ecossistema Spring Boot com banco H2 Database e criar o script DDL inicial versionado (`V1__criar_tabelas_iniciais_petcentric.sql`),  
> **Para que** todas as tabelas do sistema sejam criadas com integridade referencial, constraints limpas e console interativo `/h2-console` para testes com o app Mobile.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Dependências `org.flywaydb:flyway-core` e `com.h2database:h2` configuradas no `build.gradle`.
- [ ] Propriedades do Spring configuradas para habilitar o Flyway e H2 (`spring.datasource.url=jdbc:h2:mem:petguardian`, `spring.h2.console.enabled=true`, `spring.flyway.enabled=true`, `spring.jpa.hibernate.ddl-auto=validate`).
- [ ] Script `V1__criar_tabelas_iniciais_petcentric.sql` criado em `src/main/resources/db/migration/` contendo:
  - Tabela `estado`, `cidade`, `bairro`, `endereco`, `telefone`.
  - Tabela `usuario` e tabela de relacionamento `usuario_endereco`.
  - Tabela `raca` e tabela `pet` (`nome`, `idade`, `raca_id_raca`, `porte`, `sexo`, `castrado`).
  - Tabela `usuario_pet` com chave composta (`usuario_id_usuario`, `pet_id_pet`) e flag `respon_princ` (boolean).
  - Tabela `status` e tabela `tarefa` (`titulo`, `descricao`, `pontos_tarefa`, `criacao`, `prazo`, `conclusao`, `status_id_status`, `pet_id_pet`, `usuario_id_usuario`).
- [ ] Definição explícita de PKs, FKs, restrições `NOT NULL` e integridade referencial.
- [ ] Inicialização da aplicação executa a migração no H2 com sucesso e cria a tabela `flyway_schema_history`.
- [ ] Console do H2 acessível em `/h2-console` para inspeção visual dos dados.

##### Tarefas Técnicas (Child Tasks)
* **Task 9.1:** [TASK-23] Configurar dependências do Flyway e H2 no `build.gradle` e `application.properties`. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Habilitar execução de migrações automáticas no startup da aplicação.
* **Task 9.2:** [TASK-24] Elaborar script DDL completo `V1__criar_tabelas_iniciais_petcentric.sql` com todas as tabelas do domínio limpo. *(Activity: Development, Est: 3.5h)*
  * *Descrição:* Escrever scripts SQL com PKs, FKs e restrições de integridade.
* **Task 9.3:** [TASK-25] Testar a inicialização do Spring Boot validando a criação da tabela `flyway_schema_history` e acesso ao `/h2-console`. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Checar histórico de versões e integridade dos esquemas criados.

---

#### 🔹 [PBI-10] Migrações Incrementais de Domínio, Status e Seeds de Segurança (V2 e V3)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 05] Versionamento de Banco de Dados & Governança de Esquema (Flyway)`
* **State:** `Approved`
* **Priority:** `2 - High`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, Flyway, SeedData`

##### Descrição (História de Usuário)
> **Como** Desenvolvedor Backend e Administrador do Sistema,  
> **Eu quero** criar migrações incrementais do Flyway para inserir sementes de dados de domínio (status, raças, endereços e usuários de teste),  
> **Para que** a aplicação já inicie pronta para testes e demonstração imediata no Mobile.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Script `V2__popular_status_e_racas.sql` insere status (`PENDENTE`, `CONCLUIDO`, `EXPIRADO`) e raças iniciais.
- [ ] Script `V3__adicionar_perfis_e_usuarios_iniciais.sql` insere usuários de teste com senhas hash BCrypt, telefones, endereços, pets e vínculos familiares.
- [ ] Migrações são idempotentes e executam sequencialmente.

##### Tarefas Técnicas (Child Tasks)
* **Task 10.1:** [TASK-26] Elaborar script `V2__popular_status_e_racas.sql` com constantes de status e raças. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Inserir registros de lookup para tarefas e animais.
* **Task 10.2:** [TASK-27] Elaborar script `V3__adicionar_perfis_e_usuarios_iniciais.sql` com seeds de usuários hash BCrypt e pets vinculados. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Cadastrar base de tutores, co-cuidadores e pets com senhas seguras.
* **Task 10.3:** [TASK-28] Validar integridade referencial e execução sequencial no banco de dados. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Executar banco limpo e verificar população correta das tabelas.

---

### 🏆 [FEATURE 06] Fluxo 1: Gestão de Pets, Rede Familiar e Co-Cuidadores (N:N)
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`
* **Title:** `[FEATURE 06] Fluxo 1: Gestão de Pets, Rede Familiar e Co-Cuidadores (N:N)`
* **Tags:** `Sprint3, JavaAdvanced, BusinessFlow, RedeCuidado, UsuarioPet, CoCuidadores, PetHistory, NonCRUD`
* **Start Date:** `2026-08-27`
* **Target Date:** `2026-08-28`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `4`
* **Description:** Implementação do primeiro fluxo de negócio complexo não-CRUD obrigatório: governança do Care Circle através do UsuarioPetController e UsuarioPetService, gestão de Responsável Principal, convite de co-cuidadores exclusivamente por e-mail, transferência de responsabilidade e agregação de alta performance da Rede de Cuidado com RedeCuidadoMapper.

#### 🔹 [PBI-11] [Fluxo 1.1] Gestão de Vínculos N:N, Responsável Principal e Convite de Co-Cuidadores por E-mail
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 06] Fluxo 1: Gestão de Pets, Rede Familiar e Co-Cuidadores (N:N)`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Tags:** `Sprint3, JavaAdvanced, UsuarioPetService, UsuarioPetController, CoCuidadores, ResponsavelPrincipal`

##### Descrição (História de Usuário)
> **Como** Tutor e Responsável Principal pelo Pet,  
> **Eu quero** cadastrar meus pets, definir quem é o responsável principal, convidar co-cuidadores por e-mail e transferir a titularidade,  
> **Para que** a rotina de cuidados do animal possa ser compartilhada de forma segura e colaborativa entre membros da família.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Na criação do pet (`POST /pets`), o tutor criador é automaticamente vinculado como `responsavelPrincipal = true`.
- [ ] Sub-recurso RESTful no `UsuarioPetController`:
  - `POST /pets/{petId}/cuidadores`: convida co-cuidador por e-mail (`CoCuidadorRequest`), validando se quem convida é o responsável principal.
  - `GET /pets/{petId}/cuidadores`: lista os co-cuidadores vinculados ao pet em `CoCuidadorResponse`.
  - `DELETE /pets/{petId}/cuidadores/{usuarioId}`: desvincula co-cuidador, impedindo desvinculação do responsável principal.
  - `PATCH /pets/{petId}/responsavel-principal`: transfere a titularidade principal com validação `@DiferentesUsuariosValidation`.
- [ ] Validações de domínio encapsuladas no `UsuarioPetValidator` sem poluir a camada de serviço com blocos condicionais.

##### Tarefas Técnicas (Child Tasks)
* **Task 11.1:** [TASK-29] Implementar entidade `UsuarioPet` com chave composta `@EmbeddedId` `UsuarioPetId` e queries com `JOIN FETCH` no repositório. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Mapear relacionamento N:N entre tutores e pets com atributos adicionais.
* **Task 11.2:** [TASK-30] Desenvolver `UsuarioPetService` e `UsuarioPetValidator` com regras de convite por e-mail, titularidade e transferência. *(Activity: Development, Est: 2.5h)*
  * *Descrição:* Isolar regras de negócio de permissão e convites colaborativos.
* **Task 11.3:** [TASK-31] Criar endpoints no `UsuarioPetController` e testar operações de vínculo familiar. *(Activity: Testing, Est: 1.5h)*
  * *Descrição:* Testar rotas REST de gerenciamento de co-cuidadores.

---

#### 🔹 [PBI-12] [Fluxo 1.2] Visualização Agregada da Rede de Cuidado (RedeCuidadoMapper) e Histórico do Pet
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 06] Fluxo 1: Gestão de Pets, Rede Familiar e Co-Cuidadores (N:N)`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Tags:** `Sprint3, JavaAdvanced, RedeCuidado, RedeCuidadoMapper, AntiNPlusOne, PetHistory`

##### Descrição (História de Usuário)
> **Como** Cuidador ou Tutor cadastrado,  
> **Eu quero** visualizar a rede completa de cuidado vinculada ao meu perfil e o histórico consolidado de tarefas cumpridas de cada pet,  
> **Para que** eu acompanhe todos os co-cuidadores, pets compartilhados, tarefas pendentes/concluídas e pontos acumulados de forma instantânea.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Endpoint `GET /usuarios/{id}/rede-cuidado` retorna DTO `RedeCuidadoResponse` com dados agregados de pets, co-cuidadores e métricas.
- [ ] Mapeamento e transformação desacoplados no componente `@Component` `RedeCuidadoMapper`.
- [ ] Eliminação de consultas N+1 de tarefas com consulta em lote no `TarefaRepository` (`findTarefaIdsByPetIdIn`) e agrupamento em memória.
- [ ] Endpoint `GET /pets/{id}/historico` retorna DTO `PetHistoryResponse` com a lista ordenada de tarefas concluídas do pet.

##### Tarefas Técnicas (Child Tasks)
* **Task 12.1:** [TASK-32] Implementar `RedeCuidadoMapper` puro com conversão funcional de `PetResumo` e `CuidadorResumo`. *(Activity: Development, Est: 2.5h)*
  * *Descrição:* Realizar agrupamento em memória e transformação de objetos de domínio para DTOs.
* **Task 12.2:** [TASK-33] Desenvolver query de carregamento em lote de tarefas no `TarefaRepository` e método `getConsolidatedHistory` no `PetService`. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Executar batch query anti-N+1 e montar histórico cronológico.
* **Task 12.3:** [TASK-34] Validar performance e integridade dos dados da Rede de Cuidado. *(Activity: Testing, Est: 1.5h)*
  * *Descrição:* Homologar tempo de resposta e consistência dos vínculos familiares.

---

### 🏆 [FEATURE 07] Fluxo 2: Gestão da Rotina do Pet, Expiração Atômica & Gamificação
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`
* **Title:** `[FEATURE 07] Fluxo 2: Gestão da Rotina do Pet, Expiração Atômica & Gamificação`
* **Tags:** `Sprint3, JavaAdvanced, BusinessFlow, Gamification, Tarefas, ExpiracaoAutomatica, Pontuacao, NonCRUD`
* **Start Date:** `2026-08-27`
* **Target Date:** `2026-08-28`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `4`
* **Description:** Implementação do segundo fluxo de negócio complexo não-CRUD obrigatório: gestão de tarefas da rotina do pet por cuidadores, mecanismo atômico de expiração automática de tarefas pendentes com prazo vencido, conclusão com atribuição de cuidador e cálculo de pontuação acumulada.

#### 🔹 [PBI-13] [Fluxo 2.1] Criação de Rotina Familiar e Mecanismo Atômico de Expiração Automática
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 07] Fluxo 2: Gestão da Rotina do Pet, Expiração Atômica & Gamificação`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Tags:** `Sprint3, JavaAdvanced, Rotina, Tarefas, ExpiracaoAutomatica, ModifyingQuery`

##### Descrição (História de Usuário)
> **Como** Tutor do pet,  
> **Eu quero** criar tarefas da rotina do animal (alimentação, passeio, medicação, escovação) com prazo e pontuação, contando com a expiração automática de tarefas vencidas,  
> **Para que** a família mantenha os cuidados em dia e tarefas não realizadas dentro do prazo expirem sem intervenção manual.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Criação de tarefas via `POST /tarefas` exige `petId`, `titulo`, `descricao`, `prazo` (no presente/futuro) e `pontosTarefa` (> 0).
- [ ] Tarefas devem ser criadas inicialmente sem usuário executor associado (`usuarioId` nulo) e status `PENDENTE`.
- [ ] Query atômica `@Modifying` no `TarefaRepository` atualiza em lote tarefas pendentes cujo prazo seja menor que `LocalDateTime.now()` para o status `EXPIRADO`.
- [ ] Expiração automática é executada antes de consultas (`findAll`, `findById`, `findByUsuarioId`) e antes de tentativas de conclusão.
- [ ] Endpoint `GET /tarefas/by-usuario?usuarioId={id}` retorna com paginação e ordenação por prazo apenas tarefas pendentes válidas dos pets vinculados ao cuidador.

##### Tarefas Técnicas (Child Tasks)
* **Task 13.1:** [TASK-35] Implementar endpoints de criação e listagem paginada no `TarefaController` e `TarefaService`. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Criar rotas de tarefas com filtros por usuário e status.
* **Task 13.2:** [TASK-36] Desenvolver query atômica `@Modifying` de expiração no `TarefaRepository` e orquestrar chamada no `TarefaService`. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Executar update em lote no banco para tarefas com prazo expirado.
* **Task 13.3:** [TASK-37] Testar cenários de criação, consulta paginada e transição automática para `EXPIRADO`. *(Activity: Testing, Est: 1.5h)*
  * *Descrição:* Validar expiração atômica antes de leituras e tentativas de conclusão.

---

#### 🔹 [PBI-14] [Fluxo 2.2] Conclusão de Tarefas, Registro de Executor e Sistema de Pontuação
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 07] Fluxo 2: Gestão da Rotina do Pet, Expiração Atômica & Gamificação`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Tags:** `Sprint3, JavaAdvanced, Gamification, ConclusaoTarefa, Pontuacao`

##### Descrição (História de Usuário)
> **Como** Cuidador ou Tutor vinculado ao pet,  
> **Eu quero** concluir tarefas pendentes registrando meu ID como executor e acumulando pontos,  
> **Para que** o histórico de cuidados do pet registre quem realizou a atividade e meu total de pontos seja somado.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Endpoint `PATCH /tarefas/{id}/concluir` recebe `TarefaConclusaoRequest` com `concluinteId`.
- [ ] Valida se o usuário que está concluindo é vinculado ao pet da tarefa (lança erro caso não pertença à rede de cuidado).
- [ ] Impede conclusão de tarefas que não estejam com status `PENDENTE` (tarefas já concluídas ou expiradas lançam `IllegalArgumentException` 400 Bad Request).
- [ ] Ao concluir, atualiza status para `CONCLUIDO`, registra o cuidador executor e a data/hora de conclusão (`LocalDateTime.now()`).
- [ ] Endpoint `GET /tarefas/by-usuario/pontos?usuarioId={id}` calcula a soma acumulada de pontos obtidos pelo usuário em tarefas concluídas via query com `coalesce(sum(t.pontosTarefa), 0)`.

##### Tarefas Técnicas (Child Tasks)
* **Task 14.1:** [TASK-38] Implementar método transacional `@Transactional` `concluir` no `TarefaService` com validações de vínculo e status. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Registrar conclusão de tarefa com validação estrita de segurança e integridade.
* **Task 14.2:** [TASK-39] Implementar query agregadora de pontos `calcularPontosTotaisUsuario` no `TarefaRepository`. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Somar pontos obtidos em atividades concluídas.
* **Task 14.3:** [TASK-40] Criar testes de validação do ciclo completo de vida da tarefa (criação ➔ expiração / conclusão ➔ pontos). *(Activity: Testing, Est: 2.0h)*
  * *Descrição:* Homologar ciclo de gamificação e concorrência na conclusão de rotinas.

---

### 🏆 [FEATURE 08] Documentação Técnica, Demonstração em Vídeo & Preparação para Banca Oral
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - Java Advanced: Evolução da Arquitetura Spring Boot, Segurança JWT e Governança Pet-Centric`
* **Title:** `[FEATURE 08] Documentação Técnica, Demonstração em Vídeo & Preparação para Banca Oral`
* **Tags:** `Sprint3, JavaAdvanced, Documentation, Video, OralExam, Delivery`
* **Start Date:** `2026-08-28`
* **Target Date:** `2026-08-29`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Description:** Elaboração da documentação técnica no README, gravação do vídeo demonstrativo de até 10 minutos e consolidação da matriz de argumentação para a avaliação oral individual da disciplina.

#### 🔹 [PBI-15] Atualização Completa do README.md e Guia de Execução (Gradle)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 08] Documentação Técnica, Demonstração em Vídeo & Preparação para Banca Oral`
* **State:** `Approved`
* **Priority:** `2 - High`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, Documentation, README, Gradle`

##### Descrição (História de Usuário)
> **Como** Professor avaliador e Desenvolvedor da equipe,  
> **Eu quero** um README.md completo e organizado no repositório GitHub,  
> **Para que** qualquer pessoa consiga clonar, compilar via Gradle, rodar a aplicação, autenticar e testar os fluxos de rede de cuidado e rotina de tarefas.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Pré-requisitos documentados (JDK 21, Gradle 8.12+).
- [ ] Passo a passo de build e execução (`.\gradlew.bat bootRun` / `./gradlew bootRun` / `./gradlew compileJava`).
- [ ] Tabela com credenciais de teste e descrição de perfis.
- [ ] Documentação dos endpoints principais, URLs do Swagger UI, H2 Console e Actuator.
- [ ] Descrição arquitetural dos princípios SOLID, Clean Code, Spring Security com RSA 2048, Flyway e dos 2 fluxos complexos.

##### Tarefas Técnicas (Child Tasks)
* **Task 15.1:** [TASK-41] Atualizar introdução, arquitetura, stack tecnológica Gradle e princípios SOLID no `README.md`. *(Activity: Documentation, Est: 1.5h)*
  * *Descrição:* Documentar visão geral e boas práticas aplicadas no projeto.
* **Task 15.2:** [TASK-42] Documentar credenciais de teste, instruções de execução Gradle e links do Swagger/H2. *(Activity: Documentation, Est: 1.5h)*
  * *Descrição:* Criar instruções de inicialização rápida para a banca avaliadora.

---

#### 🔹 [PBI-16] Roteiro e Gravação do Vídeo Demonstrativo da Aplicação (Máx. 10 min)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 08] Documentação Técnica, Demonstração em Vídeo & Preparação para Banca Oral`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, Video, Demonstracao`

##### Descrição (História de Usuário)
> **Como** Equipe do Projeto PetGuardian,  
> **Eu quero** gravar e publicar um vídeo de até 10 minutos narrado demonstrando a aplicação em tempo real,  
> **Para que** o professor valide a execução de todos os requisitos obrigatórios da 3ª Sprint.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Vídeo com no máximo 10 minutos de duração, resolução nítida e áudio claro narrado pelos integrantes.
- [ ] Demonstração do Spring Security e autenticação JWT.
- [ ] Demonstração prática do Fluxo 1 (Pets, Co-Cuidadores e Rede de Cuidado).
- [ ] Demonstração prática do Fluxo 2 (Criação de Tarefas, Expiração Automática, Conclusão e Pontuação).
- [ ] Demonstração de validação de formulários (Bean Validation) e tratamento amigável de exceções no Global Exception Handler.
- [ ] Link público/não-listado do YouTube adicionado ao `README.md`.

##### Tarefas Técnicas (Child Tasks)
* **Task 16.1:** [TASK-43] Criar roteiro técnico sequencial para gravação do vídeo (Timebox: 10 min). *(Activity: Documentation, Est: 1.5h)*
  * *Descrição:* Estruturar ordem de apresentação das rotas e fluxos.
* **Task 16.2:** [TASK-44] Gravar a demonstração prática da aplicação (Security + Fluxos + Validações + Swagger). *(Activity: Documentation, Est: 2.5h)*
  * *Descrição:* Demonstrar endpoints e respostas em tempo real.
* **Task 16.3:** [TASK-45] Editar, realizar upload no YouTube e adicionar o link ao `README.md`. *(Activity: Deployment, Est: 1.5h)*
  * *Descrição:* Publicar e fixar URL de acesso no repositório.

---

#### 🔹 [PBI-17] Matriz de Argumentação e Preparação para a Avaliação Oral Individual
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 08] Documentação Técnica, Demonstração em Vídeo & Preparação para Banca Oral`
* **State:** `Approved`
* **Priority:** `2 - High`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, JavaAdvanced, AvaliacaoOral, Estudo`

##### Descrição (História de Usuário)
> **Como** Integrante da equipe PetGuardian,  
> **Eu quero** revisar os trechos de código, decisões técnicas, Clean Code e justificativas arquiteturais,  
> **Para que** eu esteja plenamente preparado para responder individualmente às perguntas do professor na banca avaliativa.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Cada integrante domina a explicação de trechos específicos do código (SecurityFilterChain, Streams/groupingBy, RedeCuidadoMapper, GlobalExceptionHandler, Services transacionais, ViaCEP).
- [ ] Justificativas claras formuladas para as decisões de design (por que DTOs Records, por que RSA 2048-bit, por que injeção por construtor, por que `@Transactional` seletivo).
- [ ] Mapeamento das dificuldades encontradas e soluções implementadas.

##### Tarefas Técnicas (Child Tasks)
* **Task 17.1:** [TASK-46] Elaborar documento de apoio com perguntas frequentes e justificativas de arquitetura do projeto. *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Consolidar argumentos sobre decisões de design e SOLID.
* **Task 17.2:** [TASK-47] Realizar rodada interna de simulação da avaliação oral entre os membros do grupo. *(Activity: Testing, Est: 1.5h)*
  * *Descrição:* Praticar respostas e defesa técnica do código-fonte.

