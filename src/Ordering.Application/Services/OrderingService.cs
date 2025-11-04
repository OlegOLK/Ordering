using Cortex.Mediator;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Domain.Models;
using Ordering.Domain.Services;

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
    /// Initializes a new instance of the <see cref="OrderingService" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="mediator">The mediator.</param>
    public OrderingService(
        ILogger<OrderingService> logger,
        IMediator mediator)
    {
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
        int orderId = await _mediator.SendCommandAsync<CreateOrderCommand, int>(cmd, CancellationToken.None);

        return orderId;
    }
}
