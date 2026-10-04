# Spec Delta

## MODIFIED Requirements

### Requirement: Autofac Compatibility
The system SHALL use Autofac version compatible with .NET 10.

#### Scenario: Autofac version supports .NET 10
- **WHEN** inspecting `Portfolio.Domain.csproj`, `Portfolio.Application.csproj`, `Portfolio.Infrastructure.Persistence.csproj`
- **THEN** `Autofac` package reference SHALL be version 9.0.0 or higher (supports .NET 10)

### Requirement: Autofac.Extensions.DependencyInjection Compatibility
The system SHALL use Autofac.Extensions.DependencyInjection version compatible with .NET 10.

#### Scenario: Autofac.Extensions.DependencyInjection version supports .NET 10
- **WHEN** inspecting `Portfolio.Presentation.WebAPI.csproj`
- **THEN** `Autofac.Extensions.DependencyInjection` package reference SHALL be version 10.0.0 or higher (supports .NET 10)