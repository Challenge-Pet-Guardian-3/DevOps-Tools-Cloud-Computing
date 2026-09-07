# 1. variaveis de ambiente
$RESOURCE_GROUP = "rg-petguardian"
$LOCATION = "brazilsouth"
$ACR_NAME = "acrpetguardian"
$STORAGE_ACCOUNT_NAME = "stpetguardiandata"
$SHARE_NAME = "pgdata"

# credenciais do Banco
$DB_PASSWORD = "Fiap@2tdspg"
$DB_USER = "petguardian"
$DB_NAME = "petguardian_db"

# 2. Azure Container Registry (ACR)
az acr create `
  --resource-group $RESOURCE_GROUP `
  --name $ACR_NAME `
  --sku Basic `
  --admin-enabled true

az acr login --name $ACR_NAME

$ACR_USERNAME = $(az acr credential show --name $ACR_NAME --query "username" -o tsv)
$ACR_PASSWORD = $(az acr credential show --name $ACR_NAME --query "passwords[0].value" -o tsv)

# 3. persistência de dados (e storage account e tudo mais lá)

# cria a conta de armazenamento para os arquivos do banco
az storage account create `
  --resource-group $RESOURCE_GROUP `
  --name $STORAGE_ACCOUNT_NAME `
  --location $LOCATION `
  --sku Standard_LRS

# pega a chave da conta
$STORAGE_KEY = $(az storage account keys list --resource-group $RESOURCE_GROUP --account-name $STORAGE_ACCOUNT_NAME --query "[0].value" -o tsv)

# cria o File Share para o volume de dados
az storage share create `
  --name $SHARE_NAME `
  --account-name $STORAGE_ACCOUNT_NAME `
  --account-key $STORAGE_KEY

# 4. enviando imagem do PostgreSQL para o ACR (Azure Container Registry)

# pull da imagem oficial do PostgreSQL
docker pull postgres:16

# tag da imagem para o registro privado
docker tag postgres:16 "${ACR_NAME}.azurecr.io/postgres-db-petguardian:v1"

# push da imagem do banco para o ACR
docker push "${ACR_NAME}.azurecr.io/postgres-db-petguardian:v1"

# 5. subindo o container do PostgreSQL no ACI (Azure Container Instance)
az container create `
  --resource-group $RESOURCE_GROUP `
  --name aci-db-petguardian `
  --image "${ACR_NAME}.azurecr.io/postgres-db-petguardian:v1" `
  --cpu 1 `
  --memory 1.5 `
  --registry-login-server "${ACR_NAME}.azurecr.io" `
  --registry-username $ACR_USERNAME `
  --registry-password $ACR_PASSWORD `
  --dns-name-label postgres-petguardian `
  --ports 5432 `
  --environment-variables `
    POSTGRES_DB=$DB_NAME `
    POSTGRES_USER=$DB_USER `
    POSTGRES_PASSWORD=$DB_PASSWORD `
  --azure-file-volume-account-name $STORAGE_ACCOUNT_NAME `
  --azure-file-volume-account-key $STORAGE_KEY `
  --azure-file-volume-share-name $SHARE_NAME `
  --azure-file-volume-mount-path "/var/lib/postgresql/data"

# 6. build e push da imagem da API de Java para o ACR (Azure Container Registry)
docker build -t "${ACR_NAME}.azurecr.io/api-petguardian:v1" .
docker push "${ACR_NAME}.azurecr.io/api-petguardian:v1"

# 7. subindo o container da API apontada pro banco.

# FQDN do banco criado acima no ACI
$DB_HOST = "postgres-petguardian.${LOCATION}.azurecontainer.io"

az container create `
  --resource-group $RESOURCE_GROUP `
  --name aci-api-petguardian `
  --image "${ACR_NAME}.azurecr.io/api-petguardian:v1" `
  --cpu 0.5 `
  --memory 1 `
  --registry-login-server "${ACR_NAME}.azurecr.io" `
  --registry-username $ACR_USERNAME `
  --registry-password $ACR_PASSWORD `
  --dns-name-label api-petguardian `
  --ports 8080 `
  --environment-variables `
    PGHOST=$DB_HOST `
    PGPORT="5432" `
    PGDATABASE=$DB_NAME `
    PGUSER=$DB_USER `
    PGPASSWORD=$DB_PASSWORD

# 8. verificação dos containers criados (se quiser '-')
az container list --resource-group $RESOURCE_GROUP --output table