# Spec Delta

## Purpose

Provides a value object for email validation and equality comparison.

## ADDED Requirements

### Requirement: Email value object creation
The system SHALL allow creating an Email value object from a valid email string.

#### Scenario: Valid email accepted
- **WHEN** an Email value object is created with a valid email format (e.g., "user@example.com")
- **THEN** the Email instance is created with the normalized value

#### Scenario: Invalid email format rejected
- **WHEN** an Email value object is created with an invalid email format
- **THEN** a DomainException with code "INVALID_EMAIL_FORMAT" is thrown

#### Scenario: Empty email rejected
- **WHEN** an Email value object is created with empty or whitespace string
- **THEN** a DomainException with code "EMAIL_REQUIRED" is thrown

### Requirement: Email equality
The system SHALL compare Email value objects by their normalized value.

#### Scenario: Emails with same value are equal
- **WHEN** two Email instances have the same email address (case-insensitive)
- **THEN** they are considered equal

#### Scenario: Emails with different values are not equal
- **WHEN** two Email instances have different email addresses
- **THEN** they are not equal

#### Scenario: Email hash code consistency
- **WHEN** two equal Email instances are compared
- **THEN** they produce the same hash code

### Requirement: Email normalization
The system SHALL normalize email addresses to lowercase for storage and comparison.

#### Scenario: Email is normalized to lowercase
- **WHEN** an Email is created with "User@Example.COM"
- **THEN** the stored value is "user@example.com"