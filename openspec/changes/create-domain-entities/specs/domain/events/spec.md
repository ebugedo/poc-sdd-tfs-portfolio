# Spec Delta

## Purpose

Defines domain events emitted by aggregates to enable eventual consistency and cross-boundary communication.

## ADDED Requirements

### Requirement: Client domain events
The system SHALL emit the following events for Client aggregate lifecycle changes.

#### Scenario: ClientCreated event emitted
- **WHEN** a new Client is successfully created
- **THEN** a ClientCreated domain event is emitted with ClientId, Name, Email, and OccurredOn timestamp

#### Scenario: ClientUpdated event emitted
- **WHEN** an existing Client is successfully updated
- **THEN** a ClientUpdated domain event is emitted with ClientId, updated fields, and OccurredOn timestamp

#### Scenario: ClientDeactivated event emitted
- **WHEN** a Client is successfully deactivated
- **THEN** a ClientDeactivated domain event is emitted with ClientId and OccurredOn timestamp

### Requirement: Sector domain events
The system SHALL emit the following events for Sector aggregate lifecycle changes.

#### Scenario: SectorCreated event emitted
- **WHEN** a new Sector is successfully created
- **THEN** a SectorCreated domain event is emitted with SectorId, Name, and OccurredOn timestamp

#### Scenario: SectorUpdated event emitted
- **WHEN** an existing Sector is successfully updated
- **THEN** a SectorUpdated domain event is emitted with SectorId, updated fields, and OccurredOn timestamp

### Requirement: Technology domain events
The system SHALL emit the following events for Technology aggregate lifecycle changes.

#### Scenario: TechnologyCreated event emitted
- **WHEN** a new Technology is successfully created
- **THEN** a TechnologyCreated domain event is emitted with TechnologyId, Name, Category, and OccurredOn timestamp

#### Scenario: TechnologyUpdated event emitted
- **WHEN** an existing Technology is successfully updated
- **THEN** a TechnologyUpdated domain event is emitted with TechnologyId, updated fields, and OccurredOn timestamp

### Requirement: Project domain events
The system SHALL emit the following events for Project aggregate lifecycle changes.

#### Scenario: ProjectCreated event emitted
- **WHEN** a new Project is successfully created
- **THEN** a ProjectCreated domain event is emitted with ProjectId, Name, ClientId, SectorId, and OccurredOn timestamp

#### Scenario: ProjectUpdated event emitted
- **WHEN** an existing Project is successfully updated
- **THEN** a ProjectUpdated domain event is emitted with ProjectId, updated fields, and OccurredOn timestamp

#### Scenario: ProjectStatusChanged event emitted
- **WHEN** a Project status is successfully changed
- **THEN** a ProjectStatusChanged domain event is emitted with ProjectId, OldStatus, NewStatus, and OccurredOn timestamp

#### Scenario: TechnologyAddedToProject event emitted
- **WHEN** a Technology is successfully associated with a Project
- **THEN** a TechnologyAddedToProject domain event is emitted with ProjectId, TechnologyId, and OccurredOn timestamp

#### Scenario: TechnologyRemovedFromProject event emitted
- **WHEN** a Technology is successfully removed from a Project
- **THEN** a TechnologyRemovedFromProject domain event is emitted with ProjectId, TechnologyId, and OccurredOn timestamp

### Requirement: Domain event base properties
The system SHALL ensure all domain events have required base properties.

#### Scenario: All events have EventId and OccurredOn
- **WHEN** any domain event is created
- **THEN** it has a unique EventId (Guid) and OccurredOn (DateTime UTC) timestamp