using System.Text.Json.Serialization;

namespace Ordering.Domain.Messaging.Messages;

/// <summary>
/// BaseMessage
/// </summary>
public abstract class BaseMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseMessage" /> class.
    /// </summary>
    public BaseMessage()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

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
