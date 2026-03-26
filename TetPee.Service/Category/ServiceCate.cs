using Microsoft.EntityFrameworkCore;
using TetPee.Repository;

namespace TetPee.Service.Category;

public class ServiceCate : IServiceCate
{
    
    private readonly AppDbContext _dbContext;

    public ServiceCate(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Response.GetCateResponse>> GetAllCates()
    {
        
        var query =  _dbContext.Categories.Where(x => true);
        query = query.OrderBy(x => x.Name);
        var selectedQuery = query
            .Select(x => new Response.GetCateResponse()
            {
                Id = x.Id,
                Name = x.Name,
            });
        
        var result = await selectedQuery.ToListAsync();
        return result;
    }

    public async Task<List<Response.GetCateResponse>> GetAllChildCatesById(Guid cateId)
    { 
        var query = _dbContext.Categories
            .Where(x => x.ParentId == cateId)
            .OrderBy(x => x.Name);

        var selectedQuery = query
            .Select(x => new Response.GetCateResponse()
            {
                Id = x.Id,
                Name = x.Name,
            });
        var result = await selectedQuery.ToListAsync();
        
        return result;  

    }
}