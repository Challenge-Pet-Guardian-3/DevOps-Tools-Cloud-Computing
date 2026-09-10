#!/usr/bin/env bash
# =============================================================================
# PetGuardian — Deploy Completo no Azure (ACR + ACI)
# Disciplina: DevOps Tools & Cloud Computing — Sprint 3
# Aplicação: Java Spring Boot 4.1.1 + PostgreSQL 16
# =============================================================================
# Executar no Git Bash. Antes de rodar:
#   export DB_PASSWORD="petguardian_senha"
#   export DB_USER="petguardian"
# =============================================================================

set -euo pipefail
export MSYS_NO_PATHCONV=1

# -----------------------------------------------------------------------------
# 1. Variáveis de Configuração
# -----------------------------------------------------------------------------
RESOURCE_GROUP="rg-petguardian"
LOCATION="southafricanorth"
ACR_NAME="acrpetguardian"
STORAGE_ACCOUNT_NAME="stpetguardiandata"
SHARE_NAME="pgdata"
DB_NAME="petguardian"

# Variáveis sensíveis obrigatórias
: "${DB_PASSWORD:?Exporte a variável DB_PASSWORD antes de executar}"
: "${DB_USER:?Exporte a variável DB_USER antes de executar}"

# -----------------------------------------------------------------------------
# 2. Criação do Resource Group
# -----------------------------------------------------------------------------
echo "[1/8] Criando Resource Group: $RESOURCE_GROUP em $LOCATION..."
az group create \
  --name "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --output table

# -----------------------------------------------------------------------------
# 3. Azure Container Registry (ACR)
# -----------------------------------------------------------------------------
echo "[2/8] Criando Azure Container Registry: $ACR_NAME..."
az acr create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$ACR_NAME" \
  --sku Basic \
  --admin-enabled true \
  --output table

az acr login --name "$ACR_NAME"

ACR_LOGIN_SERVER=$(az acr show --name "$ACR_NAME" --query "loginServer" -o tsv)
ACR_USERNAME=$(az acr credential show --name "$ACR_NAME" --query "username" -o tsv)
ACR_PASSWORD=$(az acr credential show --name "$ACR_NAME" --query "passwords[0].value" -o tsv)

echo "ACR Login Server: $ACR_LOGIN_SERVER"

# -----------------------------------------------------------------------------
# 4. Persistência de Dados — Azure Storage Account + File Share
# -----------------------------------------------------------------------------
echo "[3/8] Criando Storage Account para persistência do banco..."
az storage account create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$STORAGE_ACCOUNT_NAME" \
  --location "$LOCATION" \
  --sku Standard_LRS \
  --output table

STORAGE_KEY=$(az storage account keys list \
  --resource-group "$RESOURCE_GROUP" \
  --account-name "$STORAGE_ACCOUNT_NAME" \
  --query "[0].value" -o tsv)

echo "[4/8] Criando File Share para volume de dados do PostgreSQL..."
az storage share create \
  --name "$SHARE_NAME" \
  --account-name "$STORAGE_ACCOUNT_NAME" \
  --account-key "$STORAGE_KEY" \
  --output table

# -----------------------------------------------------------------------------
# 5. Envio da Imagem do PostgreSQL para o ACR
# -----------------------------------------------------------------------------
echo "[5/8] Fazendo pull e push da imagem PostgreSQL 16 para o ACR..."
docker pull postgres:16-alpine
docker tag postgres:16-alpine "${ACR_LOGIN_SERVER}/postgres-db-petguardian:v1"
docker push "${ACR_LOGIN_SERVER}/postgres-db-petguardian:v1"

# -----------------------------------------------------------------------------
# 6. Deploy do Container do Banco PostgreSQL no ACI
# -----------------------------------------------------------------------------
echo "[6/8] Subindo container do banco PostgreSQL no ACI com volume persistente..."
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
  --dns-name-label postgres-petguardian \
  --ports 5432 \
  --environment-variables \
    POSTGRES_DB="$DB_NAME" \
    POSTGRES_USER="$DB_USER" \
    POSTGRES_PASSWORD="$DB_PASSWORD" \
    PGDATA="/var/lib/postgresql/data/pgdata" \
  --azure-file-volume-account-name "$STORAGE_ACCOUNT_NAME" \
  --azure-file-volume-account-key "$STORAGE_KEY" \
  --azure-file-volume-share-name "$SHARE_NAME" \
  --azure-file-volume-mount-path "/var/lib/postgresql/data" \
  --output table

