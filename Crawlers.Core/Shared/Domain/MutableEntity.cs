namespace Crawlers.Core.Shared.Domain;

public abstract class MutableEntity
{
    /// <summary>
    /// Timestamp for when this entity was inserted.
    /// </summary>
    public DateTime CreatedAt { get; set; }
        
    /// <summary>
    /// The principal user or role that inserted this entity.
    /// </summary>
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// Timestamp for when this entity was last updated.
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }
        
    /// <summary>
    /// The principal user or role that last updated this entity.
    /// </summary>
    public string? LastUpdatedBy { get; set; }
    
}