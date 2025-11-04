using Microsoft.Extensions.DependencyInjection;
using Ordering.Persistance.EventStreaming;

namespace Ordering.Persistance.Extensions;

/// <summary>
/// ServiceCollectionExtensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the application.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection RegisterPersistancecDependencies(this IServiceCollection services)
    {
        services.AddScoped<IEventStreamingService, EventStreamingService>();

        return services;
    }
}
