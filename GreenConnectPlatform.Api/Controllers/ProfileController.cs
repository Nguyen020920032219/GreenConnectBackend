using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/profile")]
[Authorize]
[Tags("2. Profile & Verification")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    /// <summary>
    ///     (All) Get current user's full profile information.
    /// </summary>
    /// <remarks>
    ///     Retrieves detailed profile data including: <br />
    ///     - Basic info: Name, Phone, Address, Gender, DOB. <br />
    ///     - Gamification info: Point Balance, Current Rank. <br />
    ///     - Metadata: Roles, Avatar URL.
    /// </remarks>
    /// <response code="200">Returns the detailed `ProfileModel`.</response>
    /// <response code="401">Unauthorized (Token expired or missing).</response>
    /// <response code="404">Profile or User not found in database.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(ProfileModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetCurrentUserId();
        var profile = await _profileService.GetMyProfileAsync(userId);
        return Ok(profile);
    }

    /// <summary>
    ///     (All) Update personal information.
    /// </summary>
    /// <remarks>
    ///     Updates editable fields such as Full Name, Address, Gender, and Date of Birth. <br />
    ///     Any null fields in the request will be ignored (partial update).
    /// </remarks>
    /// <param name="request">The fields to be updated.</param>
    /// <response code="200">Update successful (Returns the updated `ProfileModel`).</response>
    /// <response code="400">Invalid data submitted (Validation error).</response>
    /// <response code="404">User not found.</response>
    [HttpPut("me")]
    [ProducesResponseType(typeof(ProfileModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = GetCurrentUserId();
        var updatedProfile = await _profileService.UpdateMyProfileAsync(userId, request);
        return Ok(updatedProfile);
    }

    /// <summary>
    ///     (All) Confirm and update user avatar.
    /// </summary>
    /// <remarks>
    ///     **Prerequisite:** The client must first upload the image to Storage via the `Files API` to get the file path.
    ///     <br />
    ///     This endpoint saves the file path into the user's profile and automatically deletes the old avatar (if any) to save
    ///     space.
    /// </remarks>
    /// <param name="request">Contains the `FileName` (path) of the uploaded image.</param>
    /// <response code="200">Avatar updated successfully.</response>
    /// <response code="400">Invalid file name.</response>
    [HttpPost("avatar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateFileRequestModel request)
    {
        var userId = GetCurrentUserId();
        await _profileService.UpdateAvatarAsync(userId, request);
        return Ok(new { Message = "Cập nhật ảnh đại diện thành công." });
    }

    /// <summary>
    ///     (Household) Apply to upgrade account to a Collector role.
    /// </summary>
    /// <remarks>
    ///     Only users with the `Household` role can use this. <br />
    ///     Submits verification documents (ID Card or Business License) for Admin review. <br />
    ///     **Side Effects:** <br />
    ///     - User Status changes to `PendingVerification`. <br />
    ///     - Role changes from `Household` to `IndividualCollector` or `BusinessCollector`. <br />
    ///     - Old verification images (if rejected previously) will be cleaned up.
    /// </remarks>
    /// <param name="request">Contains the `BuyerType` target and document image paths.</param>
    /// <response code="200">Application submitted successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">User not found.</response>
    [HttpPost("verification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitVerification([FromBody] SubmitVerificationRequest request)
    {
        var userId = GetCurrentUserId();
        await _profileService.SubmitVerificationAsync(userId, request);
        return Ok(new { Message = "Gửi thông tin xác minh thành công. Vui lòng chờ Admin duyệt." });
    }

    private Guid GetCurrentUserId()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdStr, out var userId)) return userId;
        throw new UnauthorizedAccessException("Invalid User Token");
    }
}