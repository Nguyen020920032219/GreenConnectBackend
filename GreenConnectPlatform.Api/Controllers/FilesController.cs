using System.Security.Claims;
using GreenConnectPlatform.Bussiness.Models.Files;
using GreenConnectPlatform.Bussiness.Services.FileStorage;
using GreenConnectPlatform.Data.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/files")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly GreenConnectDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public FilesController(IFileStorageService fileStorageService, GreenConnectDbContext context)
    {
        _fileStorageService = fileStorageService;
        _context = context;
    }

    [HttpPost("generate-avatar-upload-url")]
    public async Task<IActionResult> GenerateAvatarUploadUrl([FromBody] FileUploadRequestModel requestModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var objectName = $"avatars/{userId}/{Guid.NewGuid()}{Path.GetExtension(requestModel.FileName)}";

        var uploadUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(objectName, requestModel.ContentType);
        return Ok(new { UploadUrl = uploadUrl, FileName = objectName });
    }

    [HttpPost("generate-scrap-image-upload-url")]
    public async Task<IActionResult> GenerateScrapImageUploadUrl([FromBody] ScrapImageUploadRequestModel requestModel)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var isOwner = await _context.ScrapPosts
            .AnyAsync(p => p.ScrapPostId == requestModel.PostId && p.HouseholdId == userId);

        if (!isOwner) return Forbid("Bạn không có quyền upload ảnh cho bài đăng này.");

        var objectName =
            $"scraps/{userId}/{requestModel.PostId}/{Guid.NewGuid()}{Path.GetExtension(requestModel.FileName)}";

        var uploadUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(objectName, requestModel.ContentType);
        return Ok(new { UploadUrl = uploadUrl, FileName = objectName });
    }

    [HttpPost("generate-verification-upload-url")]
    public async Task<IActionResult> GenerateVerificationUploadUrl([FromBody] FileUploadRequestModel requestModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var objectName = $"verifications/{userId}/{Guid.NewGuid()}{Path.GetExtension(requestModel.FileName)}";

        var uploadUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(objectName, requestModel.ContentType);
        return Ok(new { UploadUrl = uploadUrl, FileName = objectName });
    }

    [HttpPost("generate-checkin-upload-url")]
    [Authorize(Roles = "ScrapCollector")]
    public async Task<IActionResult> GenerateCheckinUploadUrl(
        [FromBody] CheckinUploadRequestModelModel requestModelModel)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var isValidCollector = await _context.Transactions
            .AnyAsync(t => t.TransactionId == requestModelModel.TransactionId && t.ScrapCollectorId == userId);

        if (!isValidCollector) return Forbid("Bạn không được chỉ định cho giao dịch này.");

        var objectName =
            $"checkins/{requestModelModel.TransactionId}/{Guid.NewGuid()}{Path.GetExtension(requestModelModel.FileName)}";

        var uploadUrl =
            await _fileStorageService.GenerateUploadSignedUrlAsync(objectName, requestModelModel.ContentType);
        return Ok(new { UploadUrl = uploadUrl, FileName = objectName });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFile([FromBody] DeleteFileRequestModel request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var pathSegments = request.FileName.Split('/');
        if (pathSegments.Length < 2) return BadRequest("Định dạng tên file không hợp lệ.");

        var ownerId = pathSegments[1];

        if (userId != ownerId) return Forbid("Bạn không có quyền xóa file này.");

        await _fileStorageService.DeleteFileAsync(request.FileName);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("admin/delete-file")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AdminDeleteFile([FromBody] DeleteFileRequestModel request)
    {
        if (string.IsNullOrEmpty(request.FileName)) return BadRequest("Tên file là bắt buộc.");

        await _fileStorageService.DeleteFileAsync(request.FileName);

        var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine($"Admin '{adminId}' đã xóa file: '{request.FileName}'");

        return NoContent();
    }
}