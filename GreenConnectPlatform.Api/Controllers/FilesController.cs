using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/files")]
[Authorize]
[Tags("9. Files")]
public class FilesController : ControllerBase
{
    private readonly IStorageService _storageService;

    public FilesController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    /// <summary>
    ///     Get upload URL for User Avatar.
    /// </summary>
    [HttpPost("upload-url/avatar")]
    public async Task<ActionResult<FileUploadResponse>> UploadAvatar([FromBody] FileUploadBaseRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateAvatarUploadUrlAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    ///     Get upload URL for ID Card/License verification.
    /// </summary>
    [HttpPost("upload-url/verification")]
    public async Task<ActionResult<FileUploadResponse>> UploadVerification([FromBody] FileUploadBaseRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateVerificationUploadUrlAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    ///     Get upload URL for Scrap Post images.
    /// </summary>
    [HttpPost("upload-url/scrap-post")]
    public async Task<ActionResult<FileUploadResponse>> UploadScrapPost([FromBody] EntityFileUploadRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateScrapPostUploadUrlAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    ///     Get upload URL for Transaction Check-in (Proof of presence).
    /// </summary>
    [HttpPost("upload-url/check-in")]
    public async Task<ActionResult<FileUploadResponse>> UploadCheckIn([FromBody] EntityFileUploadRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateCheckInUploadUrlAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    ///     Delete a file owned by the user.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteFile([FromBody] DeleteFileRequest request)
    {
        var userId = GetCurrentUserId();
        await _storageService.DeleteFileAsync(userId, request.FilePath);
        return NoContent();
    }

    // --- Helper ---
    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}