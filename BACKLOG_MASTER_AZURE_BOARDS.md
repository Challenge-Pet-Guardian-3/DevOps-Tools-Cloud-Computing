# 📋 Backlog Master Azure Boards — Sprint 3: DevOps Tools & Cloud Computing

> **Projeto:** Pet Guardian (Challenge Clyvo 2026 — 2º Semestre)  
> **Disciplina:** DevOps Tools & Cloud Computing (FIAP — 2TDSPG)  
> **Referência Oficial:** Manual do Challenge 2026 — Páginas 10 a 16  
> **Formato:** Scrum / Azure DevOps (Azure Boards)  

---

## 🎯 1. Diagnóstico dos Requisitos da Sprint 3 (Páginas 10 a 16)

Na Sprint 3, a disciplina de **DevOps Tools & Cloud Computing** exige a **evolução completa da infraestrutura em nuvem (Azure)**. A abordagem baseada em Máquina Virtual (VM) da Sprint 1 foi **descontinuada**. O grupo deve escolher e implementar **uma de duas opções arquiteturais puras**, com rigorosa proibição de misturar tecnologias.

### ⚖️ Comparativo de Opções e Regras Arquiteturais

| Critério | Opção 1: ACR + ACI (Recomendada) | Opção 2: Azure App Service + Banco PaaS |
| :--- | :--- | :--- |
| **Computação da Aplicação** | **Azure Container Instances (ACI)** executando imagem do ACR | **Azure App Service** (Deploy nativo de código / pacote) |
| **Registro de Imagens** | **Azure Container Registry (ACR)** obrigatório | Não aplicável |
| **Banco de Dados em Nuvem** | **Container de Banco no ACI** (Oracle XE / Postgres / MySQL) com volume | **Banco PaaS Gerenciado** (Azure SQL PaaS / MySQL PaaS / Oracle FIAP) |
| **Uso de Containers** | **100% Containerizado** (App E Banco em containers) | **0% Containerizado** (NADA pode ser container) |
| **Provisionamento** | **100% via Azure CLI** (`az group`, `az acr`, `az container`) | **100% via Azure CLI** (`az group`, `az appservice`, `az webapp`, `az sql`) |
| **Segurança do Container** | Container do App **NÃO PODE rodar como root/admin** (`USER appuser`) | Não aplicável |
| **Penalidade por Misturar** | **-40 pontos** se usar App no ACI com Banco PaaS ou sem container | **-40 pontos** se containerizar App ou Banco |

---

## 🚨 2. Gaps Críticos e Análise do Repositório Atual (`DevOps-Tools-Cloud-Computing`)

Ao analisar a pasta herdada da Sprint 1, foram identificados os seguintes itens que **devem ser descartados, substituídos ou refatorados**:

```
[ESTADO ATUAL DO REPOSITÓRIO (SPRINT 1)]       ➔       [ESTADO ALVO (SPRINT 3 - ACR + ACI)]
❌ azure-cli-script.sh (Criação de VM Linux)           ✨ azure-cli-sprint3-acr-aci.sh (Provisiona ACR, ACI App e ACI DB)
❌ draw_macro.png (Arquitetura com VM Ubuntu)          ✨ docs/arquitetura_cloud_azure.png (Diagrama Oficial de Cloud Azure)
❌ Sem usuário não-root no Dockerfile                 ✨ Dockerfile multi-stage com USER appuser (Segurança non-root)
❌ Sem script DDL segregado                           ✨ script_bd.sql (DDL completo com comentários das 2+ tabelas core)
❌ Endpoints PUT ausentes no CRUD da API              ✨ CRUD completo funcional em 2+ tabelas core conectadas
```

