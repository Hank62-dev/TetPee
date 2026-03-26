namespace TetPee.Service.Category;

public interface IServiceCate
{
    //
    public Task<List<Response.GetCateResponse>> GetAllCates();
    
    //
    public Task<List<Response.GetCateResponse>> GetAllChildCatesById(Guid id);
}   