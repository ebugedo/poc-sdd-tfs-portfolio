---
name: tfs-openspec-validate
description: Audits and verifies that C# (.NET / ASP.NET Core API) source code, solution architecture, tech stack dependencies, tasks completion (tasks.md), package security, build status, and test projects comply with OpenSpec specifications and design decisions for a specific target change ($1).
---

# Skill: tfs-openspec-validate - Target Change Audit (.NET / ASP.NET Core API)

Act as an **independent software auditor and .NET / C# QA specialist**. Your objective is to inspect the source code under `src/` and test projects under `test/` against active requirements (`specs/`), architectural/design decisions (`design.md`), and task checklists (`tasks.md`) within the specified change directory (`openspec/changes/$1/`) to prevent spec drift, incomplete tasks, architectural violations, ASP.NET Core structural flaws, stack mismatch, and unrequested code.

## Command & Invocation
To invoke this skill in OpenCode, use either:
- `/tfs-openspec-validate "add-sector-crud"`
- `tfs-openspec-validate "add-sector-crud"`

## Input Arguments
- **Target Change Name**: `$1` (e.g., `add-sector-crud` or `refactor-services`)

---

## Execution Instructions

1. **Locate Target Change Artifacts**:
   - Resolve the active change directory at `openspec/changes/$1/`.
   - If the directory `openspec/changes/$1/` does not exist, report an error immediately and halt execution.
   - Thoroughly read `openspec/changes/$1/proposal.md`, `openspec/changes/$1/design.md`, `openspec/changes/$1/tasks.md`, and all `.md` specification files under `openspec/changes/$1/specs/`.

2. **Verify Tasks Completion (`tasks.md`)**:
   - Read all items listed in `openspec/changes/$1/tasks.md`.
   - Verify if all task checkboxes are marked as completed (`[x]`).
   - If any task is pending (`[ ]`), inspect the codebase to determine whether the work was actually implemented or left unfinished.
   - Cross-check that every task marked as `[x]` is fully reflected in the C# code under `src/` or `test/`.

3. **Verify Tech Stack & Dependency Decisions (`design.md`)**:
   - Check the targeted **.NET SDK version** (e.g., .NET 8, .NET 9) defined in `.csproj` files against `openspec/changes/$1/design.md`.
   - Inspect all `<PackageReference>` elements in `.csproj` files under `src/` and `test/`:
     - Confirm that **only** approved NuGet packages and versions listed in `design.md` are used.
     - Detect any forbidden or unapproved libraries (e.g., using Dapper when Entity Framework Core was specified, or vice versa).

4. **Verify ASP.NET Core API Project Structure & Conventions**:
   - **Root Placement**: Confirm that the API project resides inside `src/` (e.g., `src/MyApi/MyApi.csproj`).
   - **Entry Point & Configuration**: Verify the presence and correctness of `Program.cs` and configuration files (`appsettings.json`, `appsettings.Development.json`).
   - **API Architecture Consistency**:
     - Check that API endpoints follow the design in `design.md` (e.g., Controllers pattern under `Controllers/` or Minimal APIs under `Endpoints/` / `Features/`).
     - Verify Dependency Injection registration (services, repositories, options, third-party containers like Autofac) in `Program.cs` or extension methods.
     - Ensure Middlewares (e.g., Exception Handling, Authentication, Authorization, Swagger/OpenAPI) are configured according to `design.md`.
   - **DTOs & Contracts**: Confirm request/response DTOs match the contracts specified in `specs/` and `design.md`.

5. **Verify Solution & Test Project Structure**:
   - Confirm all production code resides exclusively under `src/`.
   - Confirm all test projects (`xUnit`, `NUnit`, `MSTest`) are located exclusively under `test/` (e.g., `test/MyApi.UnitTests/` or `test/MyApi.IntegrationTests/`).
   - Ensure the solution file (`.sln`) accurately references project paths under `src/` and `test/`.

6. **Verify Acceptance Criteria & Functional Specs (`specs/`)**:
   - Compare C# classes, interfaces, endpoints, and domain logic against every requirement in `openspec/changes/$1/specs/`.
   - Verify that design patterns defined in `design.md` (e.g., Clean Architecture, Vertical Slices, CQRS) are followed.
   - Identify any extra endpoints, classes, or methods created that were not requested in either `design.md` or `specs/` (over-engineering / spec drift).

