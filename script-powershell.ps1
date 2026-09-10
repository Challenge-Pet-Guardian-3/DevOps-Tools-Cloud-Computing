# =============================================================================
# PetGuardian -- Deploy Completo no Azure (ACR + ACI)
# Disciplina: DevOps Tools & Cloud Computing -- Sprint 3
# Aplicacao: Java Spring Boot 4.1.1 + PostgreSQL 16
# Adaptado para PowerShell / Windows (.ps1)
# =============================================================================

# -----------------------------------------------------------------------------
# 1. Variaveis de Configuracao
# -----------------------------------------------------------------------------
$RESOURCE_GROUP       = "rg-petguardian"
$LOCATION             = if ($env:LOCATION) { $env:LOCATION } else { "canadacentral" }
$ACR_NAME             = "acrpetguardian"
$STORAGE_ACCOUNT_NAME = "stpetguardiandata"
$SHARE_NAME           = "pgdata"
$DB_NAME              = "petguardian"

# Variaveis sensiveis
$DB_PASSWORD = if ($env:DB_PASSWORD) { $env:DB_PASSWORD } else { "petguardian_senha" }
$DB_USER     = if ($env:DB_USER) { $env:DB_USER } else { "petguardian" }

if ([string]::IsNullOrWhiteSpace($DB_PASSWORD)) {
    Write-Host "[ERRO] DB_PASSWORD nao foi definida." -ForegroundColor Red
    exit 1
}

if ([string]::IsNullOrWhiteSpace($DB_USER)) {
    Write-Host "[ERRO] DB_USER nao foi definido." -ForegroundColor Red
    exit 1
}

# Localizacao do codigo Java
$JAVA_DIR = if (Test-Path "./Java-Advanced/Dockerfile") {
    "./Java-Advanced"
} elseif (Test-Path "../Java-Advanced/Dockerfile") {
    "../Java-Advanced"
} else {
    "./Java-Advanced"
}

# -----------------------------------------------------------------------------
# 2. Criacao do Resource Group
# -----------------------------------------------------------------------------
Write-Host "`n[1/8] Criando Resource Group: $RESOURCE_GROUP em $LOCATION..." -ForegroundColor Cyan
az group create --name $RESOURCE_GROUP --location $LOCATION --output table

# -----------------------------------------------------------------------------
# 3. Azure Container Registry (ACR)
# -----------------------------------------------------------------------------
Write-Host "`n[2/8] Criando Azure Container Registry: $ACR_NAME..." -ForegroundColor Cyan
az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Basic --admin-enabled true --output table

az acr login --name $ACR_NAME

$ACR_LOGIN_SERVER = az acr show --name $ACR_NAME --query "loginServer" -o tsv
$ACR_USERNAME     = az acr credential show --name $ACR_NAME --query "username" -o tsv
$ACR_PASSWORD     = az acr credential show --name $ACR_NAME --query "passwords[0].value" -o tsv

Write-Host "[INFO] ACR Login Server: $ACR_LOGIN_SERVER" -ForegroundColor Green

# -----------------------------------------------------------------------------
# 4. Persistencia de Dados -- Azure Storage Account + File Share
# -----------------------------------------------------------------------------
Write-Host "`n[3/8] Criando Storage Account para persistencia do banco..." -ForegroundColor Cyan
az storage account create --resource-group $RESOURCE_GROUP --name $STORAGE_ACCOUNT_NAME --location $LOCATION --sku Standard_LRS --output table

$STORAGE_KEY = az storage account keys list --resource-group $RESOURCE_GROUP --account-name $STORAGE_ACCOUNT_NAME --query "[0].value" -o tsv

Write-Host "`n[4/8] Criando File Share para volume de dados do PostgreSQL..." -ForegroundColor Cyan
az storage share create --name $SHARE_NAME --account-name $STORAGE_ACCOUNT_NAME --account-key $STORAGE_KEY --output table

