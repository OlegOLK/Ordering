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
internal class RabbitMqMessageService : IMessageService<CreateOrderMessage>, IConsumer<CreateOrderMessage>
{
    private readonly ILogger<RabbitMqMessageService> _logger;
    private readonly IBus _bus;
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;

    public const string QueueName = "/create-order-command";
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
        
        ISendEndpoint endpoint = await _bus.GetSendEndpoint(_endpoint);
        await endpoint.Send(message);
    }

    public async Task Consume(ConsumeContext<CreateOrderMessage> context)
    {
        await ConsumeAsync(context.Message);
    }

    public Task ConsumeAsync(CreateOrderMessage message)
    {
        _logger.LogInformation("New message recieved {@Message}", message);
        return Task.CompletedTask;
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
