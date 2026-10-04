# Proposal

## Why

The project currently targets .NET 8 which reaches end-of-support in November 2024. Upgrading to .NET 10 (LTS, supported until November 2027) ensures long-term support, security updates, and access to performance improvements (e.g., improved JIT, AOT compilation enhancements, better memory management).

## What Changes

- Update `tech-stack.md` standard: Runtime & Framework from ".NET (ASP.NET Core Web API)" to ".NET 10 (ASP.NET Core Web API)"
- Update all 6 project files (4 src + 2 test) TargetFramework from `net8.0` to `net10.0`
- Update GitHub Actions CI/CD workflows (`ci.yml`, `cd.yml`) DOTNET_VERSION from `8.0.x` to `10.0.x`
- Update NuGet package references to .NET 10 compatible versions (EF Core, Npgsql, etc.) when available

## Capabilities

### New Capabilities
None - this is a framework upgrade with no new behavioral capabilities.

### Modified Capabilities
- `architecture/solution-structure`: Target framework version requirement updated
- `architecture/dependency-injection`: Autofac version may need update for .NET 10 compatibility
- `architecture/persistence`: EF Core and Npgsql provider versions must be upgraded to .NET 10 compatible versions
- `architecture/api`: ASP.NET Core 10 runtime changes may affect middleware, authentication, OpenAPI
- `architecture/deployment-ci-cd`: Docker base images updated to .NET 10 runtime/sdk images
- `architecture/testing`: xUnit, Moq, Bogus, Testcontainers versions updated for .NET 10

## Impact

- All source code projects (`src/Core/Domain`, `src/Core/Application`, `src/Infrastructure/Persistence`, `src/Presentation/WebAPI`)
- All test projects (`test/UnitTests`, `test/IntegrationTests`)
- CI/CD pipelines (GitHub Actions workflows)
- Docker images (base image tags in Dockerfile)
- NuGet package ecosystem - requires compatible versions for all dependencies
- Breaking changes possible from ASP.NET Core 8 → 10 (minimal APIs, routing, authentication, OpenAPI)