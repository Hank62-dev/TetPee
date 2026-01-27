using TetPee.Repositoty.Abstraction;

namespace TetPee.Repositoty.Entity;

public class Order: BaseEntity<Guid>, IAuditableEntity
{
    public decimal Total { get; set; }
    public string Status { get; set; } = "Pending";
    public required string Address { get; set; }
    
    public Guid UserId { get; set; } 
    public User User { get; set; }
    
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}