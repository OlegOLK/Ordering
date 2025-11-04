namespace Ordering.Domain.Services
{
    /// <summary>
    /// IWarehouseService
    /// </summary>
    public interface IWarehouseService
    {
        /// <summary>
        /// Gets the item quantity.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<int> GetItemQuantity(int itemId, CancellationToken cancellationToken);
    }
}
