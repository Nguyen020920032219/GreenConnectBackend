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
[Tags("09. Files & Storage")]
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
    ///     **Mục đích:** Cho phép Household upload ảnh rác lên Cloud trước khi tạo bài đăng chính thức. <br />
    ///     **Quy trình:** <br />
    ///     1. Gọi API này với tên file và loại file. <br />
    ///     2. Nhận về `UploadUrl` (để upload) và `FilePath` (đường dẫn file). <br />
    ///     3. Sau khi upload thành công, dùng chuỗi `FilePath` này điền vào trường `ImageUrl` khi gọi API tạo bài đăng (`POST
    ///     /api/v1/posts`).
    /// </remarks>
    /// <param name="request">Thông tin file (Tên file, Content-Type).</param>
    /// <response code="200">Thành công. Trả về Signed URL và FilePath.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">User không phải là Household.</response>
    [HttpPost("upload-url/scrap-post")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<FileUploadResponse>> UploadScrapPost([FromBody] FileUploadBaseRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateScrapPostUploadUrlAsync(userId, request);
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
    ///     **Mục đích:** Upload ảnh bằng chứng trước khi tạo khiếu nại. <br />
    ///     **Quy trình:** <br />
    ///     1. Gọi API này để lấy `UploadUrl` và `FilePath`. <br />
    ///     2. Upload ảnh lên Cloud. <br />
    ///     3. Dùng `FilePath` gửi vào API `POST /api/v1/complaints` (trường `EvidenceUrl`).
    /// </remarks>
    /// <param name="request">Thông tin file.</param>
    /// <response code="200">Thành công. Trả về Signed URL.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    [HttpPost("upload-url/complaint")]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FileUploadResponse>> UploadComplaintImage([FromBody] FileUploadBaseRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _storageService.GenerateComplaintImageUploadUrlAsync(userId, request);
        return Ok(result);
    }
    
    /// <summary>
    ///     (Admin) Xin link upload ảnh cho Loại ve chai (Scrap Category).
    /// </summary>
    /// <remarks>
    ///     **Quy trình:** <br />
    ///     1. Admin gọi API này để lấy `UploadUrl` và `FilePath`. <br />
    ///     2. Upload ảnh binary lên `UploadUrl`. <br />
    ///     3. Dùng `FilePath` để tạo/cập nhật Category (`POST /api/v1/scrap-categories`).
    /// </remarks>
    /// <param name="request">Thông tin file (Tên, Content-Type).</param>
    /// <response code="200">Thành công.</response>
    [HttpPost("upload-url/scrap-category")]
    [Authorize(Roles = "Admin")] 
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<FileUploadResponse>> UploadScrapCategory([FromBody] FileUploadBaseRequest request)
    {
        var result = await _storageService.GenerateScrapCategoryUploadUrlAsync(request);
        return Ok(result);
    }

    // --- Helper ---
    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}