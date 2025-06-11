# EventFlow API

### 📌 Descrição do Projeto

O ***EventFlow API*** é um projeto de API RESTful desenvolvido em .NET 8, projetado para ser uma solução robusta e escalável para a gestão e organização de eventos. O projeto demonstra as melhores práticas de desenvolvimento de software, incluindo a implementação de uma Arquitetura Limpa (Clean Architecture) em camadas, um sistema de autenticação e autorização via JWT, validação de dados com FluentValidation e uma suíte de testes unitários.

OBS: Esse projeto foi pensado para implementação no Google Developers Group (GDG) Aracaju.

A API proporciona funcionalidades completas para o ciclo de vida de eventos, abrangendo o gerenciamento de organizadores, palestrantes e participantes, e o tratamento de relacionamentos complexos (one-to-many e many-to-many).

---

### 🚀 Objetivos do Projeto

- Implementar uma **Arquitetura Limpa** desacoplada, com clara separação entre as camadas de Domínio, Aplicação, Infraestrutura e Apresentação.
- Aplicar práticas recomendadas para uso de **Entity Framework Core**, incluindo mapeamento com Fluent API.
- Garantir a qualidade e a confiabilidade do código através de **testes unitários** (xUnit, Moq, FluentAssertions).
- Oferecer uma solução de **autenticação e autorização** segura utilizando JWT.
- Fornecer uma documentação de API clara e interativa com **Swagger/OpenAPI**.
- Apresentar um código limpo, organizado e facilmente extensível.

---

### 🛠️ Tecnologias Utilizadas

**Backend:**
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (com migrations)
- SQL Server

**Padrões e Conceitos:**
- Arquitetura Limpa (Clean Architecture)
- Injeção de Dependência (DI)
- Mapeamento de objetos com **AutoMapper**
- Validação de dados com **FluentValidation**
- Documentação de API com **Swagger/OpenAPI**
- Autenticação e Autorização com **JWT (JSON Web Tokens)**

**Testes:**

- **xUnit** (Framework de Teste)
- **Moq** (Biblioteca para Mocking de dependências)
- **FluentAssertions** (Para asserções mais legíveis)

### 🏛️ Arquitetura do Projeto

A solução é organizada em projetos distintos que representam as camadas da Arquitetura Limpa, garantindo a separação de responsabilidades:

- **EventFlow.Core (Domínio):** O coração da aplicação. Contém as entidades de negócio, DTOs, comandos e as interfaces dos repositórios e serviços. Não depende de nenhuma outra camada.
- **EventFlow.Application (Aplicação):** Contém a lógica de negócio e os casos de uso. Implementa as interfaces de serviço definidas no Core e orquestra as ações, mas não sabe como os dados são persistidos.
- **EventFlow.Infrastructure (Infraestrutura):** Contém os detalhes técnicos e as implementações das interfaces do Core. É aqui que reside o acesso ao banco de dados com Entity Framework, implementações de repositórios, helpers, etc.
- **EventFlow.Presentation (Apresentação):** A camada de entrada da aplicação. Neste caso, é a Web API com seus Controllers, configuração de inicialização (Program.cs), e tudo relacionado ao protocolo HTTP.
- **EventFlow.Tests (Testes):** Projeto dedicado aos testes unitários das outras camadas.

### 🚧 Próximos Passos

Agora que a base arquitetural está sólida, os próximos passos para evoluir o projeto incluem:

- **Paginação, Ordenação e Filtragem:** Implementar nos endpoints GET All para torná-los escaláveis.
- **Middleware de Exceções:** Criar um middleware global para tratamento de erros, limpando os controllers.
- **Autorização por Papéis (Roles):** Adicionar papéis de "Admin" e "Organizer" para proteger endpoints críticos.
- **Logging Estruturado:** Integrar o Serilog para um logging mais robusto e preparado para produção.
- **Gerenciamento de Segredos:** Mover segredos (ConnectionString, Jwt:Key) do appsettings.json para User Secrets (desenvolvimento) e Environment Variables (produção).
- **CI/CD:** Configurar um pipeline de Integração e Entrega Contínua (ex: GitHub Actions).

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
  - Um organizador pode gerenciar múltiplos eventos. Cada evento tem apenas um organizador.

- **Event ↔ Speaker** *(Many-to-Many)*
  - Um evento pode ter múltiplos palestrantes.
  - Um palestrante pode participar de múltiplos eventos.
  - A relação é feita através da entidade SpeakerEvent.

- **Event ↔ Participant** *(Many-to-Many)*
  - Um evento pode ter vários participantes.
  - Um participante pode se inscrever em diversos eventos.

---

### 📁 Estrutura de Diretórios e Arquivos
A estrutura de diretórios foi organizada para promover a separação de responsabilidades e facilitar a manutenção.

