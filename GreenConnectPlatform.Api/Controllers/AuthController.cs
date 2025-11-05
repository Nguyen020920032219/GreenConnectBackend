using GreenConnectPlatform.Business.Models.Auth;
using GreenConnectPlatform.Business.Services.Auth;
using GreenConnectPlatform.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<User> _userManager;

    public AuthController(IAuthService authService, UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _authService = authService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("login/firebase")]
    [ProducesResponseType(typeof(AuthResultModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResultModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginWithFirebase([FromBody] FirebaseLoginRequestModel request)
    {
        var result = await _authService.LoginWithFirebaseAsync(request);

        if (result.Success) return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("admin/login")]
    [ProducesResponseType(typeof(AuthResultModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResultModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequestModel request)
    {
        var result = await _authService.AdminLoginAsync(request);

        if (result.Success) return Ok(result);

        return BadRequest(result);
    }
}