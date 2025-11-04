using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Eventing;
using Ordering.Domain.Messaging.Messages;
using Ordering.Domain.Models;
using Ordering.Persistance.EventStreaming;
using Ordering.Persistance.Repositories;
using Ordering.Processing.Exceptions;
using Ordering.Processing.Processors;
using Ordering.Processing.Services;
using System.Diagnostics.Metrics;

namespace Ordering.Processing.Consumers;

/// <summary>
/// CreateOrderMessageConsumer
/// </summary>
/// <seealso cref="MassTransit.IConsumer&lt;Ordering.Domain.Messaging.Messages.CreateOrderMessage&gt;" />
public class CreateOrderMessageConsumer : IConsumer<CreateOrderMessage>, ICreateOrderProcessingService
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
    /// The order repository
    /// </summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderMessageConsumer"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="processors">The processors.</param>
    /// <param name="outboxEventRepository">The outbox event repository.</param>
    public CreateOrderMessageConsumer(
        ILogger<CreateOrderMessageConsumer> logger,
        IEnumerable<IProcessor> processors,
        IOrderRepository orderRepository,
        IOutboxEventRepository outboxEventRepository)
    {
        _logger = logger;
        _processors = processors;
        _outboxEventRepository = outboxEventRepository;
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Consumes the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public async Task Consume(ConsumeContext<CreateOrderMessage> context)
    {
        await ProcessMessage(context.Message);
    }

    /// <summary>
    /// Processes the message.
    /// </summary>
    /// <param name="message">The message.</param>
    public async Task ProcessMessage(CreateOrderMessage message)
    {
        ProcessingContext ctx = new ProcessingContext
        {
            OrderId = message.OrderId,
            EventId = message.Id
        };

        _logger.LogInformation("Start processing message with event id {EventId} for order id {OrderId}", ctx.EventId, ctx.OrderId);

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

        OutboxEventEntity eventEntity = _outboxEventRepository.GetAll().Where(x => x.Id == message.EventId).Single();
        eventEntity.ErrorMessage = error;
        eventEntity.RetryCount += retry;
        eventEntity.EventState = state;
        eventEntity.ProcessedOn = DateTime.UtcNow;

        Order order = _orderRepository.GetOrders().Where(x => x.Id == message.OrderId).Single();

        if (state == EventState.Failed)
        {
            order.SetAsFailed();
        }
        else
        {
            order.SetAsProcessed();
        }

        _logger.LogInformation("End processing message with event id {EventId} for order id {OrderId} with status {ProcessingStatus}", ctx.EventId, ctx.OrderId, state);

        await _outboxEventRepository.UnitOfWork.SaveChangesAsync();
    }
}
