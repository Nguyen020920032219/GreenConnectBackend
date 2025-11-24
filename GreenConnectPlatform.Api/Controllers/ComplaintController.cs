using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Complaints;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.Complaints;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/complaints")]
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
    /// <param name="complaintId">ID của khiếu nại cần xem.</param>
    /// <response code="200">Thành công. Trả về chi tiết `ComplaintModel`.</response>
    /// <response code="404">Không tìm thấy khiếu nại với ID này.</response>
    [HttpGet("{complaintId:Guid}")]
    [ProducesResponseType(typeof(ComplaintModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComplaint([FromRoute] Guid complaintId)
    {
        var result = await complaintService.GetComplaint(complaintId);
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
    /// <param name="complaintId">ID của khiếu nại cần xử lý.</param>
    /// <param name="isAccept">`true`: Chấp thuận khiếu nại, `false`: Từ chối khiếu nại.</param>
    /// <response code="200">Xử lý thành công.</response>
    /// <response code="400">Lỗi logic (Ví dụ: Khiếu nại đã được xử lý trước đó).</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền Admin.</response>
    /// <response code="404">Không tìm thấy khiếu nại.</response>
    [HttpPost("{complaintId:Guid}/process")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessComplaint([FromRoute] Guid complaintId, [FromQuery] bool isAccept)
    {
        await complaintService.ProcessComplaintAsync(complaintId, isAccept);
        return Ok("Xử lý khiếu nại thành công");
    }
}