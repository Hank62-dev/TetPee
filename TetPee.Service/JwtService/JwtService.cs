using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TetPee.Service.JwtService;

public class JwtService : IJwtService
{

    private readonly JwtOptions _jwtOptions = new();

    public JwtService(IConfiguration configuration)
    {
        configuration.GetSection(nameof(JwtOptions)).Bind(_jwtOptions);
        //ánh xạ du liệu ừ Appsettings vào obj JwtOption
    }


    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        //tạo 1 key để mã hoá token, sudung secretKey từ JwtOptions
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        //tạo 1 đối tợng SigningCredentials để xác ịnh thuật toán mã hoá và key sửu dụng để ký token

        var takeOptions = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer, //cai token nay dc ki - tao ra boi ai, to chuc nao
            audience: _jwtOptions.Audience, //casi token nay danh cho ai, to chuc nao
            claims: claims, //nhung thong tin ma ban muon luu tru trong token
            //thuong la thong tin ve nguoi dung nhu ID, email, vai tro,..
            //nam trong payload
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpireMinutes),// token se het han sau bao phut
            signingCredentials: signingCredentials
        );
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(takeOptions);
        //sau do goij JwtSecurityTokenHandler
            //de tao ra token duoi dang chuoi(string) tu cac thong tin da cung cap o tren
            
        return tokenString;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        throw new NotImplementedException();
    }
}