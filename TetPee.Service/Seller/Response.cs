namespace TetPee.Service.Seller;

public class Response
{
    public class GetSellerResponse: User.Response.GetAllUsersResponse
    {

        
        public string? CompanyName { get; set; } = null;
        public string? TaxCode { get; set; } = null;
    }
    
    public class GetSellerResponseById: User.Response.GetAllUsersResponse
    {

  
        public string? CompanyName { get; set; } = null;
        public string? TaxCode { get; set; } = null;
    }
}