# -----------------------------------------------------------------------------
# 5. Envio da Imagem do PostgreSQL para o ACR
# -----------------------------------------------------------------------------
Write-Host "`n[5/8] Fazendo pull e push da imagem PostgreSQL 16 para o ACR..." -ForegroundColor Cyan
docker pull postgres:16-alpine
docker tag postgres:16-alpine ($ACR_LOGIN_SERVER + "/postgres-db-petguardian:v1")
docker push ($ACR_LOGIN_SERVER + "/postgres-db-petguardian:v1")

# -----------------------------------------------------------------------------
# 6. Deploy do Container do Banco PostgreSQL no ACI
# -----------------------------------------------------------------------------
Write-Host "`n[6/8] Subindo container do banco PostgreSQL no ACI com volume persistente..." -ForegroundColor Cyan

az container create `
    --resource-group $RESOURCE_GROUP `
    --name "aci-db-petguardian" `
    --image ($ACR_LOGIN_SERVER + "/postgres-db-petguardian:v1") `
    --os-type Linux `
    --cpu 1 `
    --memory 1.5 `
    --restart-policy Always `
    --registry-login-server $ACR_LOGIN_SERVER `
    --registry-username $ACR_USERNAME `
    --registry-password $ACR_PASSWORD `
    --dns-name-label "postgres-petguardian" `
    --ports 5432 `
    --environment-variables `
        POSTGRES_DB="$DB_NAME" `
        POSTGRES_USER="$DB_USER" `
        POSTGRES_PASSWORD="$DB_PASSWORD" `
        PGDATA="/var/lib/postgresql/data/pgdata" `
    --azure-file-volume-account-name $STORAGE_ACCOUNT_NAME `
    --azure-file-volume-account-key $STORAGE_KEY `
    --azure-file-volume-share-name $SHARE_NAME `
    --azure-file-volume-mount-path "/var/lib/postgresql/data" `
    --output table

$DB_HOST   = "postgres-petguardian." + $LOCATION + ".azurecontainer.io"
$DB_IP     = az container show --resource-group $RESOURCE_GROUP --name "aci-db-petguardian" --query "ipAddress.ip" -o tsv
$DB_TARGET = if (-not [string]::IsNullOrWhiteSpace($DB_IP)) { $DB_IP } else { $DB_HOST }

Write-Host "`nAguardando PostgreSQL inicializar na porta 5432 (Alvo: $DB_TARGET)..." -ForegroundColor Yellow
$dbReady = $false
for ($i = 1; $i -le 24; $i++) {
    Start-Sleep -Seconds 5
    $test = Test-NetConnection -ComputerName $DB_TARGET -Port 5432 -WarningAction SilentlyContinue
    if ($test.TcpTestSucceeded) {
        Write-Host "[OK] PostgreSQL esta pronto e aceitando conexoes na porta 5432!" -ForegroundColor Green
        $dbReady = $true
        break
    }
    Write-Host ("  [" + $i + "/24] PostgreSQL ainda inicializando... (" + ($i * 5) + "s decorridos)")
}

if (-not $dbReady) {
    Write-Host "[AVISO] PostgreSQL demorou mais de 120s para responder na porta 5432, continuando deploy..." -ForegroundColor Yellow
}

# -----------------------------------------------------------------------------
# 7. Build e Push da Imagem da API Java para o ACR
# -----------------------------------------------------------------------------
Write-Host "`n[7/8] Buildando e enviando imagem da API Java ($JAVA_DIR) para o ACR..." -ForegroundColor Cyan
docker build -t ($ACR_LOGIN_SERVER + "/api-petguardian:v1") $JAVA_DIR
docker push ($ACR_LOGIN_SERVER + "/api-petguardian:v1")

# -----------------------------------------------------------------------------
# 8. Deploy do Container da API Java no ACI
# -----------------------------------------------------------------------------
Write-Host "`n[8/8] Subindo container da API Java Spring Boot no ACI..." -ForegroundColor Cyan

