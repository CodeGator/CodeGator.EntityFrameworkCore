
namespace CodeGator.EntityFrameworkCore.Contracts;

/// <summary>
/// This interface represents audited entities that capture only creation metadata.
/// </summary>
/// <remarks>
/// Classes that implement this interface automatically have their <c>CreatedAt</c> 
/// and <c>CreatedBy</c> properties populated before they are written to the database 
/// - provided the audit interceptors are registered with the data-context. 
/// </remarks>
public interface IImmutableAuditedEntity
{
    /// <summary>
    /// This property holds the UTC instant when this entity row was first persisted.
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// This property holds the identifier of the actor that created this entity.
    /// </summary>
    string CreatedBy { get; set; }
}
