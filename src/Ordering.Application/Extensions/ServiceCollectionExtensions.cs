using Cortex.Mediator.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviors;
using Ordering.Application.Services;
using Ordering.Domain.Services;

namespace Ordering.Application.Extensions
{
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
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IOrderingService, OrderingService>();

            services.AddCortexMediator(
                configuration,
                new[] { typeof(ServiceCollectionExtensions) }, // Assemblies to scan for handlers
                options => options
                    .AddDefaultBehaviors() // Logging here
                    .AddOpenCommandPipelineBehavior(typeof(TransactionBehavior<,>)));

            return services;
        }
    }
}
