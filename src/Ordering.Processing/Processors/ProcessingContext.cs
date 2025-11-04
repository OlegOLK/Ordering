using Ordering.Domain.Models;

namespace Ordering.Processing.Processors;

/// <summary>
/// ProcessingContext
/// </summary>
public class ProcessingContext
{
    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>
    /// The order identifier.
    /// </value>
    public int OrderId { get; set; }
    /// <summary>
    /// Gets or sets the event identifier.
    /// </summary>
    /// <value>
    /// The event identifier.
    /// </value>
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>
    /// The order.
    /// </value>
    public Order Order { get; set; }
}
