using System.ComponentModel.DataAnnotations;

namespace Ordering.Persistance.Postgres;

/// <summary>
/// PostgresDbConfiguration
/// </summary>
public class PostgresDbConfiguration
{
    /// <summary>
    /// The configuration section
    /// </summary>
    public const string ConfigurationSection = "PosgresDb";

    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    /// <value>
    /// The connection string.
    /// </value>
    [Required]
    public string ConnectionString { get; set; }
    /// <summary>
    /// Gets or sets the migration assembly.
    /// </summary>
    /// <value>
    /// The migration assembly.
    /// </value>
    [Required]
    public string MigrationAssembly { get; set; }
}
