using Cortex.Mediator.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Models;
using Ordering.Persistance.Repositories;

namespace Ordering.Application.Commands
{
    public class GetOrderCommandHandler : ICommandHandler<GetOrderCommand, Order>
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<GetOrderCommandHandler> _logger;

        /// <summary>
        /// The order repository
        /// </summary>
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrderCommandHandler" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="orderRepository">The order repository.</param>
        public GetOrderCommandHandler(
            ILogger<GetOrderCommandHandler> logger,
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Order> Handle(GetOrderCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetOrderCommandHandler: Getting order with Id: {@OrderId}", command.OrderId);

            Order order = _orderRepository
                .GetOrders()
                .AsNoTracking()
                .Include(x => x.Items)
                .Where(x => x.Id == command.OrderId).Single();

            return order;
        }
    }
}
