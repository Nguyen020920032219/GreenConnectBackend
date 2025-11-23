using GreenConnectPlatform.Business.Models.Auth;
using GreenConnectPlatform.Business.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Tags("1. Authentication")]
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
    ///     - If the phone number from the token does not exist -> Auto-registers a new `Household` user with a default
    ///     Profile. <br />
    ///     - If the phone number exists -> Logs in. <br />
    ///     - If the user is a `Buyer` (Individual/Business) and their `Status` is `PendingVerification` -> Returns 403
    ///     (Forbidden). <br />
    ///     - If the user is `Blocked` -> Returns 403.
    /// </remarks>
    /// <param name="request">Contains the valid Firebase IdToken.</param>
    /// <response code="200">Login successful (Returns `AuthResponse`).</response>
    /// <response code="201">Registration successful (Returns `AuthResponse` for new user).</response>
    /// <response code="400">The Firebase Token is invalid or missing phone number.</response>
    /// <response code="401">Unauthorized (Invalid Token Signature).</response>
    /// <response code="403">Account is pending verification or blocked.</response>
    [HttpPost("login-or-register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> LoginOrRegister([FromBody] LoginOrRegisterRequest request)
    {
        var (authResponse, isNewUser) = await _authService.LoginOrRegisterAsync(request);

        if (isNewUser) return CreatedAtAction(nameof(ProfileController.GetMyProfile), "Profile", null, authResponse);

        return Ok(authResponse);
    }

    /// <summary>
    ///     (Admin) Logs in for the Admin Web Portal.
    /// </summary>
    /// <remarks>
    ///     The Admin logs in using their `Email` and `Password`. <br />
    ///     Supports Dev Backdoor for test accounts (`@1` password) to bypass Admin role check.
    /// </remarks>
    /// <param name="request">Contains `Email` and `Password`.</param>
    /// <response code="200">Login successful (Returns `AuthResponse` with Admin Token).</response>
    /// <response code="401">Invalid credentials (Password incorrect).</response>
    /// <response code="403">User exists but does not have Admin role (Forbidden).</response>
    /// <response code="404">User not found.</response>
    [HttpPost("admin-login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
    {
        var authResponse = await _authService.AdminLoginAsync(request);
        return Ok(authResponse);
    }
}