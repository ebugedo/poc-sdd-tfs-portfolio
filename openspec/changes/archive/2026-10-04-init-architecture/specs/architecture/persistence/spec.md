# Spec Delta

## Purpose

Defines EF Core DbContext configuration, repository patterns, migration strategy, and PostgreSQL database setup for the persistence layer.

## ADDED Requirements

### Requirement: DbContext resides in Infrastructure/Persistence
The Infrastructure/Persistence project SHALL contain a single `ApplicationDbContext` (or similarly named) class inheriting from `DbContext` that defines all `DbSet<T>` properties for Domain entities.

#### Scenario: DbContext existence
- **WHEN** inspecting Infrastructure/Persistence project
- **THEN** a DbContext class exists with DbSet properties for all entities

### Requirement: Entity configurations use Fluent API in separate classes
Each Domain entity SHALL have its EF Core configuration in a separate `IEntityTypeConfiguration<T>` class within Infrastructure/Persistence. Data Annotations SHALL NOT be used on Domain entities.

#### Scenario: Fluent API configuration separation
- **WHEN** inspecting entity configurations
- **THEN** each entity has a dedicated `EntityTypeConfiguration<T>` class with Fluent API mappings

### Requirement: Primary keys use Guid or long identity
All entity primary keys SHALL use either `Guid` (with `Guid.NewGuid()` or database-generated) or `long` (identity column). Composite keys SHALL be avoided unless required by domain.

#### Scenario: Primary key convention
- **WHEN** creating a new entity
- **THEN** its primary key is Guid or long following the established convention

### Requirement: Soft delete via global query filter
Entities requiring soft delete SHALL implement a `IsDeleted` property (or similar) and the DbContext SHALL apply a global query filter to exclude soft-deleted records by default.

#### Scenario: Soft delete filter applied
- **WHEN** querying a soft-deletable entity
- **THEN** deleted records are excluded unless explicitly included via `.IgnoreQueryFilters()`

### Requirement: Repository pattern with generic interface
The Infrastructure/Persistence layer SHALL provide a generic `IRepository<T>` interface and a base `Repository<T>` implementation for common CRUD operations. Specialized repositories MAY extend this base.

#### Scenario: Generic repository usage
- **WHEN** an Application handler needs data access
- **THEN** it uses `IRepository<Entity>` injected via DI

### Requirement: Unit of Work pattern for transaction management
The Infrastructure/Persistence layer SHALL provide an `IUnitOfWork` interface (implemented by DbContext) with `SaveChangesAsync()` for explicit transaction boundaries.

#### Scenario: Unit of Work in command handlers
- **WHEN** a command handler completes business logic
- **THEN** it calls `await _unitOfWork.SaveChangesAsync()` to persist changes

### Requirement: Migrations generated via dotnet ef CLI
Database migrations SHALL be created using `dotnet ef migrations add <Name>` from the Infrastructure/Persistence project. Existing migrations in main branch SHALL NEVER be modified.

#### Scenario: Migration creation workflow
- **WHEN** a schema change is needed
- **THEN** a new migration is generated and committed without modifying existing ones

### Requirement: PostgreSQL provider with Npgsql
The DbContext SHALL use `Npgsql.EntityFrameworkCore.PostgreSQL` as the database provider. Connection string SHALL be configured via environment variables or User Secrets.

#### Scenario: PostgreSQL connection
- **WHEN** the application connects to the database
- **THEN** it uses Npgsql provider with a connection string from configuration

### Requirement: Database seeding for reference data
The Infrastructure/Persistence layer SHALL support seeding reference/lookup data via `HasData()` in entity configurations or a dedicated seeder service.

#### Scenario: Reference data available on deploy
- **WHEN** the database is migrated
- **THEN** reference data (e.g., portfolio types, currencies) is present