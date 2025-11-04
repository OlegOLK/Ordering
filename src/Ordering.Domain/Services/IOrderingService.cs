using Ordering.Domain.Models;

namespace Ordering.Domain.Services;

/// <summary>
/// IOrderingService
/// </summary>
public interface IOrderingService
{
    /// <summary>
    /// Creates the order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<int> CreateOrder(Order order, CancellationToken cancellationToken);
}
