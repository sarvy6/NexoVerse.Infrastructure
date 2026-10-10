using Libraries.Orm.Extensions;
using Libraries.Orm.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexoVerse.Contracts.Entity.Models;
using NexoVerse.Contracts.Space.Models;

namespace NexoVerse.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNexoVerseInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var connectionString = configuration.GetConnectionString("NexoVerse");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("ConnectionStrings:NexoVerse is required.", nameof(configuration));
        return services.AddNexoVerseInfrastructure(connectionString);
    }

    public static IServiceCollection AddNexoVerseInfrastructure(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        services.RegisterItems(typeof(NexoVerseDbContext).Assembly);
        services.AddDbContext<NexoVerseDbContext>(options => options.UseSqlServer(connectionString,
            sql => sql.MigrationsAssembly(typeof(NexoVerseDbContext).Assembly.GetName().Name)));
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<NexoVerseDbContext>());
        services.AddLibrariesOrm();
        services.AddEntityService<User, AuditableEntityService<User>>();
        services.AddEntityService<Space, AuditableEntityService<Space>>();
        services.AddEntityService<SpaceInvitation, AuditableEntityService<SpaceInvitation>>();
        return services;
    }
}
