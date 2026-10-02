# Design

## Context

This is a greenfield project with no existing code. The OpenSpec standards define:
- Clean Architecture + DDD + CQRS (architecture.md)
- .NET 8 / ASP.NET Core WebAPI with Autofac, AutoMapper, EF Core, PostgreSQL (tech-stack.md)
- Fluent API entity configurations, Guid/long PKs, soft delete, migration rules (database-and-migrations.md)
- PascalCase, async/await, custom Domain Exceptions (coding-standards.md)
- JWT/OAuth2, no secrets in git, User Secrets for dev (security-and-auth.md)
- RESTful endpoints, ProblemDetails, standard status codes (api-and-http-contracts.md)

The proposal establishes 6 new architecture capabilities that need implementation.

## Goals / Non-Goals

**Goals:**
- Scaffold complete solution structure with 4-layer Clean Architecture
- Configure Autofac DI container with per-layer modules
- Set up AutoMapper with Application-layer profiles
- Configure EF Core with PostgreSQL, repositories, Unit of Work, migrations
- Build WebAPI with JWT auth, ProblemDetails, CORS, logging
- Create UnitTests and IntegrationTests projects with xUnit/Moq/Bogus
- Create Dockerfile with multi-stage build for WebAPI
- Create GitHub Actions CI/CD pipeline (build, test, push to GHCR, deploy to VPS)
- Configure GitHub Container Registry (GHCR) for image storage
- Configure VPS deployment via SSH with Docker CLI
- All code follows the established coding standards

**Non-Goals:**
- No domain-specific features (portfolios, transactions, users) - those are separate capabilities
- No API documentation (Swagger/OpenAPI) - can be added later
- No health checks or observability beyond basic logging

## Decisions

### 1. Solution Structure: Single .sln at root with src/ and test/
**Decision**: Create `Portfolio.sln` at repository root. Projects organized as:
```
src/
  Core/
    Domain/                 # Class Library (.NET 8)
    Application/            # Class Library (.NET 8)
  Infrastructure/
    Persistence/            # Class Library (.NET 8)
  Presentation/
    WebAPI/                 # ASP.NET Core Web API
test/
  UnitTests/                # xUnit Class Library
  IntegrationTests/         # xUnit Class Library
```
**Rationale**: Matches architecture.md exactly. Single solution file enables `dotnet build` from root. Clear separation enables dependency rule enforcement.

**Alternatives considered**:
- Multiple solutions (one per layer) - rejected: complicates builds and refactoring
- All projects flat under src/ - rejected: loses architectural boundary visibility

### 2. CQRS: MediatR for Commands/Queries
**Decision**: Use `MediatR` (via `MediatR.Extensions.Microsoft.DependencyInjection`) in Core/Application for `IRequest<T>`, `IRequestHandler<T>`, `INotification`, `INotificationHandler<T>`.
**Rationale**: Industry standard for CQRS in .NET. Decouples handlers from controllers. Works naturally with Autofac.

**Alternatives considered**:
- Custom mediator - rejected: reinventing wheel, less tested
- Brighter/Paramore - rejected: less common, steeper learning curve

### 3. Dependency Injection: Autofac Modules per Layer
**Decision**: Each layer has `Module.cs`:
- `DomainModule` - registers domain services, factories, events
- `ApplicationModule` - registers MediatR, handlers, validators, AutoMapper profiles
- `PersistenceModule` - registers DbContext, repositories, UnitOfWork
- `WebApiModule` - registers controllers, middleware, Swagger
Composed in `Program.cs`: `builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())` then `ConfigureContainer<ContainerBuilder>`.

**Rationale**: Architecture.md mandates Autofac. Module pattern keeps registrations co-located with their layer. Composition root in Presentation maintains dependency direction.

### 4. AutoMapper: Profiles in Application Layer Only
**Decision**: All `Profile` classes live in Core/Application. No profiles in Domain, Infrastructure, or Presentation. Presentation DTOs map to/from Application DTOs in controllers/endpoints.
**Rationale**: Architecture.md says Application layer owns mapping. Domain entities never exposed to Presentation. Single source of truth for mapping config.

### 5. EF Core: Single DbContext with Fluent Configurations
**Decision**: One `ApplicationDbContext` in Infrastructure/Persistence. Each entity has `EntityTypeConfiguration<T>` in `Configurations/` folder. DbContext applies all via `modelBuilder.ApplyConfigurationsFromAssembly()`.
**Rationale**: Database-and-migrations.md mandates Fluent API in separate classes. Single DbContext simplifies transactions and migrations for this bounded context.

### 6. Repositories: Generic + Specialized
**Decision**: 
- `IRepository<T>` with `GetByIdAsync`, `AddAsync`, `Update`, `Delete`, `ListAsync`
- `Repository<T>` base implementation
- Specialized interfaces (e.g., `IPortfolioRepository`) extending `IRepository<Portfolio>` for custom queries
**Rationale**: Generic covers 80% of cases. Specialized allows domain-specific queries without leaking EF Core into Application.

### 7. Unit of Work: DbContext as IUnitOfWork
**Decision**: `IUnitOfWork` interface with `SaveChangesAsync(CancellationToken)` implemented by `ApplicationDbContext`. Registered as scoped.
**Rationale**: Simple, explicit transaction boundary. Application handlers call `await _unitOfWork.SaveChangesAsync()` after business logic.