### 🗑️ O que APAGAR / SUBSTITUIR:
1. **`azure-cli-script.sh` (Sprint 1):** APAGAR/SUBSTITUIR. Ele executa `az vm create`, `apt-get install docker`, etc. Na Sprint 3, **não se usa VM**.
2. **`docs/draw_macro.png` (Sprint 1):** SUBSTITUIR. O diagrama antigo ilustra uma VM e fluxo genérico. O professor penaliza em **-20 pontos** diagramas em formato de fluxo/UML/TOGAF.
3. **`docs/Documentação DevOps - Pet Guardian.pdf` (Sprint 1):** SUBSTITUIR pelo novo PDF de entrega da Sprint 3 (folha de rosto contendo estritamente nome/RM dos integrantes, link do GitHub e link do YouTube).

### 🛠️ O que MANTER e REFATORAR:
1. **Código-fonte da API (`PetGuardian/`):** Manter e garantir que o CRUD das entidades core (`Pet` e `Atendimento` / `Usuario`) esteja 100% funcional com `GET`, `POST`, `PUT` e `DELETE`.
2. **`Dockerfile`:** Atualizar para criar e trocar para usuário não privilegiado (`USER appuser` ou `USER app`).
3. **`README.md`:** Reformular completamente com: Descrição da Solução, Benefícios para o Negócio, Diagrama com Ícones Oficiais Azure, Passo a Passo (How To) de Deploy via Azure CLI, Comandos Docker (`build`, `push`, `run`), e Scripts de Consulta SQL (`SELECT`) para evidência no vídeo.

---

## ☁️ 3. Padrão Exigido para o Diagrama de Arquitetura Cloud (20 Pontos)

> ⚠️ **ATENÇÃO MÁXIMA (Página 15):** *"Desenho de arquitetura parecido com Fluxo, Togaf, UML não será aceito: -20 pontos"*.

O diagrama deve ser um **Diagrama de Arquitetura de Nuvem Azure** elaborado no **Draw.io** ou **Visual Paradigm (Azure Architecture Diagram Tool)** contendo:
* **Ícones Oficiais da Microsoft Azure:**
  * Azure Resource Group (`rg-petguardian-sprint3`)
  * Azure Container Registry (`acrpetguardian`)
  * Azure Container Instances — API (`aci-petguardian-api` na porta 8080)
  * Azure Container Instances — Banco de Dados (`aci-petguardian-db` na porta 1521 ou 5432) com Azure Files Share Volume
  * Virtual Network (VNet) / Subnet / Network Security Group (NSG)
* **Personas Identificadas:**
  * 🧑‍💻 *Desenvolvedor / DevOps:* Execução do Azure CLI, Build da imagem Docker e Push para o ACR.
  * 📱 *Tutor / Veterinário (Usuário Final):* Requisições HTTP/REST e Swagger UI.
* **Setas de Fluxo Numeradas:**
  1. `[1]` DevOps executa script Azure CLI e provisiona o Resource Group, ACR, ACI de Banco e ACI da API.
  2. `[2]` Build da imagem da aplicação .NET/Java e `docker push` para o repositório privado no ACR.
  3. `[3]` ACI faz o pull seguro da imagem armazenada no ACR via credenciais de admin gerenciadas.
  4. `[4]` Container da API se comunica internamente na rede/IP com o Container do Banco de Dados no ACI.
  5. `[5]` Usuário/Cliente consome a API através do IP público/FQDN na porta 8080.
  6. `[6]` Mutações de dados persistem no volume montado do Azure Files.

---

## 👑 4. Estrutura do Backlog no Azure Boards (Scrum)

