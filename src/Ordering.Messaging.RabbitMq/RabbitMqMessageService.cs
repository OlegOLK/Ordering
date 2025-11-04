using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Domain.Messaging.Messages;
using Ordering.Domain.Messaging.Services;

namespace Ordering.Messaging.RabbitMq;

/// <summary>
/// RabbitMqMessageService
/// </summary>
/// <seealso cref="Ordering.Domain.Messaging.Services.IMessageService&lt;Ordering.Domain.Messaging.Messages.CreateOrderMessage&gt;" />
/// <seealso cref="MassTransit.IConsumer&lt;Ordering.Domain.Messaging.Messages.CreateOrderMessage&gt;" />
public class RabbitMqMessageService : IMessageService<CreateOrderMessage>
{
    private readonly ILogger<RabbitMqMessageService> _logger;
    private readonly IBus _bus;
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;

    public const string QueueName = "create-order-command";
    private Uri _endpoint;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqMessageService"/> class.
    /// </summary>
    /// <param name="rabbitMqConfiguration">The rabbit mq configuration.</param>
    /// <param name="bus">The bus.</param>
    /// <param name="logger">The logger.</param>
    public RabbitMqMessageService(
        IOptionsMonitor<RabbitMqConfiguration> rabbitMqConfiguration,
        IBus bus,
        ILogger<RabbitMqMessageService> logger)
    {
        _rabbitMqConfiguration = rabbitMqConfiguration.CurrentValue;
        _bus = bus;
        _logger = logger;
        _endpoint = BuildConnectionUri();
    }
    public async Task PublishAsync(CreateOrderMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        _logger.LogInformation("Start publishing message to queue with id {MessageId} for order: {OrderId}", message.Id, message.OrderId);
        ISendEndpoint endpoint = await _bus.GetSendEndpoint(_endpoint);
        await endpoint.Send(message);
        _logger.LogInformation("End sending message successfully to queue with id {MessageId} for order: {OrderId}", message.Id, message.OrderId);
    }

    protected Uri BuildConnectionUri()
    {
        if (_endpoint is null)
        {
            UriBuilder uriBuilder = new UriBuilder(_rabbitMqConfiguration.Host);
            uriBuilder.Path += QueueName;
            _endpoint = uriBuilder.Uri;
            return _endpoint;
        }

        return _endpoint;
    }
}