az container create `
    --resource-group $RESOURCE_GROUP `
    --name "aci-api-petguardian" `
    --image ($ACR_LOGIN_SERVER + "/api-petguardian:v1") `
    --os-type Linux `
    --cpu 1 `
    --memory 1.5 `
    --restart-policy Always `
    --registry-login-server $ACR_LOGIN_SERVER `
    --registry-username $ACR_USERNAME `
    --registry-password $ACR_PASSWORD `
    --dns-name-label "api-petguardian" `
    --ports 8091 `
    --environment-variables `
        PGHOST="$DB_TARGET" `
        PGPORT="5432" `
        PGDATABASE="$DB_NAME" `
        PGUSER="$DB_USER" `
        PGPASSWORD="$DB_PASSWORD" `
        SPRING_DATASOURCE_URL=("jdbc:postgresql://" + $DB_TARGET + ":5432/" + $DB_NAME) `
        SPRING_DATASOURCE_USERNAME="$DB_USER" `
        SPRING_DATASOURCE_PASSWORD="$DB_PASSWORD" `
        SPRING_DOCKER_COMPOSE_ENABLED="false" `
    --output table

$API_FQDN = "api-petguardian." + $LOCATION + ".azurecontainer.io"
$API_IP   = az container show --resource-group $RESOURCE_GROUP --name "aci-api-petguardian" --query "ipAddress.ip" -o tsv

# -----------------------------------------------------------------------------
# 9. Verificacao de Saude e Inicializacao da API Java
# -----------------------------------------------------------------------------
Write-Host "`n=== Aguardando ACI baixar a imagem e iniciar a JVM (40 a 75 segundos) ===" -ForegroundColor Yellow
$apiReady = $false

for ($i = 1; $i -le 30; $i++) {
    Start-Sleep -Seconds 5

    # 1. Testa endpoint de saude
    $healthHost = if (-not [string]::IsNullOrWhiteSpace($API_IP)) { $API_IP } else { $API_FQDN }
    try {
        $response = Invoke-WebRequest -Uri ("http://" + $healthHost + ":8091/actuator/health") -UseBasicParsing -TimeoutSec 3 -ErrorAction SilentlyContinue
        if ($response -and $response.StatusCode -eq 200) {
            Write-Host "`n[OK] Spring Boot inicializado e respondendo com HTTP 200 OK!" -ForegroundColor Green
            $apiReady = $true
            break
        }
    } catch {}

    # 2. Verifica se o log do Spring Boot registrou a subida
    $logs = az container logs --resource-group $RESOURCE_GROUP --name "aci-api-petguardian" 2>$null
    if ($logs -and $logs -ne "None" -and $logs -match "Started PetGuardianApplication") {
        Write-Host "`n[OK] Log do container confirmou: 'Started PetGuardianApplication'!" -ForegroundColor Green
        $apiReady = $true
        break
    }

    Write-Host ("  [" + $i + "/30] Aguardando API inicializar... (" + ($i * 5) + "s decorridos)")
}

Write-Host "`n=== Ultimos logs do container da API Java ===" -ForegroundColor Cyan
az container logs --resource-group $RESOURCE_GROUP --name "aci-api-petguardian" --tail 25

Write-Host "`n=== Status Final dos Containers Provisionados ===" -ForegroundColor Cyan
az container list --resource-group $RESOURCE_GROUP --output table

Write-Host "`n=================================================================" -ForegroundColor Green
Write-Host "[SUCESSO] DEPLOY FINALIZADO!" -ForegroundColor Green
Write-Host "=================================================================" -ForegroundColor Green
Write-Host ("[SWAGGER FQDN] http://" + $API_FQDN + ":8091/swagger-ui/index.html") -ForegroundColor Yellow
if (-not [string]::IsNullOrWhiteSpace($API_IP)) {
    Write-Host ("[SWAGGER IP]   http://" + $API_IP + ":8091/swagger-ui/index.html") -ForegroundColor Yellow
}
Write-Host ("[HEALTH CHECK] http://" + $API_FQDN + ":8091/actuator/health") -ForegroundColor Yellow
Write-Host ("[POSTGRES FQDN] " + $DB_HOST + ":5432") -ForegroundColor Yellow
if (-not [string]::IsNullOrWhiteSpace($DB_IP)) {
    Write-Host ("[POSTGRES IP]   " + $DB_IP + ":5432") -ForegroundColor Yellow
}
Write-Host "=================================================================`n" -ForegroundColor Green
