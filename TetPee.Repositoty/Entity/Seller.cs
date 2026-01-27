using TetPee.Repositoty.Abstraction;

namespace TetPee.Repositoty.Entity;

public class Seller: BaseEntity<Guid>, IAuditableEntity
{
    public string TaxCode { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    
    public Guid UserId { get; set; } 
    public User User { get; set; }
    
    public ICollection<Product> Products { get; set; }  = new List<Product>();
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}