# Spec Delta

## MODIFIED Requirements

### Requirement: xUnit Version
The test projects SHALL use xUnit version compatible with .NET 10.

#### Scenario: xUnit version supports .NET 10
- **WHEN** inspecting `Portfolio.UnitTests.csproj` and `Portfolio.IntegrationTests.csproj`
- **THEN** `xunit` package reference SHALL be version 3.0.0 or higher (supports .NET 10)
- **THEN** `xunit.runner.visualstudio` package reference SHALL be version 3.0.0 or higher

### Requirement: Microsoft.NET.Test.Sdk Version
The test projects SHALL use Microsoft.NET.Test.Sdk compatible with .NET 10.

#### Scenario: Test SDK version supports .NET 10
- **WHEN** inspecting `Portfolio.UnitTests.csproj` and `Portfolio.IntegrationTests.csproj`
- **THEN** `Microsoft.NET.Test.Sdk` package reference SHALL be version 17.14.0 or higher

### Requirement: Moq Version
The unit test project SHALL use Moq version compatible with .NET 10.

#### Scenario: Moq version supports .NET 10
- **WHEN** inspecting `Portfolio.UnitTests.csproj`
- **THEN** `Moq` package reference SHALL be version 4.20.0 or higher (supports .NET 10)

### Requirement: Bogus Version
The unit test project SHALL use Bogus version compatible with .NET 10.

#### Scenario: Bogus version supports .NET 10
- **WHEN** inspecting `Portfolio.UnitTests.csproj`
- **THEN** `Bogus` package reference SHALL be version 35.0.0 or higher (supports .NET 10)

### Requirement: AutoMapper Version
The unit test project SHALL use AutoMapper version compatible with .NET 10.

#### Scenario: AutoMapper version supports .NET 10
- **WHEN** inspecting `Portfolio.UnitTests.csproj` and `Portfolio.Application.csproj`
- **THEN** `AutoMapper` package reference SHALL be version 14.0.0 or higher (supports .NET 10)

### Requirement: FluentValidation Version
The unit test and application projects SHALL use FluentValidation version compatible with .NET 10.

#### Scenario: FluentValidation version supports .NET 10
- **WHEN** inspecting `Portfolio.UnitTests.csproj` and `Portfolio.Application.csproj`
- **THEN** `FluentValidation` and `FluentValidation.DependencyInjectionExtensions` package references SHALL be version 12.0.0 or higher (supports .NET 10)

### Requirement: MediatR Version
The application project SHALL use MediatR version compatible with .NET 10.

#### Scenario: MediatR version supports .NET 10
- **WHEN** inspecting `Portfolio.Application.csproj`
- **THEN** `MediatR` and `MediatR.Extensions.Microsoft.DependencyInjection` package references SHALL be version 13.0.0 or higher (supports .NET 10)

### Requirement: Testcontainers Version
The integration test project SHALL use Testcontainers version compatible with .NET 10.

#### Scenario: Testcontainers version supports .NET 10
- **WHEN** inspecting `Portfolio.IntegrationTests.csproj`
- **THEN** `Testcontainers.PostgreSql` package reference SHALL be version 4.0.0 or higher (supports .NET 10)

### Requirement: Microsoft.AspNetCore.Mvc.Testing Version
The integration test project SHALL use Mvc.Testing version compatible with .NET 10.

#### Scenario: Mvc.Testing version supports .NET 10
- **WHEN** inspecting `Portfolio.IntegrationTests.csproj`
- **THEN** `Microsoft.AspNetCore.Mvc.Testing` package reference SHALL be version 10.0.0 or higher