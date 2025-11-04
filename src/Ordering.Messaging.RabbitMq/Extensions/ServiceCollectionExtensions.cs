using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Messaging.Messages;
using Ordering.Domain.Messaging.Services;
using Ordering.Messaging.RabbitMq;

namespace Ordering.Messaging.RabbitMq.Extensions
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

            RabbitMqConfiguration rabbitMqConfiguration = configuration.GetSection(RabbitMqConfiguration.ConfigurationSection).Get<RabbitMqConfiguration>();
            services.AddMassTransit(x =>
            {

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqConfiguration.Host, h =>
                    {
                        h.Username(rabbitMqConfiguration.Name);
                        h.Password(rabbitMqConfiguration.Password);
                    });
                });
            });

            services.AddTransient<IMessageService<CreateOrderMessage>, RabbitMqMessageService>();

            return services;
        }
    }
}
