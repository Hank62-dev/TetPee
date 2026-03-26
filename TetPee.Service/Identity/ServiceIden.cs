using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TetPee.Repository;
using TetPee.Service.JwtService;

namespace TetPee.Service.Identity;

public class ServiceIden: IServiceIden
{
    private readonly IJwtService _jwtService;
    private readonly AppDbContext _dbContext;
    private readonly JwtOptions _jwtOptions = new();
    

    public ServiceIden(IJwtService jwtService, AppDbContext dbContext, IConfiguration configuration)
    {
        _jwtService = jwtService;
        _dbContext = dbContext;
        configuration.GetSection(nameof(JwtOptions)).Bind(_jwtOptions);

    }

    public async Task<Response.IdentityResponse> Login(string email, string password)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (user.HashedPassword != password)
        {
            throw new Exception("Invalid password");
        }
        
        //user nay chac chan la ton tai
        var claims = new List<Claim>
        {
            new Claim("UserID", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim("Role", user.Role),
            new Claim(ClaimTypes.Role, user.Role),
                //Phai co claim nay de phan quyen cho cac API endpoints, neu thieu claim nay thi se khong phan quyen duoc
            new Claim(ClaimTypes.Expired,
                DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.ExpireMinutes).ToString()),
        };
        
        var token = _jwtService.GenerateAccessToken(claims);

        var result =  new Response.IdentityResponse()
        {
            AccessToken = token,
        };

        return result;
    }
}