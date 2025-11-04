using Ordering.Domain.Models;
using Ordering.Domain.Services;
using Ordering.Processing.Exceptions;

namespace Ordering.Processing.Processors;

public class ItemsAvailabilityCheckProcessor : IProcessor
{
    public int Order => 10;
    private readonly IWarehouseService _warehouseService;
    public ItemsAvailabilityCheckProcessor(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }
    public Task Compensate(ProcessingContext ctx, CancellationToken cancellationToken = default)
    {
        // Potential rollback if item is booked etc.
        return Task.CompletedTask;
    }

    public async Task Process(ProcessingContext ctx, CancellationToken cancellationToken = default)
    {
        foreach (OrderItem orderItem in ctx.Order.Items)
        {
            int quantity = await _warehouseService.GetItemQuantity(orderItem.Id, cancellationToken);
            if (quantity <= 0)
            {
                throw new ProcessingException($"Item with ID {orderItem.Id} is out of stock");
            }
        }
    }
}
