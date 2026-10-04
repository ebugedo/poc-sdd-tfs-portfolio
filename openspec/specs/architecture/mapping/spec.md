# architecture/mapping Specification

## Purpose
Defines AutoMapper configuration, profiles, and conventions for cross-layer object mapping between Domain entities, Application DTOs, and API contracts.

## Requirements

### Requirement: AutoMapper is configured in Application layer
The Core/Application project SHALL contain AutoMapper `Profile` classes that define mapping rules between Domain entities and Application DTOs.

#### Scenario: Application mapping profiles exist
- **WHEN** inspecting Core/Application project
- **THEN** one or more `Profile` classes exist mapping Domain ↔ DTO

### Requirement: Domain entities are not mapped directly in Presentation
The Presentation/WebAPI layer SHALL NOT reference Domain entities directly for mapping. All API contracts (request/response DTOs) SHALL be mapped through Application layer DTOs.

#### Scenario: No direct Domain-to-API mapping
- **WHEN** inspecting Presentation/WebAPI mappings
- **THEN** no mapping profiles reference Core/Domain types directly

### Requirement: Mapping profiles are registered via Autofac
AutoMapper profiles SHALL be registered through the Application layer's Autofac module using `builder.RegisterAssemblyTypes().AssignableTo<Profile>()`.

#### Scenario: Profiles auto-registered
- **WHEN** the Autofac container builds
- **THEN** all Profile classes in Core/Application are discovered and registered automatically

### Requirement: Bidirectional mapping for entity-DTO pairs
For each Domain entity that crosses layer boundaries, the Application layer SHALL provide bidirectional mapping (Entity → DTO and DTO → Entity).

#### Scenario: Bidirectional mapping availability
- **WHEN** a command creates an entity from a DTO
- **THEN** Mapper.Map<Entity>(dto) succeeds
- **WHEN** a query returns an entity as a DTO
- **THEN** Mapper.Map<Dto>(entity) succeeds

### Requirement: Mapping ignores internal/private members
AutoMapper configuration SHALL ignore private setters, internal constructors, and domain-only properties that should not cross layer boundaries.

#### Scenario: Internal domain properties not mapped
- **WHEN** mapping an entity with internal domain logic properties
- **THEN** those properties are not included in the DTO output

### Requirement: Mapping validation on startup
The application SHALL validate all mapping configurations at startup and fail fast if any mapping is incomplete or invalid.

#### Scenario: Startup mapping validation
- **WHEN** the application starts
- **THEN** `mapper.ConfigurationProvider.AssertConfigurationIsValid()` passes without errors

### Requirement: Value objects map to/from primitives or dedicated DTOs
Domain Value Objects SHALL map to simple types (string, int, Guid) or dedicated DTOs in the Application layer, never exposed as raw domain types in API contracts.

#### Scenario: Value object mapping
- **WHEN** a command accepts a Value Object as input
- **THEN** the API receives a primitive/DTO and the Application layer constructs the Value Object
