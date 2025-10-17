using GreenConnectPlatform.Bussiness.Models.Auth;
using GreenConnectPlatform.Bussiness.Services.Auth;
using GreenConnectPlatform.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpGet("test-role/{email}")]
    public async Task<IActionResult> TestRole(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound($"Không tìm thấy user với email: {email}");

        var rolesFromUserManager = await _userManager.GetRolesAsync(user);

        var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");

        return Ok(new
        {
            Message = "Kết quả kiểm tra vai trò",
            UserFound = user.UserName,
            DoesAdminRoleExistInDb = adminRoleExists,
            RolesFoundByUserManager = rolesFromUserManager
        });
    }

    /// <summary>
    ///     Đăng nhập hoặc tự động đăng ký cho người dùng (Household/Collector) bằng Firebase ID Token.
    /// </summary>
    [HttpPost("login/firebase")]
    [ProducesResponseType(typeof(AuthResultModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResultModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginWithFirebase([FromBody] FirebaseLoginRequestModel request)
    {
        var result = await _authService.LoginWithFirebaseAsync(request);

        if (result.Success) return Ok(result);

        return BadRequest(result);
    }

    /// <summary>
    ///     Đăng nhập cho tài khoản Quản trị viên (Admin).
    /// </summary>
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