# EventFlow API

### 📌 Descrição do Projeto

O **EventFlow API** é um projeto desenvolvido em .NET, voltado para a gestão e organização de eventos, projetado com a finalidade de demonstrar boas práticas na utilização do framework ASP.NET Core, Entity Framework Core e arquitetura MVC (Model-View-Controller).

OBS: Esse projeto foi pensado para implementação no Google Developers Group (GDG) Aracaju.

Esta API proporciona funcionalidades completas para criação, gerenciamento e manipulação de eventos, palestrantes, organizadores e participantes, incluindo relacionamentos complexos, como relacionamentos um-para-muitos e muitos-para-muitos.

---

### 🚀 Objetivos do Projeto

- Demonstrar conhecimento avançado em ASP.NET Core Web API.
- Aplicar práticas recomendadas para uso de Entity Framework Core com diferentes tipos de relacionamentos.
- Seguir princípios de design e arquitetura MVC.
- Apresentar um código limpo, organizado e escalável.
- Facilitar futuras implementações como autenticação, testes unitários e logging.

---

### 🛠️ Tecnologias Utilizadas

**Backend:**
- .NET 8 (ou versão mais recente disponível)
- ASP.NET Core Web API
- Entity Framework Core (com migrations)
- SQL Server
- Swagger

**Padrões e Conceitos:**
- Arquitetura MVC (Model-View-Controller)
- Fluent API para mapeamento personalizado das entidades

**Futuras Implementações:**
- Autenticação JWT
- Validações adicionais com Fluent Validation
- Testes unitários (xUnit)
- Logging detalhado para auditoria e monitoramento
- Docker para containerização

---

### 🔗 Estrutura das Entidades

O projeto atualmente é composto pelas seguintes entidades principais:

- **Event**: Representa o evento em si, com dados como título, descrição, data e local.
- **Organizer**: Responsável pela organização do evento.
- **Speaker**: Palestrantes convidados para o evento.
- **Participant**: Pessoas que participam dos eventos.

---

### 📚 Detalhes dos Relacionamentos

- **Organizer → Event** *(One-to-Many)*
  - Um organizador pode gerenciar múltiplos eventos.

- **Event → Speaker** *(One-to-Many)*
  - Um evento pode ter múltiplos palestrantes.
  - Um palestrante pode participar de apenas um evento específico (modelo atual simplificado).

- **Event ↔ Participant** *(Many-to-Many)*
  - Um evento pode ter vários participantes.
  - Um participante pode participar de diversos eventos diferentes.

---

### 🧩 Organização do Projeto

```
EventFlow_API/
├── Commands/
│   ├── EventCommand.cs
│   ├── OrganizerCommand.cs
│   ├── ParticipantCommand.cs
│   └── SpeakerCommand.cs
├── Controllers/
│   ├── EventController.cs
│   ├── OrganizerController.cs
│   ├── ParticipantController.cs
│   └── SpeakerController.cs
├── Config/
│   └──AppConfiguration.cs
├── Data/
│   ├── Mapping/
│   │   ├── EventMap.cs
│   │   ├── OrganizerMap.cs
│   │   ├── ParticipantMap.cs
│   │   └── SpeakerMap.cs
│   └── EventFlowContext.cs
├── Models/
│   ├── Event.cs
│   ├── Organizer.cs
│   ├── Participant.cs
│   └── Speaker.cs
├── Repository/
│   ├── Interfaces/
│   │   ├── IEventRepository.cs
│   │   ├── IOrganizerRepository.cs
│   │   ├── IParticipantRepository.cs
│   │   └── ISpeakerRepository.cs
│   ├── EventRepository.cs
│   ├── OrganizerRepository.cs
│   ├── ParticipantRepository.cs
│   └── SpeakerRepository.cs
├── Services/
│   ├── Interfaces/
│   │   ├── IEventService.cs
│   │   ├── IOrganizerService.cs
│   │   ├── IParticipantService.cs
│   │   └── ISpeakerService.cs
│   ├── EventService.cs
│   ├── OrganizerService.cs
│   ├── ParticipantService.cs
│   └── SpeakerService.cs
├── Dockerfile
├── EventFlow-API.http
├── appsettings.json
├── Program.cs
└── README.md
```

---

### 🚧 Próximos Passos

- Implementar autenticação JWT.
- Criar camada de Services e Repositories.
- Realizar testes unitários abrangentes.
- Implementar logging detalhado.
- Aplicar Docker para containerização da aplicação.

---

### 📌 Como Rodar o Projeto

- Clone o repositório
- Configure a conexão com o banco de dados em `appsettings.json`
- Execute `dotnet ef database update` para aplicar as migrations.
- Rode o projeto com `dotnet run`
- Acesse o Swagger em: `https://localhost:<porta>/swagger`
- Para HTTPS utilize a porta 7221 e para HTTP a porta 5041.

---

### 👨‍💻 Autor

- Alyson Souza Carregosa 👨‍💻 Back-end Developer

---

### 📝 Licença

Este projeto está disponível sob a licença MIT.