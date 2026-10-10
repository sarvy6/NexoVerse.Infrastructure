using Libraries.Orm.Persistence;
using Microsoft.EntityFrameworkCore;

namespace NexoVerse.Infrastructure;

public sealed class NexoVerseDbContext(DbContextOptions<NexoVerseDbContext> options, PersistenceRegistry persistence)
    : PersistableDbContext(options, persistence);
