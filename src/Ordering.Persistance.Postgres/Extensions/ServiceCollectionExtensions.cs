using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ordering.Persistance.Repositories;

namespace Ordering.Persistance.Postgres.Extensions;

/// <summary>
/// ServiceCollectionExtensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the application.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IServiceCollection RegisterPostgresDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IOrderRepository, OrderRepository>();

        services.AddOptions<PostgresDbConfiguration>()
           .Configure<IConfiguration>((options, cfg) =>
           {
               configuration.GetSection(PostgresDbConfiguration.ConfigurationSection).Bind(options);
           })
           .ValidateDataAnnotations()
           .ValidateOnStart();


        services.AddDbContext<OrderContext>((sp, options) =>
        {
            PostgresDbConfiguration pgConfiguration = sp.GetRequiredService<IOptions<PostgresDbConfiguration>>().Value;
            options.UseNpgsql(
                pgConfiguration.ConnectionString,
                b => b.MigrationsAssembly(pgConfiguration.MigrationAssembly));
        });

        services.AddTransient<IUnitOfWork>(sp =>
        {
            return sp.GetRequiredService<OrderContext>();
        });

        services.AddTransient<IDbContext>(sp =>
        {
            return sp.GetRequiredService<OrderContext>();
        });


        return services;
    }
}
