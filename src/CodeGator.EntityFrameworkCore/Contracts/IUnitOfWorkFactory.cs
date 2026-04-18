
namespace CodeGator.EntityFrameworkCore.Contracts;

/// <summary>
/// This interface represents a factory for creating scoped unit-of-work instances.
/// </summary>
/// <typeparam name="TContext">The type of database context to use with
/// the unit of work.</typeparam>
/// <remarks>
/// Each call to <see cref="Create"/> should return a new unit-of-work instance
/// that owns or shares a <typeparamref name="TContext"/> according to your composition root.
/// </remarks>
public interface IUnitOfWorkFactory<out TContext>
    where TContext : DbContext
{
    /// <summary>
    /// This method creates a new unit-of-work instance for the configured context type.
    /// </summary>
    /// <returns>A new unit-of-work bound to a context instance.</returns>
    IUnitOfWork<TContext> Create();
}
