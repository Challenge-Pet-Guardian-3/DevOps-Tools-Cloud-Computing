# java
git clone https://github.com/Challenge-Pet-Guardian-3/Java-Advanced.git

# VARIAVEIS (caso vc precise mudar alguma coisa)

#  mudar location do resource group
$LOCATION = "southafricanorth"

# Dados do SQL Server
$SQL_SERVER = "" # só o nome (sem .database.windows.net)
$DB_NAME = "petguardian_db"
$DB_USER = "petguardian"
$DB_PASSWORD = "petguardian"

# credenciais do ACR

$ACR_USERNAME = $(az acr credential show --name acrpetguardian --query "username" -o tsv)
$ACR_PASSWORD = $(az acr credential show --name acrpetguardian --query "passwords[0].value" -o tsv)

# 1. resource group e os revedores

# criação do rg
az group create --name rg-java-petguardian --location $LOCATION

# registro dos provedores
az provider register --namespace Microsoft.Sql
az provider register --namespace Microsoft.ContainerRegistry
az provider register --namespace Microsoft.ContainerInstance

# 2. acesso do firewall
az sql server firewall-rule create `
  --resource-group $RESOURCE_GROUP `
  --server $SQL_SERVER `
  --name AllowAzureServices `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 255.255.255.255

# 3. criação do ACR (Azure Container Registry '-') e upload da imagem do docker
az acr create `
  --resource-group $RESOURCE_GROUP `
  --name acrpetguardian `
  --sku F1 `
  --admin-enabled true

az acr login --name acrpetguardian

# build e push da imagem da API Java
docker build -t acrpetguardian.azurecr.io/api-petguardian:v1 .
docker push acrpetguardian.azurecr.io/api-petguardian:v1

# 4. subindo a API no ACI (Azure Container Instance) apontada pro banco
$JDBC_URL = "jdbc:sqlserver://${SQL_SERVER}.database.windows.net:1433;database=${DB_NAME};encrypt=true;trustServerCertificate=false;"

az container create `
  --resource-group $RESOURCE_GROUP `
  --name aci-api-petguardian `
  --image acrpetguardian.azurecr.io/api-petguardian:v1 `
  --cpu 1 `
  --memory 1.5 `
  --registry-login-server acrpetguardian.azurecr.io `
  --registry-username $ACR_USERNAME `
  --registry-password $ACR_PASSWORD `
  --dns-name-label api-petguardian `
  --ports 8080 `
  --environment-variables `
    SPRING_DATASOURCE_URL=$JDBC_URL `
    SPRING_DATASOURCE_USERNAME=$DB_USER `
    SPRING_DATASOURCE_PASSWORD=$DB_PASSWORD