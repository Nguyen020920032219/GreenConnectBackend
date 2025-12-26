using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Notifications;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/notifications")]
[ApiController]
[Authorize]
[Tags("21. Notifications (Thông Báo)")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    ///     (Mobile) Đăng ký FCM Token (Device Token).
    /// </summary>
    /// <remarks>
    ///     Đây là API quan trọng nhất để nhận Push Notification. <br />
    ///     **Khi nào cần gọi API này?** <br />
    ///     1. Ngay sau khi **Login thành công**. <br />
    ///     2. Mỗi khi App **Khởi động lại** (để cập nhật Token nếu Firebase có refresh). <br />
    ///     3. Khi hàm `onTokenRefresh` của Firebase SDK được kích hoạt. <br />
    ///     <br />
    ///     **Lưu ý:** Token này sẽ được Server lưu lại và gắn với User ID hiện tại. Khi Server muốn bắn thông báo, nó sẽ tìm
    ///     Token này trong DB.
    /// </remarks>
    /// <param name="request">Chứa chuỗi `FcmToken` và thông tin nền tảng (`android`/`ios`).</param>
    /// <response code="200">Đăng ký thành công.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    [HttpPost("device-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceRequest request)
    {
        var userId = GetCurrentUserId();
        await _notificationService.RegisterDeviceAsync(userId, request);
        return Ok(new { Message = "Đăng ký thiết bị thành công." });
    }

    /// <summary>
    ///     (All) Lấy danh sách lịch sử thông báo.
    /// </summary>
    /// <remarks>
    ///     Dùng để hiển thị màn hình "Thông báo" trong App. <br />
    ///     - Kết quả trả về bao gồm cả thông báo **Đã đọc** và **Chưa đọc**. <br />
    ///     - Sắp xếp theo thời gian: **Mới nhất lên đầu**.
    /// </remarks>
    /// <param name="pageNumber">Trang hiện tại (Mặc định: 1).</param>
    /// <param name="pageSize">Số lượng thông báo mỗi lần tải (Mặc định: 20).</param>
    /// <response code="200">Thành công. Trả về danh sách có phân trang.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<NotificationModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var userId = GetCurrentUserId();
        return Ok(await _notificationService.GetMyNotificationsAsync(userId, pageNumber, pageSize));
    }

    /// <summary>
    ///     (All) Đánh dấu đã đọc một thông báo.
    /// </summary>
    /// <remarks>
    ///     Mobile App cần gọi API này khi: <br />
    ///     - Người dùng **bấm vào** một thông báo cụ thể. <br />
    ///     - Hoặc khi thông báo đó hiện lên màn hình (nếu muốn auto-read). <br />
    ///     <br />
    ///     Server sẽ cập nhật trạng thái `IsRead = true` cho thông báo này.
    /// </remarks>
    /// <param name="id">ID của thông báo (`NotificationId`).</param>
    /// <response code="204">Thành công (Không có nội dung trả về).</response>
    /// <response code="404">Không tìm thấy thông báo.</response>
    /// <response code="403">Thông báo này không phải của bạn.</response>
    [HttpPatch("{id:guid}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> MarkRead(Guid id)
    {
        var userId = GetCurrentUserId();
        await _notificationService.MarkAsReadAsync(userId, id);
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}