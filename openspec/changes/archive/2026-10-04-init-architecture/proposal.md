# Proposal

## Why

This project is a greenfield portfolio management system that needs a solid architectural foundation. The OpenSpec standards define Clean Architecture + DDD + CQRS with .NET/ASP.NET Core, but no implementation exists yet. This change establishes the initial solution structure, project scaffolding, and core infrastructure to enable future domain capabilities.

## What Changes

- Create solution file (`.sln`) at repository root
- Create `src/` directory with four project layers:
  - `src/Core/Domain` - Class library for enterprise domain logic (Entities, Aggregates, Value Objects, Domain Events)
  - `src/Core/Application` - Class library for business use cases (CQRS Commands, Queries, DTOs, AutoMapper profiles, MediatR handlers)
  - `src/Infrastructure/Persistence` - Class library for EF Core DbContext, Repositories, Migrations (PostgreSQL)
  - `src/Presentation/WebAPI` - ASP.NET Core Web API project (Controllers/Endpoints, Autofac modules, Middleware, Program.cs)
- Create `test/` directory with two test projects:
  - `test/UnitTests` - xUnit + Moq + Bogus for Domain & Application unit tests
  - `test/IntegrationTests` - xUnit for API & Database integration tests
- Configure Autofac as the DI container across all layers
- Configure AutoMapper for entity-DTO mapping
- Configure EF Core with PostgreSQL provider and initial migration
- Set up JWT authentication middleware in WebAPI
- Configure ProblemDetails (RFC 7807) for error responses
- Create Dockerfile with multi-stage build for WebAPI
- Create `.dockerignore` at repository root
- Create GitHub Actions CI/CD workflow (build, test, push to GHCR, deploy to VPS)
- Configure GitHub Container Registry (GHCR) for image storage
- Configure VPS deployment via SSH with Docker CLI

## Capabilities

### New Capabilities
- `architecture/solution-structure`: Defines the Clean Architecture layer boundaries, project organization, and dependency rules
- `architecture/dependency-injection`: Autofac configuration and module registration patterns across layers
- `architecture/mapping`: AutoMapper profiles and conventions for cross-layer object mapping
- `architecture/persistence`: EF Core DbContext, repository patterns, migration strategy, and PostgreSQL configuration
- `architecture/api`: ASP.NET Core WebAPI setup, endpoint conventions, middleware pipeline, and ProblemDetails error handling
- `architecture/testing`: Unit and integration test project structure, test data generation (Bogus), and mocking patterns (Moq)
- `architecture/deployment-ci-cd`: Docker containerization, GitHub Actions CI/CD pipeline, GitHub Container Registry (GHCR), and VPS deployment via SSH

### Modified Capabilities
None - this is a greenfield initialization

## Impact

- All future domain capabilities (portfolio management, transactions, users, etc.) will depend on this foundation
- Establishes the dependency rule: Domain → Application → Infrastructure/Presentation (no reverse dependencies)
- Sets up the technology stack defined in `tech-stack.md` and `database-and-migrations.md`
- Enables the architectural patterns defined in `architecture.md`
- Provides the coding standards baseline from `coding-standards.md`
- Configures security baseline from `security-and-auth.md`
- Establishes API contract conventions from `api-and-http-contracts.md`
- Enables containerized deployment and automated CI/CD per `deployment-and-ci-cd.md`