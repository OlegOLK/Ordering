using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Eventing;
using Ordering.Domain.Messaging.Messages;
using Ordering.Persistance.EventStreaming;
using Ordering.Persistance.Repositories;
using Ordering.Processing.Exceptions;
using Ordering.Processing.Processors;
using System.Diagnostics.Metrics;

namespace Ordering.Processing.Consumers;

/// <summary>
/// CreateOrderMessageConsumer
/// </summary>
/// <seealso cref="MassTransit.IConsumer&lt;Ordering.Domain.Messaging.Messages.CreateOrderMessage&gt;" />
public class CreateOrderMessageConsumer : IConsumer<CreateOrderMessage>
{
    /// <summary>
    /// The meter
    /// </summary>
    private static readonly Meter Meter = new("Ordering.Processing");

    /// <summary>
    /// The orders processed
    /// </summary>
    private static readonly Counter<int> OrdersProcessed = Meter.CreateCounter<int>("orders_processed");

    /// <summary>
    /// The orders failed
    /// </summary>
    private static readonly Counter<int> OrdersFailed = Meter.CreateCounter<int>("orders_failed");

    /// <summary>
    /// The processors
    /// </summary>
    private readonly IEnumerable<IProcessor> _processors;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<CreateOrderMessageConsumer> _logger;
    /// <summary>
    /// The outbox event repository
    /// </summary>
    private readonly IOutboxEventRepository _outboxEventRepository;
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderMessageConsumer"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="processors">The processors.</param>
    /// <param name="outboxEventRepository">The outbox event repository.</param>
    public CreateOrderMessageConsumer(
        ILogger<CreateOrderMessageConsumer> logger,
        IEnumerable<IProcessor> processors,
        IOutboxEventRepository outboxEventRepository)
    {
        _logger = logger;
        _processors = processors;
        _outboxEventRepository = outboxEventRepository;
    }

    /// <summary>
    /// Consumes the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public async Task Consume(ConsumeContext<CreateOrderMessage> context)
    {
        ProcessingContext ctx = new ProcessingContext
        {
            OrderId = context.Message.OrderId,
            TransactionId = context.Message.Id
        };
        HashSet<IProcessor> completedSteps = new HashSet<IProcessor>();
        string error = string.Empty;
        EventState state = EventState.Completed;
        int retry = 0;
        foreach (IProcessor processor in _processors.OrderBy(x => x.Order))
        {
            try
            {
                await processor.Process(ctx);
                completedSteps.Add(processor);
            }
            catch (ProcessingException ex)
            {
                _logger.LogError(ex, "Rolling back proccessing.");
                foreach (IProcessor item in completedSteps)
                {
                    await item.Compensate(ctx);
                }
                state = EventState.Failed;
                error = ex.Message;
                retry++;
                OrdersFailed.Add(1);
                break;
            }
        }

        if (state == EventState.Completed)
        {
            OrdersProcessed.Add(1);
        }

        OutboxEventEntity eventEntity = _outboxEventRepository.GetAll().Where(x => x.Id == context.Message.EventId).Single();
        eventEntity.ErrorMessage = error;
        eventEntity.RetryCount += retry;
        eventEntity.EventState = state;

        await _outboxEventRepository.UnitOfWork.SaveEntitiesAsync();
    }
}