```
EventFlow/
├── EventFlow.sln
│
├── 📁 EventFlow.Core
│   ├── 📁 Commands
│   │   ├── EventCommand.cs
│   │   ├── LoginUserCommand.cs
│   │   ├── OrganizerCommand.cs
│   │   ├── ParticipantCommand.cs
│   │   ├── RegisterUserCommand.cs
│   │   └── SpeakerCommand.cs
│   ├── 📁 Models
│   │   ├── 📁 DTOs
│   │   │   ├── EventDTO.cs
│   │   │   ├── EventSummaryDTO.cs
│   │   │   ├── OrganizerDTO.cs
│   │   │   ├── ParticipantDTO.cs
│   │   │   ├── SpeakerDTO.cs
│   │   │   ├── UserDTO.cs
│   │   │   └── UserPasswordDTO.cs
│   │   ├── Event.cs
│   │   ├── Organizer.cs
│   │   ├── Participant.cs
│   │   ├── Speaker.cs
│   │   ├── SpeakerEvent.cs
│   │   └── User.cs
│   ├── 📁 Repository
│   │   └── 📁 Interfaces
│   │       ├── IEventRepository.cs
│   │       ├── IOrganizerRepository.cs
│   │       ├── IParticipantRepository.cs
│   │       ├── ISpeakerRepository.cs
│   │       └── IUserRepository.cs
│   ├── 📁 Services
│   │   └── 📁 Interfaces
│   │       ├── IAuthService.cs
│   │       ├── IEventService.cs
│   │       ├── IOrganizerService.cs
│   │       ├── IParticipantService.cs
│   │       └── ISpeakerService.cs
│   └── GlobalUsing.cs
│
├── 📁 EventFlow.Application
│   ├── 📁 Services
│   │   ├── AuthService.cs
│   │   ├── EventService.cs
│   │   ├── OrganizerService.cs
│   │   ├── ParticipantService.cs
│   │   └── SpeakerService.cs
│   ├── 📁 Validators
│   │   ├── EventCommandValidator.cs
│   │   ├── OrganizerCommandValidator.cs
│   │   ├── ParticipantCommandValidator.cs
│   │   └── SpeakerCommandValidator.cs
│   └── GlobalUsing.cs
│
├── 📁 EventFlow.Infrastructure
│   ├── 📁 Data
│   │   ├── 📁 Mapping
│   │   │   ├── EventMap.cs
│   │   │   ├── OrganizerMap.cs
│   │   │   ├── ParticipantMap.cs
│   │   │   ├── SpeakerEventMap.cs
│   │   │   └── SpeakerMap.cs
│   │   └── EventFlowContext.cs
│   ├── 📁 Helpers
│   │   └── DateTimeConverterHelper.cs
│   ├── 📁 Profiles
│   │   └── MappingProfile.cs
│   ├── 📁 Repository
│   │   ├── EventRepository.cs
│   │   ├── OrganizerRepository.cs
│   │   ├── ParticipantRepository.cs
│   │   ├── SpeakerRepository.cs
│   │   └── UserRepository.cs
│   └── GlobalUsing.cs
│
├── 📁 EventFlow.Presentation
│   ├── 📁 Config
│   │   └── AppConfiguration.cs
│   ├── 📁 Controllers
│   │   ├── AuthController.cs
│   │   ├── EventController.cs
│   │   ├── OrganizerController.cs
│   │   ├── ParticipantController.cs
│   │   └── SpeakerController.cs
│   ├── 📁 Properties
│   │   ├── launchSettings.json
│   │   └── serviceDependencies.json
│   ├── appsettings.json
│   ├── GlobalUsing.cs
│   └── Program.cs
│
├── 📁 EventFlow.Tests
│   ├── 📁 Data
│   │   └── IntegrationTestBase.cs
│   ├── 📁 Controllers
│   │   ├── EventControllerTests.cs
│   │   ├── OrganizerControllerTests.cs
│   │   ├── ParticipantControllerTests.cs
│   │   └── SpeakerControllerTests.cs
│   ├── 📁 Services
│   │   ├── EventServiceTests.cs
│   │   ├── OrganizerServiceTests.cs
│   │   ├── ParticipantServiceTests.cs
│   │   └── SpeakerServiceTests.cs
│   └── 📁 Validators
│       ├── EventCommandValidatorTests.cs
│       ├── OrganizerCommandValidatorTests.cs
│       ├── ParticipantCommandValidatorTests.cs
│       └── SpeakerCommandValidatorTests.cs
│
├── .dockerignore
├── .gitignore
├── Dockerfile
└── README.md
```

---

### 📌 Como Rodar o Projeto

- Clone o repositório
- No arquivo `appsettings.json` do projeto EventFlow.Presentation, configure sua `ConnectionString` para o banco de dados.
- No mesmo arquivo, certifique-se de que a `Jwt:Key` está definida.
- Abra um terminal na pasta raiz da solução (`EventFlow/`).
- Execute o comando para aplicar as migrations do Entity Framework: `dotnet ef database update --project EventFlow.Infrastructure --startup-project EventFlow.Presentation`
- Rode o projeto com `dotnet run --project EventFlow.Presentation`
- Acesse o Swagger em: `https://localhost:7221/swagger` (ou a porta configurada).
- Para HTTPS utilize a porta 7221 e para HTTP a porta 5041.

---

### 👨‍💻 Autor

- Alyson Souza Carregosa 👨‍💻 Back-end Developer

---

### 📝 Licença

Este projeto está disponível sob a licença MIT.
