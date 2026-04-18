
namespace CodeGator.EntityFrameworkCore.Contracts;

/// <summary>
/// This interface represents a unit-of-work pattern for one EF Core DbContext.
/// </summary>
/// <typeparam name="TContext">The type of database context associated with
/// this unit of work.</typeparam>
/// <remarks>
/// <para>
/// A unit-of-work is a design pattern that manages a shared database context 
/// so that it becomes possible to create a database transaction and use that 
/// transaction across multiple repository operations - all without leaking 
/// database details in the process, or forcing the transaction back into the 
/// application or infrastructure layers. You get to create and manage the 
/// unit-of-work where you want it.
/// </para>
/// <para>
/// The pattern only works with a single data-context type. It doesn't support
/// transactions across multiple databases. Also, the underlying database 
/// connection must support transactions. These limitations are all something 
/// to consider before using this pattern.
/// </para>
/// </remarks>
public interface IUnitOfWork<out TContext> : IDisposable, IAsyncDisposable
    where TContext : DbContext
{
    /// <summary>
    /// This property exposes the database context coordinated by this unit of work.
    /// </summary>
    TContext Context { get; }

    /// <summary>
    /// This property indicates whether a database transaction is currently active.
    /// </summary>
    bool IsInTransaction { get; }

    /// <summary>
    /// This method begins a new database transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the transaction has started.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a transaction is already active.</exception>
    Task BeginTransactionAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method begins a new database transaction synchronously.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when a transaction is already active.</exception>
    void BeginTransaction();

    /// <summary>
    /// This method commits the active transaction and saves changes asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no transaction is active.</exception>
    Task<int> CommitAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method commits the active transaction and saves changes synchronously.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no transaction is active.</exception>
    int Commit();

    /// <summary>
    /// This method rolls back the active transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the rollback has finished.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no transaction is active.</exception>
    Task RollbackAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method rolls back the active transaction synchronously.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no transaction is active.</exception>
    void Rollback();
}
