# Spec Delta

## Purpose

Manages technologies for tracking skills and tech stack used in portfolio projects.

## ADDED Requirements

### Requirement: Technology creation
The system SHALL allow creating a new technology with a unique name, optional description, and category.

#### Scenario: Successful technology creation
- **WHEN** a request to create a technology with valid unique name, optional description, and category is received
- **THEN** a new Technology aggregate is created with Id, Name, Description, Category, IsActive=true, CreatedAt, and a TechnologyCreated domain event is emitted

#### Scenario: Duplicate name rejected
- **WHEN** a request to create a technology with a name that already exists
- **THEN** a DomainException with code "TECHNOLOGY_NAME_ALREADY_EXISTS" is thrown

#### Scenario: Empty name rejected
- **WHEN** a request to create a technology with empty or whitespace name
- **THEN** a DomainException with code "NAME_REQUIRED" is thrown

#### Scenario: Invalid category rejected
- **WHEN** a request to create a technology with an invalid category
- **THEN** a DomainException with code "INVALID_TECHNOLOGY_CATEGORY" is thrown

### Requirement: Technology update
The system SHALL allow updating technology name, description, and category while preserving name uniqueness.

#### Scenario: Successful technology update
- **WHEN** a request to update a technology with valid unique name, optional description, and category is received
- **THEN** the Technology aggregate is updated with new values and a TechnologyUpdated domain event is emitted

#### Scenario: Update with duplicate name rejected
- **WHEN** a request to update a technology name to one that already exists for another technology
- **THEN** a DomainException with code "TECHNOLOGY_NAME_ALREADY_EXISTS" is thrown

### Requirement: Technology category enumeration
The system SHALL support the following technology categories: Frontend, Backend, Database, DevOps, Mobile, Other.

#### Scenario: Valid categories accepted
- **WHEN** a technology is created or updated with any of the supported categories
- **THEN** the operation succeeds

#### Scenario: Unknown category rejected
- **WHEN** a technology is created or updated with an unsupported category
- **THEN** a DomainException with code "INVALID_TECHNOLOGY_CATEGORY" is thrown

### Requirement: Technology identity and equality
The system SHALL identify technologies by their unique Id and enforce equality based on Id.

#### Scenario: Technologies with same Id are equal
- **WHEN** two Technology instances have the same Id
- **THEN** they are considered equal regardless of other property values

#### Scenario: Technologies with different Ids are not equal
- **WHEN** two Technology instances have different Ids
- **THEN** they are not equal