```
[EPIC-03] Sprint 3 - DevOps Tools & Cloud Computing: Arquitetura em Nuvem Serverless Containerizada (ACR + ACI)
│
├── [FEAT-01] Engenharia de Containers & Segurança da Aplicação (.NET / Java)
│   ├── [PBI-01] Refatoração do Dockerfile Multi-Stage com Usuário Sem Privilégios (Non-Root)
│   ├── [PBI-02] Segregação do DDL do Banco Core com Comentários (script_bd.sql)
│   └── [PBI-03] Validação e Ajuste do CRUD de 2+ Entidades Relacionadas com Conteúdo Significativo
│
├── [FEAT-02] Infraestrutura como Código (IaC) via Azure CLI
│   ├── [PBI-04] Script Azure CLI para Provisionamento do Resource Group e Azure Container Registry (ACR)
│   ├── [PBI-05] Script Azure CLI para Provisionamento do Container de Banco de Dados no ACI com Volume
│   └── [PBI-06] Script Azure CLI para Build, Push e Deploy do Container da Aplicação no ACI
│
├── [FEAT-03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub
│   ├── [PBI-07] Elaboração do Diagrama Macro de Arquitetura Cloud Azure com Ícones Oficiais e Fluxos
│   └── [PBI-08] Reestruturação Completa do README.md (How-To, Comandos, Benefícios e DDL)
│
└── [FEAT-04] Roteiro de Demonstração, Gravação do Vídeo sem Cortes e Entrega Oficial
    ├── [PBI-09] Roteiro e Execução dos Testes com Evidência de Persistência no Banco via SELECT
    └── [PBI-10] Gravação do Vídeo Explicativo por Voz (720p+) e Geração do PDF Oficial da Entrega
```

---

## 📊 5. Tabela Resumo do Backlog (Story Points & Prioridades)

| ID | Título do Item de Backlog (PBI) | Feature Pai | Story Points | Prioridade | Horas Estimadas |
| :--- | :--- | :--- | :---: | :---: | :---: |
| **PBI-01** | Dockerfile Multi-Stage com Usuário Non-Root | `[FEAT-01]` Containers & App | 3 pts | 1 - Critical | 4h |
| **PBI-02** | DDL Estruturado com Comentários (`script_bd.sql`) | `[FEAT-01]` Containers & App | 2 pts | 1 - Critical | 2h |
| **PBI-03** | Validação do CRUD de 2+ Tabelas Core Relacionadas | `[FEAT-01]` Containers & App | 3 pts | 1 - Critical | 4h |
| **PBI-04** | Script Azure CLI para Resource Group e ACR | `[FEAT-02]` Azure CLI IaC | 3 pts | 1 - Critical | 3h |
| **PBI-05** | Script Azure CLI para Deploy do Banco de Dados no ACI | `[FEAT-02]` Azure CLI IaC | 5 pts | 1 - Critical | 5h |
| **PBI-06** | Script Azure CLI para Deploy da API no ACI | `[FEAT-02]` Azure CLI IaC | 5 pts | 1 - Critical | 5h |
| **PBI-07** | Diagrama de Arquitetura Cloud Azure (Visual Paradigm/Draw.io) | `[FEAT-03]` Arquitetura & Docs | 5 pts | 1 - Critical | 5h |
| **PBI-08** | README.md Completo com How-To, Benefícios e Comandos | `[FEAT-03]` Arquitetura & Docs | 5 pts | 1 - Critical | 4h |
| **PBI-09** | Bateria de Testes do CRUD com Validação por SELECT | `[FEAT-04]` Vídeo & Entrega | 3 pts | 1 - Critical | 4h |
| **PBI-10** | Gravação do Vídeo sem Cortes e PDF Oficial de Entrega | `[FEAT-04]` Vídeo & Entrega | 5 pts | 1 - Critical | 6h |
| **TOTAL** | **10 PBIs / 25 Child Tasks** | **4 Features / 1 Epic** | **39 pts** | — | **42h** |

---

## 📄 6. Detalhamento dos Product Backlog Items (PBIs) e Child Tasks

---

