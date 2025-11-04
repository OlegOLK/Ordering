using Cortex.Mediator.Commands;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Models;
using Ordering.Persistance;
using Ordering.Persistance.Repositories;

namespace Ordering.Application.Commands;

/// <summary>
/// CreateOrderCommandHandler
/// </summary>
/// <seealso cref="ICommandHandler&lt;CreateOrderCommand, bool&gt;" />
public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, int>
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    /// <summary>
    /// The order repository
    /// </summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// The database context
    /// </summary>
    private readonly IDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderCommandHandler" /> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="orderRepository">The order repository.</param>
    public CreateOrderCommandHandler(
        IDbContext dbContext,
        ILogger<CreateOrderCommandHandler> logger,
        IOrderRepository orderRepository)
    {
        _dbContext = dbContext;
        _logger = logger;
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handles the specified command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        Order order = new Order(command.CustomerId);
        foreach (OrderItem item in command.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.Name, item.Price, item.Quantity);
        }

        _logger.LogInformation("CreateOrderCommandHandler: Creating order: {@Order}", order);

        _orderRepository.Add(order);

        await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return order.Id;
    }
}
