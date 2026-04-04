using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TetPee.Repository;
using TetPee.Repository.Entity;

namespace TetPee.Service.Cart;

public class Service : IService
{
    
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Service(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateCart()
    {
        var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type =="UserId")?.Value;
        
        var userIdGuidId =  Guid.Parse(userId!);

        var query = _dbContext.Carts.Where(x=>x.UserId == userIdGuidId);
        var cartDetail = await query.AnyAsync();
        if (cartDetail)
        {
            throw new Exception("Cart exists");
        }
        
        var cart = new Repository.Entity.Cart()
        {
            UserId = userIdGuidId,
        };
        
        
        _dbContext.Carts.Add(cart);
        await  _dbContext.SaveChangesAsync();
    }

    public async Task AddProductToCart(Request.AddProductToCartRequest request)
    {
        var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type =="UserId")?.Value;
        
        var userIdGuidId =  Guid.Parse(userId!);

        var query = _dbContext.Carts.Where(x=>x.UserId == userIdGuidId);
        var cart = query.FirstOrDefault();
        if (cart == null)
        {
            cart = new Repository.Entity.Cart()
            {
                UserId = userIdGuidId,
            };
        
        
            _dbContext.Carts.Add(cart);
            await  _dbContext.SaveChangesAsync();
        }
        
        var productQuery = _dbContext.CartDetails.Where(
            x=>x.CartId == cart.Id && x.ProductId == request.ProductId);
        var cartExitst = productQuery.FirstOrDefault();
        if (cartExitst != null)
        {
            cartExitst.Quantity += request.Quantity;
            _dbContext.CartDetails.Update(cartExitst);
            await  _dbContext.SaveChangesAsync();
            return;
        }
        
        var cartDetail = new CartDetail()
        {
            CartId = userIdGuidId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
        };
        _dbContext.CartDetails.Add(cartDetail);
        await  _dbContext.SaveChangesAsync();
    }

    public async Task RemoveProductFromCart(Request.RemoveProductFromCartRequest request)
    {
        var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type =="UserId")?.Value;
        var userIdGuidId =  Guid.Parse(userId!);

        var query = _dbContext.CartDetails.Where(
            x => x.Cart.UserId == userIdGuidId &&  x.ProductId == request.ProductId);
        
        var cartDetail = query.FirstOrDefault();

        if (cartDetail == null)
        {
            throw new Exception("Product not found");
        }
        _dbContext.Remove(cartDetail);
        await  _dbContext.SaveChangesAsync();
    }

    public async Task<List<Response.ProductResponse>> GetCart()
    {
        var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

        var userIdGuid = Guid.Parse(userId!);

        var query = _dbContext.CartDetails.Where(x => x.Cart.UserId == userIdGuid)
            .Select(x => new Response.ProductResponse()
            {
                Name = x.Product.Name,
                Description = x.Product.Description,
                Price = x.Product.Price,
                Url = x.Product.UrlImage,
                Quantity = x.Quantity
            });

        var result = await query.ToListAsync();

        return result;
    }
}