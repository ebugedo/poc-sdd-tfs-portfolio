# Tasks

## 1. Standards & Configuration

- [x] 1.1 Update `openspec/specs/standars/tech-stack.md` - change Runtime & Framework to ".NET 10 (ASP.NET Core Web API)" - verify file updated
- [x] 1.2 Remove all `[cite: ...]` markers from all standards files in `openspec/specs/standars/` - verify no cite markers remain
- [x] 1.3 Update `openspec/specs/standars/architecture.md` - remove cite markers - verify file updated

## 2. Project Files - Target Framework

- [x] 2.1 Update `src/Core/Domain/Portfolio.Domain.csproj` TargetFramework to `net10.0` - verify build
- [x] 2.2 Update `src/Core/Application/Portfolio.Application.csproj` TargetFramework to `net10.0` - verify build
- [x] 2.3 Update `src/Infrastructure/Persistence/Portfolio.Infrastructure.Persistence.csproj` TargetFramework to `net10.0` - verify build
- [x] 2.4 Update `src/Presentation/WebAPI/Portfolio.Presentation.WebAPI.csproj` TargetFramework to `net10.0` - verify build
- [x] 2.5 Update `test/UnitTests/Portfolio.UnitTests.csproj` TargetFramework to `net10.0` - verify build
- [x] 2.6 Update `test/IntegrationTests/Portfolio.IntegrationTests.csproj` TargetFramework to `net10.0` - verify build

## 3. NuGet Package Updates - Domain & Application

- [ ] 3.1 Update `Portfolio.Domain.csproj` - Autofac to 9.0.0+ (if needed) - verify `dotnet build`
- [ ] 3.2 Update `Portfolio.Application.csproj` - Autofac to 9.0.0+, AutoMapper to 14.0.0+, FluentValidation to 12.0.0+, MediatR to 13.0.0+ - verify `dotnet build`
- [ ] 3.3 Update `Portfolio.Application.csproj` - FluentValidation.DependencyInjectionExtensions to 12.0.0+, MediatR.Extensions.Microsoft.DependencyInjection to 13.0.0+ - verify `dotnet build`

## 4. NuGet Package Updates - Infrastructure

- [ ] 4.1 Update `Portfolio.Infrastructure.Persistence.csproj` - EF Core to 10.0.0+, Npgsql to 10.0.0+, EF Core Design/Tools to 10.0.0+ - verify `dotnet build`
- [ ] 4.2 Update `Portfolio.Infrastructure.Persistence.csproj` - Microsoft.Extensions.Configuration packages to 10.0.0+ - verify `dotnet build`
- [ ] 4.3 Verify EF Core migrations still work with new version - run `dotnet ef migrations list`

## 5. NuGet Package Updates - WebAPI

- [ ] 5.1 Update `Portfolio.Presentation.WebAPI.csproj` - JwtBearer to 10.0.0+, Swashbuckle to 8.0.0+, ProblemDetails to 7.0.0+, Serilog to 9.0.0+, OpenApi to 10.0.0+ - verify `dotnet build`
- [ ] 5.2 Update `Portfolio.Presentation.WebAPI.csproj` - Autofac.Extensions.DependencyInjection to 10.0.0+ - verify `dotnet build`

## 6. NuGet Package Updates - Tests

- [ ] 6.1 Update `Portfolio.UnitTests.csproj` - xUnit to 3.0.0+, xunit.runner.visualstudio to 3.0.0+, Test.Sdk to 17.14.0+, Moq to 4.20.0+, Bogus to 35.0.0+, AutoMapper to 14.0.0+, FluentValidation to 12.0.0+, MediatR to 13.0.0+ - verify `dotnet test`
- [ ] 6.2 Update `Portfolio.IntegrationTests.csproj` - Mvc.Testing to 10.0.0+, Testcontainers.PostgreSql to 4.0.0+, xUnit to 3.0.0+, Test.Sdk to 17.14.0+ - verify `dotnet test`

## 7. GitHub Actions CI/CD

- [x] 7.1 Update `.github/workflows/ci.yml` - DOTNET_VERSION to `'10.0.x'` - verify workflow file
- [x] 7.2 Update `.github/workflows/cd.yml` - DOTNET_VERSION to `'10.0.x'` - verify workflow file

## 8. Docker Configuration

- [ ] 8.1 Update `src/Presentation/WebAPI/Dockerfile` - build stage base image to `mcr.microsoft.com/dotnet/sdk:10.0` - verify Docker build
- [ ] 8.2 Update `src/Presentation/WebAPI/Dockerfile` - runtime stage base image to `mcr.microsoft.com/dotnet/aspnet:10.0` - verify Docker build

## 9. Verification & Build

- [x] 9.1 Run `dotnet build --no-incremental` - verify all projects compile with zero errors
- [x] 9.2 Run `dotnet test` (unit tests only) - verify all 97+ unit tests pass
- [ ] 9.3 Run integration tests (requires Docker) - verify integration tests pass
- [ ] 9.4 Run `dotnet ef migrations list` - verify migrations work with EF Core 10

## 10. Open Questions Resolution

- [ ] 10.1 AutoMapper license evaluation - check if Mapster or manual mapping needed (v14+ requires paid license)
- [ ] 10.2 Verify EF Core 10 + Npgsql 10.0.0 are released and stable
- [ ] 10.3 Verify Testcontainers 4.x PostgreSQL module works with .NET 10
- [ ] 10.4 Verify Hellang.Middleware.ProblemDetails 7.x supports .NET 10