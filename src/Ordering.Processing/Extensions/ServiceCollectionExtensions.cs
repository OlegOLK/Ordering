using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Messaging.Messages;
using Ordering.Messaging.RabbitMq;
using Ordering.Processing.Consumers;
using Ordering.Processing.Processors;

namespace Ordering.Processing.Extensions;

/// <summary>
/// ServiceCollectionExtensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the processing dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IServiceCollection RegisterProcessingDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IConsumer<CreateOrderMessage>, CreateOrderMessageConsumer>();
        services.AddSingleton(new RoutingConsumer
        {
            Consumer = typeof(CreateOrderMessageConsumer),
            RouteEndpoint = RabbitMqMessageService.QueueName
        });

        services.AddTransient<IProcessor, ItemsAvailabilityCheckProcessor>();
        services.AddTransient<IProcessor, ContextEnricherProcessor>();
        return services;
    }
}
