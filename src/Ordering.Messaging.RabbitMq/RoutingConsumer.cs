namespace Ordering.Messaging.RabbitMq;

/// <summary>
/// RoutingConsumer
/// </summary>
public class RoutingConsumer
{
    /// <summary>
    /// Gets or sets the consumer.
    /// </summary>
    /// <value>
    /// The consumer.
    /// </value>
    public Type Consumer {  get; set; }
    /// <summary>
    /// Gets or sets the route endpoint.
    /// </summary>
    /// <value>
    /// The route endpoint.
    /// </value>
    public string RouteEndpoint { get; set; }
}
