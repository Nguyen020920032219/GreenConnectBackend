using GreenConnectPlatform.Business.Models.Auth;
using GreenConnectPlatform.Business.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login-or-register")]
    public async Task<IActionResult> LoginOrRegister([FromBody] LoginOrRegisterRequest request)
    {
        var authResponse = await _authService.LoginOrRegisterAsync(request);

        return Ok(authResponse);
    }
}