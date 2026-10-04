# Global Technology Stack Specification

## Core Platform
- **Runtime & Framework:** .NET 10 (ASP.NET Core Web API).
- **Language:** C# (Latest language features enabled).
- **JSON Serialization:** Newtonsoft.Json (Json.NET) - Required for all API serialization, configuration, and logging.

## Domain & Application Layer Libraries
- **Dependency Injection:** Autofac (Used as the IoC/DI container for modular component registration).
- **Object Mapping:** AutoMapper (For entity-to-DTO and DTO-to-entity mapping across layers).

## Persistence & Database
- **Database:** PostgreSQL.
- **ORM Provider:** Entity Framework Core using `Npgsql.EntityFrameworkCore.PostgreSQL`.

## Testing & Quality Assurance
- **Test Framework:** xUnit.
- **Mocking Library:** Moq.
- **TestData Generation:** Bogus (For fake data creation in unit tests, integration tests, and database seeding).

## Repository & Solution Layout Conventions
- **Production Code:** All production source code projects MUST be located exclusively under `src/`.
- **Test Code:** All unit, integration, and functional test projects MUST be located exclusively under `test/`.
- **Solution File:** Use `.slnx` (XML-based solution file) format. Do NOT use legacy `.sln` format.