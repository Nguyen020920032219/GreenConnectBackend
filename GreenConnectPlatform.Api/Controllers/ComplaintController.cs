using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Complaints;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.Complaints;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/complaints")]
[Tags("12. Complaints (Đơn khiếu nại)")]
public class ComplaintController(IComplaintService complaintService) : ControllerBase
{
    /// <summary>
    ///     (All) Xem danh sách khiếu nại.
    /// </summary>
    /// <remarks>
    ///     Lấy danh sách các khiếu nại trong hệ thống có hỗ trợ phân trang và lọc.
    ///     <br/>
    ///     - **Admin**: Xem được tất cả khiếu nại của hệ thống.
    ///     - **User/Collector**: Chỉ xem được lịch sử khiếu nại của chính mình (liên quan đến mình).
    /// </remarks>
    /// <param name="pageNumber">Số trang hiện tại (Mặc định: 1).</param>
    /// <param name="pageSize">Số lượng bản ghi trên một trang (Mặc định: 10).</param>
    /// <param name="sortByCreatedAt">Sắp xếp theo ngày tạo (`true`: Mới nhất, `false`: Cũ nhất).</param>
    /// <param name="sortByStatus">Lọc theo trạng thái (`Pending`, `Accepted`, `Rejected`). Để trống sẽ lấy tất cả.</param>
    /// <response code="200">Thành công. Trả về danh sách khiếu nại phân trang.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền truy cập.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedResult<ComplaintModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetComplaints([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
        [FromQuery] bool sortByCreatedAt = true, [FromQuery] ComplaintStatus? sortByStatus = null)
    {
        Guid? userId = null;
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out Guid parsedId))
        {
            userId = parsedId;
        }        
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var result = await complaintService.GetComplaints(pageNumber, pageSize, sortByCreatedAt, sortByStatus, userId, userRole);
        return Ok(result);
    }
    
    /// <summary>
    ///     (All) Xem chi tiết một khiếu nại.
    /// </summary>
    /// <remarks>
    ///     Lấy thông tin đầy đủ của một khiếu nại cụ thể dựa trên ID.
    ///     Bao gồm: Thông tin người tố cáo, người bị tố cáo, giao dịch liên quan, lý do và bằng chứng.
    /// </remarks>
    /// <param name="id">ID của khiếu nại cần xem.</param>
    /// <response code="200">Thành công. Trả về chi tiết `ComplaintModel`.</response>
    /// <response code="404">Không tìm thấy khiếu nại với ID này.</response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ComplaintModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComplaint([FromRoute] Guid id)
    {
        var result = await complaintService.GetComplaint(id);
        return Ok(result);
    }
    
    /// <summary>
    ///     (Admin) Xử lý/Duyệt khiếu nại.
    /// </summary>
    /// <remarks>
    ///     Cho phép Admin đưa ra quyết định cuối cùng về khiếu nại.
    ///     <br/>
    ///     - Nếu **Accept (true)**: Khiếu nại đúng. Hệ thống sẽ xử phạt người bị tố cáo (trừ điểm uy tín, hoàn tiền...).
    ///     - Nếu **Reject (false)**: Khiếu nại sai/không đủ bằng chứng. Hủy bỏ khiếu nại.
    /// </remarks>
    /// <param name="id">ID của khiếu nại cần xử lý.</param>
    /// <param name="isAccept">`true`: Chấp thuận khiếu nại, `false`: Từ chối khiếu nại.</param>
    /// <response code="200">Xử lý thành công.</response>
    /// <response code="400">Lỗi logic (Ví dụ: Khiếu nại đã được xử lý trước đó).</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền Admin.</response>
    /// <response code="404">Không tìm thấy khiếu nại.</response>
    [HttpPatch("{id:Guid}/process")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessComplaint([FromRoute] Guid id, [FromQuery] bool isAccept)
    {
        await complaintService.ProcessComplaintAsync(id, isAccept);
        return Ok("Xử lý khiếu nại thành công");
    }
    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }

    /// <summary>
    ///     (Household/Collector) Tạo khiếu nại mới.
    /// </summary>
    /// <remarks>
    ///     Gửi yêu cầu khiếu nại/tố cáo về một giao dịch hoặc người dùng khác.
    ///     <br/>
    ///     Ví dụ: Người thu gom không đến đúng giờ, thái độ không tốt, hoặc hủy kèo không lý do.
    ///     Yêu cầu: Phải có `TransactionId` hợp lệ.
    /// </remarks>
    /// <param name="complaintModel">Thông tin khiếu nại (Lý do, Bằng chứng, Người bị tố cáo...).</param>
    /// <response code="201">Tạo thành công. Trả về thông tin khiếu nại.</response>
    /// <response code="400">Dữ liệu không hợp lệ (Thiếu lý do, giao dịch không tồn tại...).</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền (Role không hợp lệ).</response>
    [HttpPost]
    [Authorize(Roles = "Household, IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ComplaintModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateComplaint([FromBody] ComplaintCreateModel complaintModel)
    {
        var userId = GetCurrentUserId();
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var complaint = await complaintService.CreateComplaint(userId, role, complaintModel);
        return CreatedAtAction(nameof(GetComplaint), new { id = complaint.ComplainantId }, complaint);
    }

    /// <summary>
    ///     (Household/Collector) Cập nhật thông tin khiếu nại.
    /// </summary>
    /// <remarks>
    ///     Cho phép người tạo khiếu nại chỉnh sửa lý do hoặc cập nhật lại link bằng chứng (ảnh/video).
    ///     <br/>
    ///     **Lưu ý:** Chỉ được phép sửa khi khiếu nại đang ở trạng thái `Pending` (Chưa được Admin xử lý).
    /// </remarks>
    /// <param name="id">ID của khiếu nại cần sửa.</param>
    /// <param name="reason">Lý do khiếu nại mới (Tùy chọn).</param>
    /// <param name="evidenceUrl">Đường dẫn bằng chứng mới (Tùy chọn).</param>
    /// <response code="200">Cập nhật thành công.</response>
    /// <response code="400">Không thể sửa (Do khiếu nại đã được xử lý hoặc dữ liệu sai).</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền (Không phải người tạo).</response>
    /// <response code="404">Không tìm thấy khiếu nại.</response>
    [HttpPatch("{id:Guid}")]
    [Authorize(Roles = "Household, IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ComplaintModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateComplaint([FromRoute] Guid id, string? reason, string? evidenceUrl)
    {
        var userId = GetCurrentUserId();
        var complaint = await complaintService.UpdateComplaint(id, userId, reason, evidenceUrl);
        return Ok(complaint);
    }

    /// <summary>
    ///     (Household/Collector) Mở lại khiếu nại.
    /// </summary>
    /// <remarks>
    ///     Cho phép người dùng yêu cầu Admin xem xét lại một khiếu nại đã bị từ chối hoặc đã đóng.
    ///     Thường dùng khi người dùng bổ sung thêm bằng chứng mới.
    /// </remarks>
    /// <param name="id">ID của khiếu nại cần mở lại.</param>
    /// <response code="200">Mở lại thành công. Trạng thái chuyển về `Pending`.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền (Không phải chủ sở hữu).</response>
    /// <response code="404">Không tìm thấy khiếu nại.</response>
    [HttpPatch("{id:Guid}/reopen")]
    [Authorize(Roles = "Household, IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ComplaintModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ReopenComplaint([FromRoute] Guid id)
    {
        var userId = GetCurrentUserId();
        await complaintService.ReopenComplaint(id, userId);
        return Ok("Mở lại phàn nàn thành công");
    }

}