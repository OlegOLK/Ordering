using MassTransit.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Domain.Eventing.Events;
using Ordering.Domain.Messaging.Messages;
using Ordering.Persistance.EventStreaming;
using Ordering.Persistance.Repositories;
using Ordering.Processing.Services;
using System.Text.Json;

namespace Ordering.Processing.Workers;

/// <summary>
/// RetryFailedOrdersWorker
/// </summary>
/// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService" />
public class RetryFailedOrdersWorker : BackgroundService
{
    /// <summary>
    /// The service provider
    /// </summary>
    private readonly IServiceProvider _serviceProvider;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<RetryFailedOrdersWorker> _logger;
    /// <summary>
    /// The options
    /// </summary>
    private readonly IOptions<BackgroundServiceConfiguration> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="RetryFailedOrdersWorker"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public RetryFailedOrdersWorker(
        IOptions<BackgroundServiceConfiguration> options,
        ILogger<RetryFailedOrdersWorker> logger,
        IServiceProvider serviceProvider)
    {
        _options = options;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    /// <summary>
    /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
    /// the lifetime of the long running operation(s) being performed.
    /// </summary>
    /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
    /// <remarks>
    /// See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.
    /// </remarks>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOrders(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing scheduled task.");
            }

            _logger.LogInformation("Waiting {Interval} before next run...", _options.Value.TriggerInterval);
            try
            {
                await Task.Delay(_options.Value.TriggerInterval, stoppingToken);
            }
            catch (TaskCanceledException)
            { }
        }
    }

    /// <summary>
    /// Does the work asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    protected async Task ProcessOrders(CancellationToken cancellationToken)
    {
        using AsyncServiceScope scope = _serviceProvider.CreateAsyncScope();
        IOutboxEventRepository repository = scope.ServiceProvider.GetRequiredService<IOutboxEventRepository>();
        DateTime pickupDate = DateTime.UtcNow.Add(-_options.Value.PickupInterval);
        List<OutboxEventEntity> entities = await repository
            .GetAll()
            .Where(x => x.EventState == Domain.Eventing.EventState.Failed && x.ProcessedOn < pickupDate)
            .Take(10)
            .ToListAsync();

        ICreateOrderProcessingService processingService = scope.ServiceProvider.GetRequiredService<ICreateOrderProcessingService>();

        foreach (var item in entities)
        {
            OutboxEvent @event = JsonSerializer.Deserialize<OutboxEvent>(item.Content)!;

            try
            {
                await processingService.ProcessMessage(new CreateOrderMessage(@event.OrderId, @event.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception durng message processing for Order Id {OrderId}", @event.OrderId);
                continue;
            }
           
        }
    }
}
