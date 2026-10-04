# Spec Delta

## MODIFIED Requirements

### Requirement: Entity Framework Core Version
The system SHALL use Entity Framework Core version compatible with .NET 10.

#### Scenario: EF Core version supports .NET 10
- **WHEN** inspecting `Portfolio.Infrastructure.Persistence.csproj`
- **THEN** `Microsoft.EntityFrameworkCore` and `Microsoft.EntityFrameworkCore.Design` package references SHALL be version 10.0.0 or higher

### Requirement: Npgsql PostgreSQL Provider Version
The system SHALL use Npgsql Entity Framework Core PostgreSQL provider version compatible with .NET 10.

#### Scenario: Npgsql provider version supports .NET 10
- **WHEN** inspecting `Portfolio.Infrastructure.Persistence.csproj`
- **THEN** `Npgsql.EntityFrameworkCore.PostgreSQL` package reference SHALL be version 10.0.0 or higher

### Requirement: EF Core Tools Version
The system SHALL use EF Core Tools version compatible with .NET 10.

#### Scenario: EF Core Tools version supports .NET 10
- **WHEN** inspecting `Portfolio.Infrastructure.Persistence.csproj`
- **THEN** `Microsoft.EntityFrameworkCore.Tools` package reference SHALL be version 10.0.0 or higher

### Requirement: Microsoft.Extensions.Configuration Packages
The system SHALL use Microsoft.Extensions.Configuration packages compatible with .NET 10.

#### Scenario: Configuration packages version supports .NET 10
- **WHEN** inspecting `Portfolio.Infrastructure.Persistence.csproj`
- **THEN** `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.Configuration.Json`, `Microsoft.Extensions.Configuration.EnvironmentVariables` package references SHALL be version 10.0.0 or higher