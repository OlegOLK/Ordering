using Ordering.Domain.Models;

namespace Ordering.Persistance.Repositories;

/// <summary>
/// IOrderRepository
/// </summary>
/// <seealso cref="IRepository&lt;OrderProcessing.Aggregates.Orders.Models.Order&gt;" />
public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// Adds the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns></returns>
    Order Add(Order order);

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <returns></returns>
    IQueryable<Order> GetOrders();
}