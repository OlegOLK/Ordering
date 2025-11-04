using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Domain.Eventing;
using Ordering.Domain.Eventing.Events;
using Ordering.Persistance.Repositories;
using System.Text.Json;

namespace Ordering.Persistance.EventStreaming;

/// <summary>
/// EventStreamingService
/// </summary>
/// <seealso cref="Ordering.Persistance.EventStreaming.IEventStreamingService" />
public class EventStreamingService : IEventStreamingService
{
    /// <summary>
    /// The outbox event repository
    /// </summary>
    private readonly IOutboxEventRepository _outboxEventRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStreamingService"/> class.
    /// </summary>
    /// <param name="outboxEventRepository">The outbox event repository.</param>
    public EventStreamingService(IOutboxEventRepository outboxEventRepository)
    {
        _outboxEventRepository = outboxEventRepository;
    }

    /// <summary>
    /// Gets the pending events.
    /// </summary>
    /// <param name="transactionId">The transaction identifier.</param>
    /// <returns></returns>
    public async Task<IEnumerable<OutboxEvent>> GetPendingEvents(Guid transactionId)
    {
        List<OutboxEventEntity> entities = await _outboxEventRepository
             .GetAll()
             .Where(x => x.EventState == Domain.Eventing.EventState.Added && x.TransactionId == transactionId)
             .ToListAsync();

        if (!entities.Any())
        {
            return [];
        }

        return entities.Select(x => JsonSerializer.Deserialize<OutboxEvent>(x.Content))!;
    }

    /// <summary>
    /// Propages the event.
    /// </summary>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="eventState">State of the event.</param>
    /// <returns></returns>
    public Task PropageEvent(Guid eventId, EventState eventState)
    {
        OutboxEventEntity entity = _outboxEventRepository.GetAll().Where(x => x.Id == eventId).Single();
        switch (eventState)
        {
            case EventState.InProgress:
            case EventState.Completed:
                {
                    entity.EventState = eventState;
                    break;
                }
            case EventState.Failed:
                {
                    entity.EventState = EventState.Added;
                    entity.RetryCount += 1;
                    break;
                }
        }

        return _outboxEventRepository.UnitOfWork.SaveEntitiesAsync();
    }

    /// <summary>
    /// Saves the event asynchronous.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public Task SaveEventAsync(OutboxEvent @event, IDbContextTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        OutboxEventEntity outboxEventEntity = new OutboxEventEntity(@event, transaction.TransactionId);

        _outboxEventRepository.Add(outboxEventEntity);

        return _outboxEventRepository.UnitOfWork.SaveEntitiesAsync();
    }
}
