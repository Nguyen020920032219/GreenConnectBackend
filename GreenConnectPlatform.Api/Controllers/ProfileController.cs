using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly GreenConnectDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public ProfileController(GreenConnectDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    [HttpPost("avatar")]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateFileRequestModel request)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var userProfile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (userProfile == null)
        {
            userProfile = new Profile { UserId = userId, ProfileId = Guid.NewGuid() };
            _context.Profiles.Add(userProfile);
        }

        var oldAvatarFileName = userProfile.AvatarUrl;

        userProfile.AvatarUrl = request.FileName;

        await _context.SaveChangesAsync();

        if (!string.IsNullOrEmpty(oldAvatarFileName)) _ = _fileStorageService.DeleteFileAsync(oldAvatarFileName);

        return Ok(new { Message = "Cập nhật ảnh đại diện thành công." });
    }
}