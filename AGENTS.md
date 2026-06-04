# UrbanLand — AGENTS.md

## Build & Run

```powershell
# Build solution (use full path or run from repo root)
dotnet build "UrbanLand/UrbanLand.sln"

# Run web host (port 5271 / 7273)
dotnet run --project "UrbanLand/src/host/UrbanLand.Web"
```

All projects target `net10.0` with `<ImplicitUsings>enable</ImplicitUsings>` and `<Nullable>enable</Nullable>`. Available SDKs include 10.0.102.

No test projects exist yet. No CI/CD pipeline configured.

## Architecture — DDD with Bounded Contexts

Solution at `UrbanLand/UrbanLand.sln`. Three bounded contexts, a shared kernel, and one host:

```
src/
├── host/UrbanLand.Web/              # ASP.NET entrypoint (bare template — no domain refs yet)
├── shared/
│   ├── UrbanLand.SharedKernel/       # Base classes, Result/Error, identity interfaces
│   └── UrbanLand.SharedKernel.Infrastructure/  # EF Core 10.0.7, MassTransit 9.1.0 + RabbitMQ
└── contexts/
    ├── ProjectManagement/
    │   ├── UrbanLand.ProjectManagement.Domain/      # Aggregate: Project
    │   └── UrbanLand.ProjectManagement.Application/  # MediatR CQRS, MassTransit integration events
    ├── SceneDesign/UrbanLand.SceneDesign.Domain/
    └── AssetCatalog/UrbanLand.AssetCatalog.Domain/
```

All domain projects reference `UrbanLand.SharedKernel`. Infrastructure references SharedKernel. The Application project references its Domain + SharedKernel + MediatR + MassTransit. The Web project currently has zero project references.

## Shared Kernel Conventions

- **Base types**: `Entity<TId>` (equality by ID), `AggregateRoot<TId>`, `ValueObject` (abstract record with `GetEqualityComponents()`)
- **Domain events**: sealed `record` inheriting `DomainEvent` (has `EventId` + `OccurredOn`)
- **Integration events**: class inheriting `IntegrationEvent` (for MassTransit cross-context messaging)
- **Result pattern**: `Result` / `Result<T>` with `Error` record. `Error` has factory methods: `NotFound`, `Validation`, `Forbidden`, `Unauthorized`, `Conflict`, `Internal`
- **DomainException**: base exception for invalid domain state
- **Identity**: `UserId(Guid Value)` is a plain record (not ValueObject)
- **Contracts**: `DateTimeProvider` (mutable for testing), `ICurrentUserService`, `IEmailService`

## Domain Layer Conventions (per context)

- **IDs**: value objects inheriting `ValueObject` — e.g. `record ProjectId(Guid Value) : ValueObject` with `New()` / `From()` statics. Each bounded context defines its own ID types to stay decoupled
- **Entity pattern**: private default constructor (for EF), private typed-ID constructor, `static Create(...)` factory returning `Result<T>`
- **Validation**: `ArgumentNullException.ThrowIfNull` for null guards, `DomainException` for business rule violations, `Error.*` for validation results
- **Domain events**: `sealed record` in an `Events/` folder, raised via `AddDomainEvent()` in the aggregate
- **File-scoped namespaces** everywhere

## Application Layer (ProjectManagement)

- **CQRS with MediatR** — commands/queries in `Commands/` and `Queries/` folders, one subfolder per operation with separate `Command`/`Query` record + `Handler` class
- **Handlers** receive `IProjectRepository`, `ICurrentUserService`, and for cross-context operations — `IPublishEndpoint` (MassTransit)
- **Cross-context messaging**: commands publish `IntegrationEvent` via MassTransit instead of referencing other contexts directly. Consumers live in the target context (SceneDesign, etc.)

## Infrastructure Notes

- **EF Core**: `BaseDbContext` auto-dispatches domain events via MassTransit mediator before `SaveChangesAsync`. Entity configs discovered via `ApplyConfigurationsFromAssembly`. Outbox configured in `MassTransitConfiguration`
- **Messaging**: MassTransit + RabbitMQ with retry (exponential, 5 tries), circuit breaker, rate limiting
- **RabbitMQ config**: read from `RabbitMq` section in appsettings

## Cross-Context ID Pattern

Each context duplicates identity types to avoid coupling. For example, `SceneDesign.Domain.ValueObjects.AssetId` and `AssetCatalog.Domain.ValueObjects.AssetId` are separate types. The SharedKernel `UserId` is used across all contexts directly.
