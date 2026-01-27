using TetPee.Repositoty.Abstraction;

namespace TetPee.Repositoty.Entity;

public class Category: BaseEntity<Guid>, IAuditableEntity
{
    public required string Name { get; set; }
    
    public Guid? CategoryParentId { get; set; } 
    //nếu mà là null thì nó la thằng cha cao nhất
    //nếu mà có giá trị thì nó là thằng con của parentId    
    public Category? CategoryParent { get; set; }
    
    public Guid ProductId { get; set; }
    public ProductCate ProductCate { get; set; }
    
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<ProductCate> ProductCategory { get; set; } = new List<ProductCate>();
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}