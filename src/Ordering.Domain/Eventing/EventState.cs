namespace Ordering.Domain.Eventing;

/// <summary>
/// EventState
/// </summary>
public enum EventState
{
    /// <summary>
    /// The added
    /// </summary>
    Added = 0,
    /// <summary>
    /// The in progress
    /// </summary>
    InProgress = 1,
    /// <summary>
    /// The completed
    /// </summary>
    Completed = 2,
    /// <summary>
    /// The failed
    /// </summary>
    Failed = 3,
}
