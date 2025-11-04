using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Domain.Eventing;
using Ordering.Domain.Eventing.Events;

namespace Ordering.Persistance.EventStreaming;

public interface IEventStreamingService
{
    Task<IEnumerable<OutboxEvent>> GetPendingEvents(Guid transactionId);
    Task SaveEventAsync(OutboxEvent @event, IDbContextTransaction transaction);

    Task PropageEvent(Guid eventId, EventState eventState);
}
