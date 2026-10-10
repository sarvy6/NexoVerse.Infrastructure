# Persistence

API, Worker and future hosts use the same registration:

```csharp
using NexoVerse.Infrastructure;

builder.Services.AddNexoVerseInfrastructure(builder.Configuration);
```

Supply `ConnectionStrings:NexoVerse` through configuration, user secrets, or
the `ConnectionStrings__NexoVerse` environment variable. Alternatively, pass
a SQL Server connection string directly to the registration overload.
Missing connection information is rejected during registration.

Infrastructure owns SQL Server EF Core 10 configuration, migration assembly,
the singleton ORM persistence registry, and scoped NexoVerseDbContext and
DbContext registrations pointing to the same instance. ORM entity services
are registered centrally, with AuditableEntityService selected for the three
domain models. Core references Contracts and consumes IEntityService<T>
without referencing Infrastructure. Background services must resolve scoped
persistence services within a service scope.

Contracts use ORM Guid identity, audit timestamps, IPersistable's marker
contract and explicit relationship interface implementations forwarding to
named concrete properties. Infrastructure mappings implement the actual
IPersistable<TEntity>.Configure(EntityTypeBuilder<TEntity>) contract.
No mappings are placed in Contracts, and configurations are discovered only
from the Infrastructure assembly.

Membership uses EF skip navigations and the SpaceMembers join table, with a
composite SpaceId/UserId key; no membership domain entity is introduced.
CreatedById, SpaceId and InvitedUserId are explicit required Guid foreign keys.
Space deletion cascades to memberships and invitations. User relationships use
NoAction to preserve references and avoid SQL Server multiple cascade paths.

Existing ORM audit services stamp CreatedAt on CreateAsync/CreateRangeAsync.
UpdatedAt is not automatically stamped by the library; direct DbContext writes
do not invoke entity-service auditing.

Registration does not connect to the database, create it, or apply migrations.
The migration assembly is Infrastructure; no initial migration is included.

Validation:

```text
dotnet build NexoVerse.slnx
dotnet test NexoVerse.slnx --no-build
```

PersistenceTests validates the SQL Server model and generated schema without
connecting to SQL Server, scoped DI services, required configuration, and
SQLite relational round trips, creation auditing and delete constraints.
API and Worker startup were also checked using the same connection setting.
Live SQL Server execution and migration deployment were not verified.
