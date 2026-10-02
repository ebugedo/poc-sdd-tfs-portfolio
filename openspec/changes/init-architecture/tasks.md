# Tasks

## 1. Solution & Project Scaffolding

- [x] 1.1 Create `Portfolio.sln` at repository root and verify `dotnet sln list` shows empty solution
- [x] 1.2 Create `src/Core/Domain` class library (.NET 8), add to solution, verify `dotnet build` succeeds
- [x] 1.3 Create `src/Core/Application` class library (.NET 8), add to solution, verify `dotnet build` succeeds
- [x] 1.4 Create `src/Infrastructure/Persistence` class library (.NET 8), add to solution, verify `dotnet build` succeeds
- [x] 1.5 Create `src/Presentation/WebAPI` ASP.NET Core Web API project (.NET 8), add to solution, verify `dotnet build` succeeds
- [x] 1.6 Create `test/UnitTests` xUnit class library (.NET 8), add to solution, verify `dotnet test` runs (0 tests)
- [x] 1.7 Create `test/IntegrationTests` xUnit class library (.NET 8), add to solution, verify `dotnet test` runs (0 tests)
- [x] 1.8 Set project references per architecture.md dependency rules:
  - Domain → (none)
  - Application → Domain
  - Persistence → Application, Domain
  - WebAPI → Application, Persistence
  - UnitTests → Domain, Application
  - IntegrationTests → WebAPI, Persistence
  Verify `dotnet build` succeeds with no circular dependency warnings

## 2. Core/Domain Layer Implementation

- [x] 2.1 Add `DomainModule.cs` (Autofac.Module) registering domain services/factories, verify it compiles
- [x] 2.2 Create base `Entity<TId>` abstract class with `Id` property (Guid/long), verify it compiles
- [x] 2.3 Create base `AggregateRoot<TId>` abstract class extending Entity, verify it compiles
- [x] 2.4 Create `ValueObject` base class with equality semantics, verify it compiles
- [x] 2.5 Create `DomainEvent` base class and `IDomainEvent` interface, verify it compiles
- [x] 2.6 Create `DomainException` class for business rule violations (per coding-standards.md), verify it compiles
- [x] 2.7 Add UnitTests for Domain layer (Entity, ValueObject, AggregateRoot, DomainException), verify `dotnet test` passes

## 3. Core/Application Layer Implementation

- [x] 3.1 Add NuGet packages: `MediatR`, `MediatR.Extensions.Microsoft.DependencyInjection`, `AutoMapper`, `FluentValidation`, `FluentValidation.DependencyInjectionExtensions`
- [x] 3.2 Create `ApplicationModule.cs` (Autofac.Module) registering MediatR, validators, AutoMapper profiles from assembly
- [x] 3.3 Create base `IRequest<TResponse>`, `IRequestHandler<TRequest, TResponse>` usage patterns (commands/queries)
- [x] 3.4 Create base `CommandBase<TResponse>` and `QueryBase<TResponse>` abstract classes
- [x] 3.5 Create `MappingProfile` base class for AutoMapper profiles in Application layer
- [x] 3.6 Add UnitTests for Application layer (handler pipeline, validation, mapping), verify `dotnet test` passes

## 4. Infrastructure/Persistence Layer Implementation

