using System.Text.Json.Serialization;

namespace Ordering.Domain.Models;

/// <summary>
/// OrderStatus
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    /// <summary>
    /// The created
    /// </summary>
    Created = 1,

    /// <summary>
    /// The processed
    /// </summary>
    Processed = 10,

    /// <summary>
    /// The cancelled
    /// </summary>
    Cancelled = 40,

    /// <summary>
    /// The failed
    /// </summary>
    Failed = 50
}
