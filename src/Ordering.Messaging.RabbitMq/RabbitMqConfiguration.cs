using System.ComponentModel.DataAnnotations;

namespace Ordering.Messaging.RabbitMq;

/// <summary>
/// RabbitMqConfiguration
/// </summary>
public class RabbitMqConfiguration
{
    /// <summary>
    /// The configuration section
    /// </summary>
    public const string ConfigurationSection = "RabbitMq";

    /// <summary>
    /// Gets or sets the host.
    /// </summary>
    /// <value>
    /// The host.
    /// </value>
    [Required]
    public Uri Host { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>
    /// The password.
    /// </value>
    [Required]
    public string Password { get; set; }
}
