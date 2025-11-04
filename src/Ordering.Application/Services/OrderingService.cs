using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Domain.Models;
using Ordering.Domain.Services;
using Ordering.Persistance.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ordering.Application.Services;

/// <summary>
/// OrderingService
/// </summary>
/// <seealso cref="Ordering.Domain.Services.IOrderingService" />
internal class OrderingService : IOrderingService
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<OrderingService> _logger;

    /// <summary>
    /// The mediator
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// The order repository
    /// </summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderingService" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="mediator">The mediator.</param>
    public OrderingService(
        IOrderRepository orderRepository,
        ILogger<OrderingService> logger,
        IMediator mediator)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Creates the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<int> CreateOrder(Order order, CancellationToken cancellationToken)
    {
        var cmd = new CreateOrderCommand(order.CustomerId, order.Items);
        int orderId = await _mediator.SendCommandAsync<CreateOrderCommand, int>(cmd, cancellationToken);

        return orderId;
    }

    public async Task<Order> GetOrder(int orderId, CancellationToken cancellationToken)
    {
        Order order = await _orderRepository
               .GetOrders()
               .AsNoTracking()
               .Include(x => x.Items)
               .Where(x => x.Id == orderId)
               .SingleAsync();

        return order;
    }
}
