using Ordering.Domain.Eventing.Events;

namespace Ordering.Domain.Eventing.Services;

/// <summary>
/// IEventPublishingService
/// </summary>
public interface IEventPublishingService
{
    /// <summary>
    /// Adds the event.
    /// </summary>
    /// <param name="outboxEvent">The outbox event.</param>
    /// <returns></returns>
    Task AddEvent(OutboxEvent outboxEvent);

    /// <summary>
    /// Propagates the event.
    /// </summary>
    /// <param name="transactionId">The transaction identifier.</param>
    /// <returns></returns>
    Task PropagateEvent(Guid transactionId);
}
