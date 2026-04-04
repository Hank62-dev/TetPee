using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TetPee.Api.Extensions;
using TetPee.Repository;
using TetPee.Repository.Entity;
using TetPee.Service.Cart;
using TetPee.Service.Models;

namespace TetPee.Api.Controllers;
[Authorize(Policy = JwtExtensions.UserPolicy)]
[ApiController]
[Route("[controller]")]

public class CartController: ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IService _service;

    public CartController(AppDbContext dbContext, IService service)
    {
        _dbContext = dbContext;
        _service = service;
    }
    
    [HttpPost("")]
    public async Task<IActionResult> CreateCart()
    {
        await _service.CreateCart();
        return Ok(ApiResponseFactory.SuccessReponse(null,"Cart created", HttpContext.TraceIdentifier));
    }
    
    [HttpPost("Addproduct")]
    public async Task<IActionResult> AddProductToCart(Request.AddProductToCartRequest request)
    {
        await _service.AddProductToCart(request);
        return Ok(ApiResponseFactory.SuccessReponse("success","Add created", HttpContext.TraceIdentifier));
    }
    
    [HttpDelete("Removeproduct")]
    public async Task<IActionResult> DeleteProduct(Request.RemoveProductFromCartRequest request)
    {
        await _service.RemoveProductFromCart(request);
        return Ok(ApiResponseFactory.SuccessReponse("success","Add created", HttpContext.TraceIdentifier));
    }
    [HttpGet("Getproduct")]
    public async Task<IActionResult> GetCart(){
        var result =  await _service.GetCart();
        return Ok(ApiResponseFactory.SuccessReponse(result,"Add created", HttpContext.TraceIdentifier));
    }
}