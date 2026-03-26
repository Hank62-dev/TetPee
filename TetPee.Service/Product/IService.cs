namespace TetPee.Service.Product;

public interface IService
{
    public Task<string> CreateProduct(Resquest.CreateProductRequest request);
}