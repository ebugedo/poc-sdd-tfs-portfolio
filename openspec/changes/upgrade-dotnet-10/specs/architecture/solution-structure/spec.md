# Spec Delta

## MODIFIED Requirements

### Requirement: Target Framework Version
The system SHALL target .NET 10 as the runtime framework for all projects.

#### Scenario: All projects target net10.0
- **WHEN** inspecting any `.csproj` file in `src/` or `test/`
- **THEN** the `TargetFramework` element SHALL be `net10.0`

#### Scenario: Solution builds on .NET 10 SDK
- **WHEN** running `dotnet build` with .NET 10 SDK installed
- **THEN** all projects compile successfully without framework version errors