using TetPee.Repositoty.Abstraction;

namespace TetPee.Repositoty.Entity;

public class ProductStorage: BaseEntity<Guid>, IAuditableEntity
{
    
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}