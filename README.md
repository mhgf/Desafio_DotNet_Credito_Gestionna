# 📘 Desafio DotNet Crédito – Gestionna

Este projeto é uma **API RESTful em .NET C#** desenvolvida como parte de um desafio técnico para avaliação de
conhecimentos em arquitetura, boas práticas e desenvolvimento backend.

---

## 🚀 Tecnologias Utilizadas

- **.NET SDK**
- **ASP.NET Core Web API**
- **C#**
- **Docker / Docker Compose**
- **Arquitetura em Camadas**

---

## 🧠 Estrutura do Projeto

```
/
├── Api/
├── Core/
├── Infra/
├── Shared/
└── README.md
```

### 📂 Camadas

- **Api** – Camada de apresentação (controllers, endpoints, configuração da API).
- **Core** – Domínio da aplicação (entidades, regras de negócio, interfaces).
- **Infra** – Persistência, acesso a dados e integrações externas.
- **Shared** – Componentes e utilitários compartilhados.
- **init-scripts** – Scripts auxiliares para inicialização de ambiente/banco.
- **compose.yaml** – Orquestração de containers Docker.

---

## 🛠️ Pré-requisitos

Antes de rodar o projeto, certifique-se de ter:

- ✔️ .NET SDK instalado
- ✔️ Docker e Docker Compose (opcional, mas recomendado)
- ✔️ Git

---

## 🏁 Como Executar o Projeto

### 🔹 1. Clone o repositório

```bash
git clone https://github.com/mhgf/Desafio_DotNet_Credito_Gestionna.git
cd Desafio_DotNet_Credito_Gestionna
```

---

### 🔹 2. Restaurar dependências

```bash
dotnet restore
```

---

### 🔹 3. Build da solução

```bash
dotnet build
```

### 🔹 4. Configurar Variaveis de Ambiente

```json
  "AzureServiceBus": {
    "ConnectionString": "",
    "QueueName": "Credito-Queue"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost; Port=5432; Username=local; Password=local; Database=gestionnna"
  }
```

---

### 🔹 5. Executar a API

```bash
dotnet run --project Api
```

A aplicação será iniciada e ficará disponível conforme a configuração padrão do ASP.NET Core.

---

## 🐳 Executando com Docker

Caso queira subir os serviços via Docker:

### 🔹 1. Criar o Env na raiz do projeto.

```dotenv
ServiceBus_Connection=""
ServiceBus_Queue=Credito-Queue
```

### 🔹 2. Rodar do Dokcer Compose.

```bash
docker compose up -d
```

---

## 📌 Documentação da API

Se habilitado, o Swagger pode ser acessado em:

```
https://localhost:{porta}/swagger
```

Ele permite visualizar e testar todos os endpoints disponíveis.

---

## 🧪 Testes

Caso existam projetos de teste:

```bash
dotnet test
```

---

## 🧠 Boas Práticas Aplicadas

- ✔️ Separação clara de responsabilidades
- ✔️ Arquitetura em camadas
- ✔️ Código organizado e extensível
- ✔️ Configuração preparada para ambientes diferentes
- ✔️ Pronto para CI/CD

---

## 📜 Licença

Projeto desenvolvido para fins de **desafio técnico**.

---

## 👤 Autor

**Matheus Henrique**  
GitHub: https://github.com/mhgf