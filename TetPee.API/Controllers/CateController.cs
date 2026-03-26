using Microsoft.AspNetCore.Mvc;
using TetPee.Repository;
using TetPee.Repository.Entity;
using TetPee.Service.Category;
using TetPee.Service.User;

namespace TetPee.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class CateController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    
    private readonly IServiceCate _cateService;

    public CateController(AppDbContext dbContext, IServiceCate cateService)
    {
        _dbContext = dbContext;
        _cateService = cateService;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetAllCates()
    {
        var result = await _cateService.GetAllCates();
        return Ok(result);
    }
    
    [HttpGet("{parentId}/childrens")]
    public async Task<IActionResult> GetAllChildCatesById(Guid parentId)
    {
        var result = await _cateService.GetAllChildCatesById(parentId);
        return Ok(parentId);
    }
}