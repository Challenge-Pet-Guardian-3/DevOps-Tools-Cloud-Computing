# DevOps-Tools-Cloud-Computing — 🐾 PetGuardian

> **DevOps Tools & Cloud Computing — Sprint 3**
>
> API REST em **Java Spring Boot 4.1.1** para gestão colaborativa do cuidado de pets. Arquitetura 100% containerizada na Microsoft Azure utilizando **ACR + ACI** com banco de dados **PostgreSQL 16** em container com volume persistente em nuvem.

---

## 🛠️ Tecnologias & Badges

![Java](https://img.shields.io/badge/Java-17-ED8B00?logo=openjdk&logoColor=white&style=for-the-badge)
![Spring Boot](https://img.shields.io/badge/Spring%20Boot-4.1.1-6DB33F?logo=springboot&logoColor=white&style=for-the-badge)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql&logoColor=white&style=for-the-badge)
![Docker](https://img.shields.io/badge/Docker-Multi--Stage-2496ED?logo=docker&logoColor=white&style=for-the-badge)
![Azure](https://img.shields.io/badge/Azure-ACR%20%2B%20ACI-0078D4?logo=microsoftazure&logoColor=white&style=for-the-badge)
![Azure CLI](https://img.shields.io/badge/Azure_CLI-IaC-0089D6?logo=gnubash&logoColor=white&style=for-the-badge)

---

## 🔗 Repositório GitHub e Vídeo de Apresentação

[Repositório GitHub](https://github.com/Challenge-Pet-Guardian-3/DevOps-Tools-Cloud-Computing) | [Vídeo de Demonstração]()

---

## 👥 Integrantes

| Nome | RM | Turma | GitHub | LinkedIn |
| :--- | :---: | :---: | :--- | :--- |
| **Enzo Okuizumi** | **561432** | 2TDSPG | [EnzoOkuizumiFiap](https://github.com/EnzoOkuizumiFiap) | [Enzo Okuizumi](https://www.linkedin.com/in/enzo-okuizumi-b60292256/) |
| **Gustavo Okada** | **563428** | 2TDSPG | [Gdev3356](https://github.com/Gdev3356) | [Gustavo Okada](https://www.linkedin.com/in/gustavo-okada-53a3b8359/) |
| **Lucas Barros Gouveia** | **566422** | 2TDSPG | [LuzBGouveia](https://github.com/LuzBGouveia) | [Lucas Barros Gouveia](https://www.linkedin.com/in/lucas-barros-gouveia-09b147355/) |
| **Luna de Carvalho Guimarães** | **562290** | 2TDSPG | [lunaguima](https://github.com/lunaguima) | [Luna M. Guimarães](https://www.linkedin.com/in/luna-m-guimar%C3%A3es-1850ab173/) |
| **Milton Marcelino** | **564836** | 2TDSPG | [MiltonMarcelino](https://github.com/MiltonMarcelino) | [Milton Marcelino](http://linkedin.com/in/milton-marcelino-250298142) |

---

## 💡 Sobre o Produto

O **PetGuardian** resolve o problema da descentralização do cuidado diário de animais domésticos quando múltiplos cuidadores estão envolvidos. A plataforma organiza responsabilidades, registra o histórico clínico e incentiva a realização de tarefas através de um sistema gamificado centrado no bem-estar do animal.

### 🌟 Pilares da Arquitetura Pet-Centric

* **Círculo de Cuidado Colaborativo:** Vínculo dinâmico `N:N` entre cuidadores (`Usuario`) e `Pet` via tabela associativa `usuario_pet`.
* **Tarefas de Saúde Gamificadas:** Cada tarefa concluída acumula pontos para o cuidador responsável.
* **Histórico Clínico Unificado:** Consolidação cronológica de vacinas, consultas e pesagens do pet.
* **Infraestrutura 100% Containerizada:** API Java e banco PostgreSQL executando no **Azure Container Instances (ACI)**, imagens gerenciadas no **Azure Container Registry (ACR)**, persistência garantida por **Azure Files**.

---

## 📈 Benefícios para o Negócio

* **Centralização do Cuidado:** Elimina falhas de comunicação entre tutores e co-cuidadores ao centralizar histórico e responsabilidades em uma única plataforma.
* **Engajamento por Gamificação:** O sistema de pontuação por tarefa converte obrigações de saúde em interações motivadoras, aumentando a retenção e o engajamento ativo dos cuidadores.
* **Prevenção de Complicações Médicas:** O acompanhamento rigoroso de prazos de vacinas, medicamentos e consultas evita esquecimentos que poderiam resultar em internações emergenciais e custos elevados.
* **Portabilidade e Escalabilidade:** Arquitetura containerizada garante paridade entre desenvolvimento local e nuvem, permitindo escalar os containers de API e banco de forma independente conforme a demanda.
* **Segurança e Auditabilidade:** Controle de acesso por JWT (OAuth2 Resource Server com RSA) e histórico de atendimentos auditável permitem rastreabilidade completa das ações dos cuidadores.

---

## 📐 Desenho Macro da Arquitetura

A arquitetura adota containers gerenciados em nuvem com **Azure Container Registry (ACR)** e **Azure Container Instances (ACI)**, com provisionamento e orquestração 100% automatizados via Azure CLI:

![Desenho Macro](docs/challenge3-petguardian.drawio.png)

### Fluxo da Arquitetura

```text
[Desenvolvedor / Git Bash] ──(1) az acr build (código Java) ────────► [ACR: acrpetguardian]
[Docker Hub Oficial]       ──(2) az acr import (postgres:16-alpine) ─► [ACR: acrpetguardian]
                                                                              │
[ACR: acrpetguardian]      ───────────────────────────────────────────(3)──► [ACI: aci-api-petguardian:8091]
                                                                              │
[Azure Files: pgdata]      ──────volume mount (/mnt/azure)────────────(4)──► [ACI: aci-db-petguardian:5432]
                                                                              │
[Cliente / Swagger / App]  ───────────────────────────────────────────(5)──► [aci-api-petguardian] ──► [aci-db-petguardian]
```

| Componente Azure | Recurso | Função |
| :--- | :--- | :--- |
| Resource Group | `rg-petguardian` | Agrupamento lógico de todos os recursos da solução |
| Container Registry | `acrpetguardian` | Repositório privado de imagens Docker gerenciado no Azure |
| Container Instance (API) | `aci-api-petguardian` | Executa o container da API Java Spring Boot na porta 8091 |
| Container Instance (Banco) | `aci-db-petguardian` | Executa o container do PostgreSQL 16 na porta 5432 |
| Storage Account | `stpetg<suffix>` | Conta de armazenamento com nome dinâmico único baseado na assinatura |
| File Share | `pgdata` | Compartilhamento Azure Files montado em `/mnt/azure` para persistência de volume |

---

## 📂 Estrutura do Repositório

```text
DevOps-Tools-Cloud-Computing/
  ├── Java-Advanced/           # Código-fonte da API Java Spring Boot
  │   ├── src/                 # Pacotes da aplicação
  │   ├── build.gradle         # Configuração de build Gradle
  │   ├── compose.yml          # Docker Compose para ambiente local de desenvolvimento
  │   └── Dockerfile           # Dockerfile multi-stage com usuário non-root (appuser)
  ├── docs/
  │   ├── challenge3-petguardian.drawio.png # Diagrama oficial de arquitetura Cloud Azure
  │   ├── Logical.png          # Modelo lógico do banco de dados
  │   └── Relational.png       # Modelo relacional do banco de dados
  ├── script-novo.sh           # Automação completa de deploy via Azure CLI para Git Bash
  └── script_bd.sql            # DDL das tabelas CORE com comentários e relacionamentos (PostgreSQL)
```

---

## 🗄️ Modelagem do Banco de Dados

### 📐 Modelo Lógico
![Modelo Lógico](docs/Logical.png)

### 🗄️ Modelo Relacional
![Modelo Relacional](docs/Relational.png)

---

## 🐋 Dockerfile da API Java (Multi-Stage + Non-Root)

Por boas práticas de segurança em ambientes de produção (princípio do menor privilégio), o container da aplicação **não executa como root**, utilizando o usuário dedicado `appuser`. O `Dockerfile` multi-stage está localizado em `Java-Advanced/Dockerfile`:

```dockerfile
# Stage 1 — BUILD: compilação com Gradle
FROM gradle:jdk17 AS build
WORKDIR /app
COPY build.gradle settings.gradle ./
COPY gradle/ gradle/
RUN gradle dependencies --no-daemon || true
COPY src/ src/
RUN gradle bootJar --no-daemon -x test

# Stage 2 — RUNTIME: imagem mínima de produção
FROM eclipse-temurin:17-jre-alpine AS runtime
WORKDIR /app

# Cria usuário e grupo sem privilégios administrativos
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

COPY --from=build /app/build/libs/pet-guardian-0.0.1-SNAPSHOT.jar app.jar
COPY --from=build /app/src/main/resources/keys/ /app/keys/
RUN chown -R appuser:appgroup /app

# Executa como usuário não privilegiado
USER appuser

EXPOSE 8091
ENTRYPOINT ["java", "-jar", "app.jar"]
```

---

## 🚀 Como Executar — Guia de Deploy

### Pré-requisitos

* [Azure CLI](https://learn.microsoft.com/pt-br/cli/azure/install-azure-cli) instalado e autenticado (`az login`)
* Terminal [Git Bash](https://gitforwindows.org/) (Windows) ou terminal bash (Linux / macOS)
* Assinatura Azure ativa com permissões para criação de recursos
* *(Opcional)* [Docker Desktop](https://www.docker.com/products/docker-desktop/) — necessário apenas caso queira rodar o teste local com Docker Compose antes de ir para a nuvem. O deploy em nuvem não depende de Docker local.

---

### 1. Clone do Repositório

```bash
git clone https://github.com/Challenge-Pet-Guardian-3/DevOps-Tools-Cloud-Computing.git
cd DevOps-Tools-Cloud-Computing
```

---

### 2. Execução Local com Docker Compose (Opcional - Desenvolvimento)

Para validar a aplicação localmente antes de enviar para a nuvem:

```bash
cd Java-Advanced

# Sobe o banco PostgreSQL localmente
docker compose up -d

# Build da imagem da API
docker build -t petguardian-api:local .

# Executa a API conectada ao banco local
docker run -d \
  -p 8091:8091 \
  -e PGHOST=host.docker.internal \
  -e PGPORT=5432 \
  -e PGDATABASE=petguardian \
  -e PGUSER=petguardian \
  -e PGPASSWORD=petguardian \
  --name petguardian-api \
  petguardian-api:local

# Acesse o Swagger local:
# http://localhost:8091/swagger-ui/index.html
```

---

### 3. Deploy na Nuvem Azure — Script CLI Automatizado (`script-novo.sh`)

O provisionamento de toda a infraestrutura em nuvem é automatizado pelo script `script-novo.sh`. O script foi otimizado para execução sequencial ou comando por comando no **Git Bash**:

```bash
cd DevOps-Tools-Cloud-Computing

# Permissão de execução e início do deploy
chmod +x ./script-novo.sh
./script-novo.sh
```

#### Parâmetros Configuráveis no Início do Script:
| Variável | Valor Padrão | Descrição |
| :--- | :--- | :--- |
| `RESOURCE_GROUP` | `rg-petguardian` | Nome do Resource Group no Azure |
| `LOCATION` | `southafricanorth` | Região Azure com disponibilidade para ACI e ACR |
| `ACR_NAME` | `acrpetguardian` | Nome do Azure Container Registry privado |
| `SHARE_NAME` | `pgdata` | Nome do File Share para o volume persistente |
| `DB_NAME` | `petguardian` | Nome da base de dados PostgreSQL |
| `DB_USER` | `petguardian` | Usuário administrador do banco de dados |
| `DB_PASSWORD` | `petguardian_senha` | Senha de acesso ao banco de dados |
| `JAVA_DIR` | `./Java-Advanced` | Caminho do código-fonte da API Java |

#### Etapas Executadas pelo Script:

| Etapa | Comando Principal | Descrição Operacional |
| :---: | :--- | :--- |
| **Limpeza** | `az group delete` | Remove de forma síncrona o Resource Group anterior caso já exista, garantindo deploy limpo |
| **1/8** | `az group create` | Cria o Resource Group `rg-petguardian` na região configurada |
| **2/8** | `az acr create` | Cria o Azure Container Registry em SKU Basic e obtém as credenciais administrativas |
| **3/8** | `az storage account create` | Cria a conta de armazenamento `stpetg<suffix>` com nome dinâmico único baseado na assinatura |
| **4/8** | `az storage share create` | Cria o compartilhamento Azure Files `pgdata` para o volume de dados |
| **5/8** | `az acr import` | Importa a imagem oficial `postgres:16-alpine` do Docker Hub diretamente para o ACR (sem Docker local) |
| **6/8** | `az container create` (DB) | Provisiona o PostgreSQL no ACI com volume Azure Files montado em `/mnt/azure` |
| **7/8** | `az acr build` | Envia o contexto Java para o Azure e compila a imagem Docker em nuvem via ACR Tasks |
| **8/8** | `az container create` (API) | Sobe o container da API Java Spring Boot no ACI conectado ao IP do container PostgreSQL |
| **URLs** | `Exibição de Endpoints` | Apresenta os links do Swagger UI, Health Check e endpoints de conexão do banco |

---

### 4. Verificação e Monitoramento pós-Deploy

```bash
# Lista os containers provisionados no Resource Group
az container list --resource-group rg-petguardian --output table

# Visualiza os logs da API Java Spring Boot
az container logs --resource-group rg-petguardian --name aci-api-petguardian

# Visualiza os logs do banco de dados PostgreSQL
az container logs --resource-group rg-petguardian --name aci-db-petguardian
```

---

## 🧪 Validação da Aplicação e Persistência de Dados (CRUD + SQL)

Roteiro de testes ponta a ponta dos endpoints REST com conferência direta das alterações relacionais no PostgreSQL.

### 🔌 Acesso ao Terminal Interativo do Banco (psql via Azure CLI)

Abra uma sessão interativa no container do PostgreSQL:

```bash
az container exec \
  --resource-group rg-petguardian \
  --name aci-db-petguardian \
  --exec-command "psql -U petguardian -d petguardian"
```

---

### Configuração da URL Base

Defina a URL base retornada ao final da execução do `script-novo.sh`:

```bash
API_URL="http://api-petg-01bd2f0c.southafricanorth.azurecontainer.io:8091"
# ou utilizando o IP público: API_URL="http://<SEU_IP_PUBLICO>:8091"
```

---

### 📌 Passo 1 — Cadastro de Usuário / Cuidador (INSERT + SELECT)

Como o banco no ACI inicia limpo, o primeiro passo obrigatório é criar um cuidador no sistema via endpoint público (`/usuarios`):

```bash
# POST /usuarios — Cadastro de cuidador com validação de CEP e telefone
curl -X POST "$API_URL/usuarios" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Usuario Teste",
    "email": "usuario@petguardian.com",
    "senha": "senha123",
    "ddd": "11",
    "numeroTelefone": "987654321",
    "role": "COMUM",
    "endereco": {
      "cep": "01001-000",
      "numero": "100"
    }
  }'
```

**Consulta de confirmação no PostgreSQL:**
```sql
SELECT u.id_usuario, u.nome, u.email, u.role, t.num_ddd, t.num_tel 
FROM usuario u 
JOIN telefone t ON t.id_telefone = u.telefone_id_telefone 
ORDER BY u.id_usuario DESC 
LIMIT 5;
```

> **Nota:** Se o usuário já tiver sido cadastrado anteriormente, o banco acusará violação de chave única no campo `email`. Nesse caso, você já pode avançar diretamente para o **Passo 2 (Login)**.

---

### 📌 Passo 2 — Autenticação (Obtenção do Token JWT)

Com o usuário persistido no banco, realize a autenticação para gerar o Bearer Token assinado:

```bash
# POST /login — Autenticação com e-mail e senha cadastrados
curl -X POST "$API_URL/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "usuario@petguardian.com", 
    "senha": "senha123"
  }'
```

Guarde o token retornado para os passos seguintes:
```bash
TOKEN="<SEU_JWT_TOKEN_AQUI>"
```

---

### 📌 Passo 3 — Cadastro de Pet (INSERT + SELECT)

Cadastre um pet associando ao usuário autenticado (`usuarioId: 1`). A raça é resolvida e vinculada automaticamente pelo serviço backend:

```bash
# POST /pets — Cadastro de animal de estimação
curl -X POST "$API_URL/pets" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Max",
    "dataNasc": "2023-05-10",
    "raca": "Golden Retriever",
    "porte": "GRANDE",
    "sexo": "M",
    "castrado": true,
    "usuarioId": 1
  }'
```

**Consulta de confirmação no PostgreSQL:**
```sql
SELECT id_pet, nome, data_nasc, porte, sexo, castrado
FROM pet
ORDER BY id_pet DESC
LIMIT 5;
```

---

### 📌 Passo 4 — Atualização de Dados do Pet (UPDATE + SELECT)

Atualize os dados cadastrais do pet informando seu identificador:

```bash
# PUT /pets/{id} — Atualização cadastral
curl -X PUT "$API_URL/pets/1" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Max Atualizado",
    "dataNasc": "2023-05-10",
    "raca": "Golden Retriever",
    "porte": "GRANDE",
    "sexo": "M",
    "castrado": false,
    "usuarioId": 1
  }'
```

**Consulta de confirmação no PostgreSQL:**
```sql
SELECT id_pet, nome, castrado 
FROM pet 
WHERE id_pet = 1;
```

---

### 📌 Passo 5 — Criação de Tarefa de Cuidado (INSERT com relacionamento + JOIN)

Crie uma tarefa de cuidado vinculada ao cuidador e ao pet:

```bash
# POST /tarefas — Agendamento de tarefa com pontuação
curl -X POST "$API_URL/tarefas" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json; charset=utf-8" \
  -d '{
    "titulo": "Vacina Antirrabica",
    "descricao": "Dose de reforco anual",
    "pontosTarefa": 50,
    "prazo": "2026-10-01T10:00:00",
    "status": "PENDENTE",
    "usuarioId": 1,
    "petId": 1
  }'
```

**Consulta relacional no PostgreSQL (JOIN entre tarefa, status e pet):**
```sql
SELECT t.id_tarefa, t.titulo, t.pontos_tarefa, t.prazo, s.nome_status, p.nome AS nome_pet
FROM tarefa t
JOIN status s ON s.id_status = t.status_id_status
JOIN pet p ON p.id_pet = t.pet_id_pet
ORDER BY t.id_tarefa DESC
LIMIT 5;
```

---

### 📌 Passo 6 — Conclusão de Tarefa (PATCH + SELECT)

Marque a tarefa como concluída indicando o cuidador que executou a ação:

```bash
# PATCH /tarefas/{id}/concluir — Conclusão da tarefa e crédito de pontuação
curl -X PATCH "$API_URL/tarefas/1/concluir" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"concluinteId": 1}'
```

**Consulta de confirmação no PostgreSQL:**
```sql
SELECT id_tarefa, titulo, conclusao, status_id_status 
FROM tarefa 
WHERE id_tarefa = 1;
```

---

### 📌 Passo 7 — Remoção de Tarefa (DELETE + SELECT)

Remova a tarefa finalizada do sistema:

```bash
# DELETE /tarefas/{id} — Exclusão lógica/física da tarefa
curl -X DELETE "$API_URL/tarefas/1" \
  -H "Authorization: Bearer $TOKEN"
```

**Consulta de confirmação no PostgreSQL:**
```sql
SELECT COUNT(*) AS total_tarefas 
FROM tarefa 
WHERE id_tarefa = 1;
-- Retorno esperado: 0
```

---

## 📋 Documentação de Rotas (OpenAPI / Swagger UI)

A interface interativa do Swagger UI está disponível no endpoint:
`http://<API_FQDN_OU_IP>:8091/swagger-ui/index.html`

### Autenticação
| Método | Rota | Descrição |
|:---:|:---|:---|
| POST | `/login` | Autenticação e geração do token JWT (Bearer) |

### Pets (Entidade CORE — CRUD Completo)
| Método | Rota | Descrição |
|:---:|:---|:---|
| GET | `/pets` | Listar todos os pets |
| GET | `/pets/by-usuario` | Listar pets vinculados ao usuário autenticado |
| GET | `/pets/by-nome` | Buscar pet por filtro de nome |
| GET | `/pets/{id}` | Buscar pet por identificador |
| GET | `/pets/{id}/historico` | Buscar histórico clínico detalhado do pet |
| GET | `/pets/{id}/pontos` | Consultar score acumulado do pet |
| POST | `/pets` | Cadastrar novo pet |
| PUT | `/pets/{id}` | Atualizar dados cadastrais do pet |
| DELETE | `/pets/{id}` | Remover pet do sistema |

### Tarefas de Cuidado (Entidade CORE — CRUD Completo)
| Método | Rota | Descrição |
|:---:|:---|:---|
| GET | `/tarefas` | Listar todas as tarefas |
| GET | `/tarefas/by-usuario` | Listar tarefas do usuário autenticado |
| GET | `/tarefas/by-pet/{petId}` | Listar tarefas associadas a um pet |
| GET | `/tarefas/{id}` | Buscar tarefa por identificador |
| GET | `/tarefas/by-usuario/{usuarioId}/{id}` | Buscar tarefa específica de um usuário |
| GET | `/tarefas/by-usuario/pontos` | Score total de pontos do cuidador |
| POST | `/tarefas` | Criar nova tarefa de cuidado |
| PUT | `/tarefas/{id}` | Atualizar tarefa existente |
| PATCH | `/tarefas/{id}/concluir` | Marcar tarefa como concluída (soma pontuação) |
| PATCH | `/tarefas/{id}/desmarcar` | Desmarcar conclusão de tarefa |
| DELETE | `/tarefas/{id}` | Remover tarefa |

### Rede de Cuidado — UsuarioPet (Círculo Colaborativo)
| Método | Rota | Descrição |
|:---:|:---|:---|
| GET | `/pets/{petId}/cuidadores` | Listar co-cuidadores vinculados ao pet |
| POST | `/pets/{petId}/cuidadores` | Adicionar novo co-cuidador ao pet |
| DELETE | `/pets/{petId}/cuidadores/{usuarioId}` | Remover cuidador do pet |
| PATCH | `/pets/{petId}/responsavel-principal` | Transferir responsabilidade principal do pet |

### Usuários
| Método | Rota | Descrição |
|:---:|:---|:---|
| GET | `/usuarios` | Listar todos os usuários |
| GET | `/usuarios/{id}` | Buscar usuário por identificador |
| POST | `/usuarios` | Cadastrar novo usuário |
| PUT | `/usuarios/{id}` | Atualizar dados do usuário |
| DELETE | `/usuarios/{id}` | Remover usuário |

### Histórico Clínico
| Método | Rota | Descrição |
|:---:|:---|:---|
| GET | `/historicos` | Listar histórico de atendimentos clínicos |
| GET | `/historicos/{id}` | Buscar registro clínico por identificador |
| POST | `/historicos` | Registrar novo atendimento / vacina / consulta |
| DELETE | `/historicos/{id}` | Remover registro clínico |

### Trilhas & Módulos (Conteúdo Educativo)
| Método | Rota | Descrição |
|:---:|:---|:---|
| GET | `/trilhas` | Listar trilhas educativas disponíveis |
| GET | `/modulos` | Listar módulos de uma trilha |
| GET | `/aulas` | Listar aulas de um módulo |
