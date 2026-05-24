# EcomForge API

Backend compacto, limpo e funcional para e-commerce, construído como um **Monolito Modular** com influência clara de **Clean Architecture**.

O objetivo deste boilerplate é servir como base profissional para portfólio: simples o suficiente para entender rapidamente, organizado o suficiente para crescer sem virar uma massa acoplada.

## Stack

- ASP.NET Core 9 com Controllers
- Entity Framework Core
- PostgreSQL
- Redis para carrinho
- JWT com refresh token
- FluentValidation
- Result Pattern
- Serilog
- Semantic Kernel com chat e memória leve

## Arquitetura

```text
src/
├── EcomForge.Api             # Program.cs, Controllers compartilhados, Middleware
├── EcomForge.Application     # Use cases, DTOs, interfaces, serviços de aplicação
├── EcomForge.Domain          # Entidades, Value Objects, Enums, eventos, exceções
├── EcomForge.Infrastructure  # EF Core, Redis, JWT, Repositories, Semantic Kernel
├── EcomForge.Modules         # Produtos, Pedidos, Clientes, Carrinho e IA
└── EcomForge.Common          # Result Pattern, extensions e constantes
```

### Dependências entre camadas

```text
Domain
  ↑
Application
  ↑
Infrastructure ── Api ── Modules
```

- **Domain** não depende de nenhuma outra camada.
- **Application** conhece apenas Domain e Common.
- **Infrastructure** implementa contratos da Application.
- **Modules** organiza os recursos por domínio funcional, mantendo Controllers e serviços próximos da funcionalidade.
- **Api** faz a composição final: DI, autenticação, Swagger, Serilog e middleware.

## Funcionalidades

- Autenticação:
  - `POST /api/auth/register`
  - `POST /api/auth/login`
  - `POST /api/auth/refresh`
- Produtos:
  - CRUD de produtos
  - CRUD de categorias
- Carrinho:
  - Redis como storage
  - `GET /api/cart/{customerId}`
  - `POST /api/cart/{customerId}/items`
  - `DELETE /api/cart/{customerId}`
- Pedidos:
  - Criação a partir dos itens enviados
  - Listagem por cliente
- IA:
  - `POST /api/ai/chat`
  - Semantic Kernel configurado na Infrastructure
  - Plugin simples para recomendações de e-commerce
  - Memória leve em Redis para manter contexto por conversa

## Como Rodar

### 1. Configure o ambiente

Copie o arquivo de exemplo:

```bash
cp .env.example .env
```

Preencha as variáveis de IA caso queira usar o chat com Semantic Kernel.

### 2. Suba PostgreSQL, Redis e API

```bash
docker compose up --build
```

A API ficará disponível em:

- `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`

### 3. Rodar localmente sem Docker

Suba apenas banco e Redis:

```bash
docker compose up postgres redis
```

Depois rode a API:

```bash
dotnet restore
dotnet run --project src/EcomForge.Api/EcomForge.Api.csproj
```

## Semantic Kernel

O módulo de IA foi mantido intencionalmente simples:

- A configuração do Kernel fica em `EcomForge.Infrastructure/AI`.
- Os plugins ficam em `EcomForge.Modules/AI/Plugins`.
- O controller público fica em `EcomForge.Modules/AI/Controllers`.
- A memória da conversa usa Redis com uma chave por `conversationId`.

Variáveis relevantes:

```env
AI__Provider=OpenAI
AI__Model=gpt-4.1-mini
AI__ApiKey=your-api-key
```

Se a chave não estiver configurada, o endpoint responde com uma mensagem controlada, mantendo o boilerplate executável para desenvolvimento.

## Migrations

Exemplo para criar uma migration:

```bash
dotnet ef migrations add InitialCreate \
  --project src/EcomForge.Infrastructure \
  --startup-project src/EcomForge.Api
```

Aplicar migration:

```bash
dotnet ef database update \
  --project src/EcomForge.Infrastructure \
  --startup-project src/EcomForge.Api
```

## Credenciais de Desenvolvimento

Não há seed automático de usuário admin. Use o endpoint de register para criar o primeiro cliente.

## Princípios do Boilerplate

- Controllers claros e objetivos.
- Serviços pequenos, com contratos explícitos.
- Repositories apenas onde simplificam acesso a dados.
- Sem MediatR e sem CQRS completo.
- Result Pattern para evitar exceptions como fluxo comum.
- Código pronto para evoluir em módulos sem perder simplicidade.
