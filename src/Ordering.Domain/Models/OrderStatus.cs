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
    /// The validated
    /// </summary>
    Validated = 10,

    /// <summary>
    /// The enriched
    /// </summary>
    Enriched = 20,

    /// <summary>
    /// The processed
    /// </summary>
    Processed = 30,

    /// <summary>
    /// The cancelled
    /// </summary>
    Cancelled = 40,
}
