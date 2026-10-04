# architecture/api Specification

## Purpose
Defines ASP.NET Core WebAPI setup, endpoint conventions, middleware pipeline, and ProblemDetails error handling for the presentation layer.

## Requirements

### Requirement: WebAPI project targets .NET 8+ with minimal APIs or controllers
The Presentation/WebAPI project SHALL be an ASP.NET Core Web API targeting .NET 8 or later, using either minimal APIs or controller-based endpoints consistently.

#### Scenario: API project runs
- **WHEN** executing `dotnet run` in Presentation/WebAPI
- **THEN** the API starts and listens on configured ports

### Requirement: Endpoints follow RESTful conventions with plural lowercase URIs
All API endpoints SHALL use plural, lowercase, kebab-case resource names (e.g., `/api/v1/portfolios`, `/api/v1/transactions`). Version SHALL be in the URL path.

#### Scenario: Endpoint URI format
- **WHEN** listing all endpoints
- **THEN** all URIs match `/api/v{version}/{plural-resource}` pattern

### Requirement: Standard HTTP status codes are used
Endpoints SHALL return appropriate status codes: 200 (OK), 201 (Created with Location header), 204 (No Content), 400 (Bad Request), 401 (Unauthorized), 403 (Forbidden), 404 (Not Found), 500 (Internal Server Error).

#### Scenario: Status code compliance
- **WHEN** testing each endpoint
- **THEN** responses use correct status codes per the operation outcome

### Requirement: ProblemDetails (RFC 7807) for all errors
All error responses SHALL return `application/problem+json` with `type`, `title`, `status`, `detail`, and `instance` fields. Validation errors SHALL include `errors` dictionary.

#### Scenario: Error response format
- **WHEN** an error occurs (validation, not found, server error)
- **THEN** response body is ProblemDetails with required fields

### Requirement: JWT authentication middleware configured
The WebAPI SHALL configure JWT Bearer token authentication with `[Authorize]` attribute or endpoint policies protecting secured endpoints.

#### Scenario: Protected endpoint requires token
- **WHEN** calling a secured endpoint without a valid JWT
- **THEN** response is 401 Unauthorized

### Requirement: Global exception handling middleware
A global exception handling middleware SHALL catch unhandled exceptions, log them, and return a 500 ProblemDetails response without exposing stack traces.

#### Scenario: Unhandled exception handling
- **WHEN** an unhandled exception occurs in a request
- **THEN** response is 500 ProblemDetails with generic error message

### Requirement: Request/response logging middleware
The pipeline SHALL include middleware for structured request/response logging (correlation IDs, timing, status codes) using Serilog or equivalent.

#### Scenario: Request logging
- **WHEN** a request is processed
- **THEN** logs contain correlation ID, method, path, status, and duration

### Requirement: CORS policy configured for known origins
The WebAPI SHALL configure a CORS policy allowing only known frontend origins (configurable via environment), not wildcard.

#### Scenario: CORS enforcement
- **WHEN** a request comes from an unconfigured origin
- **THEN** CORS headers are not present and request is blocked

### Requirement: API versioning via URL path
API version SHALL be in the URL path (`/api/v1/...`). The application SHALL support at least one version at launch.

#### Scenario: Version in URL
- **WHEN** accessing any endpoint
- **THEN** URL contains `/api/v1/` prefix
