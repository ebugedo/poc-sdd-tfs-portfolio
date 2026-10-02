# Global Technology Stack Specification

## Core Platform
- **Runtime & Framework:** .NET (ASP.NET Core Web API)[cite: 3].
- **Language:** C# (Latest language features enabled).

## Domain & Application Layer Libraries
- **Dependency Injection:** Autofac (Used as the IoC/DI container for modular component registration)[cite: 2, 3].
- **Object Mapping:** AutoMapper (For entity-to-DTO and DTO-to-entity mapping across layers).

## Persistence & Database
- **Database:** PostgreSQL[cite: 3].
- **ORM Provider:** Entity Framework Core using `Npgsql.EntityFrameworkCore.PostgreSQL`[cite: 3].

## Testing & Quality Assurance
- **Test Framework:** xUnit[cite: 3].
- **Mocking Library:** Moq[cite: 3].
- **TestData Generation:** Bogus (For fake data creation in unit tests, integration tests, and database seeding).

## Repository & Solution Layout Conventions
- **Production Code:** All production source code projects MUST be located exclusively under `src/`[cite: 2, 3].
- **Test Code:** All unit, integration, and functional test projects MUST be located exclusively under `test/`[cite: 2, 3].