### 8. Migrations: Infrastructure Project as Startup Project
**Decision**: Run `dotnet ef migrations add` from Infrastructure/Persistence project with `--startup-project ../Presentation/WebAPI`. Migrations output to `Migrations/` folder.
**Rationale**: EF Core requires startup project for DbContext discovery. Keeps migrations with Infrastructure code.

### 9. WebAPI: Minimal APIs with MapGroup
**Decision**: Use Minimal APIs (`MapGet`, `MapPost`, etc.) organized by `MapGroup("/api/v1/portfolios")` for versioning and resource grouping. Controllers not used.
**Rationale**: Modern .NET approach. Less boilerplate. Vertical slice friendly. Works with Autofac.

### 10. Error Handling: ProblemDetails with Hellang.Middleware.ProblemDetails
**Decision**: Use `Hellang.Middleware.ProblemDetails` package for automatic ProblemDetails (RFC 7807) conversion of exceptions. Custom `DomainException` → 400, `NotFoundException` → 404, unhandled → 500.
**Rationale**: api-and-http-contracts.md mandates ProblemDetails. This package handles it globally with minimal code.

### 11. Authentication: JWT Bearer with Microsoft.AspNetCore.Authentication.JwtBearer
**Decision**: Configure JWT in `Program.cs` with `AddAuthentication().AddJwtBearer()`. Validate issuer, audience, lifetime. Keys from configuration (User Secrets in dev).
**Rationale**: security-and-auth.md mandates JWT. Standard ASP.NET Core approach.

### 12. Testing: Testcontainers for Integration Tests
**Decision**: Use `Testcontainers.PostgreSQL` for IntegrationTests to spin up real PostgreSQL per test run. UnitTests use Moq/Bogus only.
**Rationale**: Real DB catches EF Core issues. Testcontainers provides isolation. No shared dev DB needed.

### 13. Containerization: Multi-stage Dockerfile for WebAPI
**Decision**: Create `Dockerfile` in `src/Presentation/WebAPI` with two stages:
- Build stage: `mcr.microsoft.com/dotnet/sdk:8.0` - restore, build, publish
- Runtime stage: `mcr.microsoft.com/dotnet/aspnet:8.0` - copy published output, create non-root user, expose port 8080
**Rationale**: deployment-and-ci-cd.md mandates multi-stage builds with official Microsoft images and non-root user.

### 14. Container Registry: GitHub Container Registry (GHCR)
**Decision**: Push images to `ghcr.io/<owner>/<repo>` using `GITHUB_TOKEN` for authentication. Tag with commit SHA and `latest` on main branch pushes; add semantic version tags on Git tag pushes.
**Rationale**: deployment-and-ci-cd.md specifies GHCR as registry with SHA+latest tagging strategy.

### 15. CI/CD: GitHub Actions Workflows
**Decision**: Create two workflows in `.github/workflows/`:
- `ci.yml`: Runs on push/PR to main - `dotnet build`, `dotnet test`
- `cd.yml`: Runs on successful CI completion on main - Docker build/push to GHCR, SSH deploy to VPS
**Rationale**: deployment-and-ci-cd.md mandates GitHub Actions for CI/CD with separate build/test and deploy phases.

### 16. VPS Deployment: Direct Docker CLI via SSH
**Decision**: Use `appleboy/ssh-action` in CD workflow to execute on VPS:
1. `docker login ghcr.io` with GHCR_PAT
2. `docker pull ghcr.io/<owner>/<repo>:latest`
3. `docker stop <container> && docker rm <container>`
4. `docker run -d --name <container> --restart unless-stopped -p <host>:<container> -e ASPNETCORE_ENVIRONMENT=Production --env-file /path/to/vps/.env ghcr.io/<owner>/<repo>:latest`
5. `docker image prune -f`
**Rationale**: deployment-and-ci-cd.md specifies direct Docker CLI over SSH without Docker Compose.

### 17. Secrets Management: GitHub Repository Secrets
**Decision**: All deployment secrets stored in GitHub Repository Secrets (`VPS_HOST`, `VPS_USERNAME`, `VPS_SSH_KEY`, `VPS_PORT`, `GHCR_PAT`). No secrets in repository files.
**Rationale**: deployment-and-ci-cd.md mandates zero secrets in source control.

## Risks / Trade-offs

| Risk | Mitigation |
|------|------------|
| Autofac + Minimal APIs integration complexity | Use `Autofac.Extensions.DependencyInjection` and register `IServiceProviderFactory` early in Program.cs |
| MediatR + Autofac registration order | Register MediatR in ApplicationModule before handlers; use `RegisterMediatR(typeof(ApplicationModule).Assembly)` |
| EF Core migration conflicts in team | Enforce "never modify existing migrations" in code review; use CI check |
| Testcontainers startup time in CI | Use single container per test class (collection fixture) not per test |
| Versioning strategy locked to URL path | Acceptable for v1; can add header-based versioning later if needed |
| No API documentation at launch | Add Swashbuckle/Scalar in follow-up change |
| Circular dependency risk between layers | Use `dotnet-validate` or architecture tests to enforce dependency rules |
| Docker build time in CI | Use layer caching, multi-stage builds, and `.dockerignore` to minimize context |
| GHCR rate limits | Use `GITHUB_TOKEN` (higher limits) and consider self-hosted runners for heavy usage |
| SSH deployment failures | Implement idempotent deployment commands (`|| true` for stop/rm), add retry logic in workflow |
| VPS environment drift | Manage VPS `.env` file via infrastructure-as-code or documented runbook |
| Non-root user permissions in container | Ensure published output has correct ownership; test locally with `docker run --user` |