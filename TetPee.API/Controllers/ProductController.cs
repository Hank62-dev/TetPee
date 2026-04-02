using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TetPee.Api.Extensions;
using TetPee.Repository;
using TetPee.Repository.Entity;
using TetPee.Service.Product;

namespace TetPee.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class ProductController: ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IService _serviceProduct;

    public ProductController(AppDbContext dbContext, IService serviceProduct)
    {
        _dbContext = dbContext;
        _serviceProduct = serviceProduct;
    }

    [Authorize(Policy = JwtExtensions.SellerPolicy)]
    [HttpPost("")]
    public async Task<IActionResult> CreateProduct([FromBody] Resquest.CreateProductRequest request)
    {
        var result = await _serviceProduct.CreateProduct(request);
        return Ok(result);
    }
}