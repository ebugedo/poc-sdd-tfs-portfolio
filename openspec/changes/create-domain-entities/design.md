# Design

## Context

See proposal.md for motivation. Current state: the Domain project contains only base infrastructure classes (Entity, AggregateRoot, ValueObject, DomainEvent, DomainException). No business entities exist yet. The Infrastructure Persistence project has an empty ApplicationDbContext with no DbSets. All code must follow English naming conventions per coding-standards.md.

## Goals / Non-Goals

**Goals:**
- Implement all domain aggregates (Client, Sector, Technology, Project) and supporting entities/value objects
- Follow DDD principles: encapsulation, invariants in constructors/methods, domain events for state changes
- Use English identifiers for all code (classes, properties, methods, enums)
- Configure EF Core with Fluent API in Infrastructure (no Data Annotations in Domain)
- Generate initial migration for all entities

**Non-Goals:**
- Application layer (commands, queries, handlers) - separate change
- API endpoints - separate change
- Repository implementations - separate change
- Unit/integration tests - separate change (though testability is a design consideration)

## Decisions

### 1. Aggregate Root Design
**Decision:** All four main entities (Client, Sector, Technology, Project) are Aggregate Roots inheriting from `AggregateRoot<Guid>`.

**Rationale:** Each has independent lifecycle, identity, and business invariants. They are consistency boundaries. Project references Client and Sector by ID only (not navigation properties in Domain).

**Alternatives considered:** Making Sector/Technology entities within Project aggregate - rejected because they are shared across projects and have independent lifecycles.

### 2. Many-to-Many: Project ↔ Technology
**Decision:** Explicit join entity `ProjectTechnology` with payload (AssignedAt, Notes) instead of EF Core's implicit many-to-many.

**Rationale:** Business requirement to track when a technology was assigned and optional notes. Explicit entity enables domain events (`TechnologyAddedToProject`, `TechnologyRemovedFromProject`) and future extensibility.

**Alternatives considered:** EF Core implicit many-to-many - rejected due to payload requirement and domain event needs.

### 3. Email Value Object
**Decision:** `Email` value object wrapping string with validation and normalization (lowercase).

**Rationale:** Encapsulates email validation logic, prevents primitive obsession, ensures consistent comparison/storage. Used by Client aggregate.

**Alternatives considered:** Plain string with validation in Client - rejected as it leaks validation logic and complicates testing.

### 4. Enum Handling
**Decision:** `ProjectStatus` and `TechnologyCategory` as C# enums with EF Core value converters (string in DB).

**Rationale:** Type-safe in Domain, human-readable in database, easy to query. String conversion avoids ordinal mapping issues.

**Alternatives considered:** Int enums in DB - rejected due to maintenance risk when adding values. Separate lookup tables - rejected as overkill for stable enums.

### 5. Soft Delete
**Decision:** `IsActive` boolean on all aggregates (no query filters yet).

**Rationale:** Simple, explicit, enables audit trail. Query filters can be added later when read models are defined.

**Alternatives considered:** EF Core query filters with `IsDeleted` - deferred until Application layer read models exist.

### 6. Domain Event Emission
**Decision:** Domain events raised in aggregate methods (not constructors), collected via `AddDomainEvent`, cleared after dispatch.

**Rationale:** Follows existing `AggregateRoot` base class pattern. Events represent *completed* state changes.

**Alternatives considered:** Events in constructors - rejected as it complicates testing and object creation.

### 7. EF Core Configuration Location
**Decision:** All `IEntityTypeConfiguration<T>` classes in `Infrastructure/Persistence/Configurations/` folder.

**Rationale:** Follows database-and-migrations.md standard: "Entity configurations MUST be isolated using Fluent API in IEntityTypeConfiguration<T> classes within Infrastructure. Do NOT use Data Annotations in Domain entities."

### 8. Unique Constraints
**Decision:** Unique indexes on Email (Client), Name (Sector, Technology), composite Name+ClientId (Project), composite ProjectId+TechnologyId (ProjectTechnology).

**Rationale:** Enforces business rules at database level. Composite index for Project ensures name uniqueness per client.

### 9. Currency Handling
**Decision:** `Currency` string property on Project (default "USD"), `Budget` as nullable decimal. No Money value object yet.

**Rationale:** Simple start, matches current requirements. Money VO can be introduced later if multi-currency operations needed.

**Alternatives considered:** Full Money VO with currency - deferred as YAGNI.

## Risks / Trade-offs

| Risk | Mitigation |
|------|------------|
| Circular references between aggregates (Project → Client/Sector) | Domain references only by ID; navigation properties only in EF config / Application layer DTOs |
| Technology category enum changes | Use string conversion in EF; add new values only at end; never reorder |
| Project status transition logic complexity | Encapsulate in `ChangeStatus` method with explicit validation; unit test all transitions |
| Missing aggregate for ProjectTechnology | It's an Entity, not Aggregate Root - managed exclusively through Project aggregate |
| Duplicate domain event emission | Events only in command methods; clear after dispatch; idempotent handlers in future |
| Migration complexity with composite unique indexes | Generate initial migration after all entities configured; review SQL before apply |
| English vs Spanish naming inconsistency | All new code uses English; existing base classes already English |