DB_HOST="postgres-petguardian.${LOCATION}.azurecontainer.io"
DB_IP=$(az container show --resource-group "$RESOURCE_GROUP" --name aci-db-petguardian --query "ipAddress.ip" -o tsv || echo "")
DB_TARGET="${DB_IP:-$DB_HOST}"

echo ""
echo "Aguardando PostgreSQL inicializar e abrir a porta 5432 (alvo: $DB_TARGET)..."
for i in {1..24}; do
  sleep 5
  if timeout 3 bash -c "cat < /dev/null > /dev/tcp/${DB_TARGET}/5432" 2>/dev/null; then
    echo "✅ PostgreSQL está pronto e aceitando conexões na porta 5432!"
    break
  fi
  echo "  [$i/24] PostgreSQL ainda inicializando... ($((i * 5))s decorridos)"
done

# -----------------------------------------------------------------------------
# 7. Build e Push da Imagem da API Java para o ACR
# -----------------------------------------------------------------------------
echo ""
echo "[7/8] Buildando e enviando imagem da API Java ($JAVA_DIR) para o ACR..."
docker build -t "${ACR_LOGIN_SERVER}/api-petguardian:v1" "$JAVA_DIR"
docker push "${ACR_LOGIN_SERVER}/api-petguardian:v1"

# -----------------------------------------------------------------------------
# 8. Deploy do Container da API Java no ACI
# -----------------------------------------------------------------------------
echo "[8/8] Subindo container da API Java Spring Boot no ACI..."
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
  --dns-name-label api-petguardian \
  --ports 8091 \
  --environment-variables \
    PGHOST="$DB_TARGET" \
    PGPORT="5432" \
    PGDATABASE="$DB_NAME" \
    PGUSER="$DB_USER" \
    PGPASSWORD="$DB_PASSWORD" \
    SPRING_DATASOURCE_URL="jdbc:postgresql://${DB_TARGET}:5432/${DB_NAME}" \
    SPRING_DATASOURCE_USERNAME="$DB_USER" \
    SPRING_DATASOURCE_PASSWORD="$DB_PASSWORD" \
    SPRING_DOCKER_COMPOSE_ENABLED="false" \
  --output table

API_FQDN="api-petguardian.${LOCATION}.azurecontainer.io"
API_IP=$(az container show --resource-group "$RESOURCE_GROUP" --name aci-api-petguardian --query "ipAddress.ip" -o tsv || echo "")

# -----------------------------------------------------------------------------
# 9. Verificação de Saúde e Inicialização da API Java
# -----------------------------------------------------------------------------
echo ""
echo "=== Aguardando ACI baixar a imagem e iniciar a JVM (40 a 75 segundos) ==="
HEALTH_TARGET="${API_IP:-$API_FQDN}"

for i in {1..30}; do
  sleep 5
  HTTP_CODE=$(curl -s -o /dev/null -w "%{http_code}" "http://${HEALTH_TARGET}:8091/actuator/health" || echo "000")
  if [ "$HTTP_CODE" = "200" ]; then
    echo ""
    echo "✅ Spring Boot inicializado com sucesso (Health Check HTTP 200 OK)!"
    break
  fi

  LOGS=$(az container logs --resource-group "$RESOURCE_GROUP" --name aci-api-petguardian 2>/dev/null || true)
  if echo "$LOGS" | grep -q "Started PetGuardianApplication"; then
    echo ""
    echo "✅ Log do container confirmou: 'Started PetGuardianApplication'!"
    break
  fi

  echo "  [$i/30] Aguardando API inicializar... ($((i * 5))s decorridos)"
done

echo ""
echo "=== Últimos logs do container da API Java ==="
az container logs --resource-group "$RESOURCE_GROUP" --name aci-api-petguardian --tail 25 || true

echo ""
echo "=== Status Final dos Containers Provisionados ==="
az container list --resource-group "$RESOURCE_GROUP" --output table

echo ""
echo "================================================================="
echo "✅ DEPLOY FINALIZADO COM SUCESSO!"
echo "================================================================="
echo "📖 Swagger UI (FQDN): http://${API_FQDN}:8091/swagger-ui/index.html"
if [ -n "$API_IP" ]; then
  echo "📖 Swagger UI (IP):   http://${API_IP}:8091/swagger-ui/index.html"
fi
echo "🩺 Health Check:      http://${API_FQDN}:8091/actuator/health"
echo "🗄️ PostgreSQL (FQDN): ${DB_HOST}:5432"
if [ -n "$DB_IP" ]; then
  echo "🗄️ PostgreSQL (IP):   ${DB_IP}:5432"
fi
echo "================================================================="