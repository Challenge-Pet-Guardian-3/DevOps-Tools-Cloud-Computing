#!/bin/bash
# =============================================================================
# PetGuardian — Deploy Azure (Comandos Limpos e Narrados para Git Bash)
# Disciplina: DevOps Tools & Cloud Computing — Sprint 3
# =============================================================================

# Desativa conversão automática de paths do Git Bash para evitar quebra no Azure CLI
export MSYS_NO_PATHCONV=1

# -----------------------------------------------------------------------------
# 0. Variáveis de Configuração
# -----------------------------------------------------------------------------
RESOURCE_GROUP="rg-petguardian"
LOCATION="southafricanorth"
ACR_NAME="acrpetguardian"
SHARE_NAME="pgdata"
DB_NAME="petguardian"
DB_USER="petguardian"
DB_PASSWORD="petguardian_senha"
JAVA_DIR="./Java-Advanced"

# Sufixo dinâmico único baseado na subscription (evita conflito global de nomes)
SUB_SUFFIX=$(az account show --query 'id' -o tsv | tr -d '-' | cut -c1-8)
STORAGE_ACCOUNT_NAME="stpetg${SUB_SUFFIX}"

echo ""
echo "================================================================="
echo "⚙️ INICIANDO PROVISIONAMENTO DA INFRAESTRUTURA PETGUARDIAN"
echo "  Resource Group:  $RESOURCE_GROUP"
echo "  Região (Location): $LOCATION"
echo "  ACR:             $ACR_NAME"
echo "  Storage Account: $STORAGE_ACCOUNT_NAME"
echo "================================================================="

# -----------------------------------------------------------------------------
# 1. Limpeza Prévia (Opcional - apaga o RG anterior se já existir)
# -----------------------------------------------------------------------------
if [ "$(az group exists --name "$RESOURCE_GROUP" 2>/dev/null)" = "true" ]; then
  echo ""
  echo "🧹 [LIMPEZA] Resource Group $RESOURCE_GROUP anterior detectado. Apagando..."
  az group delete --name "$RESOURCE_GROUP" --yes
  echo "✅ Resource Group anterior removido com sucesso."
fi

# -----------------------------------------------------------------------------
# 2. Criação do Resource Group
# -----------------------------------------------------------------------------
echo ""
echo "▶ [1/8] Criando o Resource Group ($RESOURCE_GROUP) na região ($LOCATION)..."
az group create \
  --name "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --output table

# -----------------------------------------------------------------------------
# 3. Azure Container Registry (ACR)
# -----------------------------------------------------------------------------
echo ""
echo "▶ [2/8] Criando o Azure Container Registry ($ACR_NAME) para hospedar as imagens..."
az acr create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$ACR_NAME" \
  --sku Basic \
  --admin-enabled true \
  --output table

echo ""
echo "🔑 Obtendo credenciais administrativas do ACR..."
ACR_LOGIN_SERVER=$(az acr show --name "$ACR_NAME" --resource-group "$RESOURCE_GROUP" --query "loginServer" -o tsv)
ACR_USERNAME=$(az acr credential show --name "$ACR_NAME" --resource-group "$RESOURCE_GROUP" --query "username" -o tsv)
ACR_PASSWORD=$(az acr credential show --name "$ACR_NAME" --resource-group "$RESOURCE_GROUP" --query "passwords[0].value" -o tsv)
echo "  Login Server: $ACR_LOGIN_SERVER"

# -----------------------------------------------------------------------------
# 4. Storage Account e File Share (Volume Persistente)
# -----------------------------------------------------------------------------
echo ""
echo "▶ [3/8] Criando a Storage Account ($STORAGE_ACCOUNT_NAME) para o volume persistente..."
az storage account create \
  --name "$STORAGE_ACCOUNT_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --sku Standard_LRS \
  --kind StorageV2 \
  --output table

echo ""
echo "🔑 Obtendo chave de acesso da Storage Account..."
STORAGE_KEY=$(az storage account keys list --resource-group "$RESOURCE_GROUP" --account-name "$STORAGE_ACCOUNT_NAME" --query "[0].value" -o tsv)

echo ""
echo "▶ [4/8] Criando o Azure File Share ($SHARE_NAME) para persistência de dados do banco..."
az storage share create \
  --account-name "$STORAGE_ACCOUNT_NAME" \
  --account-key "$STORAGE_KEY" \
  --name "$SHARE_NAME" \
  --output table

# -----------------------------------------------------------------------------
# 5. Import da Imagem do PostgreSQL para o ACR (sem Docker local)
# -----------------------------------------------------------------------------
echo ""
echo "▶ [5/8] Importando a imagem oficial postgres:16-alpine do Docker Hub diretamente para o ACR..."
az acr import \
  --name "$ACR_NAME" \
  --source "docker.io/library/postgres:16-alpine" \
  --image "postgres-db-petguardian:v1" \
  --force

