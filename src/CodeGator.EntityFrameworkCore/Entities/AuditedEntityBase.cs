
namespace CodeGator.EntityFrameworkCore.Entities;

/// <summary>
/// This class is the base for audited data entities.
/// </summary>
/// <remarks>
/// Derive from this type to receive automatic population of audit columns when
/// <see cref="AuditedEntityInterceptor"/> is registered on the DbContext.
/// </remarks>
[Index(
    nameof(CreatedAt),
    nameof(CreatedBy),
    nameof(UpdatedAt),
    nameof(UpdatedBy)
    )]
public abstract class AuditedEntityBase : IAuditedEntity
{
    /// <inheritdoc/>
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc/>
    [Required]
    [Unicode]
    [MaxLength(Constants.ColumnLengths.ActorMaxLength)]
    public string CreatedBy { get; set; } = null!;

    /// <inheritdoc/>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <inheritdoc/>
    [Unicode]
    [MaxLength(Constants.ColumnLengths.ActorMaxLength)]
    public string? UpdatedBy { get; set; }
}
