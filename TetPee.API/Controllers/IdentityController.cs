using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TetPee.Service.Identity;

namespace TetPee.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class IdentityController: ControllerBase
{
    private readonly IServiceIden _idenService;
    public IdentityController(IServiceIden idenService)
    {
        _idenService = idenService;
    }
    

    [HttpGet("Login")]
    public async Task<IActionResult> Login(string email, string password )
    {
        var result = await _idenService.Login(email, password);
        return Ok(result);
    }
    
}