### 🔹 [PBI-01] Refatoração do Dockerfile Multi-Stage com Usuário Sem Privilégios (Non-Root)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-01] Engenharia de Containers & Segurança da Aplicação`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Tags:** `DevOps`, `Docker`, `Security`, `Non-Root`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** engenheiro DevOps,  
> **Eu quero** criar e otimizar o `Dockerfile` multi-stage da aplicação configurando um usuário não privilegiado (`USER appuser`),  
> **Para que** a imagem containerizada execute em conformidade com as práticas de segurança em nuvem e atenda à exigência estrita do manual da Sprint 3 (evitando desconto de até -10 pts).

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Build multi-stage implementado com estágios claros (`base`, `build`, `publish`, `final`).
- [ ] Criação explícita de usuário e grupo sem permissões administrativas (`RUN adduser --disabled-password --gecos "" appuser`).
- [ ] Troca de contexto no estágio final com `USER appuser` (não rodar como `root` ou `admin`).
- [ ] Permissões de escrita e leitura configuradas corretamente nos diretórios necessários da aplicação.
- [ ] Imagem gerada e testada localmente executando com sucesso e respondendo na porta exposta (8080).

#### Tarefas Técnicas (Child Tasks)
* **Task 1.1:** Ajustar estágios do `Dockerfile` para .NET 8/10 ou Java com multi-stage build. *(Estimativa: 2h)*
  * *Descrição:* Separar estágio de compilação SDK do estágio leve de runtime.
* **Task 1.2:** Configurar usuário não-root (`appuser`) e ajustar permissões no container. *(Estimativa: 1h)*
  * *Descrição:* Adicionar diretiva `USER appuser` e validar que o processo não roda como root (`docker exec id`).
* **Task 1.3:** Testar build local e validar execução de endpoints pelo container. *(Estimativa: 1h)*
  * *Descrição:* Rodar `docker build` e `docker run -p 8080:8080` validando Swagger e logs.

---

### 🔹 [PBI-02] Segregação do DDL do Banco Core com Comentários (`script_bd.sql`)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-01] Engenharia de Containers & Segurança da Aplicação`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `2`
* **Tags:** `DevOps`, `Database`, `DDL`, `Oracle`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** arquiteto de banco de dados,  
> **Eu quero** criar o arquivo `script_bd.sql` contendo o DDL das tabelas que compõem o core da aplicação com estrutura completa e comentários,  
> **Para que** os avaliadores possam auditar a modelagem relacional de forma isolada, evitando a penalidade de -10 pontos.

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

