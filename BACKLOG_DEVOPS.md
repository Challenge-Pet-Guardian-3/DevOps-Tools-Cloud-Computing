# 📋 Backlog Master Azure Boards — Sprint 3: DevOps Tools & Cloud Computing

> **Projeto Integrado:** PetGuardian / Clyvo Care (Challenge FIAP 2026 - 2º Ano ADS / 2TDSPG)  
> **Disciplina:** DevOps Tools & Cloud Computing (FIAP — 2TDSPG)  
> **Epic Principal:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Containerizada (Java + PostgreSQL no ACR + ACI)`  
> **Start Date:** `2026-09-02`  
> **Target Date:** `2026-09-12`  
> **Aplicação Escolhida:** Java Spring Boot 4.1.1 (`pet-guardian`) | **Banco em Nuvem:** PostgreSQL 16 containerizado no ACI com Azure Files  
> **Padrão:** Azure Boards (Scrum Process: Epic ➔ Feature ➔ PBI ➔ Task)  
> **Diretrizes Estratégicas:** 100% Provisionamento via Azure CLI, Container multi-stage com usuário non-root (`USER appuser`), diagrama com ícones oficiais Azure, persistência comprovada via SELECT, dados sensíveis via variáveis de ambiente.

---

## 🎯 1. Matriz de Requisitos & Critérios de Avaliação Oficiais (Páginas 10 a 16)

| Critério Avaliativo | Implementação do Grupo | Regras Críticas e Penalidades do Edital |
| :--- | :--- | :--- |
| **Aplicação Alvo** | **Java Spring Boot 4.1.1 (`pet-guardian`)** | CRUD completo de 2+ entidades relacionadas (`Pet`, `Tarefa`). |
| **Computação da Aplicação** | **Azure Container Instances (ACI)** executando imagem do ACR | **-40 pontos** se não containerizado. |
| **Registro de Imagens** | **Azure Container Registry (ACR)** privado com autenticação | Provisionamento 100% automatizado em linha de comando. |
| **Banco de Dados em Nuvem** | **Container PostgreSQL 16 no ACI** com volume Azure Files | Volume montado via Azure Storage File Share para persistência. |
| **Segurança do Container** | Container da API Java **NÃO roda como root** (`USER appuser`) | **-10 pontos** se o container rodar com permissões de administrador. |
| **Provisionamento** | **100% via Azure CLI** (`az group`, `az acr`, `az container`) | **-30 pontos** se criado manualmente pelo portal web. |
| **Diagrama de Arquitetura** | **Diagrama Oficial Cloud Azure** (Draw.io com ícones Azure) | **-20 pontos** se parecer fluxograma, TOGAF ou diagrama UML. |
| **DDL do Banco** | `script_bd.sql` com tabelas CORE (`pet`, `tarefa`, `usuario_pet`, `historico`) e comentários | **-10 pontos** se não incluir o DDL. |
| **Dados Sensíveis** | Variáveis de ambiente com `export` — sem senha no código | **-20 pontos** se dados sensíveis estiverem expostos no código-fonte. |
| **Vídeo e Evidências** | Vídeo contínuo narrado em 720p+ com queries `SELECT` após cada operação CRUD | **-30 pontos** se não comprovar mutações do CRUD via `SELECT`. |

---

## 🌳 2. Estrutura Hierárquica no Azure Boards

```text
[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura Containerizada Java + PostgreSQL (ACR + ACI)
│
├── 🏆 [FEATURE 01] Engenharia de Containers & Segurança da Aplicação Java
│   ├── 📄 [PBI-01] Dockerfile Multi-Stage Java com Usuário Sem Privilégios (Non-Root) (3 pts)
│   │   ├── 🔹 Task 1.1: Criar Dockerfile multi-stage (gradle:build + eclipse-temurin:runtime) (2.0h)
│   │   ├── 🔹 Task 1.2: Configurar usuário não-root (appuser) e permissões (1.0h)
│   │   └── 🔹 Task 1.3: Testar build local e validar execução dos endpoints (1.0h)
│   ├── 📄 [PBI-02] DDL do Banco CORE com Comentários PostgreSQL (script_bd.sql) (2 pts)
│   │   ├── 🔹 Task 2.1: Extrair DDL das tabelas CORE (pet, tarefa, usuario_pet, historico) (1.0h)
│   │   └── 🔹 Task 2.2: Adicionar COMMENT ON TABLE e COMMENT ON COLUMN em todas as tabelas (1.0h)
│   └── 📄 [PBI-03] Validação do CRUD Java de 2+ Tabelas Relacionadas (Pet e Tarefa) (1 pt)
│       ├── 🔹 Task 3.1: Testar rotas de CRUD completo no Swagger (POST, GET, PUT, DELETE) (2.0h)
│       └── 🔹 Task 3.2: Validar persistência física com queries SELECT no PostgreSQL (2.0h)
│
├── 🏆 [FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI
│   ├── 📄 [PBI-04] Script Azure CLI para Resource Group e Azure Container Registry (ACR) (3 pts)
│   │   ├── 🔹 Task 4.1: Escrever az group create e az acr create no script.sh (1.5h)
│   │   └── 🔹 Task 4.2: Testar execução do script na Azure e login no ACR (1.5h)
│   ├── 📄 [PBI-05] Script Azure CLI para Container PostgreSQL no ACI com Volume (1 pt)
│   │   ├── 🔹 Task 5.1: Criar Storage Account e File Share via CLI (2.0h)
│   │   ├── 🔹 Task 5.2: Escrever az container create para banco PostgreSQL com volume montado (2.0h)
│   │   └── 🔹 Task 5.3: Validar conectividade e inicialização do banco no ACI (1.0h)
│   └── 📄 [PBI-06] Script Azure CLI para Build, Push e Deploy da API Java no ACI (1 pt)
│       ├── 🔹 Task 6.1: Implementar docker build e push para o ACR (2.0h)
│       ├── 🔹 Task 6.2: Implementar az container create para a API com variáveis PGHOST/PGPORT/PGDATABASE (2.0h)
│       └── 🔹 Task 6.3: Testar acesso público ao Swagger e comunicação com o banco (1.0h)
│
├── 🏆 [FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub
│   ├── 📄 [PBI-07] Diagrama Macro de Arquitetura Cloud Azure com Ícones Oficiais (1 pt)
│   │   ├── 🔹 Task 7.1: Mapear componentes Azure (ACR, ACI API, ACI DB, Azure Files, VNet) (2.0h)
│   │   ├── 🔹 Task 7.2: Desenhar diagrama no Draw.io com ícones Azure e fluxos numerados (2.0h)
│   │   └── 🔹 Task 7.3: Exportar imagem em alta resolução e incorporar no README.md (1.0h)
│   └── 📄 [PBI-08] Reestruturação Completa do README.md (How-To, Comandos, Benefícios e DDL) (5 pts)
│       ├── 🔹 Task 8.1: Redigir seções de Apresentação, Benefícios e Arquitetura (Java/PostgreSQL) (1.0h)
│       ├── 🔹 Task 8.2: Elaborar roteiro How-To de deploy com comandos Azure CLI e Docker (2.0h)
│       └── 🔹 Task 8.3: Adicionar guia de testes com rotas reais do Java e queries SQL SELECT (1.0h)
│
└── 🏆 [FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial
    ├── 📄 [PBI-09] Roteiro e Execução dos Testes com Evidência de Persistência via SELECT (1 pt)
    │   ├── 🔹 Task 9.1: Elaborar roteiro de teste com payloads JSON e queries SQL correspondentes (2.0h)
    │   └── 🔹 Task 9.2: Realizar ensaio completo contra containers online na Azure (2.0h)
    └── 📄 [PBI-10] Gravação do Vídeo Explicativo por Voz (720p+) e Geração do PDF Oficial (1 pt)
        ├── 🔹 Task 10.1: Preparar ambiente de gravação (terminal, VS Code, Portal Azure, Swagger) (2.0h)
        ├── 🔹 Task 10.2: Gravar demonstração em alta definição narrando detalhadamente cada etapa (2.0h)
        └── 🔹 Task 10.3: Gerar PDF com nomes completos e RMs em ordem alfabética + links oficiais (2.0h)
```

---

## 📊 3. Tabela Resumo do Backlog

| Feature Pai | ID do PBI | Título do Item de Backlog (PBI) | Story Points | Prioridade | Horas Estimadas |
| :--- | :--- | :--- | :---: | :---: | :---: |
| **[FEATURE 01] Containers & App Java** | **PBI-01** | Dockerfile Multi-Stage Java com Non-Root (appuser) | 1 pts | 1 - Critical | 4.0h |
| | **PBI-02** | DDL CORE PostgreSQL com Comentários (`script_bd.sql`) | 1 pts | 1 - Critical | 2.0h |
| | **PBI-03** | Validação CRUD Java em 2+ Tabelas Relacionadas (Pet + Tarefa) | 1 pts | 1 - Critical | 4.0h |
| **[FEATURE 02] Azure CLI IaC** | **PBI-04** | Script Azure CLI para Resource Group e ACR | 1 pts | 1 - Critical | 3.0h |
| | **PBI-05** | Script Azure CLI para Container PostgreSQL no ACI + Volume | 1 pts | 1 - Critical | 5.0h |
| | **PBI-06** | Script Azure CLI para Deploy da API Java no ACI | 1 pts | 1 - Critical | 5.0h |
| **[FEATURE 03] Arquitetura & Docs** | **PBI-07** | Diagrama de Arquitetura Cloud Azure Oficial com Ícones | 1 pts | 1 - Critical | 5.0h |
| | **PBI-08** | README.md Completo (How-To, Benefícios, Comandos, Testes) | 1 pts | 1 - Critical | 4.0h |
| **[FEATURE 04] Vídeo & Entrega** | **PBI-09** | Bateria de Testes do CRUD com Validação por SELECT | 1 pts | 1 - Critical | 4.0h |
| | **PBI-10** | Gravação do Vídeo sem Cortes e PDF Oficial de Entrega | 1 pts | 1 - Critical | 6.0h |
| **TOTAL CONSOLIDADO** | **4 Features** | **10 PBIs / 26 Child Tasks Técnicas** | **10 pts** | — | **42.0h** |

---

## 📦 4. Detalhamento dos Itens de Trabalho

---

### 🏛️ ÉPICO
* **Work Item Type:** `Epic`
* **Title:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura Containerizada Java + PostgreSQL (ACR + ACI)`
* **Tags:** `Sprint3, DevOps, AzureCLI, ACR, ACI, Docker, Java, PostgreSQL, Security, NonRoot, IaC`
* **Start Date:** `2026-09-02`
* **Target Date:** `2026-09-12`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `10`
* **Business Value:** `100`
* **Description:** Evolução completa da infraestrutura em nuvem na Microsoft Azure provisionando arquitetura 100% containerizada (Opção 1: ACR + ACI) com aplicação Java Spring Boot 4.1.1 e banco PostgreSQL 16 no Azure Container Instances (ACI), imagens no Azure Container Registry (ACR), Dockerfile multi-stage com usuário não privilegiado, scripts automatizados via Azure CLI e validação do CRUD via consultas SELECT.

---

### 🏆 [FEATURE 01] Engenharia de Containers & Segurança da Aplicação Java

* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing`
* **Title:** `[FEATURE 01] Engenharia de Containers & Segurança da Aplicação Java`
* **Tags:** `Sprint3, DevOps, Containers, Docker, Security, NonRoot, Java, SpringBoot`
* **Start Date:** `2026-09-02`
* **Target Date:** `2026-09-04`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Description:** Criação do Dockerfile multi-stage da aplicação Java Spring Boot com usuário não privilegiado (non-root), DDL das tabelas CORE com comentários e validação do CRUD em 2+ tabelas relacionadas.

#### 🔹 [PBI-01] Dockerfile Multi-Stage Java com Usuário Sem Privilégios (Non-Root)

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 01] Engenharia de Containers & Segurança da Aplicação Java`
* **State:** `Done`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Docker, Java, Security, NonRoot`

##### Descrição (História de Usuário)
> **Como** engenheiro DevOps,  
> **Eu quero** criar o `Dockerfile` multi-stage da aplicação Java configurando um usuário não privilegiado (`USER appuser`),  
> **Para que** a imagem containerizada execute em conformidade com as práticas de segurança em nuvem e atenda à exigência do edital Sprint 3 (evitando desconto de -10 pts).

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [x] Build multi-stage implementado com estágios `build` (gradle:8.12-jdk17) e `runtime` (eclipse-temurin:17-jre-alpine).
- [x] Criação explícita de usuário e grupo sem permissões administrativas (`addgroup -S appgroup && adduser -S appuser -G appgroup`).
- [x] Instrução `USER appuser` declarada antes do `ENTRYPOINT`.
- [x] Container compila e executa sem erros de permissão na porta 8091.

##### Tarefas Técnicas (Child Tasks)
* **Task 1.1:** [TASK-01] Criar `Dockerfile` multi-stage (gradle build + eclipse-temurin runtime). *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Separar estágio de compilação SDK do estágio leve de runtime.
* **Task 1.2:** [TASK-02] Configurar usuário não-root (`appuser`) e ajustar permissões no container. *(Activity: Development, Est: 1.0h)*
  * *Descrição:* Adicionar diretiva `USER appuser` e validar execução sem root.
* **Task 1.3:** [TASK-03] Testar build local e validar execução de endpoints pelo container. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Rodar `docker build` e `docker run -p 8091:8091` validando Swagger e logs.

---

#### 🔹 [PBI-02] DDL do Banco CORE com Comentários PostgreSQL (script_bd.sql)

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 01] Engenharia de Containers & Segurança da Aplicação Java`
* **State:** `Done`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, PostgreSQL, DDL, Database`

##### Descrição (História de Usuário)
> **Como** arquiteto de banco de dados,  
> **Eu quero** gerar um arquivo de script DDL chamado `script_bd.sql` contendo a definição e comentários das tabelas CORE (`pet`, `tarefa`, `usuario_pet`, `historico`),  
> **Para que** a infraestrutura do banco seja recriada facilmente no container e cumpra o requisito obrigatório da página 11 do edital (evitando desconto de -10 pts).

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [x] Arquivo `script_bd.sql` criado na raiz do repositório DevOps.
- [x] Contém o DDL completo das tabelas CORE representativas do negócio (`pet`, `tarefa`, `usuario_pet`, `historico`). Tabelas genéricas (cidade, estado, bairro) excluídas conforme item 3.4 do edital.
- [x] Chaves primárias (PK), chaves estrangeiras (FK) e constraints devidamente declaradas.
- [x] `COMMENT ON TABLE` e `COMMENT ON COLUMN` documentando o propósito de cada entidade e atributo.

##### Tarefas Técnicas (Child Tasks)
* **Task 2.1:** [TASK-04] Extrair DDL das tabelas CORE do PetGuardian em `script_bd.sql`. *(Activity: Development, Est: 1.0h)*
  * *Descrição:* Usar as entidades centrais do modelo e validar sintaxe PostgreSQL.
* **Task 2.2:** [TASK-05] Adicionar `COMMENT ON TABLE` e `COMMENT ON COLUMN` em todas as tabelas e colunas CORE. *(Activity: Documentation, Est: 1.0h)*
  * *Descrição:* Incluir metadados que expliquem o propósito de cada elemento do schema.

---

#### 🔹 [PBI-03] Validação do CRUD Java de 2+ Tabelas Relacionadas (Pet e Tarefa)

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 01] Engenharia de Containers & Segurança da Aplicação Java`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Java, API, CRUD, PostgreSQL`

##### Descrição (História de Usuário)
> **Como** desenvolvedor e integrador,  
> **Eu quero** validar que a API Java Spring Boot conectada ao banco PostgreSQL possua operações completas de `Create`, `Read`, `Update` (`PUT`) e `Delete` nas entidades `Pet` e `Tarefa`,  
> **Para que** todas as operações possam ser demonstradas e comprovadas via queries `SELECT` durante o vídeo de avaliação.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] CRUD 100% funcional sobre as entidades `Pet` e `Tarefa` (relacionadas entre si).
- [ ] Endpoints `POST`, `GET`, `PUT/PATCH` e `DELETE` operando sem erros e persistindo no banco PostgreSQL.
- [ ] Inserção e manipulação de pelo menos 2 registros com conteúdo significativo em cada tabela.
- [ ] Cada operação verificada com SELECT imediato no banco dentro do container ACI.

##### Tarefas Técnicas (Child Tasks)
* **Task 3.1:** [TASK-06] Testar rotas de CRUD completo da API Java no Swagger. *(Activity: Testing, Est: 2.0h)*
  * *Descrição:* Executar POST, GET, PUT, PATCH, DELETE em /pets e /tarefas.
* **Task 3.2:** [TASK-07] Validar persistência física das alterações no banco PostgreSQL com queries SQL. *(Activity: Testing, Est: 2.0h)*
  * *Descrição:* Conferir atualização dos registros diretamente nas tabelas via `psql` no container ACI.

---

### 🏆 [FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing`
* **Title:** `[FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI`
* **Tags:** `Sprint3, DevOps, AzureCLI, IaC, CloudInfrastructure`
* **Start Date:** `2026-09-04`
* **Target Date:** `2026-09-06`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Description:** Automação em Azure CLI para criação do Resource Group, ACR, container PostgreSQL no ACI com volume e deploy da API Java Spring Boot.

#### 🔹 [PBI-04] Script Azure CLI para Resource Group e Azure Container Registry (ACR)

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `Done`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, AzureCLI, ACR, IaC, Cloud`

##### Descrição (História de Usuário)
> **Como** engenheiro de nuvem,  
> **Eu quero** criar um script shell automatizado em Azure CLI para criar o Resource Group e o registro privado ACR,  
> **Para que** a infraestrutura base de nuvem seja provisionada de forma reproduzível 100% em linha de comando.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [x] Comando `az group create --name rg-petguardian --location brazilsouth`.
- [x] Comando `az acr create --resource-group rg-petguardian --name acrpetguardian --sku Basic --admin-enabled true`.
- [x] Script documentado com comentários explicando cada etapa.
- [x] Variáveis sensíveis (senha do banco) via `export` — não hardcoded no script.

##### Tarefas Técnicas (Child Tasks)
* **Task 4.1:** [TASK-08] Escrever comandos de criação do Resource Group e ACR em `script.sh`. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Criar arquivo shell de automação em bash POSIX válido.
* **Task 4.2:** [TASK-09] Testar execução do script na assinatura Azure e autenticação no ACR. *(Activity: Deployment, Est: 1.5h)*
  * *Descrição:* Validar execução sem intervenção manual pelo Azure CLI.

---

#### 🔹 [PBI-05] Script Azure CLI para Container PostgreSQL no ACI com Volume

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `Done`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, AzureCLI, ACI, PostgreSQL, VolumeStorage`

##### Descrição (História de Usuário)
> **Como** arquiteto de infraestrutura,  
> **Eu quero** provisionar um container PostgreSQL 16 no Azure Container Instances com volume montado no Azure Storage Account,  
> **Para que** o banco execute 100% containerizado sem violar a regra de mistura de opções da Sprint 3 (penalidade de -40 pts).

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [x] Criação de Storage Account e File Share via CLI (`az storage account create`, `az storage share create`).
- [x] Deploy do container PostgreSQL no ACI com `az container create` montando `/var/lib/postgresql/data` via Azure Files.
- [x] Variáveis de ambiente `POSTGRES_DB`, `POSTGRES_USER`, `POSTGRES_PASSWORD` injetadas sem expor no código-fonte.

##### Tarefas Técnicas (Child Tasks)
* **Task 5.1:** [TASK-10] Criar Storage Account e File Share para persistência do banco. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Criar compartilhamento de arquivos no Azure Files para armazenamento persistente.
* **Task 5.2:** [TASK-11] Escrever comando `az container create` para PostgreSQL com variáveis de ambiente e volume. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Configurar credenciais do banco e ponto de montagem do Azure Files.
* **Task 5.3:** [TASK-12] Validar conectividade e inicialização do banco PostgreSQL no container. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Testar conexão remota na porta 5432 via `az container exec`.

---

#### 🔹 [PBI-06] Script Azure CLI para Build, Push e Deploy da API Java no ACI

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `Done`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, AzureCLI, ACI, DockerDeploy, Java, SpringBoot`

##### Descrição (História de Usuário)
> **Como** engenheiro DevOps,  
> **Eu quero** automatizar o build da imagem Java com `docker build` e o provisionamento do container da aplicação no ACI com IP público e variáveis de conexão PostgreSQL,  
> **Para que** a API Java fique acessível publicamente na internet e conectada ao container de banco.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [x] Autenticação no ACR via `az acr login`.
- [x] Build e push da imagem Java para o ACR privado.
- [x] Deploy do container da API no ACI com variáveis `PGHOST`, `PGPORT`, `PGDATABASE`, `PGUSER`, `PGPASSWORD`.
- [x] Porta pública 8091 exposta com DNS name label configurado.
- [x] API respondendo no Swagger em `http://<FQDN_DA_API>:8091/swagger-ui/index.html`.

##### Tarefas Técnicas (Child Tasks)
* **Task 6.1:** [TASK-13] Implementar comandos de build e envio da imagem Docker para o ACR. *(Activity: Deployment, Est: 2.0h)*
  * *Descrição:* Automatizar o processo de docker build, tag e push da imagem Java no registro privado.
* **Task 6.2:** [TASK-14] Implementar `az container create` para a API com variáveis de conexão PostgreSQL. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Configurar PGHOST/PGPORT/PGDATABASE/PGUSER/PGPASSWORD como environment variables.
* **Task 6.3:** [TASK-15] Testar acesso público ao Swagger e comunicação ponta a ponta com o banco. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Executar requisições de teste HTTP no IP público da API e verificar persistência.

---

### 🏆 [FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing`
* **Title:** `[FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **Tags:** `Sprint3, DevOps, CloudArchitecture, Drawio, Documentation, README`
* **Start Date:** `2026-09-06`
* **Target Date:** `2026-09-09`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Description:** Diagramação de nuvem Azure oficial com ícones Microsoft e fluxos numerados, e documentação técnica completa no README.md com How-To, rotas da API Java, comandos Docker/CLI e queries SQL de evidência.

#### 🔹 [PBI-07] Diagrama Macro de Arquitetura Cloud Azure com Ícones Oficiais

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **State:** `Done`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, ArchitectureDiagram, Drawio, AzureIcons`

##### Descrição (História de Usuário)
> **Como** arquiteto de soluções em nuvem,  
> **Eu quero** criar o diagrama de arquitetura Azure oficial no Draw.io com ícones Microsoft, personas e fluxos numerados,  
> **Para que** a solução atenda rigorosamente ao critério das páginas 13 e 15 (evitando penalidade de -20 pts para diagramas em formato de fluxo ou UML).

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [x] Diagrama desenhado no Draw.io utilizando estritamente **ícones oficiais da Azure Cloud**.
- [x] Inclusão de Resource Group, ACR, ACI API Java (8091), ACI PostgreSQL (5432), Azure Files Volume.
- [x] Exportação em `docs/sprint-3.jpeg` e incorporado ao `README.md`.

##### Tarefas Técnicas (Child Tasks)
* **Task 7.1:** [TASK-16] Mapear componentes, portas e interações entre os serviços Azure. *(Activity: Design, Est: 2.0h)*
  * *Descrição:* Estruturar a topologia de rede e fluxos de comunicação.
* **Task 7.2:** [TASK-17] Desenhar o diagrama oficial no Draw.io com ícones Azure e legendas. *(Activity: Design, Est: 2.0h)*
  * *Descrição:* Diagramar com ícones corporativos Microsoft e setas explicativas numeradas.
* **Task 7.3:** [TASK-18] Exportar imagem em alta resolução e incorporar no `README.md`. *(Activity: Documentation, Est: 1.0h)*
  * *Descrição:* Validar visualização da imagem no repositório GitHub.

---

#### 🔹 [PBI-08] Reestruturação Completa do README.md

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **State:** `Done`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Documentation, README, GitHub`

##### Descrição (História de Usuário)
> **Como** avaliador e desenvolvedor,  
> **Eu quero** um README.md completo contendo descrição da solução Java, benefícios para o negócio, comandos Docker/CLI e passo a passo de deploy,  
> **Para que** qualquer pessoa consiga reproduzir o deploy e validar a aplicação (evitando penalidade de -30 pts).

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [x] Descrição da Solução e Benefícios para o Negócio.
- [x] Desenho Macro da Arquitetura embutido com legenda.
- [x] Documentação das rotas reais da API Java em tabela estruturada.
- [x] Guia How-To com comandos Azure CLI e Docker (`build`, `tag`, `push`, `run`).
- [x] Queries SQL `SELECT` para evidência de persistência de cada operação CRUD.
- [x] Checklist de conformidade com o edital.

##### Tarefas Técnicas (Child Tasks)
* **Task 8.1:** [TASK-19] Redigir seções de Apresentação, Domínio, Benefícios e Arquitetura (Java/PostgreSQL). *(Activity: Documentation, Est: 1.0h)*
  * *Descrição:* Estruturar markdown com tabelas e badges atualizados.
* **Task 8.2:** [TASK-20] Elaborar roteiro How-To de deploy com comandos CLI e Docker. *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Documentar comandos sequenciais de execução do script.sh.
* **Task 8.3:** [TASK-21] Adicionar guia de testes com rotas reais do Java e queries SQL SELECT. *(Activity: Documentation, Est: 1.0h)*
  * *Descrição:* Listar consultas PostgreSQL formatadas para conferência de cada operação CRUD.

---

### 🏆 [FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing`
* **Title:** `[FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial`
* **Tags:** `Sprint3, DevOps, Video, Demonstration, PDF, Delivery`
* **Start Date:** `2026-09-09`
* **Target Date:** `2026-09-12`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Description:** Execução de ensaios de persistência e gravação do vídeo demonstrativo contínuo sem cortes com narração por voz, cobrindo todo o fluxo do README.md.

#### 🔹 [PBI-09] Roteiro e Execução dos Testes com Evidência de Persistência via SELECT

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Testing, CRUD, SELECT`

##### Descrição (História de Usuário)
> **Como** analista de QA e DevOps,  
> **Eu quero** preparar e executar um roteiro de testes cobrindo todas as operações do CRUD e validando cada alteração com um comando `SELECT` no banco,  
> **Para que** a persistência em nuvem seja demonstrada de forma inquestionável no vídeo (evitando penalidade de -30 pts).

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Roteiro estruturado: Inserção ➔ `SELECT`, Atualização (`PUT`) ➔ `SELECT`, Exclusão (`DELETE`) ➔ `SELECT`, Consulta ➔ `SELECT`.
- [ ] Evidência clara da persistência no banco PostgreSQL executando no ACI.
- [ ] Pelo menos 2 registros com conteúdo significativo em `pet` e `tarefa`.

##### Tarefas Técnicas (Child Tasks)
* **Task 9.1:** [TASK-22] Elaborar roteiro de teste com payloads JSON e queries SQL correspondentes. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Organizar sequência de requisições curl e conferências SQL para as entidades Pet e Tarefa.
* **Task 9.2:** [TASK-23] Realizar ensaio completo do teste contra os containers online na Azure. *(Activity: Testing, Est: 2.0h)*
  * *Descrição:* Validar respostas HTTP em tempo real no ambiente de produção containerizado.

---

#### 🔹 [PBI-10] Gravação do Vídeo Explicativo por Voz (720p+) e Geração do PDF Oficial

* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Video, PDF, Delivery`

##### Descrição (História de Usuário)
> **Como** equipe Pet Guardian,  
> **Eu quero** gravar um vídeo contínuo narrado demonstrando: clone do repositório → script Azure CLI → deploy no ACI → testes do CRUD com SELECT no banco, e gerar o PDF oficial da entrega,  
> **Para que** a entrega cumpra 100% das normas avaliativas da Sprint 3.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Vídeo em resolução mínima de 720p com áudio claro e narração por voz (sem uso de legendas).
- [ ] Sequência obrigatória: clone do GitHub → execução do `script.sh` → criação de recursos na Azure → deploy → testes do CRUD com `SELECT` no banco (sem cortes).
- [ ] Link do YouTube em modo Não Listado inserido no `README.md`.
- [ ] Arquivo PDF gerado contendo **exclusivamente**: nomes completos e RMs em **ordem alfabética**, link do GitHub e link do YouTube.

##### Tarefas Técnicas (Child Tasks)
* **Task 10.1:** [TASK-24] Preparar ambiente de gravação (terminal, VS Code, Portal Azure e Swagger abertos). *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Ensaiar a sequência: Clone ➔ `export` variáveis ➔ `script.sh` ➔ Portal Azure ➔ Swagger ➔ `psql` SELECT.
* **Task 10.2:** [TASK-25] Gravar a demonstração em alta definição narrando detalhadamente cada etapa. *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Gravar sem cortes nas etapas de testes e persistência e publicar no YouTube como Não Listado.
* **Task 10.3:** [TASK-26] Gerar o documento PDF de folha de rosto e publicar no portal da FIAP. *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Montar PDF com apenas os dados dos integrantes em ordem alfabética e links oficiais. Nada mais pode constar no PDF.
