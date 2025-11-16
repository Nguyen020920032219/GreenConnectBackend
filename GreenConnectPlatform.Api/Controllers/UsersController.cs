using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Tags("2. Users & Profile")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IProfileService _profileService;

    public UsersController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    private Guid GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
            throw new UnauthorizedAccessException("Cannot find User ID from token.");
        return new Guid(userIdString);
    }

    /// <summary>
    ///     (All) Get my personal profile.
    /// </summary>
    /// <remarks>
    ///     Gets the detailed information (name, phone, points, rank) of the currently logged-in user.
    /// </remarks>
    /// <response code="200">Returns the `UserViewModel`.</response>
    /// <response code="401">Unauthorized (Token expired or missing).</response>
    /// <response code="404">User not found.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetCurrentUserId();
        var userViewModel = await _profileService.GetMyProfileAsync(userId);
        return Ok(userViewModel);
    }

    /// <summary>
    ///     (All) Update my personal profile.
    /// </summary>
    /// <param name="request">The fields to be updated (FullName, Address, Gender...)</param>
    /// <response code="200">Update successful (Returns the updated `UserViewModel`).</response>
    /// <response code="400">Invalid data submitted (Validation).</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">User not found.</response>
    [HttpPut("me")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = GetCurrentUserId();
        var updatedUser = await _profileService.UpdateMyProfileAsync(userId, request);
        return Ok(updatedUser);
    }

    /// <summary>
    ///     (Household) Apply to upgrade account to a Buyer role.
    /// </summary>
    /// <remarks>
    ///     Only users with the `Household` role can call this. <br />
    ///     Submits documents (ID card / Business License) for Admin approval. <br />
    ///     The user's account status will be set to `PendingVerification`.
    /// </remarks>
    /// <param name="request">Contains the `BuyerType` (Individual/Business) and document image URLs.</param>
    /// <response code="202">Application submitted successfully, pending admin review.</response>
    /// <response code="4custom-00BadRequest">Invalid data submitted (Validation).</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden (e.g., a `Buyer` trying to apply again).</response>
    [HttpPost("verification")]
    [Authorize(Roles = "Household")] // (Only Households can apply!)
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SubmitVerification([FromBody] SubmitVerificationRequest request)
    {
        var userId = GetCurrentUserId();
        await _profileService.SubmitVerificationAsync(userId, request);

        return Accepted(new { message = "Verification submitted. Please wait for Admin approval." });
    }
}