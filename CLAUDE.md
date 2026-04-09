# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

Build / run / test (from repo root):

- Build solution: `dotnet build Northwind.SQLite.sln`
- Run the console app: `dotnet run --project Northwind.Application`
- Run all tests: `dotnet test`
- Run a single test class: `dotnet test --filter "FullyQualifiedName~ProductServiceTests_SQLite"`
- Run a single test: `dotnet test --filter "FullyQualifiedName~ProductServiceTests_SQLite.GetTop3MostExpensiveProductsTests"`

Target framework is `net10.0` across all projects.

## Architecture

This is a learning/exercise solution that queries the classic Northwind sample database via EF Core against a local SQLite file (`northwind.db` at repo root).

Projects:

- **Northwind.Entities** — EF Core persistence layer. `NorthwindContext` and one entity class per Northwind table (`Product`, `Order`, `Customer`, etc.). These are the *database* models.
- **Northwind.Application** — Console host plus a small domain layer:
  - `Program.cs` wires DI using `AddPooledDbContextFactory<NorthwindContext>` and resolves the SQLite path relative to the build output (`../../../../northwind.db`). It throws if the DB file is missing.
  - `Application.cs` contains `ExecuteExerciseN` methods — each exercise is a self-contained LINQ query against a freshly created `DbContext` from the factory. `Program.Main` picks which exercise to run.
  - `Products/` is a mini vertical slice showing a repository/service split: `IProductRepository` → `ProductRepository` (EF queries) → `ProductService` (business logic, e.g. "top 3 most expensive"). A separate `Product` **record** in this namespace is the *domain* model, distinct from `Entities.Product`. `Extensions.ToDomain()` maps entity → domain.
- **Northwind.Tests** — xUnit tests for `ProductService`, deliberately implemented four different ways to compare testing styles against the same behavior:
  - `ProductServiceTests-Mocked.cs` — mocks `IProductRepository`
  - `ProductServiceTests-Stubbed.cs` — hand-written stub of `IProductRepository`
  - `ProductServiceTests-InMemoryDB.cs` — EF Core InMemory provider
  - `ProductServiceTests-SQLite.cs` — real EF + `Microsoft.Data.Sqlite` `:memory:` connection, calling `Database.EnsureCreated()` to materialize the schema

Key patterns to preserve when making changes:

- Entities (`Northwind.Entities`) and domain models (`Northwind.Application.Product` record) are intentionally separate — do not leak entity types out of the repository; map via `ToDomain()`.
- Use the injected `IDbContextFactory<NorthwindContext>` and `using var context = factory.CreateDbContext()` per operation rather than sharing a long-lived context.
- When adding new exercises, follow the `ExecuteExerciseN` convention in `Application.cs` and invoke from `Program.Main`.
