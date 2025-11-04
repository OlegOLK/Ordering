using Ordering.Domain.Models;
using Ordering.Domain.Services;

namespace Ordering.Processing.Processors;

public class ContextEnricherProcessor : IProcessor
{
    public int Order => 0;
    private readonly IOrderingService _orderingService;
    public ContextEnricherProcessor(IOrderingService orderingService)
    {
        _orderingService = orderingService;
    }
    public async Task Process(ProcessingContext ctx, CancellationToken cancellationToken = default)
    {
        Order order = await _orderingService.GetOrder(ctx.OrderId, cancellationToken);

        ctx.Order = order;
        return;
    }

    public Task Compensate(ProcessingContext ctx, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
