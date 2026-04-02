using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TetPee.Api.Extensions;
using TetPee.Repository;
using TetPee.Service.Seller;

namespace TetPee.Api.Controllers;

// [Authorize(Policy = JwtExtensions.AdminPolicy)]
[ApiController]
[Route("[controller]")]
public class SellerController: ControllerBase
{
    private readonly IServiceSeller _sellerService;

    public SellerController(IServiceSeller sellerService)
    {
        _sellerService = sellerService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetSellers(string? searchTerm, int pageSize = 10, int pageIndex = 1)
    {
        var result =  await _sellerService.GetSellers(searchTerm, pageSize, pageIndex);
        return Ok(result);
    }
    
    [HttpGet("{parentId}/childrens")]
    public async Task<IActionResult> GetAllChildCatesById(Guid parentId)
    {
        var result = await _sellerService.GetSellerById(parentId);
        return Ok(result);
    }
    
    
    [HttpPost("")]
    public async Task<IActionResult> CreateSeller(Request.CreateSellerRequest request)
    {
        var result =  await _sellerService.CreateSeller(request);
        return Ok(result);
    }
}