# -----------------------------------------------------------------------------
# 6. Deploy do Container PostgreSQL no ACI com Volume Persistente
# -----------------------------------------------------------------------------
echo ""
echo "▶ [6/8] Provisionando container do PostgreSQL no ACI com o volume Azure Files montado em /mnt/azure..."
az container create \
  --resource-group "$RESOURCE_GROUP" \
  --name aci-db-petguardian \
  --image "${ACR_LOGIN_SERVER}/postgres-db-petguardian:v1" \
  --os-type Linux \
  --cpu 1 \
  --memory 1.5 \
  --restart-policy Always \
  --registry-login-server "${ACR_LOGIN_SERVER}" \
  --registry-username "$ACR_USERNAME" \
  --registry-password "$ACR_PASSWORD" \
  --dns-name-label "postgres-petg-${SUB_SUFFIX}" \
  --ports 5432 \
  --environment-variables \
    POSTGRES_DB="$DB_NAME" \
    POSTGRES_USER="$DB_USER" \
    POSTGRES_PASSWORD="$DB_PASSWORD" \
  --azure-file-volume-account-name "$STORAGE_ACCOUNT_NAME" \
  --azure-file-volume-account-key "$STORAGE_KEY" \
  --azure-file-volume-share-name "$SHARE_NAME" \
  --azure-file-volume-mount-path "/mnt/azure" \
  --output table

DB_HOST="postgres-petg-${SUB_SUFFIX}.${LOCATION}.azurecontainer.io"
DB_IP=$(az container show --resource-group "$RESOURCE_GROUP" --name aci-db-petguardian --query "ipAddress.ip" -o tsv)
DB_TARGET="${DB_IP:-$DB_HOST}"
echo "  PostgreSQL FQDN: $DB_HOST:5432 (IP: $DB_TARGET:5432)"

# -----------------------------------------------------------------------------
# 7. Build da Imagem da API Java direto no ACR (sem Docker local)
# -----------------------------------------------------------------------------
echo ""
echo "▶ [7/8] Compilando a API Java e gerando a imagem Docker direto no ACR (az acr build em nuvem)..."
ACR_BUILD_DIR=$(cygpath -w "$(cd "$JAVA_DIR" && pwd)")

az acr build \
  --registry "$ACR_NAME" \
  --image api-petguardian:v1 \
  --timeout 3600 \
  "$ACR_BUILD_DIR"

JDBC_URL="jdbc:postgresql://${DB_TARGET}:5432/${DB_NAME}"

# -----------------------------------------------------------------------------
# 8. Deploy do Container da API Java Spring Boot no ACI
# -----------------------------------------------------------------------------
echo ""
echo "▶ [8/8] Provisionando container da API Java Spring Boot no ACI (porta 8091) conectado ao PostgreSQL..."
az container create \
  --resource-group "$RESOURCE_GROUP" \
  --name aci-api-petguardian \
  --image "${ACR_LOGIN_SERVER}/api-petguardian:v1" \
  --os-type Linux \
  --cpu 1 \
  --memory 1.5 \
  --restart-policy Always \
  --registry-login-server "${ACR_LOGIN_SERVER}" \
  --registry-username "$ACR_USERNAME" \
  --registry-password "$ACR_PASSWORD" \
  --dns-name-label "api-petg-${SUB_SUFFIX}" \
  --ports 8091 \
  --environment-variables \
    PGHOST="$DB_TARGET" \
    PGPORT="5432" \
    PGDATABASE="$DB_NAME" \
    SPRING_DOCKER_COMPOSE_ENABLED="false" \
  --secure-environment-variables \
    PGUSER="$DB_USER" \
    PGPASSWORD="$DB_PASSWORD" \
    SPRING_DATASOURCE_URL="$JDBC_URL" \
    SPRING_DATASOURCE_USERNAME="$DB_USER" \
    SPRING_DATASOURCE_PASSWORD="$DB_PASSWORD" \
  --output table

API_FQDN="api-petg-${SUB_SUFFIX}.${LOCATION}.azurecontainer.io"
API_IP=$(az container show --resource-group "$RESOURCE_GROUP" --name aci-api-petguardian --query "ipAddress.ip" -o tsv)

# -----------------------------------------------------------------------------
# 9. URLs Finais de Acesso
# -----------------------------------------------------------------------------
echo ""
echo "================================================================="
echo "✅ DEPLOY FINALIZADO COM SUCESSO!"
echo "================================================================="
echo "📖 Swagger UI (FQDN): http://${API_FQDN}:8091/swagger-ui/index.html"
echo "📖 Swagger UI (IP):   http://${API_IP}:8091/swagger-ui/index.html"
echo "🩺 Health Check:      http://${API_FQDN}:8091/actuator/health"
echo "🗄️ PostgreSQL (FQDN): ${DB_HOST}:5432"
echo "🗄️ PostgreSQL (IP):   ${DB_IP}:5432"
echo "================================================================="