### 🔹 [PBI-03] Validação e Ajuste do CRUD de 2+ Entidades Relacionadas com Conteúdo Significativo
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-01] Engenharia de Containers & Segurança da Aplicação`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Tags:** `DevOps`, `Backend`, `CRUD`, `REST`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** desenvolvedor backend,  
> **Eu quero** garantir que a aplicação disponibilize operações completas de CRUD (`GET`, `POST`, `PUT`, `DELETE`) em pelo menos 2 tabelas relacionadas entre si, com massa de dados de no mínimo 2 registros válidos,  
> **Para que** possamos executar a demonstração completa exigida pelas regras 4 e 5 da disciplina.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Pelo menos 2 tabelas relacionadas contempladas no CRUD (ex: `Pet` e `Atendimento`).
- [ ] Todas as 4 operações HTTP implementadas e testáveis via Swagger / Postman / cURL:
  - Inclusão (`POST`) gerando chave e persistindo no banco;
  - Consulta (`GET` por ID e `GET` paginado/lista);
  - Alteração (`PUT`) atualizando campos significativos;
  - Exclusão (`DELETE`) com integridade referencial.
- [ ] Mínimo de 2 registros com conteúdo significativo inseridos e manipulados durante a demonstração.

#### Tarefas Técnicas (Child Tasks)
* **Task 3.1:** Validar endpoints de `POST`, `GET`, `PUT` e `DELETE` para `Pet` e `Atendimento`. *(Estimativa: 2h)*
  * *Descrição:* Verificar o funcionamento dos controllers e a persistência real no banco de dados.
* **Task 3.2:** Preparar payloads JSON de teste com dados reais para o vídeo demonstrativo. *(Estimativa: 2h)*
  * *Descrição:* Criar coleção de requisições prontas para executar no Swagger e no terminal.

---

### 🔹 [PBI-04] Script Azure CLI para Provisionamento do Resource Group e Azure Container Registry (ACR)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Tags:** `DevOps`, `Azure-CLI`, `ACR`, `IaC`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** engenheiro de nuvem,  
> **Eu quero** desenvolver um script em Azure CLI que crie o Resource Group e provisione o Azure Container Registry (ACR) com SKU Basic e admin habilitado,  
> **Para que** tenhamos um repositório corporativo e seguro para hospedar as imagens da aplicação na nuvem Azure.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Script em Shell/Bash com tratamento de erros (`set -e`) e variáveis padronizadas.
- [ ] Execução com sucesso do comando `az group create --name <RG> --location <LOCATION>`.
- [ ] Execução com sucesso do comando `az acr create --resource-group <RG> --name <ACR_NAME> --sku Basic --admin-enabled true`.
- [ ] Script idempotente ou com mensagens informativas de progresso.
- [ ] Sem credenciais ou senhas fixas expostas no script (uso de variáveis e consulta dinâmica).

#### Tarefas Técnicas (Child Tasks)
* **Task 4.1:** Estruturar arquivo de script `azure-cli-deploy.sh` com cabeçalho e variáveis. *(Estimativa: 1h)*
  * *Descrição:* Definir nomes de recursos (`rg-petguardian-sprint3`, `acrpetguardian2026`, etc.).
* **Task 4.2:** Implementar comandos de criação do Resource Group e do ACR. *(Estimativa: 1h)*
  * *Descrição:* Validar sintaxe e parâmetros do Azure CLI para criação do registry.
* **Task 4.3:** Adicionar rotina de obtenção dinâmica de credenciais do ACR (`az acr credential show`). *(Estimativa: 1h)*
  * *Descrição:* Extrair usuário e senha do registry sem expor dados no repositório público.

---

### 🔹 [PBI-05] Script Azure CLI para Deploy do Container de Banco de Dados no ACI com Volume
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `Azure-CLI`, `ACI`, `Database-Container`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** engenheiro de infraestrutura,  
> **Eu quero** criar via Azure CLI a instância de container (ACI) para o banco de dados (Oracle XE / Postgres) com volume persistente (Azure File Share),  
> **Para que** a solução cumpra o requisito de 100% de containerização do banco na nuvem sem perder dados após reinicializações.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Criação de Azure Storage Account e Azure File Share via Azure CLI (`az storage share create`).
- [ ] Deploy do container do Banco de Dados no ACI via comando `az container create`:
  - Imagem do banco (ex: `gvenzl/oracle-xe:21-slim` ou `postgres:15-alpine`);
  - Definição de CPU (1 a 2 cores) e Memória (2GB a 4GB);
  - Porta de banco exposta (1521 ou 5432) com IP público ou DNS name;
  - Montagem do volume persistente do Azure File Share;
  - Variáveis de ambiente de inicialização configuradas de forma segura.
- [ ] Teste de conectividade e execução do script `script_bd.sql` no banco containerizado na nuvem.

#### Tarefas Técnicas (Child Tasks)
* **Task 5.1:** Criar comandos Azure CLI para Storage Account e File Share para persistência de volume. *(Estimativa: 2h)*
  * *Descrição:* Provisionar o compartilhamento SMB para montar no container do banco.
* **Task 5.2:** Implementar comando `az container create` para o Banco de Dados. *(Estimativa: 2h)*
  * *Descrição:* Configurar limites de recursos, portas e variáveis de ambiente no ACI.
* **Task 5.3:** Validar conexão externa com o banco no ACI e aplicar o `script_bd.sql`. *(Estimativa: 1h)*
  * *Descrição:* Testar conexão via DBeaver / SQL Developer / psql contra o IP público do ACI.

---

### 🔹 [PBI-06] Script Azure CLI para Build, Push e Deploy do Container da Aplicação no ACI
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-02] Infraestrutura como Código (IaC) via Azure CLI`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `Azure-CLI`, `ACI`, `ACR`, `App-Deploy`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** engenheiro de automação,  
> **Eu quero** criar a rotina de build/push da imagem para o ACR e realizar o deploy do container da API no ACI via Azure CLI,  
> **Para que** a aplicação execute na nuvem com alta disponibilidade, conectada ao container do banco e acessível publicamente.

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

