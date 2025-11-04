using Ordering.Domain.Models;

namespace Ordering.Processing.Processors;

public class ProcessingContext
{
    public int OrderId { get; set; }
    public Guid TransactionId { get; set; }

    public Order Order { get; set; }
}
