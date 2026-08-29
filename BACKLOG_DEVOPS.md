# 📋 Backlog Master Azure Boards — Sprint 3: DevOps Tools & Cloud Computing

> **Projeto Integrado:** PetGuardian / Clyvo Care (Challenge FIAP 2026 - 2º Ano ADS / 2TDSPG)  
> **Disciplina:** DevOps Tools & Cloud Computing (FIAP — 2TDSPG)  
> **Epic Principal:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`  
> **Start Date:** `2026-09-02`  
> **Target Date:** `2026-09-05`  
> **Aplicação Escolhida:** .NET ASP.NET Core API (`PetGuardian.API`) | **Banco em Nuvem:** Oracle Database no ACI com Azure Files  
> **Padrão:** Azure Boards (Scrum Process: Epic ➔ Feature ➔ PBI ➔ Task)  
> **Diretrizes Estratégicas:** 100% Provisionamento via Azure CLI, Containers multi-stage com usuário non-root (`USER appuser`), diagrama com ícones oficiais Azure e persistência comprovada via SELECT.

---

## 🎯 1. Matriz de Requisitos & Critérios de Avaliação Oficiais (Páginas 10 a 16)

| Critério Avaliativo | Opção 1: ACR + ACI (Implementada pelo Grupo) | Regras Críticas e Penalidades do Edital |
| :--- | :--- | :--- |
| **Aplicação Alvo** | **.NET ASP.NET Core API (`PetGuardian.API`)** | CRUD completo de 2+ entidades relacionadas (`Pet`, `Atendimento`). |
| **Computação da Aplicação** | **Azure Container Instances (ACI)** executando imagem do ACR | **-40 pontos** se misturar App em ACI com Banco PaaS ou sem container. |
| **Registro de Imagens** | **Azure Container Registry (ACR)** privado com autenticação | Provisionamento 100% automatizado em linha de comando. |
| **Banco de Dados em Nuvem** | **Container de Banco no ACI** (Oracle DB) com volume Azure Files | Volume montado via Azure Storage File Share para persistência. |
| **Segurança do Container** | Container do App .NET **NÃO PODE rodar como root** (`USER appuser`) | **-10 pontos** se o container rodar com permissões de administrador. |
| **Provisionamento** | **100% via Azure CLI** (`az group`, `az acr`, `az container`) | **-30 pontos** se criado manualmente pelo portal web. |
| **Diagrama de Arquitetura** | **Diagrama Oficial Cloud Azure** (Draw.io / Visual Paradigm) | **-20 pontos** se parecer fluxograma, TOGAF ou diagrama UML. |
| **Vídeo e Evidências** | Vídeo contínuo narrado em 720p+ com queries `SELECT` | **-30 pontos** se não comprovar mutações do CRUD via `SELECT`. |

---

## 🌳 2. Estrutura Hierárquica no Azure Boards

```text
[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)
│
├── 🏆 [FEATURE 01] Engenharia de Containers & Segurança da Aplicação .NET
│   ├── 📄 [PBI-01] Refatoração do Dockerfile Multi-Stage .NET com Usuário Sem Privilégios (Non-Root) (3 pts)
│   │   ├── 🔹 Task 1.1: Ajustar estágios do Dockerfile para multi-stage build (2.0h)
│   │   ├── 🔹 Task 1.2: Configurar usuário não-root (appuser) e permissões (1.0h)
│   │   └── 🔹 Task 1.3: Testar build local e validar execução de endpoints (1.0h)
│   ├── 📄 [PBI-02] Segregação do DDL do Banco Core com Comentários (script_bd.sql) (2 pts)
│   │   ├── 🔹 Task 2.1: Isolar o DDL das tabelas centrais em script_bd.sql (1.0h)
│   │   └── 🔹 Task 2.2: Adicionar comentários explicativos nas tabelas e colunas (1.0h)
│   └── 📄 [PBI-03] Validação e Ajuste do CRUD .NET de 2+ Entidades Relacionadas com PUT e Persistência (1 pt)
│       ├── 🔹 Task 3.1: Testar rotas de CRUD completo no Swagger e Postman (2.0h)
│       └── 🔹 Task 3.2: Validar persistência física com consultas SQL SELECT (2.0h)
│
├── 🏆 [FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI
│   ├── 📄 [PBI-04] Script Azure CLI para Provisionamento do Resource Group e Azure Container Registry (ACR) (3 pts)
│   │   ├── 🔹 Task 4.1: Escrever comandos de criação do RG e ACR no script CLI (1.5h)
│   │   └── 🔹 Task 4.2: Testar execução do script na Azure e login no ACR (1.5h)
│   ├── 📄 [PBI-05] Script Azure CLI para Provisionamento do Container de Banco Oracle no ACI com Volume (1 pt)
│   │   ├── 🔹 Task 5.1: Criar comandos de storage account e file share (2.0h)
│   │   ├── 🔹 Task 5.2: Escrever comando az container create para banco Oracle (2.0h)
│   │   └── 🔹 Task 5.3: Validar conectividade e inicialização do banco (1.0h)
│   └── 📄 [PBI-06] Script Azure CLI para Build, Push e Deploy do Container .NET no ACI (1 pt)
│       ├── 🔹 Task 6.1: Implementar comandos de build e envio para o ACR (2.0h)
│       ├── 🔹 Task 6.2: Implementar az container create para a API com Connection String (2.0h)
│       └── 🔹 Task 6.3: Testar acesso público ao Swagger e comunicação com banco (1.0h)
│
├── 🏆 [FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub
│   ├── 📄 [PBI-07] Elaboração do Diagrama Macro de Arquitetura Cloud Azure com Ícones Oficiais (1 pt)
│   │   ├── 🔹 Task 7.1: Mapear componentes, portas e interações Azure (2.0h)
│   │   ├── 🔹 Task 7.2: Desenhar diagrama oficial no Draw.io com ícones Azure (2.0h)
│   │   └── 🔹 Task 7.3: Exportar imagem em alta resolução e incorporar no README (1.0h)
│   └── 📄 [PBI-08] Reestruturação Completa do README.md (How-To, Comandos, Benefícios e DDL) (5 pts)
│       ├── 🔹 Task 8.1: Redigir seções de Apresentação, Benefícios e Arquitetura (1.0h)
│       ├── 🔹 Task 8.2: Elaborar roteiro passo a passo (How-To) de deploy (2.0h)
│       └── 🔹 Task 8.3: Adicionar guia de testes e queries SQL SELECT (1.0h)
│
└── 🏆 [FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial
    ├── 📄 [PBI-09] Roteiro e Execução dos Testes com Evidência de Persistência no Banco via SELECT (1 pt)
    │   ├── 🔹 Task 9.1: Elaborar roteiro de teste com payloads JSON e queries (2.0h)
    │   └── 🔹 Task 9.2: Realizar ensaio contra containers online na Azure (2.0h)
    └── 📄 [PBI-10] Gravação do Vídeo Explicativo por Voz (720p+) e Geração do PDF Oficial da Entrega (1 pt)
        ├── 🔹 Task 10.1: Preparar ambiente de gravação sequencial (2.0h)
        ├── 🔹 Task 10.2: Gravar demonstração em alta definição com áudio narrado (2.0h)
        └── 🔹 Task 10.3: Gerar documento PDF de folha de rosto oficial (2.0h)
```

---

## 📊 3. Tabela Resumo do Backlog

| Feature Pai | ID do PBI | Título do Item de Backlog (PBI) | Story Points | Prioridade | Horas Estimadas |
| :--- | :--- | :--- | :---: | :---: | :---: |
| **[FEATURE 01] Containers & App** | **PBI-01** | Dockerfile Multi-Stage .NET com Usuário Non-Root | 1 pts | 1 - Critical | 4.0h |
| | **PBI-02** | DDL Estruturado com Comentários (`script_bd.sql`) | 1 pts | 1 - Critical | 2.0h |
| | **PBI-03** | Validação do CRUD .NET de 2+ Tabelas Core c/ PUT | 1 pts | 1 - Critical | 4.0h |
| **[FEATURE 02] Azure CLI IaC** | **PBI-04** | Script Azure CLI para Resource Group e ACR | 1 pts | 1 - Critical | 3.0h |
| | **PBI-05** | Script Azure CLI para Deploy do Banco Oracle no ACI | 1 pts | 1 - Critical | 5.0h |
| | **PBI-06** | Script Azure CLI para Deploy da API .NET no ACI | 1 pts | 1 - Critical | 5.0h |
| **[FEATURE 03] Arquitetura & Docs** | **PBI-07** | Diagrama de Arquitetura Cloud Azure Oficial c/ Ícones | 1 pts | 1 - Critical | 5.0h |
| | **PBI-08** | README.md Completo com How-To, Benefícios e Comandos | 1 pts | 1 - Critical | 4.0h |
| **[FEATURE 04] Vídeo & Entrega** | **PBI-09** | Bateria de Testes do CRUD com Validação por SELECT | 1 pts | 1 - Critical | 4.0h |
| | **PBI-10** | Gravação do Vídeo sem Cortes e PDF Oficial de Entrega | 1 pts | 1 - Critical | 6.0h |
| **TOTAL CONSOLIDADO** | **4 Features** | **10 PBIs / 26 Child Tasks Técnicas** | **10 pts** | — | **42.0h** |

---

## 📦 4. Detalhamento dos Itens de Trabalho (Épico, Features, PBIs e Tasks)

---

### 🏛️ ÉPICO
* **Work Item Type:** `Epic`
* **Title:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`
* **Tags:** `Sprint3, DevOps, AzureCLI, ACR, ACI, Docker, Security, NonRoot, IaC`
* **Start Date:** `2026-09-02`
* **Target Date:** `2026-09-05`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `10`
* **Business Value:** `100`
* **Description:** Evolução completa da infraestrutura em nuvem na Microsoft Azure provisionando arquitetura 100% containerizada (Opção 1) com aplicação .NET e banco Oracle no Azure Container Instances (ACI), imagens no Azure Container Registry (ACR), Dockerfile multi-stage com usuário não privilegiado, scripts automatizados via Azure CLI e validação do CRUD via consultas SELECT.

---

### 🏆 [FEATURE 01] Engenharia de Containers & Segurança da Aplicação .NET
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`
* **Title:** `[FEATURE 01] Engenharia de Containers & Segurança da Aplicação .NET`
* **Tags:** `Sprint3, DevOps, Containers, Docker, Security, NonRoot`
* **Start Date:** `2026-09-02`
* **Target Date:** `2026-09-03`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Description:** Refatoração do Dockerfile da aplicação .NET para arquitetura multi-stage com usuário não privilegiado (non-root), segregação do DDL e validação do CRUD.

#### 🔹 [PBI-01] Refatoração do Dockerfile Multi-Stage .NET com Usuário Sem Privilégios (Non-Root)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 01] Engenharia de Containers & Segurança da Aplicação .NET`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Docker, DotNet, Security, NonRoot`

##### Descrição (História de Usuário)
> **Como** engenheiro DevOps,  
> **Eu quero** criar e otimizar o `Dockerfile` multi-stage da aplicação .NET configurando um usuário não privilegiado (`USER appuser`),  
> **Para que** a imagem containerizada execute em conformidade com as práticas de segurança em nuvem e atenda à exigência estrita do manual da Sprint 3 (evitando desconto de até -10 pts).

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Build multi-stage implementado no `Dockerfile` com estágios claros (`base`, `build`, `publish`, `final`).
- [ ] Criação explícita de usuário e grupo sem permissões administrativas (`RUN adduser --disabled-password --gecos "" appuser`).
- [ ] Instrução `USER appuser` declarada antes do `ENTRYPOINT`.
- [ ] Container compila e executa sem erros de permissão na porta 8080.

##### Tarefas Técnicas (Child Tasks)
* **Task 1.1:** [TASK-01] Ajustar estágios do `Dockerfile` para .NET com multi-stage build. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Separar estágio de compilação SDK do estágio leve de runtime.
* **Task 1.2:** [TASK-02] Configurar usuário não-root (`appuser`) e ajustar permissões no container. *(Activity: Development, Est: 1.0h)*
  * *Descrição:* Adicionar diretiva `USER appuser` e validar execução sem root.
* **Task 1.3:** [TASK-03] Testar build local e validar execução de endpoints pelo container. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Rodar `docker build` e `docker run -p 8080:8080` validando Swagger e logs.

---

#### 🔹 [PBI-02] Segregação do DDL do Banco Core com Comentários (script_bd.sql)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 01] Engenharia de Containers & Segurança da Aplicação .NET`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Oracle, DDL, Database`

##### Descrição (História de Usuário)
> **Como** arquiteto de banco de dados,  
> **Eu quero** gerar um arquivo de script DDL isolado chamado `script_bd.sql` contendo a definição e comentários das tabelas core (`PET`, `ATENDIMENTO`, `USUARIO`),  
> **Para que** a infraestrutura do banco seja recriada facilmente no container e cumpra o requisito obrigatório da página 11 do manual.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Arquivo `script_bd.sql` criado na raiz do repositório da disciplina de DevOps.
- [ ] Contém o DDL completo de tabelas representativas do core da aplicação (`PET`, `ATENDIMENTO`, `USUARIO_PET`, `TAREFA`).
- [ ] Chaves primárias (PK), chaves estrangeiras (FK), constraints e índices devidamente declarados.
- [ ] Comentários explicativos (`COMMENT ON TABLE` e `COMMENT ON COLUMN`) documentando o propósito de cada entidade e atributo.

##### Tarefas Técnicas (Child Tasks)
* **Task 2.1:** [TASK-04] Isolar o DDL das tabelas de negócio do PetGuardian em `script_bd.sql`. *(Activity: Development, Est: 1.0h)*
  * *Descrição:* Extrair as tabelas centrais do modelo relacional e validar a sintaxe SQL.
* **Task 2.2:** [TASK-05] Adicionar comentários explicativos nas tabelas e colunas core. *(Activity: Documentation, Est: 1.0h)*
  * *Descrição:* Incluir metadados e documentação nos scripts SQL para facilitar a avaliação.

---

#### 🔹 [PBI-03] Validação e Ajuste do CRUD .NET de 2+ Entidades Relacionadas com PUT e Persistência
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 01] Engenharia de Containers & Segurança da Aplicação .NET`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, DotNet, API, CRUD, Oracle`

##### Descrição (História de Usuário)
> **Como** desenvolvedor e integrador,  
> **Eu quero** validar que a API .NET conectada ao banco Oracle possua operações completas de `Create`, `Read`, `Update` (`PUT`) e `Delete` em tabelas relacionadas,  
> **Para que** todas as operações possam ser demonstradas e comprovadas via queries `SELECT` durante o vídeo de avaliação.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] CRUD 100% funcional sobre as entidades `Pet` e `Atendimento`.
- [ ] Endpoints `POST`, `GET`, `PUT` e `DELETE` operando sem erros e persistindo no banco Oracle.
- [ ] Inserção e manipulação de pelo menos 2 registros com conteúdo significativo.

##### Tarefas Técnicas (Child Tasks)
* **Task 3.1:** [TASK-06] Testar rotas de CRUD completo da API .NET no Swagger e Postman. *(Activity: Testing, Est: 2.0h)*
  * *Descrição:* Executar requisições de criação, leitura, atualização e exclusão.
* **Task 3.2:** [TASK-07] Validar persistência física das alterações no banco Oracle com consultas SQL. *(Activity: Testing, Est: 2.0h)*
  * *Descrição:* Conferir atualização dos registros diretamente nas tabelas.

---

### 🏆 [FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`
* **Title:** `[FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI`
* **Tags:** `Sprint3, DevOps, AzureCLI, IaC, CloudInfrastructure`
* **Start Date:** `2026-09-03`
* **Target Date:** `2026-09-04`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Description:** Automação em Azure CLI para criação do Resource Group, Azure Container Registry (ACR), container de banco Oracle no ACI com volume e deploy da API .NET.

#### 🔹 [PBI-04] Script Azure CLI para Provisionamento do Resource Group e Azure Container Registry (ACR)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, AzureCLI, ACR, IaC, Cloud`

##### Descrição (História de Usuário)
> **Como** engenheiro de nuvem,  
> **Eu quero** criar um script shell automatizado em Azure CLI para criar o Resource Group e o registro privado Azure Container Registry (ACR),  
> **Para que** a infraestrutura base de nuvem seja provisionada de forma reproduzível em linha de comando.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Comandos `az group create --name rg-petguardian-sprint3 --location eastus`.
- [ ] Comando `az acr create --resource-group rg-petguardian-sprint3 --name acrpetguardian --sku Basic --admin-enabled true`.
- [ ] Script documentado com comentários explicando cada parâmetro.

##### Tarefas Técnicas (Child Tasks)
* **Task 4.1:** [TASK-08] Escrever comandos de criação do Resource Group e ACR em `azure-cli-sprint3-acr-aci.sh`. *(Activity: Development, Est: 1.5h)*
  * *Descrição:* Criar arquivo shell de automação para a fundação da nuvem.
* **Task 4.2:** [TASK-09] Testar execução do script na assinatura Azure e autenticação no ACR. *(Activity: Deployment, Est: 1.5h)*
  * *Descrição:* Validar execução sem intervenção manual pelo Azure CLI.

---

#### 🔹 [PBI-05] Script Azure CLI para Provisionamento do Container de Banco Oracle no ACI com Volume
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, AzureCLI, ACI, Oracle, VolumeStorage`

##### Descrição (História de Usuário)
> **Como** arquiteto de infraestrutura,  
> **Eu quero** provisionar um container de banco de dados Oracle no Azure Container Instances com volume montado no Azure Storage Account,  
> **Para que** o banco execute 100% containerizado sem violar a regra de mistura de opções da Sprint 3.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Criação de Storage Account e File Share via CLI (`az storage account create`, `az storage share create`).
- [ ] Deploy do container de banco no ACI com `az container create` montando o volume de arquivos para persistência.
- [ ] Porta do banco exposta internamente na VNet/IP para acesso da aplicação .NET.

##### Tarefas Técnicas (Child Tasks)
* **Task 5.1:** [TASK-10] Criar comandos de storage account e file share para persistência do banco. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Criar compartilhamento de arquivos no Azure Files para armazenamento persistente.
* **Task 5.2:** [TASK-11] Escrever comando `az container create` para o banco Oracle com variáveis de ambiente. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Configurar senha do sys e portas de conexão Oracle.
* **Task 5.3:** [TASK-12] Validar conectividade e inicialização do banco de dados no container. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Testar conexão remota na porta 1521.

---

#### 🔹 [PBI-06] Script Azure CLI para Build, Push e Deploy do Container .NET no ACI
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, AzureCLI, ACI, DockerDeploy, DotNet`

##### Descrição (História de Usuário)
> **Como** engenheiro DevOps,  
> **Eu quero** automatizar o build da imagem .NET no ACR (`az acr build`) e o provisionamento do container da aplicação no ACI com IP público e variáveis de conexão,  
> **Para que** a API .NET fique acessível publicamente na internet e conectada ao container de banco.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Autenticação no ACR via `az acr login`.
- [ ] Build e push da imagem para o ACR privado.
- [ ] Deploy do container da API no ACI com Connection String apontando para o container do banco.
- [ ] Porta pública 8080 exposta com DNS name label configurado.
- [ ] API respondendo no Swagger em `http://<FQDN_DA_API>:8080/swagger/index.html`.

##### Tarefas Técnicas (Child Tasks)
* **Task 6.1:** [TASK-13] Implementar comandos de build e envio da imagem Docker para o ACR. *(Activity: Deployment, Est: 2.0h)*
  * *Descrição:* Automatizar o processo de tag e push da imagem no registro privado.
* **Task 6.2:** [TASK-14] Implementar comando `az container create` para a API com injeção da Connection String. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Configurar variáveis seguras de conexão e recursos de CPU/RAM.
* **Task 6.3:** [TASK-15] Testar acesso público ao Swagger e comunicação ponta a ponta com o banco. *(Activity: Testing, Est: 1.0h)*
  * *Descrição:* Executar requisições de teste HTTP no IP público da API.

---

### 🏆 [FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`
* **Title:** `[FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **Tags:** `Sprint3, DevOps, CloudArchitecture, Drawio, Documentation, README`
* **Start Date:** `2026-09-04`
* **Target Date:** `2026-09-05`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Description:** Diagramação de nuvem Azure oficial com personas e fluxos numerados, e documentação técnica no README.md.

#### 🔹 [PBI-07] Elaboração do Diagrama Macro de Arquitetura Cloud Azure com Ícones Oficiais
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, ArchitectureDiagram, Drawio, AzureIcons`

##### Descrição (História de Usuário)
> **Como** arquiteto de soluções em nuvem,  
> **Eu quero** criar o diagrama de arquitetura Azure oficial no Draw.io / Visual Paradigm com ícones Microsoft, personas e fluxos numerados,  
> **Para que** a solução atenda rigorosamente ao critério da página 13 e 15 (evitando a penalidade de -20 pts para diagramas em formato de fluxo ou UML).

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Diagrama desenhado no Draw.io ou Visual Paradigm utilizando estritamente **ícones oficiais da Azure Cloud**.
- [ ] Inclusão de Resource Group, ACR, ACI API .NET (8080), ACI Oracle DB (1521), Azure Files Volume, VNet/NSG.
- [ ] Personas representadas (Usuário Final e Desenvolvedor/DevOps).
- [ ] Setas de fluxo com números sequenciais (`[1]` a `[6]`) e legenda detalhada.
- [ ] Exportação em `docs/arquitetura_cloud_azure.png` e incorporado ao `README.md`.

##### Tarefas Técnicas (Child Tasks)
* **Task 7.1:** [TASK-16] Mapear componentes, portas e interações entre os serviços Azure. *(Activity: Design, Est: 2.0h)*
  * *Descrição:* Estruturar a topologia de rede e fluxos de comunicação.
* **Task 7.2:** [TASK-17] Desenhar o diagrama oficial no Draw.io com ícones Azure e legendas. *(Activity: Design, Est: 2.0h)*
  * *Descrição:* Diagramar com ícones corporativos Microsoft e setas explicativas.
* **Task 7.3:** [TASK-18] Exportar imagem em alta resolução e incorporar no `README.md`. *(Activity: Documentation, Est: 1.0h)*
  * *Descrição:* Validar visualização da imagem no repositório GitHub.

---

#### 🔹 [PBI-08] Reestruturação Completa do README.md (How-To, Comandos, Benefícios e DDL)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Documentation, README, GitHub`

##### Descrição (História de Usuário)
> **Como** avaliador e desenvolvedor,  
> **Eu quero** um README.md completo contendo descrição da solução, benefícios para o negócio, comandos Docker/CLI e passo a passo de deploy,  
> **Para que** qualquer pessoa consiga reproduzir o deploy e validar a aplicação.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Descrição da Solução e Benefícios para o Negócio.
- [ ] Desenho Macro da Arquitetura embutido com legenda.
- [ ] Documentação das rotas da API em tabela estruturada.
- [ ] Guia How-To com comandos de Azure CLI e Docker (`build`, `tag`, `push`, `run`).
- [ ] Queries SQL `SELECT` para evidência de persistência.

##### Tarefas Técnicas (Child Tasks)
* **Task 8.1:** [TASK-19] Redigir seções de Apresentação, Domínio, Benefícios e Arquitetura. *(Activity: Documentation, Est: 1.0h)*
  * *Descrição:* Estruturar markdown com tabelas e badges.
* **Task 8.2:** [TASK-20] Elaborar o roteiro passo a passo (How-To) de deploy e comandos de terminal. *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Documentar comandos de execução do script CLI.
* **Task 8.3:** [TASK-21] Adicionar guia de testes e queries SQL para validação das operações do CRUD. *(Activity: Documentation, Est: 1.0h)*
  * *Descrição:* Listar consultas formatadas para conferência no banco.

---

### 🏆 [FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial
* **Work Item Type:** `Feature`
* **Parent:** `[EPIC] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (.NET no ACR + ACI)`
* **Title:** `[FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial`
* **Tags:** `Sprint3, DevOps, Video, Demonstration, PDF, Delivery`
* **Start Date:** `2026-09-04`
* **Target Date:** `2026-09-05`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Description:** Execução de ensaios de persistência e gravação do vídeo demonstrativo contínuo sem cortes com narração por voz.

#### 🔹 [PBI-09] Roteiro e Execução dos Testes com Evidência de Persistência no Banco via SELECT
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Testing, CRUD, SELECT`

##### Descrição (História de Usuário)
> **Como** analista de QA e DevOps,  
> **Eu quero** preparar e executar um roteiro de testes cobrindo todas as operações do CRUD e validando imediatamente cada alteração com um comando `SELECT` no banco,  
> **Para que** a persistência em nuvem seja demonstrada de forma inquestionável no vídeo.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Roteiro estruturado: Inserção ➔ `SELECT`, Alteração (`PUT`) ➔ `SELECT`, Consulta ➔ `SELECT`, Exclusão (`DELETE`) ➔ `SELECT`.
- [ ] Evidência clara da persistência no banco Oracle executando no ACI.

##### Tarefas Técnicas (Child Tasks)
* **Task 9.1:** [TASK-22] Elaborar roteiro de teste com payloads JSON e queries SQL correspondentes. *(Activity: Development, Est: 2.0h)*
  * *Descrição:* Organizar sequência de requisições e conferências SQL.
* **Task 9.2:** [TASK-23] Realizar ensaio do teste contra os containers online na Azure. *(Activity: Testing, Est: 2.0h)*
  * *Descrição:* Validar respostas em tempo real no ambiente de produção serverless.

---

#### 🔹 [PBI-10] Gravação do Vídeo Explicativo por Voz (720p+) e Geração do PDF Oficial da Entrega
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEATURE 04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial`
* **State:** `Approved`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `1`
* **Tags:** `Sprint3, DevOps, Video, PDF, Delivery`

##### Descrição (História de Usuário)
> **Como** equipe Pet Guardian,  
> **Eu quero** gravar um vídeo contínuo narrado demonstrando a criação dos recursos via Azure CLI, deploy e testes de persistência, e gerar o PDF oficial da entrega,  
> **Para que** a entrega cumpra 100% das normas avaliativas da Sprint 3.

##### Critérios de Aceite (Acceptance Criteria / Definition of Done)
- [ ] Vídeo em resolução mínima de 720p com áudio claro e narração por voz.
- [ ] Demonstração do clone do repositório, execução do script Azure CLI, deploy no ACI e testes do CRUD com `SELECT` no banco (sem cortes na persistência).
- [ ] Link do YouTube em modo Não Listado inserido no README e no PDF.
- [ ] Arquivo PDF gerado contendo estritamente: nomes completos e RMs em **ordem alfabética**, link do GitHub e link do YouTube.

##### Tarefas Técnicas (Child Tasks)
* **Task 10.1:** [TASK-24] Preparar ambiente de gravação (terminais, VS Code, Portal Azure e Swagger abertos). *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Ensaiar a sequência: Clone ➔ Script CLI ➔ Portal Azure ➔ Swagger ➔ SELECT no Banco.
* **Task 10.2:** [TASK-25] Gravar a demonstração em alta definição narrando detalhadamente cada etapa. *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Gravar sem cortes nas etapas de testes e persistência e publicar no YouTube.
* **Task 10.3:** [TASK-26] Gerar o documento PDF de folha de rosto e publicar no portal da FIAP. *(Activity: Documentation, Est: 2.0h)*
  * *Descrição:* Montar o PDF contendo apenas os dados dos integrantes em ordem alfabética e links oficiais.

---

## 👥 5. Integrantes do Grupo e Responsabilidades (Ordem Alfabética Estrita)

| Integrante | RM | Turma | Responsabilidade Principal na Sprint 3 |
| :--- | :---: | :---: | :--- |
| **Enzo Okuizumi** | **561432** | 2TDSPG | Mobile Development (React Native), Integração TanStack Query & Coordenação Geral |
| **Gustavo Okada** | **563428** | 2TDSPG | Java Advanced (Spring Security JWT, Flyway e SOLID) & .NET Observabilidade |
| **Lucas Barros Gouveia** | **566422** | 2TDSPG | Database Advanced (PL/SQL, Funções, Procedures e Triggers DML) |
| **Luna de Carvalho Guimarães** | **562290** | 2TDSPG | Disruptive Architectures (FastAPI, IA Generativa, RAG e Chat) & Compliance |
| **Milton Marcelino** | **564836** | 2TDSPG | DevOps Tools & Cloud Computing (Azure CLI, ACR, ACI e Containers) |
