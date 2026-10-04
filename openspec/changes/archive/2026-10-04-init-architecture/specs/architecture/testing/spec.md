# Spec Delta

## Purpose

Defines the unit and integration test project structure, test data generation with Bogus, and mocking patterns with Moq for the testing layer.

## ADDED Requirements

### Requirement: Unit test project per layer under test/
The test/ directory SHALL contain a `UnitTests` project that can test Domain and Application layers in isolation using xUnit, Moq, and Bogus.

#### Scenario: Unit test project structure
- **WHEN** inspecting test/UnitTests
- **THEN** it references Core/Domain and Core/Application only (no Infrastructure, Presentation)

### Requirement: Integration test project for cross-layer testing
The test/ directory SHALL contain an `IntegrationTests` project that tests API endpoints and database interactions using a real PostgreSQL database (Testcontainers or local instance).

#### Scenario: Integration test project structure
- **WHEN** inspecting test/IntegrationTests
- **THEN** it references Presentation/WebAPI and Infrastructure/Persistence for end-to-end testing

### Requirement: xUnit as the test framework
All test projects SHALL use xUnit as the test framework. `[Fact]` for parameterless tests, `[Theory]` with `[InlineData]` or `[MemberData]` for parameterized tests.

#### Scenario: Test framework consistency
- **WHEN** running `dotnet test`
- **THEN** all tests execute via xUnit runner

### Requirement: Moq for mocking dependencies in unit tests
Unit tests SHALL use Moq to mock interfaces (repositories, services, mediators). Domain entities and value objects SHALL NOT be mocked - use real instances.

#### Scenario: Mock usage in unit tests
- **WHEN** a unit test needs a repository
- **THEN** it uses `Mock<IRepository<Entity>>` not a real implementation

### Requirement: Bogus for test data generation
All test projects SHALL use Bogus to generate realistic fake data for entities, DTOs, and commands. Hardcoded test values SHALL be avoided.

#### Scenario: Fake data generation
- **WHEN** a test needs a Portfolio entity
- **THEN** it uses `new Faker<Portfolio>().RuleFor(...).Generate()` instead of manual instantiation

### Requirement: Unit tests follow AAA pattern
All unit tests SHALL follow Arrange-Act-Assert structure with clear separation. Test names SHALL follow `MethodName_Scenario_ExpectedResult` convention.

#### Scenario: Test naming and structure
- **WHEN** reading test code
- **THEN** each test has clear Arrange, Act, Assert sections and descriptive name

### Requirement: Integration tests use WebApplicationFactory
Integration tests SHALL use `Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<TEntryPoint>` to host the API in-memory for endpoint testing.

#### Scenario: Integration test hosting
- **WHEN** an integration test runs
- **THEN** it creates a `WebApplicationFactory<Program>` to call real endpoints

### Requirement: Database per test or transaction rollback
Integration tests SHALL either use a fresh database per test (Testcontainers) or wrap each test in a transaction that rolls back. No test pollution.

#### Scenario: Test isolation
- **WHEN** running integration tests in parallel
- **THEN** each test has isolated database state

### Requirement: Test coverage baseline for critical paths
The solution SHALL enforce a minimum code coverage threshold (e.g., 70%) for Domain and Application layers via `dotnet test --collect:"XPlat Code Coverage"`.

#### Scenario: Coverage enforcement
- **WHEN** running CI pipeline
- **THEN** coverage report meets threshold for Core/Domain and Core/Application