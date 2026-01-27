using TetPee.Repositoty.Abstraction;

namespace TetPee.Repositoty.Entity;

public class ProductCate: BaseEntity<Guid>, IAuditableEntity
{
    public Guid CategoryId { get; set; } 
    public Category Category { get; set; }
    
    public Guid ProductId { get; set; } 
    public Product Product { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}