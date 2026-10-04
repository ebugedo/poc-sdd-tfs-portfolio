# Spec Delta

## MODIFIED Requirements

### Requirement: Domain layer structure
The system SHALL organize domain entities in the Domain project following Clean Architecture.

#### Scenario: Domain entities exist in correct location
- **WHEN** the Domain project is inspected
- **THEN** it contains Client, Sector, Technology, Project aggregates, ProjectTechnology entity, Email value object, and domain events under appropriate folders

#### Scenario: Domain layer has no external dependencies
- **WHEN** the Domain project references are inspected
- **THEN** it has no dependencies on Infrastructure, Presentation, or Application projects