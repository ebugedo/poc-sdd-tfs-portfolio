# C# & Code Quality Standards

## Naming Conventions
- **Classes, Interfaces, Methods & Properties:** PascalCase (e.g., `GetUserByIdQuery`, `IUserRepository`).
- **Interfaces:** Prefix with `I` (e.g., `IUnitOfWork`).
- **Private Fields:** camelCase with leading underscore (e.g., `_dbContext`).
- **Local Variables & Parameters:** camelCase (e.g., `userId`, `command`).

## Error & Exception Handling
- Use custom Domain Exceptions (`DomainException`) for business rule violations.
- Never catch generic `System.Exception` without rethrowing or handling appropriately.
- Validation failures must return standard HTTP 400 (ProblemDetails) in Presentation Layer.

## Async & Concurrency
- All I/O operations (database access, external APIs) MUST be asynchronous using `async`/`await`.
- Append `Async` to all asynchronous method names (e.g., `GetByIdAsync`).