# Spec Delta

## Purpose

Defines the Clean Architecture layer boundaries, project organization, and dependency rules that all code in the repository must follow.

## ADDED Requirements

### Requirement: Solution has four distinct layers
The solution SHALL contain exactly four project layers under `src/`: Core/Domain, Core/Application, Infrastructure/Persistence, and Presentation/WebAPI.

#### Scenario: Solution structure validation
- **WHEN** the solution is built
- **THEN** all four layer projects exist and are referenced correctly

### Requirement: Domain layer has no external dependencies
The Core/Domain project SHALL NOT reference any NuGet packages except for language/runtime fundamentals and the Core/Application project.

#### Scenario: Domain layer dependency check
- **WHEN** analyzing project references
- **THEN** Core/Domain has zero references to Infrastructure, Presentation, or third-party libraries (EF Core, ASP.NET Core, Autofac, AutoMapper, etc.)

### Requirement: Application layer references only Domain
The Core/Application project SHALL reference only Core/Domain and required CQRS/mediation abstractions (MediatR). It SHALL NOT reference Infrastructure, Presentation, or concrete implementations.

#### Scenario: Application layer dependency check
- **WHEN** analyzing project references
- **THEN** Core/Application references only Core/Domain and MediatR abstractions

### Requirement: Infrastructure layer references Application and Domain
The Infrastructure/Persistence project SHALL reference Core/Application and Core/Domain to implement repositories and DbContext. It SHALL NOT reference Presentation/WebAPI.

#### Scenario: Infrastructure layer dependency check
- **WHEN** analyzing project references
- **THEN** Infrastructure/Persistence references Core/Application and Core/Domain but not Presentation/WebAPI

### Requirement: Presentation layer references Application and Infrastructure
The Presentation/WebAPI project SHALL reference Core/Application (for commands/queries) and Infrastructure/Persistence (for DI registration). It SHALL NOT reference Core/Domain directly.

#### Scenario: Presentation layer dependency check
- **WHEN** analyzing project references
- **THEN** Presentation/WebAPI references Core/Application and Infrastructure/Persistence but not Core/Domain

### Requirement: Test projects follow layer isolation
Unit test projects SHALL test only one layer at a time. Integration test projects MAY reference multiple layers but only through public APIs.

#### Scenario: Unit test isolation
- **WHEN** running unit tests
- **THEN** Domain unit tests reference only Core/Domain, Application unit tests reference only Core/Application and Core/Domain

### Requirement: Solution file at repository root
A single `.sln` file SHALL exist at the repository root and include all src/ and test/ projects.

#### Scenario: Solution file existence
- **WHEN** listing repository root
- **THEN** a `.sln` file is present and `dotnet build` succeeds