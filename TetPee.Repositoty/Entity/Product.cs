using TetPee.Repositoty.Abstraction;

namespace TetPee.Repositoty.Entity;

public class Product: BaseEntity<Guid>, IAuditableEntity
{
    public required string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string UrlImage { get; set; }
    
    public Guid SellerId { get; set; } 
    public Seller Seller { get; set; }
    
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public ICollection<ProductCate> ProductCate { get; set; } = new List<ProductCate>();
    
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}