### 🔹 [PBI-07] Elaboração do Diagrama Macro de Arquitetura Cloud Azure com Ícones Oficiais e Fluxos
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `Architecture`, `Azure-Diagram`, `Draw.io`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** arquiteto de soluções cloud,  
> **Eu quero** desenhar o diagrama de arquitetura da solução no Draw.io/Visual Paradigm utilizando a biblioteca oficial de ícones da Azure, setas numeradas e personas,  
> **Para que** a entrega atenda 100% ao critério de arquitetura (20 pontos) e evite a penalidade de -20 pontos para diagramas em formato inadequado.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Diagrama desenhado no Draw.io ou Visual Paradigm utilizando estritamente **ícones oficiais da Azure Cloud**.
- [ ] Inclusão explícita de todos os componentes da infraestrutura:
  - Azure Resource Group
  - Azure Container Registry (ACR)
  - Azure Container Instances (ACI) - API
  - Azure Container Instances (ACI) - Banco de Dados
  - Azure Storage Account / File Share Volume
  - Portas de rede expostas (8080 e 1521/5432)
- [ ] Personas representadas (Usuário Final e Desenvolvedor/DevOps).
- [ ] Setas de fluxo com números sequenciais (1, 2, 3, 4, 5) e legenda detalhada explicando cada interação.
- [ ] Exportação do arquivo em alta resolução em `docs/arquitetura_cloud_azure.png` e inclusão no README.

#### Tarefas Técnicas (Child Tasks)
* **Task 7.1:** Mapear componentes, portas e interações entre os serviços Azure. *(Estimativa: 2h)*
  * *Descrição:* Rascunhar a topologia de rede e os fluxos de deploy e consumo.
* **Task 7.2:** Desenhar o diagrama oficial no Draw.io / Visual Paradigm com ícones Azure e legendas. *(Estimativa: 2h)*
  * *Descrição:* Diagramar com acabamento profissional, cores corporativas e setas explicativas.
* **Task 7.3:** Exportar imagem em alta resolução e incorporar no `README.md` e na documentação. *(Estimativa: 1h)*
  * *Descrição:* Salvar em `docs/arquitetura_cloud_azure.png` e validar visualização no GitHub.

---

### 🔹 [PBI-08] Reestruturação Completa do README.md (How-To, Comandos, Benefícios e DDL)
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-03] Arquitetura de Nuvem, Documentação Técnica e Repositório GitHub`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `Documentation`, `README`, `GitHub`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** Tech Lead do time,  
> **Eu quero** reescrever o arquivo `README.md` do repositório contendo todas as seções obrigatórias especificadas na página 11, 12 e 13 do manual,  
> **Para que** qualquer desenvolvedor ou avaliador consiga clonar o repositório e reproduzir o provisionamento na íntegra sem ambiguidades.

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

### 🔹 [PBI-09] Bateria de Testes do CRUD com Validação por SELECT no Banco de Dados
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-04] Roteiro de Demonstração, Gravação do Vídeo e Entrega Oficial`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `3`
* **Tags:** `DevOps`, `QA`, `Testing`, `Validation`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** analista de qualidade / QA,  
> **Eu quero** elaborar um roteiro de testes executando as 4 operações de CRUD na API em nuvem e conferindo imediatamente a mutação no banco via `SELECT`,  
> **Para que** a gravação do vídeo atenda estritamente à regra 9.3 do manual sem falhas ou inconsistências.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] Roteiro cobrindo:
  1. `POST /api/pet` ➔ `SELECT * FROM PET WHERE ID = ...;` (evidenciando a inserção);
  2. `POST /api/atendimento` ➔ `SELECT * FROM ATENDIMENTO WHERE ID = ...;` (evidenciando a relação);
  3. `PUT /api/pet/{id}` ➔ `SELECT * FROM PET WHERE ID = ...;` (evidenciando a alteração de dados);
  4. `GET /api/pet/{id}` e `GET /api/atendimento` (evidenciando a consulta de dados);
  5. `DELETE /api/atendimento/{id}` ➔ `SELECT * FROM ATENDIMENTO;` (evidenciando a exclusão).
