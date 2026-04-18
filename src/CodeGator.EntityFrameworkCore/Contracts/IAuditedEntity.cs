
namespace CodeGator.EntityFrameworkCore.Contracts;

/// <summary>
/// This interface represents audited entities with created and updated metadata.
/// </summary>
/// <remarks>
/// Classes that implement this interface automatically have their <c>CreatedAt</c>, 
/// <c>CreatedBy</c>, <c>UpdatedAt</c>, and <c>UpdatedBy</c> properties populated
/// before they are written to the database - provided the audit interceptors 
/// are registered with the data-context. 
/// </remarks>
public interface IAuditedEntity
{
    /// <summary>
    /// This property holds the UTC instant when this entity row was first persisted.
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// This property holds the identifier of the actor that created this entity.
    /// </summary>
    string CreatedBy { get; set; }

    /// <summary>
    /// This property holds the UTC instant of the last update, if the row was updated.
    /// </summary>
    DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// This property holds the identifier of the actor that last updated this entity.
    /// </summary>
    string? UpdatedBy { get; set; }
}
