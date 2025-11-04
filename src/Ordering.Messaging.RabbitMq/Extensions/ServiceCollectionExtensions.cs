using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Messaging.Messages;
using Ordering.Domain.Messaging.Services;

namespace Ordering.Messaging.RabbitMq.Extensions;

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
    public static IServiceCollection RegisterMessagingRabbitMqDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<RabbitMqConfiguration>()
            .Configure<IConfiguration>((options, cfg) =>
            {
                cfg.GetSection(RabbitMqConfiguration.ConfigurationSection).Bind(options);
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        RabbitMqConfiguration rabbitMqConfiguration = configuration.GetSection(RabbitMqConfiguration.ConfigurationSection).Get<RabbitMqConfiguration>()!;
        services.AddMassTransit(x =>
        {
            List<ServiceDescriptor> descriptors = services
            .Where(x => x.ServiceType.IsGenericType && x.ServiceType.GetGenericTypeDefinition() == typeof(IConsumer<>))
            .ToList();


            List<ServiceDescriptor> consumers = services
            .Where(x => x.ServiceType == typeof(RoutingConsumer))
            .ToList();

            foreach (ServiceDescriptor item in descriptors)
            {
                x.AddConsumer(item.ImplementationType);
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfiguration!.Host, h =>
                {
                    h.Username(rabbitMqConfiguration!.Name);
                    h.Password(rabbitMqConfiguration!.Password);
                });

                foreach (var item in consumers)
                {
                    var c = item.ImplementationInstance as RoutingConsumer;
                    cfg.ReceiveEndpoint(c!.RouteEndpoint, e =>
                    {
                        e.ConfigureConsumer(context, c.Consumer);
                    });
                }
            });
        });

        services.AddTransient<IMessageService<CreateOrderMessage>, RabbitMqMessageService>();

        return services;
    }
}
