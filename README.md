# EcomForge API

EcomForge API is a compact e-commerce backend boilerplate built with **ASP.NET Core 9**, **Clean Architecture principles**, and a pragmatic **Modular Monolith** structure.

It is designed for developers who want a clear starting point for building an e-commerce backend without jumping straight into microservices, heavy CQRS, or unnecessary framework complexity.

## Why This Project Exists

Most e-commerce examples are either too small to be useful or too large to understand quickly. EcomForge sits in the middle:

- small enough to read and adapt;
- organized enough to grow;
- realistic enough to show authentication, products, cart, orders, persistence, Redis, and AI integration;
- simple enough to avoid architectural ceremony.

This is a boilerplate, not a finished commerce platform. It gives you the foundation so you can build the business-specific parts on top.

## Tech Stack

- ASP.NET Core 9 with Controllers
- Entity Framework Core
- PostgreSQL
- Redis for cart persistence
- JWT authentication with refresh tokens
- FluentValidation
- Result Pattern
- Serilog
- Semantic Kernel for AI chat
- Docker and Docker Compose

## Solution Structure

```text
src/
|-- EcomForge.Api
|   |-- Program.cs
|   `-- Middleware
|
|-- EcomForge.Application
|   |-- Abstractions
|   |-- DTOs
|   |-- Services
|   `-- Validators
|
|-- EcomForge.Domain
|   |-- Entities
|   |-- Enums
|   |-- Events
|   |-- Exceptions
|   `-- ValueObjects
|
|-- EcomForge.Infrastructure
|   |-- AI
|   |-- Auth
|   |-- Cart
|   `-- Persistence
|
|-- EcomForge.Modules
|   |-- AI
|   |-- Cart
|   |-- Customers
|   |-- Orders
|   `-- Products
|
`-- EcomForge.Common
    |-- Constants
    `-- Results
```

## Architecture

EcomForge follows a Clean Architecture-inspired dependency direction:

```text
Domain
  ^
Application
  ^
Infrastructure
```

The API and Modules compose the application and expose HTTP endpoints.

### Layer Responsibilities

**Domain**

Contains the business model: entities, enums, value objects, domain events, and domain exceptions. It has no dependency on any other project.

**Application**

Contains use-case-oriented services, DTOs, validation, and abstractions used by outer layers. It depends only on Domain and Common.

**Infrastructure**

Implements persistence, JWT, Redis cart storage, password hashing, and Semantic Kernel integration.

**Modules**

Groups HTTP endpoints by business capability. Each module keeps its controllers and feature-specific pieces close together.

**Api**

The composition root. It wires dependency injection, Serilog, authentication, authorization, Swagger, middleware, and controller discovery.

## Features

### Authentication

- Register customers
- Login with JWT
- Refresh access tokens

Endpoints:

```text
POST /api/auth/register
POST /api/auth/login
POST /api/auth/refresh
```

### Products and Categories

- Create, read, update, and delete products
- Create, read, update, and delete categories
- PostgreSQL persistence through EF Core

Endpoints:

```text
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}

GET    /api/categories
GET    /api/categories/{id}
POST   /api/categories
PUT    /api/categories/{id}
DELETE /api/categories/{id}
```

### Cart

The cart is intentionally stored in Redis, keeping it fast and separate from the relational order history.

Endpoints:

```text
GET    /api/cart/{customerId}
POST   /api/cart/{customerId}/items
DELETE /api/cart/{customerId}
```

### Orders

- Create orders from product items
- Decrease stock when an order is created
- List orders by customer

Endpoints:

```text
POST /api/orders
GET  /api/orders/customer/{customerId}
```

### AI Chat

The AI module uses Semantic Kernel in a lightweight way:

- Kernel configuration lives in Infrastructure.
- AI plugins live in the AI module.
- Conversation memory is stored in Redis.
- The API remains usable even when no AI key is configured.

Endpoint:

```text
POST /api/ai/chat
```

Example request:

```json
{
  "conversationId": "demo-session",
  "message": "Recommend a keyboard under 300 BRL"
}
```

## Getting Started

### 1. Clone

```bash
git clone https://github.com/joaobastos000/ecomforge-api.git
cd ecomforge-api
```

### 2. Configure Environment Variables

```bash
cp .env.example .env
```

Default values are already aligned with `docker-compose.yml`.

Important variables:

```env
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__Postgres=Host=postgres;Port=5432;Database=ecomforge;Username=ecomforge;Password=ecomforge
ConnectionStrings__Redis=redis:6379
Jwt__Issuer=EcomForge
Jwt__Audience=EcomForge.Client
Jwt__Secret=change-this-development-secret-with-at-least-32-characters
Jwt__AccessTokenMinutes=30
Jwt__RefreshTokenDays=7
AI__Provider=OpenAI
AI__Model=gpt-4.1-mini
AI__ApiKey=
```

### 3. Run With Docker

```bash
docker compose up --build
```

Available URLs:

```text
API:     http://localhost:8080
Swagger: http://localhost:8080/swagger
```

### 4. Run Locally

Start only PostgreSQL and Redis:

```bash
docker compose up postgres redis
```

Run the API:

```bash
dotnet restore
dotnet run --project src/EcomForge.Api/EcomForge.Api.csproj
```

Local Swagger URL:

```text
http://localhost:5088/swagger
```

## Database Migrations

Create a migration:

```bash
dotnet ef migrations add InitialCreate \
  --project src/EcomForge.Infrastructure \
  --startup-project src/EcomForge.Api
```

Apply migrations:

```bash
dotnet ef database update \
  --project src/EcomForge.Infrastructure \
  --startup-project src/EcomForge.Api
```

## Semantic Kernel Setup

The AI endpoint works in fallback mode when no API key is configured. To enable real model calls, set:

```env
AI__Provider=OpenAI
AI__Model=gpt-4.1-mini
AI__ApiKey=your-api-key
```

Relevant files:

```text
src/EcomForge.Infrastructure/AI/SemanticKernelChatService.cs
src/EcomForge.Infrastructure/AI/RedisAiMemory.cs
src/EcomForge.Modules/AI/Plugins/EcommerceAiPlugin.cs
src/EcomForge.Modules/AI/Controllers/AiController.cs
```

## Design Choices

- Controllers are explicit and easy to follow.
- Application services keep use cases simple.
- The Domain project stays independent.
- Redis is used only where it makes sense for this boilerplate: the cart and lightweight AI memory.
- The Result Pattern handles expected failures without using exceptions as normal flow.
- No MediatR and no full CQRS, on purpose.
- Modules make feature ownership visible without splitting the system into services too early.

## What To Improve Before Production

Before using this as a production backend, consider adding:

- committed EF Core migrations;
- unit and integration tests;
- role-based authorization;
- ownership checks for customer-specific resources;
- pagination, filtering, and sorting;
- health checks;
- CI pipeline;
- payment integration;
- shipping workflow;
- stronger inventory rules;
- observability and structured dashboards.

## Good Use Cases

This repository is useful if you want to:

- study a modular monolith in .NET;
- start an e-commerce backend without a blank solution;
- build a portfolio project;
- experiment with Semantic Kernel inside a business API;
- evolve a simple backend into something more complete.

## License

Use this project as a learning resource or as a base for your own e-commerce backend.
