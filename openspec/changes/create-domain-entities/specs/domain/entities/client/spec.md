# Spec Delta

## Purpose

Manages client lifecycle including creation, updates, and deactivation for portfolio projects.

## ADDED Requirements

### Requirement: Client creation
The system SHALL allow creating a new client with a unique email, name, and optional contact details.

#### Scenario: Successful client creation
- **WHEN** a request to create a client with valid unique email, name, and optional phone/address is received
- **THEN** a new Client aggregate is created with Id, Name, Email, Phone, Address, IsActive=true, CreatedAt, and a ClientCreated domain event is emitted

#### Scenario: Duplicate email rejected
- **WHEN** a request to create a client with an email that already exists
- **THEN** a DomainException with code "EMAIL_ALREADY_EXISTS" is thrown

#### Scenario: Empty name rejected
- **WHEN** a request to create a client with empty or whitespace name
- **THEN** a DomainException with code "NAME_REQUIRED" is thrown

### Requirement: Client update
The system SHALL allow updating client name, phone, and address while preserving email uniqueness.

#### Scenario: Successful client update
- **WHEN** a request to update a client with valid name and optional phone/address is received
- **THEN** the Client aggregate is updated with new values, UpdatedAt is set, and a ClientUpdated domain event is emitted

#### Scenario: Update with duplicate email rejected
- **WHEN** a request to update a client email to one that already exists for another client
- **THEN** a DomainException with code "EMAIL_ALREADY_EXISTS" is thrown

### Requirement: Client deactivation
The system SHALL allow deactivating a client (soft delete) without removing historical data.

#### Scenario: Successful client deactivation
- **WHEN** a request to deactivate an active client is received
- **THEN** the Client aggregate IsActive is set to false, UpdatedAt is set, and a ClientDeactivated domain event is emitted

#### Scenario: Deactivating already inactive client
- **WHEN** a request to deactivate an already inactive client is received
- **THEN** no state change occurs and no domain event is emitted

### Requirement: Client identity and equality
The system SHALL identify clients by their unique Id and enforce equality based on Id.

#### Scenario: Clients with same Id are equal
- **WHEN** two Client instances have the same Id
- **THEN** they are considered equal regardless of other property values

#### Scenario: Clients with different Ids are not equal
- **WHEN** two Client instances have different Ids
- **THEN** they are not equal