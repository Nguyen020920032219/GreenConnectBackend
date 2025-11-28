using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Chat;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/chat")]
[Tags("18. Chat")]
[Authorize]
public class ChatController(IChatService chatService) : ControllerBase
{
    /// <summary>
    ///     Lấy danh sách các phòng chat của người dùng hiện tại.
    /// </summary>
    /// <remarks>
    ///     Trả về danh sách các cuộc hội thoại (Chat Rooms) mà người dùng đang tham gia. <br />
    ///     Kết quả bao gồm thông tin người chat cùng (`Partner`), tin nhắn cuối cùng và số tin chưa đọc. <br />
    ///     Hỗ trợ tìm kiếm theo tên của đối phương.
    /// </remarks>
    /// <param name="name">Tên người dùng muốn tìm kiếm (Optional - Tìm theo tên partner).</param>
    /// <param name="pageNumber">Số trang hiện tại (Mặc định là 1).</param>
    /// <param name="pageSize">Số lượng phòng chat trên mỗi trang (Mặc định là 10).</param>
    /// <response code="200">Thành công. Trả về danh sách phòng chat phân trang.</response>
    /// <response code="401">Chưa đăng nhập (Unauthorized).</response>
    [HttpGet("rooms")]
    [ProducesResponseType(typeof(PaginatedResult<ChatRoomModel>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyRooms([FromQuery] string? name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        return Ok(await chatService.GetMyChatRoomAsync(userId, name,pageNumber, pageSize));
    }

    /// <summary>
    ///     Xem lịch sử tin nhắn của một phòng chat cụ thể.
    /// </summary>
    /// <remarks>
    ///     Lấy chi tiết danh sách tin nhắn trong một cuộc hội thoại dựa trên `id` phòng chat. <br />
    ///     Danh sách tin nhắn được sắp xếp theo thời gian từ mới nhất trở về trước (thường dùng để load trang đầu tiên của khung chat).
    /// </remarks>
    /// <param name="id">ID của phòng chat (ChatRoomId).</param>
    /// <param name="pageNumber">Số trang hiện tại (Dùng để load more tin nhắn cũ).</param>
    /// <param name="pageSize">Số lượng tin nhắn trên mỗi lần tải.</param>
    /// <response code="200">Thành công. Trả về danh sách tin nhắn.</response>
    /// <response code="401">Chưa đăng nhập hoặc không phải thành viên của phòng chat này.</response>
    /// <response code="404">Không tìm thấy phòng chat.</response>
    [HttpGet("rooms/{id:Guid}")]
    [ProducesResponseType(typeof(PaginatedResult<MessageModel>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessages([FromRoute] Guid id, [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await chatService.GetChatHistoryAsync(pageNumber, pageSize, id));
    }

    /// <summary>
    ///     Gửi tin nhắn mới.
    /// </summary>
    /// <remarks>
    ///     Gửi một tin nhắn văn bản đến phòng chat. <br />
    ///     Nếu gửi thành công, tin nhắn sẽ được lưu vào cơ sở dữ liệu và gửi thông báo realtime (nếu có SignalR) đến người nhận.
    /// </remarks>
    /// <param name="request">Thông tin tin nhắn (Nội dung, ID phòng chat hoặc ID người nhận...).</param>
    /// <response code="200">Gửi thành công. Trả về thông tin tin nhắn vừa tạo.</response>
    /// <response code="400">Dữ liệu không hợp lệ (Nội dung rỗng, sai ID...).</response>
    /// <response code="401">Chưa đăng nhập.</response>
    [HttpPost("sendMessage")]
    [ProducesResponseType(typeof(MessageModel),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageModel request)
    {
        var userId = GetCurrentUserId();
        var result = await chatService.SendMessageAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    ///     Đánh dấu tất cả tin nhắn trong phòng là "Đã đọc".
    /// </summary>
    /// <remarks>
    ///     API này nên được gọi khi người dùng mở giao diện chat của một phòng cụ thể. <br />
    ///     Hệ thống sẽ cập nhật trạng thái `IsRead = true` cho tất cả tin nhắn mà đối phương gửi đến trong phòng này. <br />
    ///     Giúp reset số lượng tin nhắn chưa đọc (Unread Count) về 0.
    /// </remarks>
    /// <param name="id">ID của phòng chat cần đánh dấu đã đọc.</param>
    /// <response code="204"></response>
    [HttpPatch("rooms/{id:Guid}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid id)
    {
        var userId = GetCurrentUserId();
        await chatService.MarkAllAsReadAsync(id, userId);
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}