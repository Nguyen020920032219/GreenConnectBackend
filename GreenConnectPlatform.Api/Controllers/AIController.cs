using System.Security.Claims;
using GreenConnectPlatform.Business.Models.AI;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Services.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/ai")]
[ApiController]
[Tags("08. AI Features (Nhận Diện Thông Minh)")]
[Authorize]
public class AIController : ControllerBase
{
    private readonly IScrapRecognitionService _aiService;

    public AIController(IScrapRecognitionService aiService)
    {
        _aiService = aiService;
    }

    /// <summary>
    ///     (One-Shot) Phân tích ảnh ve chai & Tự động lưu ảnh.
    /// </summary>
    [HttpPost("analyze-scrap")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ScrapPostAiSuggestion), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AnalyzeScrap(IFormFile image)
    {
        if (image == null || image.Length == 0)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Vui lòng tải lên một hình ảnh.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".heic", ".webp" };
        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Định dạng file không hỗ trợ (chỉ nhận jpg, png, heic, webp).");

        var userId = GetCurrentUserId();
        var result = await _aiService.AnalyzeImageAsync(image, userId);

        return Ok(result);
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(idStr, out var id))
            throw new ApiExceptionModel(StatusCodes.Status401Unauthorized, "401", "Không xác thực được người dùng.");

        return id;
    }
}