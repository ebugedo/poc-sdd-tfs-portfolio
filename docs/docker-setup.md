# Docker Setup for CI/CD Pipeline

## Overview

This document describes the Docker setup requirements for the CI/CD pipeline and local development environment.

## Prerequisites

### Local Development

1. **Docker Desktop** (Windows/Mac) or **Docker Engine** (Linux)
   - Version: 24.0+
   - Enable Kubernetes (optional)
   - Allocate at least 4GB RAM for containers

2. **Docker Compose** (included with Docker Desktop)
   - Version: 2.20+

### CI/CD Pipeline (GitHub Actions)

The CI/CD pipeline uses GitHub Actions with the following Docker-related actions:

- `docker/setup-buildx-action@v3` - Sets up Buildx for multi-platform builds
- `docker/login-action@v3` - Authenticates with container registry
- `docker/build-push-action@v5` - Builds and pushes Docker images
- `docker/metadata-action@v5` - Extracts metadata for image tagging

## Testcontainers Configuration

### Local Development

Testcontainers automatically detects the local Docker daemon:

- **Linux**: Uses Unix socket at `/var/run/docker.sock`
- **macOS/Windows**: Uses Docker Desktop's exposed socket (typically `tcp://host.docker.internal:2375` or `npipe:////./pipe/docker_engine`)

No additional configuration is required for local development.

### CI/CD Pipeline (GitHub Actions)

GitHub Actions provides Docker daemon by default. The workflow uses:

```yaml
- name: Set up Docker Buildx
  uses: docker/setup-buildx-action@v3

- name: Set up Testcontainers
  # Testcontainers automatically detects Docker in GitHub Actions
```

### Docker Socket Configuration

Testcontainers automatically detects the Docker daemon. For explicit configuration:

```bash
# Linux/macOS
export DOCKER_HOST=unix:///var/run/docker.sock

# Windows (Docker Desktop)
export DOCKER_HOST=npipe:////./pipe/docker_engine
```

Or in `.testcontainers.properties`:
```properties
testcontainers.docker.host=unix:///var/run/docker.sock
testcontainers.reuse.enable=true
```

## PostgreSQL Testcontainers Configuration

The integration tests use `Testcontainers.PostgreSQL` with the following configuration:

```csharp
var container = new PostgreSqlBuilder()
    .WithDatabase("portfolio_test")
    .WithUsername("test_user")
    .WithPassword("test_password")
    .Build();
```

Testcontainers automatically:
1. Pulls the PostgreSQL image if not present
2. Starts the container with random port mapping
3. Provides connection string via `GetConnectionString()`
4. Cleans up container after tests

### Resource Limits

Recommended resource limits for CI:
```yaml
# In docker-compose.yml or Testcontainers config
mem_limit: 512m
cpus: 0.5
```

## Security Considerations

### SSH.NET Transitive Dependency

Testcontainers includes SSH.NET as a transitive dependency for remote Docker daemon access via SSH. This is only used when connecting to remote Docker daemons via SSH. For local Docker socket connections, SSH.NET is not used.

The SSH.NET transitive dependency has known vulnerabilities (GHSA-q939-rpr3-3284, GHSA-mggc-4xg6-vcxf). This is accepted as an acceptable risk for a transitive dev dependency since:
- SSH.NET is only used for remote Docker via SSH
- Local development and CI use local Docker socket
- No SSH connections are made in our configuration

### Image Security

- Base images are regularly updated via Dependabot
- Multi-stage builds minimize attack surface
- Non-root user in runtime containers
- Distroless or Alpine base images where possible

## Troubleshooting

### Docker Not Running

Error: `Docker is either not running or misconfigured`

Solutions:
1. Start Docker Desktop / Docker service
2. Verify `docker ps` works
3. Check `DOCKER_HOST` environment variable
4. On Windows: Ensure Docker Desktop is running and WSL2 integration is enabled

### Testcontainers Timeout

Error: `Timeout waiting for container to start`

Solutions:
1. Increase Testcontainers timeout: `.WithStartupTimeout(TimeSpan.FromMinutes(2))`
2. Check Docker resource limits (memory/CPU)
3. Check for port conflicts

### Port Conflicts

Error: `Port already in use`

Solutions:
1. Testcontainers uses random ports by default
2. Ensure no other services using PostgreSQL default port (5432)
3. Use `.WithPortBinding(5432, true)` for explicit port mapping

## Docker Image Build

### Multi-stage Build

The Dockerfile uses multi-stage build:
1. **Build stage**: `mcr.microsoft.com/dotnet/sdk:8.0` - Restore, build, publish
2. **Runtime stage**: `mcr.microsoft.com/dotnet/aspnet:8.0` - Copy published output, non-root user

### Build Arguments

```dockerfile
ARG BUILD_CONFIGURATION=Release
ARG VERSION=1.0.0
```

### Build Command

```bash
docker build \
  --build-arg BUILD_CONFIGURATION=Release \
  --build-arg VERSION=1.0.0 \
  -t ghcr.io/owner/repo:latest \
  -f src/Presentation/WebAPI/Dockerfile \
  .
```

## Registry Authentication

### GitHub Container Registry (GHCR)

```bash
# Login
echo $GHCR_PAT | docker login ghcr.io -u USERNAME --password-stdin

# Push
docker push ghcr.io/owner/repo:latest
```

### GitHub Actions Secrets

Required secrets:
- `GHCR_PAT` - GitHub Personal Access Token with `write:packages` scope
- `GITHUB_TOKEN` - Automatically provided by GitHub Actions

## References

- [Testcontainers Documentation](https://dotnet.testcontainers.org/)
- [Docker Documentation](https://docs.docker.com/)
- [GitHub Actions Docker Documentation](https://docs.github.com/en/actions/using-containerized-services)
- [Docker Multi-stage Builds](https://docs.docker.com/build/building/multi-stage/)