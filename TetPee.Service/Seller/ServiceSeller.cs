using Microsoft.EntityFrameworkCore;
using TetPee.Repository;
using TetPee.Service.MailService;

namespace TetPee.Service.Seller;

public class ServiceSeller: IServiceSeller
{
    
    private readonly AppDbContext _dbContext;
    private readonly IService _mailService;
    public ServiceSeller(AppDbContext dbContext, IService mailService)
    {
        _dbContext = dbContext;
        _mailService = mailService;
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


            await _mailService.SendMail(new MailContent()
            {
                To = request.Email,
                Subject = "Welcome to TetPee",
                Body = $"<!DOCTYPE html>\n\n<html>\n<head>\n    <meta charset=\"UTF-8\">\n    <title>Welcome to TetPee</title>\n</head>\n<body style=\"margin:0; padding:0; background-color:#f4f6f8; font-family:Arial, sans-serif;\">\n    <table align=\"center\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"padding:40px 0;\">\n        <tr>\n            <td>\n                <table align=\"center\" width=\"600\" cellpadding=\"0\" cellspacing=\"0\" style=\"background:#ffffff; border-radius:12px; overflow:hidden; box-shadow:0 4px 20px rgba(0,0,0,0.08);\">\n\n```\n                <!-- Header -->\n                <tr>\n                    <td style=\"background:linear-gradient(135deg,#6a11cb,#2575fc); padding:30px; text-align:center; color:#fff;\">\n                        <h1 style=\"margin:0; font-size:28px;\">Welcome to TetPee 🎉</h1>\n                    </td>\n                </tr>\n\n                <!-- Content -->\n                <tr>\n                    <td style=\"padding:30px;\">\n                        <h2 style=\"margin-top:0; color:#333;\">\n                            Dear {{{{FirstName}}}} {{{{LastName}}}},\n                        </h2>\n\n                        <p style=\"color:#555; font-size:16px; line-height:1.6;\">\n                            We're excited to have you join <b>TetPee</b> 🚀\n                        </p>\n\n                        <p style=\"color:#555; font-size:16px; line-height:1.6;\">\n                            Your account has been successfully created. You can now explore all features, manage your products, and enjoy our platform.\n                        </p>\n\n                        <!-- Button -->\n                        <div style=\"text-align:center; margin:30px 0;\">\n                            <a href=\"#\" style=\"background:#2575fc; color:#fff; padding:12px 24px; text-decoration:none; border-radius:8px; font-weight:bold;\">\n                                Get Started\n                            </a>\n                        </div>\n\n                        <p style=\"color:#555; font-size:14px;\">\n                            If you have any questions, feel free to contact us anytime.\n                        </p>\n\n                        <p style=\"color:#333; font-weight:bold;\">\n                            — The TetPee Team 💙\n                        </p>\n                    </td>\n                </tr>\n\n                <!-- Footer -->\n                <tr>\n                    <td style=\"background:#f1f1f1; padding:15px; text-align:center; font-size:12px; color:#888;\">\n                        © 2026 TetPee. All rights reserved.\n                    </td>\n                </tr>\n\n            </table>\n        </td>\n    </tr>\n</table>\n```\n\n</body>\n</html>\n"
                       
            });
            
            
            if(sellerResult > 0) return "Add seller success";
            
            return "Add seller fail";
        }
        return "Add seller fail";
    }
}