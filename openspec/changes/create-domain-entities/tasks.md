# Tasks

## 1. Domain Entities - Value Objects

- [x] 1.1 Create `Email` value object in `src/Core/Domain/ValueObjects/Email.cs` with validation, normalization (lowercase), and equality - verify by building and running existing Domain tests
- [x] 1.2 Add unit tests for `Email` value object in `test/UnitTests/Domain/EmailTests.cs` covering valid/invalid formats, equality, normalization - verify tests pass with `dotnet test`

## 2. Domain Entities - Aggregates

- [x] 2.1 Create `Client` aggregate in `src/Core/Domain/Entities/Client.cs` with Id, Name, Email, Phone, Address, IsActive, CreatedAt, UpdatedAt, and domain events (ClientCreated, ClientUpdated, ClientDeactivated) - verify by building
- [x] 2.2 Create `Sector` aggregate in `src/Core/Domain/Entities/Sector.cs` with Id, Name, Description, IsActive, CreatedAt, and domain events (SectorCreated, SectorUpdated) - verify by building
- [x] 2.3 Create `Technology` aggregate in `src/Core/Domain/Entities/Technology.cs` with Id, Name, Description, Category (enum), IsActive, CreatedAt, and domain events (TechnologyCreated, TechnologyUpdated) - verify by building
- [x] 2.4 Create `TechnologyCategory` enum in `src/Core/Domain/Entities/TechnologyCategory.cs` with values: Frontend, Backend, Database, DevOps, Mobile, Other - verify by building
- [x] 2.5 Create `ProjectStatus` enum in `src/Core/Domain/Entities/ProjectStatus.cs` with values: Planning, InProgress, Completed, Cancelled, OnHold - verify by building
- [x] 2.6 Create `Project` aggregate in `src/Core/Domain/Entities/Project.cs` with Id, Name, Description, ClientId, SectorId, StartDate, EndDate, Status, Budget, Currency, CreatedAt, UpdatedAt, Technologies collection, and domain events (ProjectCreated, ProjectUpdated, ProjectStatusChanged, TechnologyAddedToProject, TechnologyRemovedFromProject) - verify by building
- [x] 2.7 Create `ProjectTechnology` entity in `src/Core/Domain/Entities/ProjectTechnology.cs` with Id, ProjectId, TechnologyId, AssignedAt, Notes - verify by building

## 3. Domain Entities - Domain Events

- [x] 3.1 Create all domain event classes in `src/Core/Domain/Events/` (ClientCreated, ClientUpdated, ClientDeactivated, SectorCreated, SectorUpdated, TechnologyCreated, TechnologyUpdated, ProjectCreated, ProjectUpdated, ProjectStatusChanged, TechnologyAddedToProject, TechnologyRemovedFromProject) - verify by building

## 4. Domain Unit Tests

- [x] 4.1 Add unit tests for `Client` aggregate in `test/UnitTests/Domain/ClientTests.cs` covering creation, update, deactivation, validation rules, equality, domain events - verify tests pass with `dotnet test`
- [x] 4.2 Add unit tests for `Sector` aggregate in `test/UnitTests/Domain/SectorTests.cs` covering creation, update, validation rules, equality, domain events - verify tests pass with `dotnet test`
- [x] 4.3 Add unit tests for `Technology` aggregate in `test/UnitTests/Domain/TechnologyTests.cs` covering creation, update, validation rules, category enum, equality, domain events - verify tests pass with `dotnet test`
- [x] 4.4 Add unit tests for `Project` aggregate in `test/UnitTests/Domain/ProjectTests.cs` covering creation, update, status transitions, technology assignment/removal, validation rules, equality, domain events - verify tests pass with `dotnet test`
- [x] 4.5 Add unit tests for `ProjectTechnology` entity in `test/UnitTests/Domain/ProjectTechnologyTests.cs` covering creation, equality - verify tests pass with `dotnet test`

## 5. Infrastructure - EF Core Configurations

