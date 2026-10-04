# Spec Delta

## MODIFIED Requirements

### Requirement: GitHub Actions .NET Version
The CI/CD pipelines SHALL use .NET 10 SDK.

#### Scenario: CI workflow uses .NET 10
- **WHEN** inspecting `.github/workflows/ci.yml`
- **THEN** `DOTNET_VERSION` environment variable SHALL be `'10.0.x'`
- **THEN** `actions/setup-dotnet@v4` SHALL use `dotnet-version: ${{ env.DOTNET_VERSION }}`

#### Scenario: CD workflow uses .NET 10
- **WHEN** inspecting `.github/workflows/cd.yml`
- **THEN** `DOTNET_VERSION` environment variable SHALL be `'10.0.x'`

### Requirement: Docker Base Images
The Dockerfile SHALL use .NET 10 base images.

#### Scenario: Dockerfile uses .NET 10 runtime image
- **WHEN** inspecting `src/Presentation/WebAPI/Dockerfile`
- **THEN** base image SHALL be `mcr.microsoft.com/dotnet/aspnet:10.0` (or equivalent .NET 10 tag)

#### Scenario: Dockerfile uses .NET 10 SDK image
- **WHEN** inspecting `src/Presentation/WebAPI/Dockerfile`
- **THEN** build stage image SHALL be `mcr.microsoft.com/dotnet/sdk:10.0` (or equivalent .NET 10 tag)

### Requirement: GHCR Image Tagging
The CD pipeline SHALL tag images with .NET 10 version.

#### Scenario: Docker metadata includes .NET 10
- **WHEN** CD workflow builds and pushes image
- **THEN** image tags SHALL reflect .NET 10 runtime (e.g., via metadata action)