namespace TetPee.Repositoty.Entity;

public class Cart
{
    public Guid Id { get; set; }
    
    public bool IsDeleted { get; set; } = false; // soft delete, tránh xung đột khóa ngoại
    public DateTimeOffset CreatedAt { get; set; } // dong du lieu nay tao ra khi nao
    public DateTimeOffset? UpdatedAt { get; set; }//dòng dữ liệu này cập nhật lan cuối khi nào
}