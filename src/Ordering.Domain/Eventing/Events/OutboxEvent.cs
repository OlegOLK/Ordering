using System.Text.Json.Serialization;

namespace Ordering.Domain.Eventing.Events;

/// <summary>
/// OutboxEvent
/// </summary>
public class OutboxEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OutboxEvent" /> class.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    public OutboxEvent(int orderId)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        CreationDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>
    /// The order identifier.
    /// </value>
    [JsonInclude]
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    [JsonInclude]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    /// <value>
    /// The creation date.
    /// </value>
    [JsonInclude]
    public DateTime CreationDate { get; set; }
}
