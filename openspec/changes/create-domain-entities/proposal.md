# Proposal

## Why

The portfolio management system needs core domain entities to represent projects, clients, sectors, and technologies. These are the fundamental building blocks that will enable all future capabilities (project CRUD, client management, reporting, filtering by sector/technology, etc.). Currently, no domain entities exist beyond the base infrastructure classes (Entity, AggregateRoot, ValueObject, DomainEvent).

## What Changes

Create the following domain entities in `src/Core/Domain/`:

### 1. **Client** - Aggregate Root
- Properties: Id (Guid), Name (string), Email (string), Phone (string?), Address (string?), IsActive (bool), CreatedAt (DateTime), UpdatedAt (DateTime?)
- Business rules: Email must be unique, Name is required
- Domain events: `ClientCreated`, `ClientUpdated`, `ClientDeactivated`

### 2. **Sector** - Aggregate Root
- Properties: Id (Guid), Name (string), Description (string?), IsActive (bool), CreatedAt (DateTime)
- Business rules: Name must be unique
- Domain events: `SectorCreated`, `SectorUpdated`

### 3. **Technology** - Aggregate Root
- Properties: Id (Guid), Name (string), Description (string?), Category (enum: Frontend, Backend, Database, DevOps, Mobile, Other), IsActive (bool), CreatedAt (DateTime)
- Business rules: Name must be unique
- Domain events: `TechnologyCreated`, `TechnologyUpdated`

### 4. **Project** - Aggregate Root (Main)
- Properties: Id (Guid), Name (string), Description (string?), ClientId (Guid), SectorId (Guid), StartDate (DateTime), EndDate (DateTime?), Status (enum: Planning, InProgress, Completed, Cancelled, OnHold), Budget (decimal?), Currency (string, default "USD"), CreatedAt (DateTime), UpdatedAt (DateTime?)
- Navigation: Client (reference), Sector (reference), Technologies (collection)
- Business rules:
  - Name is required and unique per client
  - EndDate must be >= StartDate if provided
  - Must have exactly one Sector
  - Can have multiple Technologies (many-to-many)
- Domain events: `ProjectCreated`, `ProjectUpdated`, `ProjectStatusChanged`, `TechnologyAddedToProject`, `TechnologyRemovedFromProject`

### 5. **ProjectTechnology** - Entity (Join table with payload)
- Properties: Id (Guid), ProjectId (Guid), TechnologyId (Guid), AssignedAt (DateTime), Notes (string?)
- Represents the many-to-many relationship with additional metadata

### 6. **Value Objects**
- **Email** - Value object for email validation
- **Money** - Value object for budget/currency handling (optional, can start with decimal)

## Capabilities

### New Capabilities
- `domain/entities/client`: Client aggregate root with full lifecycle management
- `domain/entities/sector`: Sector aggregate root for project categorization
- `domain/entities/technology`: Technology aggregate root for skill/tech tracking
- `domain/entities/project`: Project aggregate root as the core domain entity
- `domain/value-objects/email`: Email validation value object
- `domain/events`: Domain events for all aggregates enabling eventual consistency

### Modified Capabilities
- `architecture/solution-structure`: Domain layer now contains real business entities
- `architecture/persistence`: Will need EF Core configurations for new entities

## Impact

- All future application use cases (commands/queries) will depend on these aggregates
- Repository interfaces and implementations will be needed in Application/Infrastructure
- API endpoints will expose these entities via DTOs
- Database migration will create tables: Clients, Sectors, Technologies, Projects, ProjectTechnologies
- Establishes the core domain model for the portfolio management system
- Enables business logic encapsulation within aggregates (DDD principles)
- Domain events enable integration with other bounded contexts in the future
- All code identifiers follow English naming convention per coding-standards.md