using System.Text.Json.Serialization;

namespace Ordering.Domain.Messaging.Messages;

/// <summary>
/// CreateOrderMessage
/// </summary>
/// <seealso cref="BaseMessage" />
public class CreateOrderMessage : BaseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderMessage"/> class.
    /// </summary>
    protected CreateOrderMessage(){ }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderMessage" /> class.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="eventId">The event identifier.</param>
    public CreateOrderMessage(int orderId, Guid eventId)
    {
        OrderId = orderId;
        EventId = eventId;
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
    /// Gets or sets the event identifier.
    /// </summary>
    /// <value>
    /// The event identifier.
    /// </value>
    [JsonInclude]
    public Guid EventId { get; set; }
}
