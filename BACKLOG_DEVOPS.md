# 📋 Backlog Master Azure Boards — Sprint 3: DevOps Tools & Cloud Computing

> **Projeto:** Pet Guardian (Challenge Clyvo 2026 — 2º Semestre)  
> **Disciplina:** DevOps Tools & Cloud Computing (FIAP — 2TDSPG)  
> **Epic Principal:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`  
> **Start Date:** `2026-09-02`  
> **Target Date:** `2026-09-05`  
> **Aplicação Escolhida para Deploy:** **Advanced Business Development with .NET (ASP.NET Core)**  
> **Banco de Dados em Nuvem:** **Oracle Database em Container (ACI com Volume)**  
> **Referência Oficial:** Manual do Challenge 2026 — Páginas 10 a 16  
> **Formato:** Scrum / Azure DevOps (Azure Boards)  

---

## 🎯 1. Diagnóstico dos Requisitos da Sprint 3 (Páginas 10 a 16)

Na Sprint 3, a disciplina de **DevOps Tools & Cloud Computing** exige a **evolução completa da infraestrutura em nuvem (Azure)**. A abordagem baseada em Máquina Virtual (VM) da Sprint 1 foi **descontinuada**. A equipe selecionou a **Opção 1 (ACR + ACI)** com a aplicação em **.NET**, em estrita conformidade com as regras arquiteturais:

### ⚖️ Comparativo de Opções e Regras Arquiteturais

| Critério | Opção 1: ACR + ACI (Implementada pelo Grupo) | Opção 2: Azure App Service + Banco PaaS |
| :--- | :--- | :--- |
| **Aplicação Alvo** | **.NET ASP.NET Core API (`PetGuardian.API`)** | Não aplicável |
| **Computação da Aplicação** | **Azure Container Instances (ACI)** executando imagem do ACR | **Azure App Service** (Deploy nativo de código / pacote) |
| **Registro de Imagens** | **Azure Container Registry (ACR)** obrigatório | Não aplicável |
| **Banco de Dados em Nuvem** | **Container de Banco no ACI** (Oracle Database) com volume Azure Files | **Banco PaaS Gerenciado** (Azure SQL PaaS / Oracle FIAP) |
| **Uso de Containers** | **100% Containerizado** (App .NET E Banco Oracle em containers) | **0% Containerizado** (NADA pode ser container) |
| **Provisionamento** | **100% via Azure CLI** (`az group`, `az acr`, `az container`) | **100% via Azure CLI** (`az group`, `az appservice`, `az sql`) |
| **Segurança do Container** | Container do App .NET **NÃO PODE rodar como root/admin** (`USER appuser`) | Não aplicável |
| **Penalidade por Misturar** | **-40 pontos** se usar App no ACI com Banco PaaS ou sem container | **-40 pontos** se containerizar App ou Banco |

---

## 🚨 2. Gaps Críticos e Análise do Repositório Atual (`DevOps-Tools-Cloud-Computing`)

Ao analisar a pasta herdada da Sprint 1, foram identificados os seguintes itens que **devem ser descartados, substituídos ou refatorados**:

```text
[ESTADO ATUAL DO REPOSITÓRIO (SPRINT 1)]       ➔       [ESTADO ALVO (SPRINT 3 - ACR + ACI)]
❌ azure-cli-script.sh (Criação de VM Linux)           ✨ azure-cli-sprint3-acr-aci.sh (Provisiona ACR, ACI .NET e ACI Oracle)
❌ draw_macro.png (Arquitetura com VM Ubuntu)          ✨ docs/arquitetura_cloud_azure.png (Diagrama Oficial de Cloud Azure)
❌ Sem usuário não-root no Dockerfile                 ✨ Dockerfile multi-stage com USER appuser (Segurança non-root)
❌ Sem script DDL segregado                           ✨ script_bd.sql (DDL completo com comentários das tabelas Pet e Atendimento)
❌ Endpoints PUT ausentes no CRUD da API              ✨ CRUD .NET completo funcional com PUT, GET, POST e DELETE
```

### 🗑️ O que APAGAR / SUBSTITUIR:
1. **`azure-cli-script.sh` (Sprint 1):** APAGAR/SUBSTITUIR. Ele executa `az vm create`, `apt-get install docker`, etc. Na Sprint 3, **não se usa VM**.
2. **`docs/draw_macro.png` (Sprint 1):** SUBSTITUIR. O diagrama antigo ilustra uma VM e fluxo genérico. O professor penaliza em **-20 pontos** diagramas em formato de fluxo/UML/TOGAF.
3. **`docs/Documentação DevOps - Pet Guardian.pdf` (Sprint 1):** SUBSTITUIR pelo novo PDF de entrega da Sprint 3 (folha de rosto contendo estritamente nome/RM dos integrantes em ordem alfabética, link do GitHub e link do YouTube).

### 🛠️ O que MANTER e REFATORAR:
1. **Código-fonte da API .NET (`PetGuardian/`):** Manter e garantir que o CRUD das entidades core (`Pet`, `Atendimento`, `Usuario`, `Tarefa`) esteja 100% funcional com `GET`, `POST`, `PUT` e `DELETE`.
2. **`Dockerfile` (.NET):** Atualizar para criar e trocar para usuário não privilegiado (`USER appuser`).
3. **`README.md`:** Reformular completamente com: Descrição da Solução, Benefícios para o Negócio, Diagrama com Ícones Oficiais Azure, Passo a Passo (How To) de Deploy via Azure CLI, Comandos Docker (`build`, `push`, `run`), e Scripts de Consulta SQL (`SELECT`) para evidência no vídeo.

---

## ☁️ 3. Padrão Exigido para o Diagrama de Arquitetura Cloud (20 Pontos)

> ⚠️ **ATENÇÃO MÁXIMA (Página 15):** *"Desenho de arquitetura parecido com Fluxo, Togaf, UML não será aceito: -20 pontos"*.

O diagrama deve ser um **Diagrama de Arquitetura de Nuvem Azure** elaborado no **Draw.io** ou **Visual Paradigm (Azure Architecture Diagram Tool)** contendo:
* **Ícones Oficiais da Microsoft Azure:**
  * Azure Resource Group (`rg-petguardian-sprint3`)
  * Azure Container Registry (`acrpetguardian`)
  * Azure Container Instances — API .NET (`aci-petguardian-api` na porta 8080)
  * Azure Container Instances — Banco de Dados Oracle (`aci-petguardian-db` na porta 1521) com Azure Files Share Volume
  * Virtual Network (VNet) / Subnet / Network Security Group (NSG)
* **Personas Identificadas:**
  * 🧑‍💻 *Desenvolvedor / DevOps:* Execução do Azure CLI, Build da imagem Docker .NET e Push para o ACR.
  * 📱 *Tutor / Veterinário (Usuário Final):* Requisições HTTP/REST e Swagger UI.
* **Setas de Fluxo Numeradas:**
  1. `[1]` DevOps executa script Azure CLI e provisiona o Resource Group, ACR, ACI de Banco e ACI da API .NET.
  2. `[2]` Build da imagem da aplicação .NET (`docker build`) e `docker push` para o repositório privado no ACR.
  3. `[3]` ACI faz o pull seguro da imagem armazenada no ACR via credenciais de admin gerenciadas.
  4. `[4]` Container da API .NET se comunica internamente na rede/IP com o Container do Banco de Dados Oracle no ACI.
  5. `[5]` Usuário consome a API através do IP público/FQDN na porta 8080.
  6. `[6]` Mutações de dados persistem no volume montado do Azure Files.

---

## 👑 4. Estrutura do Backlog no Azure Boards (Scrum)

```text
[EPIC-03] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)
│
├── 🧹 [FEAT-01] Engenharia de Containers & Segurança da Aplicação .NET
│   ├── [PBI-01] Refatoração do Dockerfile Multi-Stage .NET com Usuário Sem Privilégios (Non-Root) (3 pts)
│   ├── [PBI-02] Segregação do DDL do Banco Core com Comentários (script_bd.sql) (2 pts)
│   └── [PBI-03] Validação e Ajuste do CRUD .NET de 2+ Entidades Relacionadas com PUT e Persistência (3 pts)
│
├── ⚙️ [FEAT-02] Infraestrutura como Código (IaC) via Azure CLI
│   ├── [PBI-04] Script Azure CLI para Provisionamento do Resource Group e Azure Container Registry (ACR) (3 pts)
│   ├── [PBI-05] Script Azure CLI para Provisionamento do Container de Banco Oracle no ACI com Volume (5 pts)
│   └── [PBI-06] Script Azure CLI para Build, Push e Deploy do Container .NET no ACI (5 pts)
│
├── ☁️ [FEAT-03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub
│   ├── [PBI-07] Elaboração do Diagrama Macro de Arquitetura Cloud Azure com Ícones Oficiais e Fluxos (5 pts)
│   └── [PBI-08] Reestruturação Completa do README.md (How-To, Comandos, Benefícios e DDL) (5 pts)
│
└── 📹 [FEAT-04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial
    ├── [PBI-09] Roteiro e Execução dos Testes com Evidência de Persistência no Banco via SELECT (3 pts)
    └── [PBI-10] Gravação do Vídeo Explicativo por Voz (720p+) e Geração do PDF Oficial da Entrega (5 pts)
```

---

## 📊 5. Tabela Resumo do Backlog (Story Points & Prioridades)

| ID | Título do Item de Backlog (PBI) | Feature Pai | Story Points | Prioridade | Horas Estimadas |
| :--- | :--- | :--- | :---: | :---: | :---: |
| **PBI-01** | Dockerfile Multi-Stage .NET com Usuário Non-Root | `[FEAT-01]` Containers & App | **3 pts** | 1 - Critical | 4h |
| **PBI-02** | DDL Estruturado com Comentários (`script_bd.sql`) | `[FEAT-01]` Containers & App | **2 pts** | 1 - Critical | 2h |
| **PBI-03** | Validação do CRUD .NET de 2+ Tabelas Core c/ PUT | `[FEAT-01]` Containers & App | **3 pts** | 1 - Critical | 4h |
| **PBI-04** | Script Azure CLI para Resource Group e ACR | `[FEAT-02]` Azure CLI IaC | **3 pts** | 1 - Critical | 3h |
| **PBI-05** | Script Azure CLI para Deploy do Banco Oracle no ACI | `[FEAT-02]` Azure CLI IaC | **5 pts** | 1 - Critical | 5h |
| **PBI-06** | Script Azure CLI para Deploy da API .NET no ACI | `[FEAT-02]` Azure CLI IaC | **5 pts** | 1 - Critical | 5h |
| **PBI-07** | Diagrama de Arquitetura Cloud Azure (Visual Paradigm/Draw.io) | `[FEAT-03]` Arquitetura & Docs | **5 pts** | 1 - Critical | 5h |
| **PBI-08** | README.md Completo com How-To, Benefícios e Comandos | `[FEAT-03]` Arquitetura & Docs | **5 pts** | 1 - Critical | 4h |
| **PBI-09** | Bateria de Testes do CRUD com Validação por SELECT | `[FEAT-04]` Vídeo & Entrega | **3 pts** | 1 - Critical | 4h |
| **PBI-10** | Gravação do Vídeo sem Cortes e PDF Oficial de Entrega | `[FEAT-04]` Vídeo & Entrega | **5 pts** | 1 - Critical | 6h |
| **TOTAL** | **10 PBIs / 25 Child Tasks** | **4 Features / 1 Epic** | **39 pts** | — | **42h** |

---

## 📄 6. Detalhamento dos Product Backlog Items (PBIs) e Child Tasks

---

### 🧹 FEATURE 01: Engenharia de Containers & Segurança da Aplicação .NET
* **Work Item Type:** `Feature`
* **Parent Epic:** `[EPIC-03] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`
* **Tags:** `DevOps`
* **Start Date:** `2026-09-02`
* **Target Date:** `2026-09-03`
* **Descrição:** Refatoração do Dockerfile da aplicação .NET para arquitetura multi-stage com usuário não privilegiado (non-root), segregação do DDL e validação do CRUD.

#### 🔹 [PBI-01] Refatoração do Dockerfile Multi-Stage .NET com Usuário Sem Privilégios (Non-Root)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-01] Engenharia de Containers & Segurança da Aplicação .NET`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Tags:** `DevOps`, `Docker`, `DotNet`, `Security`, `Non-Root`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** engenheiro DevOps,  
> **Eu quero** criar e otimizar o `Dockerfile` multi-stage da aplicação .NET configurando um usuário não privilegiado (`USER appuser`),  
> **Para que** a imagem containerizada execute em conformidade com as práticas de segurança em nuvem e atenda à exigência estrita do manual da Sprint 3 (evitando desconto de até -10 pts).

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Build multi-stage implementado no `Dockerfile` com estágios claros (`base`, `build`, `publish`, `final`).
- [ ] Criação explícita de usuário e grupo sem permissões administrativas (`RUN adduser --disabled-password --gecos "" appuser`).
- [ ] Instrução `USER appuser` declarada antes do `ENTRYPOINT`.
- [ ] Container compila e executa sem erros de permissão na porta 8080.

#### Tarefas Técnicas (Child Tasks)
* **Task 1.1:** Ajustar estágios do `Dockerfile` para .NET 8/10 ou Java com multi-stage build. *(Estimativa: 2h)*
  * *Descrição:* Separar estágio de compilação SDK do estágio leve de runtime.
* **Task 1.2:** Configurar usuário não-root (`appuser`) e ajustar permissões no container. *(Estimativa: 1h)*
  * *Descrição:* Adicionar diretiva `USER appuser` e validar que o processo não roda como root (`docker exec id`).
* **Task 1.3:** Testar build local e validar execução de endpoints pelo container. *(Estimativa: 1h)*
  * *Descrição:* Rodar `docker build` e `docker run -p 8080:8080` validando Swagger e logs.

---

#### 🔹 [PBI-02] Segregação do DDL do Banco Core com Comentários (script_bd.sql)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-01] Engenharia de Containers & Segurança da Aplicação .NET`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Tags:** `DevOps`, `Oracle`, `DDL`, `Database`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** arquiteto de banco de dados,  
> **Eu quero** gerar um arquivo de script DDL isolado chamado `script_bd.sql` contendo a definição e comentários das tabelas core (`PET`, `ATENDIMENTO`, `USUARIO`),  
> **Para que** a infraestrutura do banco seja recriada facilmente no container e cumpra o requisito obrigatório da página 11 do manual (desconto de -10 pts evitado).

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Arquivo `script_bd.sql` criado na raiz do repositório da disciplina de DevOps.
- [ ] Contém o DDL completo de tabelas representativas do core da aplicação (ex: `PET`, `ATENDIMENTO`, `USUARIO_PET`, `TAREFA`), excluindo cadastros genéricos não essenciais.
- [ ] Chaves primárias (PK), chaves estrangeiras (FK), constraints e índices devidamente declarados.
- [ ] Comentários explicativos (`COMMENT ON TABLE` e `COMMENT ON COLUMN`) documentando o propósito de cada entidade e atributo.

#### Tarefas Técnicas (Child Tasks)
* **Task 2.1:** Isolar o DDL das tabelas de negócio do PetGuardian em `script_bd.sql`. *(Estimativa: 1h)*
  * *Descrição:* Extrair as tabelas centrais do modelo relacional e validar a sintaxe SQL.
* **Task 2.2:** Adicionar comentários explicativos nas tabelas e colunas core. *(Estimativa: 1h)*
  * *Descrição:* Incluir metadados e documentação nos scripts SQL para facilitar a avaliação do professor.

---

#### 🔹 [PBI-03] Validação e Ajuste do CRUD .NET de 2+ Entidades Relacionadas com PUT e Persistência
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-01] Engenharia de Containers & Segurança da Aplicação .NET`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Tags:** `DevOps`, `DotNet`, `API`, `CRUD`, `Oracle`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** desenvolvedor e integrador,  
> **Eu quero** validar que a API .NET conectada ao banco Oracle possua operações completas de `Create`, `Read`, `Update` (`PUT`) e `Delete` em tabelas relacionadas,  
> **Para que** todas as operações possam ser demonstradas e comprovadas via queries `SELECT` durante o vídeo de avaliação.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] CRUD 100% funcional sobre as entidades `Pet` e `Atendimento` (ou `Usuario`).
- [ ] Endpoints `POST`, `GET`, `PUT` e `DELETE` operando sem erros e persistindo no banco Oracle.
- [ ] Inserção e manipulação de pelo menos 2 registros com conteúdo significativo.

#### Tarefas Técnicas (Child Tasks)
* **Task 3.1:** Testar rotas de CRUD completo da API .NET no Swagger e Postman. *(Estimativa: 2h)*
* **Task 3.2:** Validar persistência física das alterações no banco Oracle com consultas SQL. *(Estimativa: 2h)*

---

### ⚙️ FEATURE 02: Infraestrutura como Código (IaC) via Azure CLI
* **Work Item Type:** `Feature`
* **Parent Epic:** `[EPIC-03] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`
* **Tags:** `DevOps`
* **Start Date:** `2026-09-03`
* **Target Date:** `2026-09-04`
* **Descrição:** Automação em Azure CLI para criação do Resource Group, Azure Container Registry (ACR), container de banco Oracle no ACI com volume e deploy da API .NET.

#### 🔹 [PBI-04] Script Azure CLI para Provisionamento do Resource Group e Azure Container Registry (ACR)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Tags:** `DevOps`, `AzureCLI`, `ACR`, `IaC`, `Cloud`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** engenheiro de nuvem,  
> **Eu quero** criar um script shell automatizado em Azure CLI para criar o Resource Group e o registro privado Azure Container Registry (ACR),  
> **Para que** a infraestrutura base de nuvem seja provisionada de forma reproduzível em linha de comando.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Comandos `az group create --name rg-petguardian-sprint3 --location eastus`.
- [ ] Comando `az acr create --resource-group rg-petguardian-sprint3 --name acrpetguardian --sku Basic --admin-enabled true`.
- [ ] Script documentado com comentários explicando cada parâmetro.

#### Tarefas Técnicas (Child Tasks)
* **Task 4.1:** Escrever comandos de criação do Resource Group e ACR em `azure-cli-sprint3-acr-aci.sh`. *(Estimativa: 1.5h)*
* **Task 4.2:** Testar execução do script na assinatura Azure e autenticação no ACR. *(Estimativa: 1.5h)*

---

#### 🔹 [PBI-05] Script Azure CLI para Provisionamento do Container de Banco Oracle no ACI com Volume
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `AzureCLI`, `ACI`, `Oracle`, `VolumeStorage`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** arquiteto de infraestrutura,  
> **Eu quero** provisionar um container de banco de dados Oracle no Azure Container Instances com volume montado no Azure Storage Account,  
> **Para que** o banco execute 100% containerizado sem violar a regra de mistura de opções da Sprint 3.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Criação de Storage Account e File Share via CLI (`az storage account create`, `az storage share create`).
- [ ] Deploy do container de banco no ACI com `az container create` montando o volume de arquivos para persistência.
- [ ] Porta do banco exposta internamente na VNet/IP para acesso da aplicação .NET.

#### Tarefas Técnicas (Child Tasks)
* **Task 5.1:** Criar comandos de storage account e file share para persistência do banco. *(Estimativa: 2h)*
* **Task 5.2:** Escrever comando `az container create` para o banco Oracle com variáveis de ambiente. *(Estimativa: 2h)*
* **Task 5.3:** Validar conectividade e inicialização do banco de dados no container. *(Estimativa: 1h)*

---

#### 🔹 [PBI-06] Script Azure CLI para Build, Push e Deploy do Container .NET no ACI
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `AzureCLI`, `ACI`, `DockerDeploy`, `DotNet`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** engenheiro DevOps,  
> **Eu quero** automatizar o build da imagem .NET no ACR (`az acr build`) e o provisionamento do container da aplicação no ACI com IP público e variáveis de conexão,  
> **Para que** a API .NET fique acessível publicamente na internet e conectada ao container de banco.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Autenticação no ACR via `az acr login` ou `docker login`.
- [ ] Build da imagem com tag do ACR (`docker build -t <ACR_SERVER>/petguardian-api:v1 .` ou `az acr build`).
- [ ] Push da imagem para o ACR (`docker push <ACR_SERVER>/petguardian-api:v1`).
- [ ] Deploy do container da API no ACI (`az container create`):
  - Autenticação com o ACR usando credenciais gerenciadas;
  - Configuração da Connection String apontando para o container de banco no ACI;
  - Porta pública 8080 exposta com DNS name label configurado;
  - Alocação de recursos adequada (1 CPU / 1.5GB RAM).
- [ ] Verificação de saúde: API respondendo no navegador/Swagger em `http://<FQDN_DA_API>:8080/index.html`.

#### Tarefas Técnicas (Child Tasks)
* **Task 6.1:** Implementar comandos de build e envio da imagem Docker para o ACR. *(Estimativa: 2h)*
  * *Descrição:* Automatizar o processo de tag e push da imagem no registro privado da Azure.
* **Task 6.2:** Implementar comando `az container create` para a API com injeção da Connection String. *(Estimativa: 2h)*
  * *Descrição:* Passar a string de conexão do banco via variável de ambiente segura no container.
* **Task 6.3:** Testar acesso público ao Swagger e comunicação ponta a ponta com o banco. *(Estimativa: 1h)*
  * *Descrição:* Efetuar requisições de teste HTTP no endpoint público do ACI.

---

### ☁️ FEATURE 03: Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub
* **Work Item Type:** `Feature`
* **Parent Epic:** `[EPIC-03] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`
* **Tags:** `DevOps`
* **Start Date:** `2026-09-04`
* **Target Date:** `2026-09-05`
* **Descrição:** Diagramação de nuvem Azure oficial com personas e fluxos numerados, e documentação técnica no README.md.

#### 🔹 [PBI-07] Elaboração do Diagrama Macro de Arquitetura Cloud Azure com Ícones Oficiais
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `ArchitectureDiagram`, `Drawio`, `AzureIcons`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** arquiteto de soluções em nuvem,  
> **Eu quero** criar o diagrama de arquitetura Azure oficial no Draw.io / Visual Paradigm com ícones Microsoft, personas e fluxos numerados,  
> **Para que** a solução atenda rigorosamente ao critério da página 13 e 15 (evitando a penalidade de -20 pts para diagramas em formato de fluxo ou UML).

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Diagrama desenhado no Draw.io ou Visual Paradigm utilizando estritamente **ícones oficiais da Azure Cloud**.
- [ ] Inclusão explícita de todos os componentes da infraestrutura:
  - Azure Resource Group (`rg-petguardian-sprint3`)
  - Azure Container Registry (`acrpetguardian`)
  - Azure Container Instances (ACI) - API .NET (porta 8080)
  - Azure Container Instances (ACI) - Banco de Dados Oracle (porta 1521)
  - Azure Storage Account / File Share Volume
  - Virtual Network (VNet) / Subnet / Network Security Group (NSG)
- [ ] Personas representadas (Usuário Final e Desenvolvedor/DevOps).
- [ ] Setas de fluxo com números sequenciais (`[1]` a `[6]`) e legenda detalhada explicando cada interação.
- [ ] Exportação do arquivo em alta resolução em `docs/arquitetura_cloud_azure.png` e incorporado ao `README.md`.

#### Tarefas Técnicas (Child Tasks)
* **Task 7.1:** Mapear componentes, portas e interações entre os serviços Azure. *(Estimativa: 2h)*
  * *Descrição:* Rascunhar a topologia de rede e os fluxos de deploy e consumo.
* **Task 7.2:** Desenhar o diagrama oficial no Draw.io / Visual Paradigm com ícones Azure e legendas. *(Estimativa: 2h)*
  * *Descrição:* Diagramar com acabamento profissional, cores corporativas e setas explicativas.
* **Task 7.3:** Exportar imagem em alta resolução e incorporar no `README.md` e na documentação. *(Estimativa: 1h)*
  * *Descrição:* Salvar em `docs/arquitetura_cloud_azure.png` e validar visualização no GitHub.

---

#### 🔹 [PBI-08] Reestruturação Completa do README.md (How-To, Comandos, Benefícios e DDL)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `Documentation`, `README`, `GitHub`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** avaliador e desenvolvedor,  
> **Eu quero** um README.md completo contendo descrição da solução, benefícios para o negócio, comandos Docker/CLI e passo a passo de deploy,  
> **Para que** qualquer pessoa consiga reproduzir o deploy e validar a aplicação (evitando penalidade de -30 pts).

#### Critérios de Aceite (Acceptance Criteria)
- [ ] **Descrição da Solução:** Explicação detalhada do PetGuardian e seus objetivos de cuidado contínuo.
- [ ] **Benefícios para o Negócio:** Impactos clínicos, redução de custos e fidelização de tutores.
- [ ] **Desenho Macro da Arquitetura:** Imagem embutida com legenda e explicação detalhada de cada fluxo.
- [ ] **Documentação das Rotas da API:** Tabela detalhada de endpoints, métodos HTTP, parâmetros e payloads.
- [ ] **Instalação da Solução (How-To passo a passo):**
  - Pré-requisitos (Azure CLI, Docker);
  - Guia de execução do script Azure CLI;
  - Comandos manuais de Docker (`docker build`, `docker tag`, `docker push`, `docker run`);
  - Como acessar a aplicação e o Swagger na nuvem.
- [ ] **Evidências de Persistência:** Queries SQL com `SELECT` para conferência das operações do CRUD.

#### Tarefas Técnicas (Child Tasks)
* **Task 8.1:** Redigir seções de Apresentação, Domínio, Benefícios e Arquitetura. *(Estimativa: 1h)*
  * *Descrição:* Estruturar o documento com markdown profissional, badges e tabelas.
* **Task 8.2:** Elaborar o roteiro passo a passo (How-To) de deploy e comandos de terminal. *(Estimativa: 2h)*
  * *Descrição:* Documentar exatamente os comandos que serão executados na demonstração em vídeo.
* **Task 8.3:** Adicionar guia de testes e queries SQL para validação das operações do CRUD. *(Estimativa: 1h)*
  * *Descrição:* Listar os comandos `SELECT` formatados para evidenciar dados no banco.

---

### 📹 FEATURE 04: Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial
* **Work Item Type:** `Feature`
* **Parent Epic:** `[EPIC-03] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`
* **Tags:** `DevOps`
* **Start Date:** `2026-09-04`
* **Target Date:** `2026-09-05`
* **Descrição:** Execução de ensaios de persistência e gravação do vídeo demonstrativo contínuo sem cortes com narração por voz.

#### 🔹 [PBI-09] Roteiro e Execução dos Testes com Evidência de Persistência no Banco via SELECT
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Tags:** `DevOps`, `Testing`, `CRUD`, `SELECT`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** analista de QA e DevOps,  
> **Eu quero** preparar e executar um roteiro de testes cobrindo todas as operações do CRUD e validando imediatamente cada alteração com um comando `SELECT` no banco,  
> **Para que** a persistência em nuvem seja demonstrada de forma inquestionável no vídeo (evitando desconto de -30 pts).

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Roteiro estruturado: Inserção ➔ `SELECT`, Alteração (`PUT`) ➔ `SELECT`, Consulta ➔ `SELECT`, Exclusão (`DELETE`) ➔ `SELECT`.
- [ ] Evidência clara da persistência no banco Oracle executando no ACI.

#### Tarefas Técnicas (Child Tasks)
* **Task 9.1:** Elaborar roteiro de teste com payloads JSON e queries SQL correspondentes. *(Estimativa: 2h)*
* **Task 9.2:** Realizar ensaio do teste contra os containers online na Azure. *(Estimativa: 2h)*

---

### 🔹 [PBI-10] Gravação do Vídeo Explicativo por Voz (720p+) e Geração do PDF Oficial da Entrega
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `Video`, `PDF`, `Delivery`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** equipe Pet Guardian,  
> **Eu quero** gravar um vídeo contínuo narrado demonstrando a criação dos recursos via Azure CLI, deploy e testes de persistência, e gerar o PDF oficial da entrega,  
> **Para que** a entrega cumpra 100% das normas avaliativas da Sprint 3.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Vídeo em resolução mínima de 720p com áudio claro e narração por voz (sem legendas).
- [ ] Demonstração do clone do repositório, execução do script Azure CLI, deploy no ACI e testes do CRUD com `SELECT` no banco (sem cortes na persistência).
- [ ] Link do YouTube em modo Não Listado inserido no README e no PDF.
- [ ] Arquivo PDF gerado contendo estritamente: nomes completos e RMs em **ordem alfabética**, link do GitHub e link do YouTube.

#### Tarefas Técnicas (Child Tasks)
* **Task 10.1:** Preparar ambiente de gravação (terminais, VS Code, DBeaver, Portal Azure e Swagger abertos). *(Estimativa: 2h)*
  * *Descrição:* Ensaiar a sequência: Clone ➔ Script CLI ➔ Portal Azure ➔ Swagger ➔ SELECT no Banco.
* **Task 10.2:** Gravar a demonstração em alta definição narrando detalhadamente cada etapa. *(Estimativa: 2h)*
  * *Descrição:* Gravar sem cortes nas etapas de testes e persistência, subir para o YouTube e testar o link.
* **Task 10.3:** Gerar o documento PDF de folha de rosto e publicar no portal da FIAP. *(Estimativa: 2h)*
  * *Descrição:* Montar o PDF contendo apenas os dados dos integrantes e links oficiais.

---

## ⚠️ 8. Matriz de Riscos e Penalidades a Evitar

| Risco / Item Avaliativo | Penalidade no Manual | Ação Preventiva no Backlog |
| :--- | :---: | :--- |
| **Entrega em LOCALHOST** | **Nota ZERO na Sprint** | Deploy obrigatório no Azure Container Instances (ACI) via Azure CLI com IP/DNS público. |
| **Misturar opções (ex: App no ACI + Banco PaaS / FIAP)** | **-40 pontos** | Opção 1 pura: App .NET no ACI + Banco Oracle em container no ACI com volume Azure File Share. |
| **Recursos não criados via Azure CLI** | **-30 pontos** | Script `azure-cli-sprint3-acr-aci.sh` automatiza 100% dos recursos sem cliques manuais no portal. |
| **Container do App rodando como root/admin** | **-10 pontos** | PBI-01 configura `USER appuser` no estágio final do Dockerfile .NET. |
| **Desenho de arquitetura parecido com Fluxo, TOGAF ou UML** | **-20 pontos** | PBI-07 cria diagrama oficial com ícones Azure, portas (8080/1521), personas e fluxos [1] a [6]. |
| **Sem evidência clara de cada operação do CRUD via SELECT** | **-30 pontos** | PBI-09 e PBI-10 executam `SELECT` imediatamente após cada `POST`, `PUT` e `DELETE`. |
| **Vídeo sem explicação falada ou baixa qualidade (<720p)** | **-30 pontos** | Gravação em 1080p com narração clara por voz dos integrantes. |
| **Conteúdo extra no PDF além de Folha de Rosto e Links** | **Perda de pontos** | PBI-10 restringe o PDF estritamente a nomes, RMs, link do GitHub e link do YouTube. |
| **Utilizar apenas 1 tabela no CRUD ou tabelas fora do core** | **-20 a -30 pontos** | PBI-03 implementa CRUD em 2 tabelas core relacionadas (`Pet` e `Atendimento`). |
| **Ausência do script DDL das tabelas (`script_bd.sql`)** | **-10 pontos** | PBI-02 fornece o arquivo `script_bd.sql` isolado e comentado na raiz. |

---

## 👥 9. Integrantes da Equipe (Ordem Alfabética Obrigatória)

| Nome Completo | RM | Turma | Papel / Responsabilidade |
| :--- | :---: | :---: | :--- |
| **Enzo Okuizumi** | 561432 | 2TDSPG | DevOps Engineer / Automação Azure CLI & ACI |
| **Gustavo Okada** | 563428 | 2TDSPG | Cloud Architect / Diagramação de Arquitetura Azure |
| **Lucas Barros Gouveia** | 566422 | 2TDSPG | Backend Developer / CRUD .NET & Integração Oracle |
| **Luna de Carvalho Guimarães** | 562290 | 2TDSPG | QA Engineer / Roteiro de Testes e Queries de Validação |
| **Milton Marcelino** | 564836 | 2TDSPG | Security & Containers / Dockerfile Non-Root & Documentação |
