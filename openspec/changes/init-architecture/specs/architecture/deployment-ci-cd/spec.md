# Spec Delta

## Purpose

Defines Docker containerization, GitHub Actions CI/CD pipeline, GitHub Container Registry (GHCR) image management, and VPS deployment via SSH for the application.

## ADDED Requirements

### Requirement: Multi-stage Dockerfile for WebAPI
The Presentation/WebAPI project SHALL include a multi-stage Dockerfile using official Microsoft .NET images: SDK image for build stage, Runtime image for final stage. The runtime image SHALL run as a non-root user.

#### Scenario: Dockerfile structure validation
- **WHEN** inspecting the Dockerfile
- **THEN** it has distinct build and runtime stages with correct base images

#### Scenario: Non-root user execution
- **WHEN** the container runs
- **THEN** the process runs as a non-privileged user (not root)

### Requirement: Repository-root .dockerignore
A `.dockerignore` file SHALL exist at the repository root excluding `bin/`, `obj/`, `.git/`, `.env`, local settings, and IDE folders.

#### Scenario: .dockerignore coverage
- **WHEN** building the Docker image
- **THEN** excluded patterns are not copied into the build context

### Requirement: GitHub Actions CI workflow
A GitHub Actions workflow SHALL exist in `.github/workflows/ci.yml` that runs on push and pull requests to main, executing `dotnet build` and `dotnet test` before any deployment steps.

#### Scenario: CI runs on push
- **WHEN** code is pushed to main or a PR is opened
- **THEN** the CI workflow runs build and test steps successfully

### Requirement: GitHub Actions CD workflow
A GitHub Actions workflow SHALL exist in `.github/workflows/cd.yml` (or combined) that triggers on successful CI completion on main branch, performing:
1. Login to GHCR using `GITHUB_TOKEN`
2. Build and push Docker image tagged with commit SHA and `latest`
3. SSH to VPS and execute deployment commands

#### Scenario: CD triggers on main
- **WHEN** CI passes on main branch
- **THEN** CD workflow builds, pushes to GHCR, and deploys to VPS

### Requirement: GHCR image naming and tagging
Docker images SHALL be pushed to `ghcr.io/<owner>/<repo>` with tags: commit SHA (`${{ github.sha }}`) and `latest`. Semantic version tags (e.g., `v1.0.0`) SHALL be pushed when a Git tag is created.

#### Scenario: Image tagging strategy
- **WHEN** CD workflow runs on main
- **THEN** images are tagged with SHA and latest
- **WHEN** a Git tag is pushed
- **THEN** images are additionally tagged with the semantic version

### Requirement: GitHub Secrets for deployment
All sensitive deployment parameters SHALL be stored exclusively as GitHub Repository Secrets:
- `VPS_HOST`, `VPS_USERNAME`, `VPS_SSH_KEY`, `VPS_PORT`
- `GHCR_PAT` / `GITHUB_TOKEN` for registry authentication

#### Scenario: No secrets in repository
- **WHEN** inspecting repository files
- **THEN** no deployment secrets are present in any committed file

### Requirement: VPS deployment via SSH with Docker CLI
The CD workflow SHALL use `appleboy/ssh-action` (or equivalent) to execute deployment commands on the VPS over SSH:
1. Authenticate to GHCR
2. Pull latest image
3. Stop and remove existing container
4. Run new container with `--restart unless-stopped`, port mapping, environment variables from `.env` file
5. Prune unused images

#### Scenario: VPS deployment execution
- **WHEN** CD workflow runs deployment step
- **THEN** SSH connection succeeds and Docker commands execute in sequence

### Requirement: Production environment configuration
The deployed container SHALL run with `ASPNETCORE_ENVIRONMENT=Production` and load configuration from a VPS-hosted `.env` file (not committed to git).

#### Scenario: Production environment variables
- **WHEN** the container starts on VPS
- **THEN** it reads configuration from `/path/to/vps/.env` and uses Production environment