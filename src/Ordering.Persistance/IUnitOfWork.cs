using Microsoft.EntityFrameworkCore.Storage;

namespace Ordering.Persistance;

/// <summary>
/// IUnitOfWork
/// </summary>
/// <seealso cref="IDisposable" />
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the context transaction.
    /// </summary>
    /// <returns></returns>
    IDbContextTransaction? GetContextTransaction();

    /// <summary>
    /// Begins the transaction asynchronous.
    /// </summary>
    /// <param name="cancellation">The cancellation.</param>
    /// <returns></returns>
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Commits the transaction asynchronous.
    /// </summary>
    /// <param name="transaction">The transaction.</param>
    /// <param name="cancellation">The cancellation.</param>
    /// <returns></returns>
    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellation = default);

    /// <summary>
    /// Rollbacks the transaction.
    /// </summary>
    void RollbackTransaction();

    /// <summary>
    /// Saves the changes asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the entities asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
