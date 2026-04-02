namespace TetPee.Service.Product;

public class Resquest
{
    public class CreateProductRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int Quantity { get; set; }
        

        // public Guid SellerId { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        // public required Guid SellerId { get; set; }
        
    }
}