namespace TetPee.Service.Identity;

public interface IServiceIden
{
    public Task<Response.IdentityResponse> Login(string email, string password);
}