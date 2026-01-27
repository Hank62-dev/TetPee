namespace TetPee.Repositoty.Abstraction;

public interface IAuditableEntity
{
    public DateTimeOffset CreatedAt { get; set; } // dong du lieu nay tao ra khi nao
    public DateTimeOffset? UpdatedAt { get; set; }//dòng dữ liệu này cập nhật lan cuối khi nào
}