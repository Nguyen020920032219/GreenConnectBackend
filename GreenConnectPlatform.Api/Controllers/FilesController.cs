using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/files")]
[Authorize]
[Tags("9. Files & Storage")]
public class FilesController : ControllerBase
{
    private readonly IStorageService _storageService;

    public FilesController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    /// <summary>
    ///     (All) Xin link upload Avatar.
    /// </summary>
    /// <remarks>
    ///     **Quy trình Upload File:** <br />
    ///     1. Client gọi API này để xin link upload. <br />
    ///     2. Server trả về `UploadUrl` (Signed URL) và `FilePath` (để lưu vào DB). <br />
    ///     3. Client dùng `UploadUrl` để `PUT` file binary lên thẳng Google Cloud Storage/Firebase. <br />
    ///     4. Sau khi upload xong, Client gọi API cập nhật profile (`/api/v1/profile/avatar`) với `FilePath`.
    /// </remarks>
    /// <param name="request">Thông tin file (Tên file, Content-Type).</param>
    /// <response code="200">Thành công. Trả về Signed URL.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    [HttpPost("upload-url/avatar")]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FileUploadResponse>> UploadAvatar([FromBody] FileUploadBaseRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateAvatarUploadUrlAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    ///     (Household) Xin link upload giấy tờ xác minh (CCCD/GPKD).
    /// </summary>
    /// <remarks>
    ///     Dùng khi Household muốn nâng cấp lên Collector. <br />
    ///     Cần upload cả mặt trước và mặt sau (gọi API này 2 lần).
    /// </remarks>
    /// <param name="request">Thông tin file.</param>
    /// <response code="200">Thành công.</response>
    [HttpPost("upload-url/verification")]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<FileUploadResponse>> UploadVerification([FromBody] FileUploadBaseRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateVerificationUploadUrlAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    ///     (Household) Xin link upload ảnh cho bài đăng ve chai.
    /// </summary>
    /// <remarks>
    ///     **Yêu cầu:** User phải là người tạo ra bài đăng (`EntityId` = `ScrapPostId`). <br />
    ///     Nếu bài đăng không tồn tại hoặc không phải chính chủ, sẽ trả về lỗi 403/404.
    /// </remarks>
    /// <param name="request">Thông tin file và `EntityId` (là `ScrapPostId`).</param>
    /// <response code="200">Thành công.</response>
    /// <response code="403">Không phải chủ bài đăng.</response>
    /// <response code="404">Bài đăng không tồn tại.</response>
    [HttpPost("upload-url/scrap-post")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileUploadResponse>> UploadScrapPost([FromBody] FileUploadBaseRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateScrapPostUploadUrlAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    ///     (Collector) Xin link upload ảnh Check-in giao dịch.
    /// </summary>
    /// <remarks>
    ///     Dùng để upload ảnh chụp tại hiện trường làm bằng chứng. <br />
    ///     **Yêu cầu:** User phải là Collector được chỉ định trong giao dịch (`EntityId` = `TransactionId`).
    /// </remarks>
    /// <param name="request">Thông tin file và `EntityId` (là `TransactionId`).</param>
    /// <response code="200">Thành công.</response>
    /// <response code="403">Không phải Collector của đơn hàng này.</response>
    /// <response code="404">Giao dịch không tồn tại.</response>
    [HttpPost("upload-url/check-in")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileUploadResponse>> UploadCheckIn([FromBody] EntityFileUploadRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateCheckInUploadUrlAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    ///     (All) Xóa file đã upload.
    /// </summary>
    /// <remarks>
    ///     Chỉ cho phép xóa file thuộc sở hữu của mình (Check dựa trên đường dẫn file). <br />
    ///     **Lưu ý:** Không thể xóa ảnh Check-in (để giữ bằng chứng).
    /// </remarks>
    /// <param name="request">Đường dẫn file cần xóa (`FilePath`).</param>
    /// <response code="204">Xóa thành công.</response>
    /// <response code="403">Không có quyền xóa file này.</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteFile([FromBody] DeleteFileRequest request)
    {
        var userId = GetCurrentUserId();
        await _storageService.DeleteFileAsync(userId, request.FilePath);
        return NoContent();
    }

    /// <summary>
    ///     (All) Xin link upload ảnh bằng chứng khiếu nại.
    /// </summary>
    /// <remarks>
    ///     Dùng khi user tạo khiếu nại và muốn đính kèm ảnh. <br />
    ///     User phải là người tạo khiếu nại (`EntityId` = `ComplaintId`).
    /// </remarks>
    [HttpPost("upload-url/complaint")]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileUploadResponse>> UploadComplaintImage([FromBody] EntityFileUploadRequest request)
    {
        var userId = GetCurrentUserId();
        return Ok(await _storageService.GenerateComplaintImageUploadUrlAsync(userId, request));
    }

    // --- Helper ---
    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}