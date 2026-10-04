# Spec Delta

## MODIFIED Requirements

### Requirement: Entity Framework Core entity configurations
The system SHALL provide EF Core configurations for all domain entities using Fluent API.

#### Scenario: Client entity configuration exists
- **WHEN** the Infrastructure Persistence project is inspected
- **THEN** it contains ClientConfiguration implementing IEntityTypeConfiguration<Client> with Id as primary key, Email as unique index, Name as required, and proper property mappings

#### Scenario: Sector entity configuration exists
- **WHEN** the Infrastructure Persistence project is inspected
- **THEN** it contains SectorConfiguration implementing IEntityTypeConfiguration<Sector> with Id as primary key, Name as unique index, and proper property mappings

#### Scenario: Technology entity configuration exists
- **WHEN** the Infrastructure Persistence project is inspected
- **THEN** it contains TechnologyConfiguration implementing IEntityTypeConfiguration<Technology> with Id as primary key, Name as unique index, Category as enum conversion, and proper property mappings

#### Scenario: Project entity configuration exists
- **WHEN** the Infrastructure Persistence project is inspected
- **THEN** it contains ProjectConfiguration implementing IEntityTypeConfiguration<Project> with Id as primary key, Name+ClientId as unique composite index, foreign keys to Client and Sector, and proper property mappings

#### Scenario: ProjectTechnology entity configuration exists
- **WHEN** the Infrastructure Persistence project is inspected
- **THEN** it contains ProjectTechnologyConfiguration implementing IEntityTypeConfiguration<ProjectTechnology> with Id as primary key, composite unique index on ProjectId+TechnologyId, foreign keys to Project and Technology, and proper property mappings

#### Scenario: Email value object configuration exists
- **WHEN** the Infrastructure Persistence project is inspected
- **THEN** it contains EmailConfiguration for value object conversion with proper column mapping

### Requirement: DbContext includes DbSets for all entities
The system SHALL expose DbSet properties for all domain entities in ApplicationDbContext.

#### Scenario: ApplicationDbContext has all DbSets
- **WHEN** ApplicationDbContext is inspected
- **THEN** it contains DbSet<Client>, DbSet<Sector>, DbSet<Technology>, DbSet<Project>, DbSet<ProjectTechnology>