7. **Audit NuGet Packages & Security Vulnerabilities**:
   - Execute in terminal to scan dependencies for known vulnerabilities or missing version resolutions:
     ```bash
     dotnet list package --vulnerable --include-transitive
     ```
   - Verify if any package contains high/critical vulnerabilities (`NU1903`) or unresolved transitive dependency conflicts (`NU1603`).
   - **If package vulnerabilities or mismatches are found**:
     - Capture package names, installed vs required versions, and CVE links.

8. **Verify Solution Compilation & Build Diagnostics**:
   - Execute in terminal to force a clean build and enforce NuGet resolution warnings as errors:
     ```bash
     dotnet build --no-incremental -warnaserror:NU1603,NU1903 /p:AnalysisLevel=latest
     ```
   - Verify that all projects under `src/` and `test/` compile with zero errors and zero critical package warnings.
   - **If compilation or Roslyn analyzer errors occur**:
     - Capture project paths, file locations, line numbers, error codes, and compiler diagnostic messages.

9. **Run .NET Tests & Capture Failures**:
   - Execute in terminal to run all unit, integration, and E2E test suites under `test/`:
     ```bash
     dotnet test --logger "console;verbosity=normal"
     ```
   - Verify if all test suites pass green.
   - **If any tests fail**:
     - Extract the exact name of each failing test method, project, and suite.
     - Capture the assertion messages, expected vs actual values, and stack traces.

10. **Generate Validation Report**:
    Respond in the chat formatted as follows:

    ---
    ### 📋 Specification & Design Verification Report (.NET / ASP.NET Core API)
    **Target Change:** `$1`

    **Overall Status:** [ 🟢 Compliant | 🟡 Incomplete | 🔴 Non-Compliant / Build Failed / Package Vulnerabilities / Tests Failing ]

    #### 1. Tasks Completion Check (`openspec/changes/$1/tasks.md`)
    - [x] **Tasks Completion**: Confirmation that all tasks defined in `tasks.md` are completed (`[x]`) and verified in code.
    - [ ] **Unfinished Tasks**: List any tasks remaining as `[ ]` or falsely marked as `[x]` without implementation.

    #### 2. Tech Stack & Dependencies Compliance (`design.md`)
    - [x] **.NET Target Framework**: Target framework version matches requirements.
    - [x] **Approved NuGet Packages**: Installed packages match `design.md` without unapproved extra dependencies.

    #### 3. Package Audit & Vulnerabilities Check
    - [x] **NuGet Security & Versions**: No vulnerable packages or version constraint mismatches detected.
    - [ ] **Package Issues**: List any vulnerable packages (`NU1903`) or version resolution warnings (`NU1603`).

    #### 4. ASP.NET Core API Structure & Conventions
    - [x] **Project & Configuration**: `Program.cs`, `appsettings.json`, and `.csproj` properly located under `src/`.
    - [x] **API Endpoints & Routing**: Controller or Minimal API layout complies with `design.md`.
    - [x] **Dependency Injection & Middleware**: Services, DI containers (e.g., Autofac), pipeline middlewares, and auth/error handling configured as designed.

    #### 5. Architectural & Solution Structure (`design.md`)
    - [x] **Folder & Solution Structure**: `.csproj` files cleanly separated into `src/` and `test/`.
    - [x] **Design Patterns**: Implementation adheres to architectural patterns defined in `design.md`.

    #### 6. Satisfied Requirements (`specs/`)
    - [x] **[Requirement/API Endpoint/C# Method]**: Explanation of the class or endpoint in `src/` fulfilling it.

    #### 7. Missing Requirements or Deviations
    - [ ] **[Unmet Requirement/Design Gap]**: Details of missing API functionality, pending tasks, tech stack mismatches, or structural deviations per `tasks.md`, `specs/`, and `design.md`.

    #### 8. Unrequested Code (Drift)
    - List any endpoints, classes, controllers, or packages added outside the specification or design scope.

    #### 9. Solution Compilation & Build Results
    - **Status**: [ 🟢 Build Succeeded | 🔴 Build Failed ]
    - **Compilation Errors**: Details on build errors or Roslyn analyzer failures (if any).

    #### 10. `dotnet test` Results & Failures
    - **Summary**: X passed, Y failed, Z skipped.
    - **Detailed Failure Output** (If tests, package checks, or build failed, format below for easy copy-paste to OpenCode):

    ```text
    ❌ AUDIT / BUILD / TEST FAILURE DETAILS:

    [Category: Package Audit / Build Error / Test Failure]
    Target: [Project/Suite Name] -> [File/Class/TestMethod]
    Error Message: [Captured error, package vulnerability, or assertion failure]
    Stack Trace / Diagnostic Output:
    [Stack trace or build output]
    ---