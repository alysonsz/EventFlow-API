# EventFlow API — Enterprise Event Management
![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![Redis](https://img.shields.io/badge/redis-%23DD0031.svg?style=for-the-badge&logo=redis&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)
![Serilog](https://img.shields.io/badge/Serilog-1cb495?style=for-the-badge&logo=serilog&logoColor=white)
![OpenTelemetry](https://img.shields.io/badge/OpenTelemetry-000000?style=for-the-badge&logo=opentelemetry&logoColor=white)
![MicrosoftSQLServer](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)

O **EventFlow API** é uma solução de back-end **robusta, escalável e orientada a produção** para gestão de eventos, desenvolvida em **.NET 8** e estruturada segundo os princípios da **Clean Architecture**.

Diferente de APIs tradicionais voltadas apenas a CRUD, este projeto foca fortemente em **Observabilidade, Performance e Resiliência**, simulando um ambiente real de produção com **tracing distribuído**, **logs estruturados** e **estratégias de cache**.

---

## Arquitetura & Design

O projeto foi refatorado para suportar **alta carga**, **baixo acoplamento** e **manutenibilidade a longo prazo**.

```mermaid
graph TD
    Client[Cliente / Swagger] -->|HTTP Request| API[EventFlow API]
    
    subgraph "Observability Layer"
        API -.->|Logs| Seq[Seq Dashboard]
        API -.->|Traces| Jaeger[Jaeger UI]
    end
    
    subgraph "Data & Performance"
        API <-->|Cache-Aside| Redis[Redis Cache]
        API <-->|EF Core| SQL[SQL Server]
    end
```

---

## 🌟 Diferenciais Técnicos

### ⚡ Performance & Caching
- **Redis (Cache-Aside Pattern)**: Implementação estratégica de cache distribuído para reduzir a carga no banco de dados.
- **Latência Otimizada**: Redução significativa no tempo de resposta em operações de leitura (ex: `GetById`).
- **Consistência**: Estratégias de invalidação de cache (TTL e remoção ativa) para garantir a integridade dos dados.

### 🔍 Observabilidade & Monitoramento
A aplicação foi projetada para não ser uma "caixa preta" em produção:
- **Tracing Distribuído (OpenTelemetry + Jaeger)**: Rastreamento ponta-a-ponta (End-to-End) das requisições, permitindo identificar gargalos exatos entre API, Cache e SQL Server.
- **Logs Estruturados (Serilog + Seq)**: Centralização de logs com enriquecimento de dados, facilitando diagnósticos rápidos em ambiente containerizado.

### 🛡️ Resiliência & Robustez
- **Políticas de Retry**: Resiliência a falhas transientes na conexão com o banco de dados e serviços externos.
- **Health Checks**: Monitoramento da saúde dos contêineres e dependências.

### 🐳 Infraestrutura como Código (IaC)
Ambiente de desenvolvimento totalmente orquestrado via **Docker Compose**, subindo instantaneamente:
- `API (.NET 8)`
- `SQL Server`
- `Redis`
- `Jaeger UI` (Tracing)
- `Seq` (Logging)

### 🧼 Code Quality
- **Modern C#**: Uso de *Primary Constructors* e *Records*.
- **Clean Architecture**: Separação estrita de responsabilidades.
- **DI Otimizada**: Uso de *Extension Methods* para manter a configuração de injeção de dependência limpa e organizada.

---

## Tech Stack

| Categoria | Tecnologias |
|---------|------------|
| **Core** | .NET 8, C# 12 |
| **Arquitetura** | Clean Architecture, RESTful, Dependency Injection |
| **Banco de Dados** | SQL Server 2022, Entity Framework Core 8 |
| **Performance** | Redis (StackExchange.Redis), Microsoft.Extensions.Caching |
| **Observabilidade** | OpenTelemetry, Jaeger, Serilog, Seq |
| **Documentação** | Swagger / OpenAPI (com suporte a Auth) |
| **Qualidade** | xUnit, Moq, FluentAssertions, FluentValidation |
| **DevOps** | Docker, Docker Compose |

---

## Como Rodar o Projeto

A forma **mais simples e profissional** de executar o EventFlow API é utilizando **Docker**, que sobe toda a infraestrutura necessária automaticamente.

### Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado.

### Passo a Passo

```bash
git clone https://github.com/alysonsz/eventflow-api.git
cd eventflow-api
```

```bash
docker-compose up -d --build
```

Aguarde alguns segundos até que todos os containers estejam prontos.

---

## Acesso aos Serviços

| Serviço | URL | Descrição | Credenciais (se houver) |
|-------|-----|-----------|-------------|
| **Swagger** | http://localhost:8079/swagger | Documentação e testes da API | - |
| **Jaeger UI** | http://localhost:16686 | Tracing e análise de performance | - |
| **Seq Logs** | http://localhost:5341 | Logs estruturados em tempo real | User: `admin` <br> Pass: `MyStrongPassword123!` |

---

## 🧪 Testando a Performance (Cache)

1. Acesse o **Swagger**
2. Execute `GET /event/{id}` (primeira chamada → SQL Server)
3. Execute a mesma requisição novamente

**Resultado:**
- A segunda resposta ocorre em **milissegundos**, pois vem do **Redis**

Vá até o **Jaeger UI** e compare os *spans* das duas requisições.

---

## 📂 Estrutura do Projeto

```
EventFlow API
├── 📁 EventFlow.Core           # Domínio (Entidades, Interfaces, DTOs)
├── 📁 EventFlow.Application    # Regras de Negócio (Services, Validations, Cache Logic)
├── 📁 EventFlow.Infrastructure # Acesso a Dados (EF Core, Repositories, Migrations)
├── 📁 EventFlow.Presentation   # API (Controllers, Docker, DI Setup)
└── 📁 EventFlow.Tests          # Testes Unitários (xUnit)
```

---

## 🔐 Autenticação

A API utiliza **JWT (JSON Web Token)**.

1. Crie uma conta em: `POST /authentication/register`
2. Faça login em: `POST /authentication/login`
3. Copie o token retornado
4. No Swagger, clique em **Authorize** e informe:

```
SEU_TOKEN
```

---

### 👨‍💻 Autor

Desenvolvido por **Alyson Souza Carregosa**  
Focado em **Engenharia de Software de Alta Performance**, Arquitetura e Sistemas Observáveis.

---

## 📄 Licença

Este projeto está licenciado sob a **MIT License**.

