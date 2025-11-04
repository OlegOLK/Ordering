using Ordering.Domain.Services;

namespace Ordering.Application.Services;

/// <summary>
/// DummyWarehouseService
/// </summary>
/// <seealso cref="Ordering.Domain.Services.IWarehouseService" />
internal class DummyWarehouseService : IWarehouseService
{
    /// <summary>
    /// Gets the item quantity.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public Task<int> GetItemQuantity(int itemId, CancellationToken cancellationToken)
    {
        return Task.FromResult(Random.Shared.Next(0, 10));
    }
}