- [ ] Todos os comandos SQL testados e validados no banco de dados containerizado no ACI.
- [ ] Massa de dados com conteúdo significativo e contextualizado com o problema da Clyvo.

#### Tarefas Técnicas (Child Tasks)
* **Task 9.1:** Executar bateria completa de testes nos endpoints da API rodando no ACI. *(Estimativa: 2h)*
  * *Descrição:* Testar as requisições HTTP e validar os retornos HTTP (`201 Created`, `200 OK`, `204 No Content`).
* **Task 9.2:** Validar a execução dos comandos `SELECT` no cliente de banco de dados. *(Estimativa: 2h)*
  * *Descrição:* Garantir que a exibição das tabelas fique clara e nítida no monitor para a gravação.

---

### 🔹 [PBI-10] Gravação do Vídeo Explicativo por Voz (720p+) e Geração do PDF Oficial de Entrega
* **Work Item Type:** `Product Backlog Item`
* **Parent Feature:** `[FEAT-04] Roteiro de Demonstração, Gravação do Vídeo e Entrega Oficial`
* **State:** `New`
* **Priority:** `1 - Critical`
* **Effort (Story Points):** `5`
* **Tags:** `DevOps`, `Video`, `Presentation`, `Deliverable`, `Sprint3`

#### Descrição (História de Usuário)
> **Como** membro da equipe Pet Guardian,  
> **Eu quero** gravar o vídeo demonstrativo narrado por voz, sem cortes durante os testes e deploy, e gerar o PDF de entrega no formato estrito exigido,  
> **Para que** a nota máxima de 80 pontos no vídeo seja atingida sem penalidades por regras de formato.

#### Critérios de Aceite (Acceptance Criteria)
- [ ] **Vídeo Gravado e Publicado no YouTube (Não Listado ou Público):**
  - Resolução mínima de **720p** com áudio claro e **explicação por voz** de integrante(s) (proibido apenas legendas);
  - Início obrigatório com o **clone do repositório no GitHub** em terminal limpo;
  - Demonstração da **execução do Script Azure CLI** criando os recursos na Azure (Resource Group, ACR, ACI Banco, ACI API);
  - Visualização dos recursos provisionados no Portal do Azure;
  - Demonstração da aplicação e Swagger rodando no IP/FQDN do ACI;
  - **Sem cortes** durante a demonstração das operações de CRUD e conferência imediata no banco via `SELECT`;
  - Demonstração de inserção, alteração, exclusão e consulta com evidência no banco.
- [ ] **Arquivo PDF Oficial de Entrega (`2TDSPG_Sprint3_DevOps.pdf`):**
  - Contém **exclusivamente**:
    1. Folha de rosto com Nome Completo, RM e Turma de todos os integrantes em ordem alfabética;
    2. Link do repositório no GitHub;
    3. Link do vídeo no YouTube.
  - **NÃO PODE CONTER MAIS NADA NO PDF** (todo o conteúdo técnico deve estar no GitHub, regra estrita da página 12).

