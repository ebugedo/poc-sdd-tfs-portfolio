# Spec Delta

## Purpose

Manages sectors for categorizing projects in the portfolio.

## ADDED Requirements

### Requirement: Sector creation
The system SHALL allow creating a new sector with a unique name and optional description.

#### Scenario: Successful sector creation
- **WHEN** a request to create a sector with valid unique name and optional description is received
- **THEN** a new Sector aggregate is created with Id, Name, Description, IsActive=true, CreatedAt, and a SectorCreated domain event is emitted

#### Scenario: Duplicate name rejected
- **WHEN** a request to create a sector with a name that already exists
- **THEN** a DomainException with code "SECTOR_NAME_ALREADY_EXISTS" is thrown

#### Scenario: Empty name rejected
- **WHEN** a request to create a sector with empty or whitespace name
- **THEN** a DomainException with code "NAME_REQUIRED" is thrown

### Requirement: Sector update
The system SHALL allow updating sector name and description while preserving name uniqueness.

#### Scenario: Successful sector update
- **WHEN** a request to update a sector with valid unique name and optional description is received
- **THEN** the Sector aggregate is updated with new values and a SectorUpdated domain event is emitted

#### Scenario: Update with duplicate name rejected
- **WHEN** a request to update a sector name to one that already exists for another sector
- **THEN** a DomainException with code "SECTOR_NAME_ALREADY_EXISTS" is thrown

### Requirement: Sector identity and equality
The system SHALL identify sectors by their unique Id and enforce equality based on Id.

#### Scenario: Sectors with same Id are equal
- **WHEN** two Sector instances have the same Id
- **THEN** they are considered equal regardless of other property values

#### Scenario: Sectors with different Ids are not equal
- **WHEN** two Sector instances have different Ids
- **THEN** they are not equal