namespace TetPee.Service.Category;

public class Response
{
    public class GetCateResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}