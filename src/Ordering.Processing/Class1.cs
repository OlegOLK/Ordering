using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Messaging.Messages;
using Ordering.Messaging.RabbitMq;
using Ordering.Processing.Consumers;
using Ordering.Processing.Processors;

namespace Ordering.Processing;

public static class E
{
    public static IServiceCollection AddWOrker(this IServiceCollection services, IConfiguration configuration)
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
