using System.Text.Json.Serialization;

namespace Ordering.Domain.Messaging.Messages;

/// <summary>
/// CreateOrderMessage
/// </summary>
/// <seealso cref="BaseMessage" />
public class CreateOrderMessage(int orderId) : BaseMessage
{
    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>
    /// The order identifier.
    /// </value>
    [JsonInclude]
    public int OrderId { get; set; } = orderId;
}
