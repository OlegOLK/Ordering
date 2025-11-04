using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Eventing;
using Ordering.Domain.Eventing.Events;
using Ordering.Domain.Eventing.Services;
using Ordering.Domain.Messaging.Messages;
using Ordering.Domain.Messaging.Services;
using Ordering.Persistance;
using Ordering.Persistance.EventStreaming;

namespace Ordering.Application.Eventing.Services;

/// <summary>
/// EventPublishingService
/// </summary>
public class EventPublishingService : IEventPublishingService
{
    private readonly IDbContext _dbContext;
    private readonly IEventStreamingService _eventStreamingService;
    private readonly IMessageService<CreateOrderMessage> _messageService;

    private readonly ILogger<EventPublishingService> _logger;
    public EventPublishingService(
        IMessageService<CreateOrderMessage> messageService,
        IEventStreamingService eventStreamingService,
        ILogger<EventPublishingService> logger,
        IDbContext dbContext)
    {
        _messageService = messageService;
        _eventStreamingService =   eventStreamingService;
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task AddEvent(OutboxEvent outboxEvent)
    {
        IDbContextTransaction? transaction = _dbContext.UnitOfWork.GetContextTransaction();
        await _eventStreamingService.SaveEventAsync(outboxEvent, transaction);
    }

    public async Task PropagateEvent(Guid transactionId)
    {
        IEnumerable<OutboxEvent> pendingEvents = await _eventStreamingService.GetPendingEvents(transactionId);

        foreach (OutboxEvent @event in pendingEvents)
        {
            _logger.LogInformation("Event publishing starts for event: {IntegrationEventId}", @event.Id);

            try
            {
                await _eventStreamingService.PropageEvent(@event.Id, EventState.InProgress);
                await _messageService.PublishAsync(new CreateOrderMessage(@event.OrderId));
                await _eventStreamingService.PropageEvent(@event.Id, EventState.Completed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Event publishing fail. Event Id: {EventId}", @event.Id);
                await _eventStreamingService.PropageEvent(@event.Id, EventState.Failed);
            }
        }
    }
}
