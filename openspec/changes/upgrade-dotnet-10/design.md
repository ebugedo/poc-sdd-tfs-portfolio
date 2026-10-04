# Design

## Context

See proposal.md for motivation. Current state: all projects target .NET 8 (`net8.0`), NuGet packages at .NET 8 compatible versions, GitHub Actions use `8.0.x`, Dockerfile uses .NET 8 base images. The implementation has already been completed in the codebase (standards updated, csproj files changed, workflows updated).

## Goals / Non-Goals

**Goals:**
- Document the technical decisions for .NET 10 upgrade
- Define package version requirements for .NET 10 compatibility
- Define migration/rollback strategy
- Capture breaking changes to watch for

**Non-Goals:**
- Implement the upgrade (already done)
- Add new features or behavior changes
- Modify domain logic or API contracts

## Decisions

### 1. Target Framework: `net10.0`
**Decision:** All 6 projects target `net10.0`.

**Rationale:** .NET 10 is the next LTS (Long Term Support) release with support until November 2027. Provides performance improvements, C# 13 features, and 3+ years of support.

**Alternatives considered:**
- Stay on .NET 8 (LTS until Nov 2026) - rejected due to shorter remaining support window
- Target .NET 9 (STS - Standard Term Support, 18 months) - rejected due to shorter support cycle

### 2. NuGet Package Version Strategy
**Decision:** Upgrade all packages to minimum versions supporting .NET 10.

**Package version mapping:**
| Package | Current (NET 8) | Target (NET 10) | Notes |
|---------|----------------|-----------------|-------|
| Microsoft.EntityFrameworkCore | 8.0.10 | 10.0.0+ | |
| Npgsql.EntityFrameworkCore.PostgreSQL | 8.0.10 | 10.0.0+ | |
| Microsoft.EntityFrameworkCore.Design/Tools | 8.0.10 | 10.0.0+ | |
| Microsoft.Extensions.Configuration* | 8.0.0 | 10.0.0+ | |
| Autofac | 9.3.4 | 9.0.0+ | 9.x supports .NET 10 |
| Autofac.Extensions.DependencyInjection | 11.0.2 | 10.0.0+ | 10.x for .NET 10 |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.10 | 10.0.0+ | |
| Swashbuckle.AspNetCore | 6.6.2 | 8.0.0+ | |
| Hellang.Middleware.ProblemDetails | 6.5.1 | 7.0.0+ | |
| Serilog.AspNetCore | 10.0.0 | 9.0.0+ | 9.x supports .NET 10 |
| Microsoft.AspNetCore.OpenApi | 8.0.31 | 10.0.0+ | |
| xunit / xunit.runner.visualstudio | 2.9.3 / 3.1.1 | 3.0.0+ / 3.0.0+ | |
| Microsoft.NET.Test.Sdk | 17.13.0 | 17.14.0+ | |
| Moq | 4.20.72 | 4.20.0+ | |
| Bogus | 35.6.2 | 35.0.0+ | |
| AutoMapper | 16.2.0 | 14.0.0+ | |
| FluentValidation | 12.1.1 | 12.0.0+ | |
| MediatR | 11.1.0 | 13.0.0+ | |
| MediatR.Extensions.Microsoft.DependencyInjection | 11.1.0 | 13.0.0+ | |
| Testcontainers.PostgreSql | 4.2.0 | 4.0.0+ | |
| Microsoft.AspNetCore.Mvc.Testing | 8.0.10 | 10.0.0+ | |
| coverlet.collector | 6.0.4 | 6.0.0+ | |

**Rationale:** Use minimum versions with .NET 10 support. Some packages (Autofac, Serilog) have newer major versions that support .NET 10 while older versions may not.

### 3. Docker Base Images
**Decision:** Update Dockerfile to use .NET 10 images.

**Changes:**
- Build stage: `mcr.microsoft.com/dotnet/sdk:10.0` (was `8.0`)
- Runtime stage: `mcr.microsoft.com/dotnet/aspnet:10.0` (was `8.0`)

**Rationale:** Match runtime to target framework.

### 4. GitHub Actions .NET Version
**Decision:** Update `DOTNET_VERSION` to `'10.0.x'` in both `ci.yml` and `cd.yml`.

**Rationale:** Ensure CI/CD uses correct SDK version.

### 5. Breaking Changes to Monitor
**Decision:** Document known ASP.NET Core 8→10 breaking changes.

**Key areas:**
- Minimal API changes (route handlers, parameter binding)
- Authentication/Authorization middleware changes
- OpenAPI/Swagger generation changes
- JSON serialization defaults (System.Text.Json)
- Dependency injection changes
- Health checks API changes
- Rate limiting middleware changes

**Mitigation:** Run full test suite after upgrade; review Microsoft's breaking changes documentation.

## Risks / Trade-offs

| Risk | Mitigation |
|------|------------|
| Package incompatibility (some packages may not have .NET 10 versions yet) | Check NuGet for pre-release versions; pin to latest compatible if needed |
| Breaking changes in ASP.NET Core 10 | Review Microsoft breaking changes guide; run full test suite; test manually |
| Docker image size increase | .NET 10 images may be larger; monitor and optimize if needed |
| CI/CD pipeline failures | Test locally with .NET 10 SDK before pushing; use GitHub Actions matrix if needed |
| Performance regressions | Benchmark key endpoints before/after upgrade |
| EF Core migrations compatibility | Regenerate migrations if model changes; test migration up/down |

## Migration Plan

1. **Pre-requisites:** Install .NET 10 SDK locally
2. **Update standards:** Modify `tech-stack.md` (done)
3. **Update project files:** Change `TargetFramework` to `net10.0` (done)
4. **Update NuGet packages:** Upgrade all packages to .NET 10 compatible versions (done - versions need verification)
5. **Update GitHub Actions:** Change `DOTNET_VERSION` to `'10.0.x'` (done)
6. **Update Dockerfile:** Change base image tags to 10.0 (done)
7. **Run local build:** `dotnet build` - verify compilation
8. **Run tests:** `dotnet test` - verify all unit tests pass
9. **Integration tests:** Ensure Docker running; run integration tests
10. **Push to CI:** Verify GitHub Actions pass
11. **Deploy:** CD pipeline deploys to VPS

**Rollback strategy:**
- Revert all `.csproj` files to `net8.0`
- Revert NuGet package versions
- Revert GitHub Actions to `8.0.x`
- Revert Dockerfile to 8.0 images
- Revert `tech-stack.md`
- Push revert commit

## Open Questions

1. **AutoMapper 14.0.0** - Verify licensing changes (Lucky Penny Software requires license for production)
2. **EF Core 10** - Confirm Npgsql 10.0.0 is released and stable
3. **Testcontainers 4.x** - Verify PostgreSQL module compatibility with .NET 10
4. **Hellang.Middleware.ProblemDetails 7.x** - Confirm .NET 10 support and any API changes
5. **AutoMapper license** - Project uses AutoMapper 16.2.0 (v14+ requires paid license for production); need to evaluate alternatives (Mapster, manual mapping) or acquire license