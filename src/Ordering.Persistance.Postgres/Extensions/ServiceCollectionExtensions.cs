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
        services.AddTransient<IOutboxEventRepository, OutboxEventRepository>();

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

        services.AddScoped<IUnitOfWork>(sp =>
        {
            return sp.GetRequiredService<OrderContext>();
        });

        services.AddScoped<IDbContext>(sp =>
        {
            return sp.GetRequiredService<OrderContext>();
        });

        return services;
    }

    /// <summary>
    /// Registers the health check.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IHealthChecksBuilder RegisterPersisntanceHealthCheck(this IHealthChecksBuilder builder)
    {
        builder.AddNpgSql(sp =>
        {
            PostgresDbConfiguration configuration = sp.GetRequiredService<IOptions<PostgresDbConfiguration>>().Value;

            return configuration.ConnectionString;
        });

        return builder;
    }
}
