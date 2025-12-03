using System.Security.Claims;
using GreenConnectPlatform.Business.Models.AI;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Services.AI;
using GreenConnectPlatform.Business.Services.Storage;
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
    private readonly IStorageService _storageService;

    public AIController(IScrapRecognitionService aiService, IStorageService storageService)
    {
        _aiService = aiService;
        _storageService = storageService;
    }

    /// <summary>
    ///     (All) Phân tích ảnh ve chai & Tự động lưu ảnh.
    /// </summary>
    /// <remarks>
    ///     **Quy trình xử lý (All-in-One):** <br />
    ///     1. Client gửi file ảnh lên Server. <br />
    ///     2. Server gửi ảnh sang Google Gemini để phân tích (Lấy tên, loại, giá trị...). <br />
    ///     3. Server tự động upload ảnh đó lên Firebase Storage (vào thư mục của User). <br />
    ///     4. Server trả về kết quả phân tích + đường dẫn ảnh đã lưu. <br />
    ///     <br />
    ///     **Client cần làm gì tiếp theo?** <br />
    ///     - Hiển thị thông tin AI trả về lên Form. <br />
    ///     - Dùng `SavedImageUrl` để hiện ảnh preview. <br />
    ///     - Khi bấm "Đăng bài", dùng `SavedImageFilePath` gửi vào API `CreatePost`. <br />
    ///     -> **Không cần upload lại lần nữa!**
    /// </remarks>
    /// <param name="image">File ảnh chụp ve chai.</param>
    /// <response code="200">Thành công. Trả về JSON kết quả.</response>
    /// <response code="400">File ảnh lỗi hoặc không đọc được.</response>
    [HttpPost("recognize-scrap")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ScrapRecognitionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecognizeScrap(IFormFile image)
    {
        var userId = GetCurrentUserId();

        // 1. Copy Stream ra Memory (Vì Stream chỉ đọc được 1 lần)
        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);

        // 2. Gọi AI phân tích
        memoryStream.Position = 0; // Reset đầu đọc
        // (Lưu ý: Hàm AI của bạn đang nhận IFormFile, nên cứ truyền nguyên image vào, nó sẽ tự open stream lại)
        var aiResult = await _aiService.RecognizeScrapImageAsync(image);

        // 3. Upload lên Firebase
        // (Dùng StorageService để đảm bảo đúng quy tắc đặt tên)
        var savedPath = await _storageService.UploadScrapImageDirectAsync(userId, image);

        // 4. Lấy Link xem ảnh (Signed URL)
        var viewUrl = await _storageService.GetFileReadUrlAsync(userId, "", savedPath);

        // 5. Gán vào kết quả trả về
        aiResult.SavedImageFilePath = savedPath;
        aiResult.SavedImageUrl = viewUrl;

        return Ok(aiResult);
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}