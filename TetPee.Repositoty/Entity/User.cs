using TetPee.Repositoty.Abstraction;

namespace TetPee.Repositoty.Entity;

public class User : BaseEntity<Guid>, IAuditableEntity
{
    public required string Email { get; set; }
    public required string HashedPassword { get; set; }
    public bool IsVerified { get; set; } = false; // khi user register, thì phaải verify email hợp lệ
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ImageUrl { get; set; }
    public string? Address { get; set; }
    public string Role { get; set; } = "User"; //User, admin, seller
    public int VerifyCode { get; set; } //Mã verify gửi về email

    public ICollection<Order> Orders { get; set; }  = new List<Order>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}