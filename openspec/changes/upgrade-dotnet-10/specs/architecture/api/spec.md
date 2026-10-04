# Spec Delta

## MODIFIED Requirements

### Requirement: ASP.NET Core Runtime
The system SHALL run on ASP.NET Core 10 runtime.

#### Scenario: WebAPI targets .NET 10
- **WHEN** inspecting `Portfolio.Presentation.WebAPI.csproj`
- **THEN** `TargetFramework` SHALL be `net10.0` and project SDK SHALL be `Microsoft.NET.Sdk.Web`

### Requirement: JWT Bearer Authentication
The system SHALL use JWT Bearer authentication compatible with .NET 10.

#### Scenario: JWT Bearer package supports .NET 10
- **WHEN** inspecting `Portfolio.Presentation.WebAPI.csproj`
- **THEN** `Microsoft.AspNetCore.Authentication.JwtBearer` package reference SHALL be version 10.0.0 or higher

### Requirement: OpenAPI/Swagger
The system SHALL use Swashbuckle.AspNetCore compatible with .NET 10.

#### Scenario: Swashbuckle version supports .NET 10
- **WHEN** inspecting `Portfolio.Presentation.WebAPI.csproj`
- **THEN** `Swashbuckle.AspNetCore` package reference SHALL be version 8.0.0 or higher (supports .NET 10)

### Requirement: ProblemDetails Middleware
The system SHALL use Hellang.Middleware.ProblemDetails compatible with .NET 10.

#### Scenario: ProblemDetails version supports .NET 10
- **WHEN** inspecting `Portfolio.Presentation.WebAPI.csproj`
- **THEN** `Hellang.Middleware.ProblemDetails` package reference SHALL be version 7.0.0 or higher (supports .NET 10)

### Requirement: Serilog ASP.NET Core
The system SHALL use Serilog.AspNetCore compatible with .NET 10.

#### Scenario: Serilog version supports .NET 10
- **WHEN** inspecting `Portfolio.Presentation.WebAPI.csproj`
- **THEN** `Serilog.AspNetCore` package reference SHALL be version 9.0.0 or higher (supports .NET 10)

### Requirement: Microsoft.AspNetCore.OpenApi
The system SHALL use Microsoft.AspNetCore.OpenApi compatible with .NET 10.

#### Scenario: OpenAPI package version supports .NET 10
- **WHEN** inspecting `Portfolio.Presentation.WebAPI.csproj`
- **THEN** `Microsoft.AspNetCore.OpenApi` package reference SHALL be version 10.0.0 or higher