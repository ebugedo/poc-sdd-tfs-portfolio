# Spec Delta

## Purpose

Defines the Autofac-based dependency injection configuration and module registration patterns across all architectural layers.

## ADDED Requirements

### Requirement: Autofac is the sole DI container
The application SHALL use Autofac as the exclusive dependency injection container. Microsoft.Extensions.DependencyInjection SHALL only be used for framework integration, not for application registrations.

#### Scenario: Container verification
- **WHEN** the application starts
- **THEN** Autofac container is built and all services are resolved through it

### Requirement: Each layer exposes an Autofac module
Each project layer (Domain, Application, Infrastructure/Persistence, Presentation/WebAPI) SHALL expose a public `Autofac.Module` class that registers its internal services.

#### Scenario: Module existence per layer
- **WHEN** inspecting each layer project
- **THEN** each contains a `Module.cs` or similar Autofac module registration class

### Requirement: Modules are composed at the composition root
The Presentation/WebAPI layer SHALL compose all layer modules in its `Program.cs` or startup configuration, registering them in dependency order (Domain → Application → Infrastructure → Presentation).

#### Scenario: Module composition order
- **WHEN** the Autofac container is built
- **THEN** modules are registered in correct dependency order with no circular dependencies

### Requirement: Domain layer registers no infrastructure services
The Core/Domain Autofac module SHALL NOT register any infrastructure services (EF Core, database connections, external API clients).

#### Scenario: Domain module purity
- **WHEN** inspecting Core/Domain module registrations
- **THEN** only domain services, domain events, and domain factories are registered

### Requirement: Application layer registers Mediator and handlers
The Core/Application Autofac module SHALL register MediatR (or equivalent) and all command/query handlers, validators, and mapping profiles.

#### Scenario: Application module registrations
- **WHEN** inspecting Core/Application module
- **THEN** MediatR, all IRequestHandler implementations, validators, and AutoMapper profiles are registered

### Requirement: Infrastructure layer registers concrete implementations
The Infrastructure/Persistence Autofac module SHALL register EF Core DbContext, repository implementations, and any external service clients.

#### Scenario: Infrastructure module registrations
- **WHEN** inspecting Infrastructure/Persistence module
- **THEN** DbContext, IRepository implementations, and UnitOfWork are registered with appropriate lifetimes

### Requirement: Scoped lifetime for request-bound services
Services that depend on HTTP request context (DbContext, UnitOfWork, Mediator) SHALL be registered with `InstancePerLifetimeScope` (scoped lifetime).

#### Scenario: Scoped service lifetime
- **WHEN** a web request is processed
- **THEN** a new scope is created and scoped services are resolved per request

### Requirement: Singleton lifetime for stateless services
Stateless services (domain factories, configuration, mappers) SHALL be registered as `SingleInstance` (singleton).

#### Scenario: Singleton service lifetime
- **WHEN** the container is built
- **THEN** stateless services resolve to the same instance across all scopes