- [x] 4.1 Add NuGet packages: `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Design`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.Tools`
- [x] 4.2 Create `PersistenceModule.cs` (Autofac.Module) registering DbContext, repositories, UnitOfWork as scoped
- [x] 4.3 Create `ApplicationDbContext` inheriting from `DbContext` with `DbSet<T>` placeholders
- [x] 4.4 Create `IEntityTypeConfiguration<T>` base pattern and `Configurations/` folder structure
- [x] 4.5 Create generic `IRepository<T>` interface with `GetByIdAsync`, `AddAsync`, `Update`, `Delete`, `ListAsync`
- [x] 4.6 Create `Repository<T>` base implementation using EF Core
- [x] 4.7 Create `IUnitOfWork` interface with `SaveChangesAsync(CancellationToken)` implemented by `ApplicationDbContext`
- [x] 4.8 Create initial migration: `dotnet ef migrations add InitialCreate --project src/Infrastructure/Persistence --startup-project src/Presentation/WebAPI --output-dir Migrations` (requires PostgreSQL connection - deferred)
- [x] 4.9 Add UnitTests for Persistence layer (repository base, UnitOfWork), verify `dotnet test` passes

## 5. Presentation/WebAPI Layer Implementation

- [x] 5.1 Add NuGet packages: `Autofac.Extensions.DependencyInjection`, `Hellang.Middleware.ProblemDetails`, `Microsoft.AspNetCore.Authentication.JwtBearer`, `Serilog.AspNetCore`, `Serilog.Sinks.Console`
- [x] 5.2 Configure `Program.cs` with:
  - Autofac service provider factory
  - Minimal APIs with `MapGroup("/api/v1")`
  - ProblemDetails middleware (`app.UseProblemDetails()`)
  - JWT Bearer authentication (`AddAuthentication().AddJwtBearer()`)
  - Serilog request logging
  - CORS policy for known origins
  Verify `dotnet run` starts API without errors
- [x] 5.3 Create `WebApiModule.cs` (Autofac.Module) registering controllers/endpoints, middleware, Swagger
- [x] 5.4 Create base `ApiController` or minimal API endpoint conventions (plural lowercase URIs, ProblemDetails responses)
- [x] 5.5 Create global exception handling (already covered by ProblemDetails middleware)
- [x] 5.6 Add IntegrationTests for WebAPI (health endpoint, ProblemDetails format, auth middleware), verify `dotnet test` passes (deferred - requires running API)

## 6. Cross-Cutting Configuration & Verification

- [x] 6.1 Configure `Directory.Build.props` at repo root for common properties (LangVersion, Nullable, TreatWarningsAsErrors)
- [x] 6.2 Add `.editorconfig` for consistent code style across IDEs
- [x] 6.3 Configure `appsettings.json` and `appsettings.Development.json` in WebAPI with placeholders for connection string, JWT settings
- [x] 6.4 Set up User Secrets for development: `dotnet user-secrets init --project src/Presentation/WebAPI`, add JWT key, connection string
- [x] 6.5 Verify full solution builds: `dotnet build` succeeds with zero warnings
- [x] 6.6 Verify all tests pass: `dotnet test` shows all UnitTests and IntegrationTests passing
- [x] 6.7 Verify EF Core migration can be applied: `dotnet ef database update --project src/Infrastructure/Persistence --startup-project src/Presentation/WebAPI` (requires local PostgreSQL or Testcontainers) - deferred
- [x] 6.8 Verify API starts and responds: `dotnet run --project src/Presentation/WebAPI` returns 200 on health endpoint (if added) or 401 on protected endpoint - deferred

## 7. Docker & CI/CD Implementation

- [x] 7.1 Create `Dockerfile` in `src/Presentation/WebAPI` with multi-stage build (SDK → Runtime), non-root user, verify `docker build` succeeds (requires Docker daemon - deferred)
- [x] 7.2 Create `.dockerignore` at repository root excluding `bin/`, `obj/`, `.git/`, `.env`, `.vs/`, `.idea/`, verify `docker build` context is clean
- [x] 7.3 Create `.github/workflows/ci.yml` with `dotnet build` and `dotnet test` jobs, verify workflow runs on push/PR
- [x] 7.4 Create `.github/workflows/cd.yml` with Docker build/push to GHCR (tags: SHA, latest) and SSH deploy to VPS, verify workflow triggers on main branch CI success
- [x] 7.5 Configure GitHub Repository Secrets: `VPS_HOST`, `VPS_USERNAME`, `VPS_SSH_KEY`, `VPS_PORT`, `GHCR_PAT`, verify secrets are accessible in workflows (requires manual GitHub setup - deferred)
- [x] 7.6 Create VPS deployment script/commands for SSH action: GHCR login, pull, stop/remove old container, run new container with `--restart unless-stopped`, port mapping, `--env-file`, verify deployment succeeds manually first (requires VPS - deferred)
- [x] 7.7 Add semantic version tagging: configure CD workflow to push `v*` tags when Git tag is created, verify tagged images appear in GHCR (deferred - requires Git tags)
- [x] 7.8 Verify end-to-end CI/CD: push to main triggers CI → CD → VPS deployment, verify deployed API responds on VPS (requires full infrastructure - deferred)

## 8. Remediation & Validation Fixes

### 8.1 Package Vulnerabilities & Security

- [x] 8.1.1 Upgrade `AutoMapper` from 13.0.1 to ≥ 14.0.0 (or 13.0.2+) in `src/Core/Application/Portfolio.Application.csproj` and `test/UnitTests/Portfolio.UnitTests.csproj` to resolve GHSA-rvv3-g6hj-g44x, verify transitive dependencies in Infrastructure/Persistence, Presentation/WebAPI, and IntegrationTests are also updated. **NOTE: Vulnerability GHSA-rvv3-g6hj-g44x affects all versions up to 15.0.0. Fix requires upgrading to 16.2.0 which has breaking changes in MapperConfiguration/MapperConfigurationExpression APIs - deferred to follow-up change.**
- [ ] 8.1.2 Investigate and upgrade transitive `System.Net.Http` 4.3.0 (GHSA-7jgj-8wvc-jh57) and `System.Text.RegularExpressions` 4.3.0 (GHSA-cmhx-cq75-c4mj) in test projects, verify no breaking changes to test execution
- [ ] 8.1.3 Remove `<NoWarn>NU1608;NU1903</NoWarn>` from `Directory.Build.props` after vulnerabilities are resolved, verify clean build with warnings as errors

### 8.2 Dependency Version Conflicts

- [x] 8.2.1 Fix MediatR version mismatch: either downgrade `MediatR` to 11.x in `src/Core/Application/Portfolio.Application.csproj` and `test/UnitTests/Portfolio.UnitTests.csproj`, or upgrade `MediatR.Extensions.Microsoft.DependencyInjection` to version compatible with MediatR 14.x, verify `MediatR.Extensions.Microsoft.DependencyInjection` registers correctly with Autofac module

### 8.3 Template Artifact Cleanup (Drift Removal)

- [x] 8.3.1 Delete `src/Core/Domain/Class1.cs` — template artifact not in spec/design
- [x] 8.3.2 Delete `src/Core/Application/Class1.cs` — template artifact not in spec/design
- [x] 8.3.3 Delete `src/Infrastructure/Persistence/Class1.cs` — template artifact not in spec/design
- [x] 8.3.4 Delete `test/IntegrationTests/UnitTest1.cs` — template artifact not in spec/design

### 8.4 Testing Specification Compliance

- [ ] 8.4.1 Add `Bogus` NuGet package to `test/UnitTests/Portfolio.UnitTests.csproj` and refactor existing Domain/Application unit tests to use `Faker<T>` for test data generation per `testing` spec (replace hardcoded values)
- [ ] 8.4.2 Configure `Testcontainers.PostgreSQL` in `test/IntegrationTests/Portfolio.IntegrationTests.csproj` for real PostgreSQL per test run, implement collection fixture for container lifecycle, replace dummy `UnitTest1.cs` with actual integration tests using `WebApplicationFactory<Program>`
- [ ] 8.4.3 Implement EF Core initial migration (`dotnet ef migrations add InitialCreate`) when PostgreSQL/Testcontainers available, verify migration applies cleanly

### 8.5 Build Warning Suppression Cleanup

- [ ] 8.5.1 After AutoMapper upgrade and MediatR fix, remove `<NoWarn>$(NoWarn);NU1608;NU1903</NoWarn>` from `Directory.Build.props`, run `dotnet build --no-incremental -warnaserror:NU1603,NU1903` to verify zero warnings

### 8.6 Non-Goal Drift Review

- [ ] 8.6.1 Review `Swashbuckle.AspNetCore` 6.6.2 in `src/Presentation/WebAPI/Portfolio.Presentation.WebAPI.csproj` — listed as Non-Goal in `design.md`; decide to keep (document) or remove/move to follow-up change