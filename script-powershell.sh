# =============================================================================

# PetGuardian — Deploy Completo no Azure (ACR + ACI)

# Disciplina: DevOps Tools & Cloud Computing — Sprint 3

# Aplicação: Java Spring Boot 4.1.1 + PostgreSQL 16

# Adaptado para PowerShell / Windows

# =============================================================================

# -----------------------------------------------------------------------------

# 1. Variáveis de Configuração

# -----------------------------------------------------------------------------

$RESOURCE_GROUP = "rg-petguardian"
$LOCATION = "canadacentral"
$ACR_NAME = "acrpetguardian"
$STORAGE_ACCOUNT_NAME = "stpetguardiandata"
$SHARE_NAME = "pgdata"
$DB_NAME = "petguardian"

# -----------------------------------------------------------------------------

# Variáveis sensíveis

# -----------------------------------------------------------------------------

$DB_PASSWORD = "petguardian_senha"
$DB_USER = "petguardian"

if ([string]::IsNullOrWhiteSpace($DB_PASSWORD)) {
Write-Host "ERRO: DB_PASSWORD não foi definida."
exit 1
}

if ([string]::IsNullOrWhiteSpace($DB_USER)) {
Write-Host "ERRO: DB_USER não foi definido."
exit 1
}

# -----------------------------------------------------------------------------

# 2. Criação do Resource Group

# -----------------------------------------------------------------------------

Write-Host "[1/8] Criando Resource Group: $RESOURCE_GROUP em $LOCATION..."

az group create --name $RESOURCE_GROUP --location $LOCATION

# -----------------------------------------------------------------------------

# 3. Azure Container Registry (ACR)

# -----------------------------------------------------------------------------

Write-Host "[2/8] Criando Azure Container Registry: $ACR_NAME..."

az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Basic --admin-enabled true

az acr login --name $ACR_NAME

$ACR_LOGIN_SERVER = az acr show --name $ACR_NAME --query "loginServer" -o tsv

$ACR_USERNAME = az acr credential show --name $ACR_NAME --query "username" -o tsv

$ACR_PASSWORD = az acr credential show --name $ACR_NAME --query "passwords[0].value" -o tsv

Write-Host "ACR Login Server: $ACR_LOGIN_SERVER"

# -----------------------------------------------------------------------------

# 4. Persistência de Dados — Azure Storage Account + File Share

# -----------------------------------------------------------------------------

Write-Host "[3/8] Criando Storage Account para persistência do banco..."

az storage account create --resource-group $RESOURCE_GROUP --name $STORAGE_ACCOUNT_NAME --location $LOCATION --sku Standard_LRS

$STORAGE_KEY = az storage account keys list --resource-group $RESOURCE_GROUP --account-name $STORAGE_ACCOUNT_NAME --query "[0].value" -o tsv

Write-Host "[4/8] Criando File Share para volume de dados do PostgreSQL..."

az storage share create --name $SHARE_NAME --account-name $STORAGE_ACCOUNT_NAME --account-key $STORAGE_KEY

# -----------------------------------------------------------------------------

# 5. Envio da Imagem do PostgreSQL para o ACR

# -----------------------------------------------------------------------------

Write-Host "[5/8] Fazendo pull e push da imagem PostgreSQL 16 para o ACR..."

docker pull postgres:16-alpine

docker tag postgres:16-alpine "${ACR_LOGIN_SERVER}/postgres-db-petguardian:v1"

docker push "${ACR_LOGIN_SERVER}/postgres-db-petguardian:v1"

# -----------------------------------------------------------------------------

# 6. Deploy do Container do Banco PostgreSQL no ACI

# -----------------------------------------------------------------------------

Write-Host "[6/8] Subindo container do banco PostgreSQL no ACI com volume persistente..."

az container create --resource-group $RESOURCE_GROUP --name "aci-db-petguardian" --image "${ACR_LOGIN_SERVER}/postgres-db-petguardian:v1" --os-type Linux --cpu 1 --memory 1.5 --registry-login-server $ACR_LOGIN_SERVER --registry-username $ACR_USERNAME --registry-password $ACR_PASSWORD --dns-name-label "postgres-petguardian" --ports 5432 --environment-variables POSTGRES_DB=$DB_NAME POSTGRES_USER=$DB_USER POSTGRES_PASSWORD=$DB_PASSWORD PGDATA="/var/lib/postgresql/data/pgdata" --azure-file-volume-account-name $STORAGE_ACCOUNT_NAME --azure-file-volume-account-key $STORAGE_KEY --azure-file-volume-share-name $SHARE_NAME --azure-file-volume-mount-path "/var/lib/postgresql/data"

$DB_HOST = "postgres-petguardian.${LOCATION}.azurecontainer.io"

Write-Host "Aguardando 25 segundos para o PostgreSQL concluir a inicialização..."

Start-Sleep -Seconds 25

# -----------------------------------------------------------------------------

# 7. Build e Push da Imagem da API Java para o ACR

# -----------------------------------------------------------------------------

Write-Host "[7/8] Buildando e enviando imagem da API Java para o ACR..."

docker build -t "${ACR_LOGIN_SERVER}/api-petguardian:v1" ./Java-Advanced

docker push "${ACR_LOGIN_SERVER}/api-petguardian:v1"

# -----------------------------------------------------------------------------

# 8. Deploy do Container da API Java no ACI

# -----------------------------------------------------------------------------

Write-Host "[8/8] Subindo container da API Java Spring Boot no ACI..."

az container create --resource-group $RESOURCE_GROUP --name "aci-api-petguardian" --image "${ACR_LOGIN_SERVER}/api-petguardian:v1" --os-type Linux --cpu 1 --memory 1.5 --registry-login-server $ACR_LOGIN_SERVER --registry-username $ACR_USERNAME --registry-password $ACR_PASSWORD --dns-name-label "api-petguardian" --ports 8091 --environment-variables PGHOST=$DB_HOST PGPORT="5432" PGDATABASE=$DB_NAME PGUSER=$DB_USER PGPASSWORD=$DB_PASSWORD SPRING_DOCKER_COMPOSE_ENABLED="false"

# -----------------------------------------------------------------------------

# 9. Verificação dos Containers Criados

# -----------------------------------------------------------------------------

$API_FQDN = "api-petguardian.${LOCATION}.azurecontainer.io"

Write-Host ""
Write-Host "=== Deploy concluído! Verificando containers provisionados... ==="

az container list --resource-group $RESOURCE_GROUP --output table

Write-Host ""
Write-Host "Swagger disponível em: http://${API_FQDN}:8091/swagger-ui/index.html"
Write-Host "Banco disponível em: ${DB_HOST}:5432"
