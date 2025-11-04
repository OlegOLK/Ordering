using Microsoft.EntityFrameworkCore.Storage;

namespace Ordering.Persistance;

/// <summary>
/// IDbContext
/// </summary>
public interface IDbContext
{
    /// <summary>
    /// Creates the execution strategy.
    /// </summary>
    /// <returns></returns>
    IExecutionStrategy CreateExecutionStrategy();

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    /// <value>
    /// The unit of work.
    /// </value>
    IUnitOfWork UnitOfWork { get; }    
}
