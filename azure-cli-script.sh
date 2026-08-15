#!/usr/bin/env bash
set -e

# Variáveis do Ambiente
RG="rg-challenge-petguardian"
LOCATION="canadacentral"
VNET="vnet_petguardian"
SUBNET="sub_net"
NSG="nsg_petguardian"
VM="vm-petguardian"
ADMIN="petguardian-adm"

# 1. Provisionar Grupo de Recursos
echo "[1/6] Criando Grupo de Recursos..."
az group create --name "$RG" --location "$LOCATION"

# 2. Provisionar VNet e Subnet
echo "[2/6] Criando VNet e Subnet..."
az network vnet create \
  --resource-group "$RG" \
  --location "$LOCATION" \
  --name "$VNET" \
  --address-prefixes 10.10.0.0/16 \
  --subnet-name "$SUBNET" \
  --subnet-prefixes 10.10.1.0/24

# 3. Provisionar Network Security Group (NSG)
echo "[3/6] Criando Network Security Group..."
az network nsg create --resource-group "$RG" --location "$LOCATION" --name "$NSG"

# 4. Provisionar VM Ubuntu 22.04 LTS
echo "[4/6] Criando Maquina Virtual Linux..."
az vm create \
  --resource-group "$RG" \
  --name "$VM" \
  --image Ubuntu2204 \
  --size Standard_B2ls_v2 \
  --admin-username "$ADMIN" \
  --generate-ssh-keys \
  --output json \
  --verbose \
  --vnet-name "$VNET" \
  --subnet "$SUBNET" \
  --nsg "$NSG"

# 5. Liberar Portas Necessárias (SSH, API .NET, Oracle)
echo "[5/6] Liberando Portas Necessárias..."
az vm open-port --resource-group "$RG" --name "$VM" --port 22 --priority 1000
az vm open-port --resource-group "$RG" --name "$VM" --port 8080 --priority 1010
az vm open-port --resource-group "$RG" --name "$VM" --port 1521 --priority 1020

# 6. Instalação automatizada do Docker e Dependências
echo "[6/6] Instalando Docker e Dependências..."
az vm run-command invoke \
  --resource-group "$RG" \
  --name "$VM" \
  --command-id RunShellScript \
  --scripts "
    # Atualizar pacotes
    sudo apt-get update -y

    # Instalar dependencias
    sudo apt-get install -y \
      ca-certificates \
      curl \
      gnupg \
      git \
      nano \
      unzip \
      wget

    # Adicionar repositorio Docker
    sudo install -m 0755 -d /etc/apt/keyrings
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
    sudo chmod a+r /etc/apt/keyrings/docker.gpg
    echo \
      \"deb [arch=\$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
      \$(. /etc/os-release && echo \"\$VERSION_CODENAME\") stable\" | \
      sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

    # Instalar Docker
    sudo apt-get update -y
    sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

    # Iniciar Docker e habilitar no boot
    sudo systemctl start docker
    sudo systemctl enable docker

    # Adicionar usuario ao grupo docker (sem precisar de sudo)
    sudo usermod -aG docker $ADMIN

    # Verificar instalacao
    docker --version
    docker compose version

    echo 'Instalacao concluida!'
  "

# Exibe o IP Público gerado para conexões
PUBLIC_IP=$(az vm show --resource-group "$RG" --name "$VM" --show-details --query publicIps --output tsv)

echo "===================================================="
echo " Provisionamento concluido com sucesso!"
echo " IP Publico da VM: $PUBLIC_IP"
echo " Acesse via SSH: ssh $ADMIN@$PUBLIC_IP"
echo " Aplicacao (apos deploy): http://$PUBLIC_IP:8080"
echo " Swagger: http://$PUBLIC_IP:8080/index.html"
echo "===================================================="
