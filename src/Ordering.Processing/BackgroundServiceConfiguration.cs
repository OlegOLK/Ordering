namespace Ordering.Processing;

/// <summary>
/// BackgroundServiceConfiguration
/// </summary>
public class BackgroundServiceConfiguration
{
    /// <summary>
    /// The configuration section
    /// </summary>
    public const string ConfigurationSection = "BackgroundWorker";

    /// <summary>
    /// Gets or sets the trigger interval.
    /// </summary>
    /// <value>
    /// The trigger interval.
    /// </value>
    public TimeSpan TriggerInterval { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Gets or sets the pickup seconds.
    /// </summary>
    /// <value>
    /// The pickup seconds.
    /// </value>
    public TimeSpan PickupInterval { get; set; } = TimeSpan.FromSeconds(10);
}
