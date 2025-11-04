using System.Data;

namespace Ordering.Persistance;

/// <summary>
/// IRepository
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    /// <value>
    /// The unit of work.
    /// </value>
    IUnitOfWork UnitOfWork { get; }
}
