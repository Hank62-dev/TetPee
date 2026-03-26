namespace TetPee.Service.Seller;

public interface IServiceSeller
{
    public Task<Base.Response.PageResult<Response.GetSellerResponse>> GetSellers(
        string? searchTerm,
        int pageSize,
        int pageIndex);
    
    public Task<Response.GetSellerResponseById?> GetSellerById(
        Guid id
    );

    public Task<string> CreateSeller(Request.CreateSellerRequest request);
}