- [x] 5.1 Create `ClientConfiguration` in `src/Infrastructure/Persistence/Configurations/ClientConfiguration.cs` implementing `IEntityTypeConfiguration<Client>` with Id PK, Email unique index, Name required, property mappings - verify by building
- [x] 5.2 Create `SectorConfiguration` in `src/Infrastructure/Persistence/Configurations/SectorConfiguration.cs` implementing `IEntityTypeConfiguration<Sector>` with Id PK, Name unique index, property mappings - verify by building
- [x] 5.3 Create `TechnologyConfiguration` in `src/Infrastructure/Persistence/Configurations/TechnologyConfiguration.cs` implementing `IEntityTypeConfiguration<Technology>` with Id PK, Name unique index, Category enum string conversion, property mappings - verify by building
- [x] 5.4 Create `ProjectConfiguration` in `src/Infrastructure/Persistence/Configurations/ProjectConfiguration.cs` implementing `IEntityTypeConfiguration<Project>` with Id PK, composite unique index on Name+ClientId, FK to Client, FK to Sector, property mappings - verify by building
- [x] 5.5 Create `ProjectTechnologyConfiguration` in `src/Infrastructure/Persistence/Configurations/ProjectTechnologyConfiguration.cs` implementing `IEntityTypeConfiguration<ProjectTechnology>` with Id PK, composite unique index on ProjectId+TechnologyId, FK to Project, FK to Technology, property mappings - verify by building
- [x] 5.6 Update `ApplicationDbContext` in `src/Infrastructure/Persistence/ApplicationDbContext.cs` to add DbSet<Client>, DbSet<Sector>, DbSet<Technology>, DbSet<Project>, DbSet<ProjectTechnology> - verify by building

## 6. Database Migration

- [x] 6.1 Generate initial migration: `dotnet ef migrations add InitialDomainEntities --project src/Infrastructure/Persistence --startup-project src/Presentation/WebAPI --output-dir Migrations` - verify migration files created
- [x] 6.2 Review generated migration SQL for correctness (tables, indexes, FKs, enums as strings) - verify by inspecting migration file

## 7. Verification

- [x] 7.1 Run full build: `dotnet build` - verify no compilation errors
- [x] 7.2 Run all unit tests: `dotnet test` - verify all Domain tests pass
- [~] 7.3 Apply migration to local database: `dotnet ef database update --project src/Infrastructure/Persistence --startup-project src/Presentation/WebAPI` - verify tables created in PostgreSQL (requires running PostgreSQL instance)

## 8. Standards Compliance - .NET 10, Newtonsoft.Json, .slnx

- [x] 8.1 Update all project files (6 projects) TargetFramework from `net8.0` to `net10.0` - verify `dotnet build`
- [x] 8.2 Add `Newtonsoft.Json` package reference to all projects requiring JSON serialization (WebAPI, Application, Domain) - verify package installed
- [x] 8.3 Replace any `System.Text.Json` usage with `Newtonsoft.Json` in API, Application, and Domain layers - verify no System.Text.Json references in application code
- [x] 8.4 Create `Portfolio.slnx` (XML-based solution file) at repository root with all 6 projects - verify `dotnet build` works with slnx
- [x] 8.5 Remove any legacy `.sln` file if present - verify only `.slnx` exists
- [x] 8.6 Update GitHub Actions CI/CD workflows to use `DOTNET_VERSION: '10.0.x'` - verify workflow files updated
- [x] 8.7 Update Dockerfile to use .NET 10 base images (`mcr.microsoft.com/dotnet/sdk:10.0`, `mcr.microsoft.com/dotnet/aspnet:10.0`) - verify Docker build
- [x] 8.8 Update NuGet packages to .NET 10 compatible versions (EF Core 10, Npgsql 10, Autofac 9+, AutoMapper 14+, FluentValidation 12+, MediatR 12+, xUnit 2.9+, Test.Sdk 17.14+, Moq 4.20+, Bogus 35+, Testcontainers 4+) - verify `dotnet build` and `dotnet test`
- [x] 8.9 Update `openspec/specs/standars/tech-stack.md` to reflect .NET 10, Newtonsoft.Json, .slnx standards - verify file updated
- [x] 8.10 Run full verification: `dotnet build --no-incremental -warnaserror:NU1603,NU1903` and `dotnet test` (unit tests) - verify all pass