using TetPee.Repository;

namespace TetPee.Service.Product;

public class Service : IService
{
    AppDbContext _dbContext;
    public Service(AppDbContext _dbContext)
    {
        this._dbContext = _dbContext;
    }
    
    public async Task<string> CreateProduct(Resquest.CreateProductRequest request)
    {
        var exitProduct = _dbContext.Products.FirstOrDefault(x => x.Name == request.Name);

        if (exitProduct != null)
        {
            throw new Exception("Name already exists");
        }

        var product = new Repository.Entity.Product()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            SellerId = request.SellerId
        };

        _dbContext.Add(product);
        
        var result = await  _dbContext.SaveChangesAsync();

        if (result > 0)
        {
            return "Add product successful";
        }
        return "Add product fail";
    }
}