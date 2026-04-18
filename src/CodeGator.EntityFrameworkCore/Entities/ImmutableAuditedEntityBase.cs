
namespace CodeGator.EntityFrameworkCore.Entities;

/// <summary>
/// This class is the base for all immutable audited data entity types.
/// </summary>
/// <remarks>
/// Derive from this type to receive automatic population of creation audit columns when
/// <see cref="ImmutableAuditedEntityInterceptor"/> is registered on the DbContext.
/// </remarks>
[Index(
    nameof(CreatedAt),
    nameof(CreatedBy)
    )]
public abstract class ImmutableAuditedEntityBase : IImmutableAuditedEntity
{
    /// <inheritdoc/>
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc/>
    [Required]
    [Unicode]
    [MaxLength(Constants.ColumnLengths.ActorMaxLength)]
    public string CreatedBy { get; set; } = null!;
}