#### Tarefas Técnicas (Child Tasks)
* **Task 10.1:** Preparar ambiente de gravação (terminais, VS Code, DBeaver, Portal Azure e Swagger abertos). *(Estimativa: 2h)*
  * *Descrição:* Ensaiar a sequência: Clone ➔ Script CLI ➔ Portal Azure ➔ Swagger ➔ SELECT no Banco.
* **Task 10.2:** Gravar a demonstração em alta definição narrando detalhadamente cada etapa. *(Estimativa: 2h)*
  * *Descrição:* Gravar sem cortes nas etapas de testes e persistência, subir para o YouTube e testar o link.
* **Task 10.3:** Gerar o documento PDF de folha de rosto e publicar no portal da FIAP. *(Estimativa: 2h)*
  * *Descrição:* Montar o PDF contendo apenas os dados dos integrantes e links oficiais.

---

## ⚠️ 7. Matriz de Riscos e Penalidades a Evitar

| Risco / Item Avaliativo | Penalidade no Manual | Ação Preventiva no Backlog |
| :--- | :---: | :--- |
| **Entrega em LOCALHOST** | **Nota ZERO na Sprint** | Deploy obrigatório no Azure Container Instances (ACI) via Azure CLI com IP/DNS público. |
| **Misturar opções (ex: App no ACI + Banco PaaS / FIAP)** | **-40 pontos** | Opção 1 pura: App no ACI + Banco de Dados em container no ACI com volume Azure File Share. |
| **Recursos não criados via Azure CLI** | **-30 pontos** | Script `azure-cli-deploy.sh` automatiza 100% dos recursos sem cliques manuais no portal. |
| **Container do App rodando como root/admin** | **-10 pontos** | PBI-01 configura `USER appuser` no estágio final do Dockerfile. |
| **Desenho de arquitetura parecido com Fluxo, TOGAF ou UML** | **-20 pontos** | PBI-07 cria diagrama oficial de Cloud Architecture com ícones oficiais da Azure e fluxos numerados. |
| **Sem evidência clara de cada operação do CRUD via SELECT** | **-30 pontos** | PBI-09 e PBI-10 executam `SELECT` no terminal/DBeaver imediatamente após cada `POST`, `PUT` e `DELETE`. |
| **Vídeo sem explicação falada ou baixa qualidade (<720p)** | **-30 pontos** | Gravação em 1080p com narração clara por voz dos integrantes. |
| **Conteúdo extra no PDF além de Folha de Rosto e Links** | **Perda de pontos** | PBI-10 restringe o PDF estritamente a nomes, RMs, link do GitHub e link do YouTube. |
| **Utilizar apenas 1 tabela no CRUD ou tabelas fora do core** | **-20 a -30 pontos** | PBI-03 implementa CRUD em 2 tabelas core relacionadas (`Pet` e `Atendimento`). |
| **Ausência do script DDL das tabelas (`script_bd.sql`)** | **-10 pontos** | PBI-02 fornece o arquivo `script_bd.sql` isolado e comentado na raiz. |

---

## 👥 8. Integrantes da Equipe (Ordem Alfabética Obrigatória)

| Nome Completo | RM | Turma | Papel / Responsabilidade |
| :--- | :---: | :---: | :--- |
| **Enzo Okuizumi** | 561432 | 2TDSPG | DevOps Engineer / Automação Azure CLI & ACI |
| **Gustavo Okada** | 563428 | 2TDSPG | Cloud Architect / Diagramação de Arquitetura Azure |
| **Lucas Barros Gouveia** | 566422 | 2TDSPG | Backend Developer / CRUD & Integração com Banco de Dados |
| **Luna de Carvalho Guimarães** | 562290 | 2TDSPG | QA Engineer / Roteiro de Testes e Queries de Validação |
| **Milton Marcelino** | 564836 | 2TDSPG | Security & Containers / Dockerfile Non-Root & Documentação |
