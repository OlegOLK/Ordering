using Ordering.Domain.Models;

namespace Ordering.Persistance.Repositories;

/// <summary>
/// IOrderRepository
/// </summary>
/// <seealso cref="IRepository&lt;OrderProcessing.Aggregates.Orders.Models.Order&gt;" />
public interface IOrderRepository : IRepository<Order>
{
    Order Add(Order order);
}