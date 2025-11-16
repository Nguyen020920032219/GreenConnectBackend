using GreenConnectPlatform.Business.Models.Auth;
using GreenConnectPlatform.Business.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Tags("1. Auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    ///     (User) Logs in or registers a new user via Firebase ID Token.
    /// </summary>
    /// <remarks>
    ///     Receives the `FirebaseToken` (IdToken) from the Flutter app (after OTP verification). <br />
    ///     The backend verifies this token. <br />
    ///     - If the phone number from the token does not exist -> Auto-registers a new `Household` user. <br />
    ///     - If the phone number exists -> Logs in. <br />
    ///     - If the user is a `Buyer` (Individual/Business) and their `Status` is `PendingVerification` -> Returns 401.
    /// </remarks>
    /// <param name="request">Contains the valid Firebase IdToken.</param>
    /// <response code="200">Login successful (Returns `AuthResponse`).</response>
    /// <response code="201">Registration successful (Returns `AuthResponse`).</response>
    /// <response code="400">The Firebase Token is invalid.</response>
    /// <response code="401">Account is pending verification or blocked.</response>
    [HttpPost("login-or-register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginOrRegister([FromBody] LoginOrRegisterRequest request)
    {
        var (authResponse, isNewUser) = await _authService.LoginOrRegisterAsync(request);

        if (isNewUser) return CreatedAtAction(nameof(UsersController.GetMyProfile), "Users", null, authResponse);

        return Ok(authResponse);
    }

    /// <summary>
    ///     (Admin) Logs in for the Admin Web Portal.
    /// </summary>
    /// <remarks>
    ///     The Admin logs in using their `Email` and `Password`.
    /// </remarks>
    /// <param name="request">Contains `UserNameOrEmail` and `Password`.</param>
    /// <response code="200">Login successful (Returns `AuthResponse`).</response>
    /// <response code="401">Invalid credentials or the user is not an Admin.</response>
    [HttpPost("admin-login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
    {
        var authResponse = await _authService.AdminLoginAsync(request);
        return Ok(authResponse);
    }
}