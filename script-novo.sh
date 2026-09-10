#!/bin/bash
# =============================================================================
# PetGuardian — Deploy Azure (Comandos Limpos para Git Bash)
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

# -----------------------------------------------------------------------------
# 1. Limpeza Prévia (Opcional - apaga o RG anterior se já existir)
# -----------------------------------------------------------------------------
if [ "$(az group exists --name "$RESOURCE_GROUP" 2>/dev/null)" = "true" ]; then
  echo "Apagando Resource Group $RESOURCE_GROUP anterior..."
  az group delete --name "$RESOURCE_GROUP" --yes
fi

# -----------------------------------------------------------------------------
# 2. Criação do Resource Group
# -----------------------------------------------------------------------------
az group create \
  --name "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --output table

# -----------------------------------------------------------------------------
# 3. Azure Container Registry (ACR)
# -----------------------------------------------------------------------------
az acr create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$ACR_NAME" \
  --sku Basic \
  --admin-enabled true \
  --output table

ACR_LOGIN_SERVER=$(az acr show --name "$ACR_NAME" --resource-group "$RESOURCE_GROUP" --query "loginServer" -o tsv)
ACR_USERNAME=$(az acr credential show --name "$ACR_NAME" --resource-group "$RESOURCE_GROUP" --query "username" -o tsv)
ACR_PASSWORD=$(az acr credential show --name "$ACR_NAME" --resource-group "$RESOURCE_GROUP" --query "passwords[0].value" -o tsv)

# -----------------------------------------------------------------------------
# 4. Storage Account e File Share (Volume Persistente)
# -----------------------------------------------------------------------------
az storage account create \
  --name "$STORAGE_ACCOUNT_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --sku Standard_LRS \
  --kind StorageV2 \
  --output table

STORAGE_KEY=$(az storage account keys list --resource-group "$RESOURCE_GROUP" --account-name "$STORAGE_ACCOUNT_NAME" --query "[0].value" -o tsv)

az storage share create \
  --account-name "$STORAGE_ACCOUNT_NAME" \
  --account-key "$STORAGE_KEY" \
  --name "$SHARE_NAME" \
  --output table

# -----------------------------------------------------------------------------
# 5. Import da Imagem do PostgreSQL para o ACR (sem Docker local)
# -----------------------------------------------------------------------------
az acr import \
  --name "$ACR_NAME" \
  --source "docker.io/library/postgres:16-alpine" \
  --image "postgres-db-petguardian:v1" \
  --force

# -----------------------------------------------------------------------------
# 6. Deploy do Container PostgreSQL no ACI com Volume Persistente
# -----------------------------------------------------------------------------
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

# -----------------------------------------------------------------------------
# 7. Build da Imagem da API Java direto no ACR (sem Docker local)
# -----------------------------------------------------------------------------
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
echo "✅ DEPLOY FINALIZADO!"
echo "================================================================="
echo "📖 Swagger UI (FQDN): http://${API_FQDN}:8091/swagger-ui/index.html"
echo "📖 Swagger UI (IP):   http://${API_IP}:8091/swagger-ui/index.html"
echo "🩺 Health Check:      http://${API_FQDN}:8091/actuator/health"
echo "🗄️ PostgreSQL (FQDN): ${DB_HOST}:5432"
echo "🗄️ PostgreSQL (IP):   ${DB_IP}:5432"
echo "================================================================="
