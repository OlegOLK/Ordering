using Ordering.Persistance.EventStreaming;
using Ordering.Persistance.Repositories;

namespace Ordering.Persistance.Postgres;

internal class OutboxEventRepository : IOutboxEventRepository
{
    /// <summary>
    /// The order context
    /// </summary>
    private readonly OrderContext _orderContext;

    public OutboxEventRepository(OrderContext orderContext)
    {
        _orderContext = orderContext;
    }

    public IUnitOfWork UnitOfWork => _orderContext.UnitOfWork;

    public OutboxEventEntity Add(OutboxEventEntity @event)
    {
        return _orderContext.OutboxEvents.Add(@event).Entity;
    }

    public IQueryable<OutboxEventEntity> GetAll()
    {
        return _orderContext.OutboxEvents.Where(x => true);
    }
}
