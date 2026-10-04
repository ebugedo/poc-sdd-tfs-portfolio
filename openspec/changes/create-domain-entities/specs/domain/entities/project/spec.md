# Spec Delta

## Purpose

Manages the core project aggregate for the portfolio, including client association, sector assignment, technology tracking, and project lifecycle.

## ADDED Requirements

### Requirement: Project creation
The system SHALL allow creating a new project with a name, client, sector, start date, and optional description, end date, budget, and currency.

#### Scenario: Successful project creation
- **WHEN** a request to create a project with valid name, existing client, existing sector, start date, and optional fields is received
- **THEN** a new Project aggregate is created with Id, Name, Description, ClientId, SectorId, StartDate, EndDate, Status=Planning, Budget, Currency="USD", CreatedAt, and a ProjectCreated domain event is emitted

#### Scenario: Duplicate name per client rejected
- **WHEN** a request to create a project with a name that already exists for the same client
- **THEN** a DomainException with code "PROJECT_NAME_ALREADY_EXISTS_FOR_CLIENT" is thrown

#### Scenario: Empty name rejected
- **WHEN** a request to create a project with empty or whitespace name
- **THEN** a DomainException with code "NAME_REQUIRED" is thrown

#### Scenario: Non-existent client rejected
- **WHEN** a request to create a project with a ClientId that does not exist
- **THEN** a DomainException with code "CLIENT_NOT_FOUND" is thrown

#### Scenario: Non-existent sector rejected
- **WHEN** a request to create a project with a SectorId that does not exist
- **THEN** a DomainException with code "SECTOR_NOT_FOUND" is thrown

#### Scenario: End date before start date rejected
- **WHEN** a request to create a project with EndDate before StartDate
- **THEN** a DomainException with code "END_DATE_BEFORE_START_DATE" is thrown

### Requirement: Project update
The system SHALL allow updating project name, description, end date, budget, and currency while preserving name uniqueness per client.

#### Scenario: Successful project update
- **WHEN** a request to update a project with valid fields is received
- **THEN** the Project aggregate is updated with new values, UpdatedAt is set, and a ProjectUpdated domain event is emitted

#### Scenario: Update with duplicate name per client rejected
- **WHEN** a request to update a project name to one that already exists for the same client
- **THEN** a DomainException with code "PROJECT_NAME_ALREADY_EXISTS_FOR_CLIENT" is thrown

#### Scenario: Update with end date before start date rejected
- **WHEN** a request to update a project EndDate to before StartDate
- **THEN** a DomainException with code "END_DATE_BEFORE_START_DATE" is thrown

### Requirement: Project status transition
The system SHALL allow changing project status through valid transitions: Planning → InProgress → Completed|Cancelled|OnHold, with OnHold allowing return to InProgress.

#### Scenario: Valid status transition
- **WHEN** a request to change project status through a valid transition is received
- **THEN** the Project aggregate Status is updated, UpdatedAt is set, and a ProjectStatusChanged domain event is emitted with old and new status

#### Scenario: Invalid status transition rejected
- **WHEN** a request to change project status through an invalid transition (e.g., Completed → Planning)
- **THEN** a DomainException with code "INVALID_STATUS_TRANSITION" is thrown

### Requirement: Technology assignment to project
The system SHALL allow associating multiple technologies with a project (many-to-many).

#### Scenario: Add technology to project
- **WHEN** a request to add an existing technology to a project is received
- **THEN** a ProjectTechnology entity is created linking the project and technology, AssignedAt is set, and a TechnologyAddedToProject domain event is emitted

#### Scenario: Add duplicate technology rejected
- **WHEN** a request to add a technology that is already associated with the project
- **THEN** a DomainException with code "TECHNOLOGY_ALREADY_ASSIGNED" is thrown

#### Scenario: Add non-existent technology rejected
- **WHEN** a request to add a technology with TechnologyId that does not exist
- **THEN** a DomainException with code "TECHNOLOGY_NOT_FOUND" is thrown

### Requirement: Technology removal from project
The system SHALL allow removing a technology association from a project.

#### Scenario: Remove technology from project
- **WHEN** a request to remove an associated technology from a project is received
- **THEN** the ProjectTechnology entity is removed and a TechnologyRemovedFromProject domain event is emitted

#### Scenario: Remove non-associated technology rejected
- **WHEN** a request to remove a technology not associated with the project
- **THEN** a DomainException with code "TECHNOLOGY_NOT_ASSIGNED" is thrown

### Requirement: Project identity and equality
The system SHALL identify projects by their unique Id and enforce equality based on Id.

#### Scenario: Projects with same Id are equal
- **WHEN** two Project instances have the same Id
- **THEN** they are considered equal regardless of other property values

#### Scenario: Projects with different Ids are not equal
- **WHEN** two Project instances have different Ids
- **THEN** they are not equal