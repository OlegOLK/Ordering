using Ordering.Domain.Eventing.Events;
using Ordering.Persistance.EventStreaming;

namespace Ordering.Persistance.Repositories;

/// <summary>
/// IOutboxEventRepository
/// </summary>
/// <seealso cref="Ordering.Persistance.IRepository&lt;Ordering.Domain.Eventing.Events.OutboxEvent&gt;" />
public interface IOutboxEventRepository : IRepository<OutboxEventEntity>
{
    /// <summary>
    /// Adds the specified event.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <returns></returns>
    OutboxEventEntity Add(OutboxEventEntity @event);

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns></returns>
    IQueryable<OutboxEventEntity> GetAll();
}
