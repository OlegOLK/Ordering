using Ordering.Domain.Models;
using Ordering.Persistance;
using Ordering.Persistance.Repositories;

namespace Ordering.Persistance.Postgres;

/// <summary>
/// OrderRepository
/// </summary>
/// <seealso cref="IOrderRepository" />
internal class OrderRepository : IOrderRepository
{
    /// <summary>
    /// The order context
    /// </summary>
    private readonly OrderContext _orderContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="orderContext">The order context.</param>
    public OrderRepository(OrderContext orderContext)
    {
        _orderContext = orderContext;
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    /// <value>
    /// The unit of work.
    /// </value>
    public IUnitOfWork UnitOfWork => _orderContext.UnitOfWork;

    /// <summary>
    /// Adds the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns></returns>
    public Order Add(Order order)
    {
        return _orderContext.Orders.Add(order).Entity;
    }
}
