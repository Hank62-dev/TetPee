using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TetPee.Repository;
using TetPee.Repository.Entity;

namespace TetPee.Service.Product;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public Service(AppDbContext _dbContext, IHttpContextAccessor _httpContextAccessor)
    {
        this._dbContext = _dbContext;
        this._httpContextAccessor = _httpContextAccessor;
    }
    
    public async Task<string> CreateProduct(Resquest.CreateProductRequest request)
    {
        
        var sellerId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "SellerId")?.Value;
        var sellerIdGuid = Guid.Parse(sellerId!);
        
        var exitProduct = _dbContext.Products.FirstOrDefault(x => x.Name == request.Name);

        if (exitProduct != null)
        {
            throw new Exception("Name already exists");
        }

        var existingSellerQuery = _dbContext.Sellers.Where(
            x => x.Id == sellerIdGuid);
        bool isExistSeller = await existingSellerQuery.AnyAsync();
        
        if(!isExistSeller) 
            throw new Exception("Seller not exist");
        
        var product = new Repository.Entity.Product()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            // Quantity = request.Quantity,
            SellerId = sellerIdGuid,
            
            
        };

        _dbContext.Add(product);
        
        var result = await  _dbContext.SaveChangesAsync();

        
        if(request.CategoryIds != null && request.CategoryIds.Count > 0)
        {
            var productCateList = request.CategoryIds.Select(id => new ProductCategory()
            {
                CategoryId = id,
                ProductId = product.Id
            });
            _dbContext.AddRange(productCateList);
            await _dbContext.SaveChangesAsync();
        }
        
        if (result > 0)
        {
            return "Add product successful";
        }
        return "Add product fail";
    }
}