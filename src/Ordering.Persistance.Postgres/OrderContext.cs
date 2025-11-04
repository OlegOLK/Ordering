using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Domain.Models;
using Ordering.Persistance;
using Ordering.Persistance.Postgres.EntityConfigurations;
using System.Data;

namespace Ordering.Persistance.Postgres;

/// <summary>
/// OrderContext
/// </summary>
/// <seealso cref="DbContext" />
public class OrderContext : DbContext, IUnitOfWork, IDbContext
{
    /// <summary>
    /// Gets or sets the orders.
    /// </summary>
    /// <value>
    /// The orders.
    /// </value>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Gets or sets the order items.
    /// </summary>
    /// <value>
    /// The order items.
    /// </value>
    public DbSet<OrderItem> OrderItems { get; set; }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    /// <value>
    /// The unit of work.
    /// </value>
    public IUnitOfWork UnitOfWork => this;

    /// <summary>
    /// The current transaction
    /// </summary>
    private IDbContextTransaction? _currentTransaction;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets the current transaction.
    /// </summary>
    /// <returns></returns>
    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

    /// <summary>
    /// Override this method to further configure the model that was discovered by convention from the entity types
    /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
    /// and re-used for subsequent instances of your derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
    /// define extension methods on this object that allow you to configure aspects of the model that are specific
    /// to a given database.</param>
    /// <remarks>
    /// <para>
    /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
    /// then this method will not be run. However, it will still run when creating a compiled model.
    /// </para>
    /// <para>
    /// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information and
    /// examples.
    /// </para>
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
    }

    /// <summary>
    /// Saves the entities asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Begins the transaction asynchronous.
    /// </summary>
    /// <param name="cancellation">The cancellation.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Another transaction is open with id: {_currentTransaction.TransactionId}</exception>
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellation = default)
    {
        if (_currentTransaction is not null)
        {
            throw new InvalidOperationException($"Another transaction is open with id: {_currentTransaction.TransactionId}");
        }

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellation);

        return _currentTransaction;
    }

    /// <summary>
    /// Commits the transaction asynchronous.
    /// </summary>
    /// <param name="transaction">The transaction.</param>
    /// <param name="cancellation">The cancellation.</param>
    /// <exception cref="ArgumentNullException">transaction</exception>
    /// <exception cref="InvalidOperationException">Transaction {transaction.TransactionId} is not current</exception>
    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (transaction != _currentTransaction)
        {
            throw new InvalidOperationException($"Another transaction is in progress with id: {transaction.TransactionId}");
        }

        try
        {
            await SaveChangesAsync(cancellation);
            await transaction.CommitAsync(cancellation);
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return Database.CreateExecutionStrategy();
    }
}
