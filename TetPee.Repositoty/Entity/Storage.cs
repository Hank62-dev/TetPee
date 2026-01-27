using TetPee.Repositoty.Abstraction;

namespace TetPee.Repositoty.Entity;

public class Storage: BaseEntity<Guid>, IAuditableEntity
{
    public decimal Price { get; set; }
    public required string Type { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}