using Microsoft.EntityFrameworkCore;
using TetPee.Repository;

namespace TetPee.Service.Seller;

public class ServiceSeller: IServiceSeller
{
    
    private readonly AppDbContext _dbContext;
    public ServiceSeller(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Base.Response.PageResult<Response.GetSellerResponse>> GetSellers(string? searchTerm, int pageSize, int pageIndex)
    {
        var query = _dbContext.Sellers.Where(x => true);
        
        if(searchTerm != null)
        {
            query = query.Where(x => 
                x.User.FirstName.Contains(searchTerm) ||
                x.User.LastName.Contains(searchTerm) ||
                x.User.Email.Contains(searchTerm));
        }
        
        query = query.OrderBy(x => x.User.Email);
        
        query = query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        
        var selectedQuery = query
            .Select(x => new Response.GetSellerResponse()
            {
                Id = x.Id,
                Email = x.User.Email,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                ImageUrl = x.User.ImageUrl,
                Role = x.User.Role,
                CompanyName = x.CompanyName,
                TaxCode =  x.TaxCode,
            });

        var listResult = await selectedQuery.ToListAsync();
        var totalItems = listResult.Count();
        
        var result = new Base.Response.PageResult<Response.GetSellerResponse>()
        {
            Items = listResult,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItems = totalItems
        };
        
        return result;
    }

    public async Task<Response.GetSellerResponseById? > GetSellerById(Guid id)
    {
        var query = _dbContext.Users.Where(x => x.Id == id);
        
        var selectedQuery = query
            .Select(x => new Response.GetSellerResponseById()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ImageUrl = x.ImageUrl, 
                Role = x.Role,
            });

        var result = await selectedQuery.FirstOrDefaultAsync();

        return result;
    }

    public async Task<string> CreateSeller(Request.CreateSellerRequest request)
    {
        var exitsUser = _dbContext.Users.FirstOrDefault(x => x.Email == request.Email);

        if (exitsUser != null)
        {
            throw new Exception("Email already exists");
        }

        var user = new Repository.Entity.User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            HashedPassword = request.Password,
            Role = "Seller"
        };
        
        _dbContext.Add(user);
    
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
        {

            var seller = new Repository.Entity.Seller()
            {
                CompanyAddress = request.CompanyAddress,
                CompanyName = request.CompanyName,
                TaxCode = request.TaxCode,
                UserId = user.Id,
            };
            _dbContext.Add(seller);
    
            var sellerResult = await _dbContext.SaveChangesAsync();
            
            if(sellerResult > 0) return "Add seller success";
            
            return "Add seller fail";
        }
        return "Add seller fail";
    }
}