using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Business.Services.ScheduleProposals;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/schedules")]
[ApiController]
[Tags("6. Schedule Proposals (Negotiation)")]
public class ScheduleProposalController : ControllerBase
{
    private readonly IScheduleProposalService _service;

    public ScheduleProposalController(IScheduleProposalService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (Collector) Xem lịch sử các đề xuất lịch hẹn.
    /// </summary>
    /// <remarks>
    ///     Giúp Collector theo dõi các lịch hẹn mình đã gửi đi. <br/>
    ///     Có thể lọc xem cái nào đang `Pending` (chờ Household trả lời) hoặc đã `Accepted` (Chốt đơn).
    /// </remarks>
    /// <param name="status">Lọc theo trạng thái (Pending, Accepted, Rejected...).</param>
    /// <param name="sortByCreateAtDesc">Sắp xếp mới nhất (Mặc định: true).</param>
    /// <param name="pageNumber">Trang số.</param>
    /// <param name="pageSize">Số lượng item.</param>
    /// <response code="200">Thành công. Trả về danh sách phân trang.</response>
    [HttpGet]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(PaginatedResult<ScheduleProposalModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyProposals(
        [FromQuery] ProposalStatus? status,
        [FromQuery] bool sortByCreateAtDesc = true,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        return Ok(await _service.GetByCollectorAsync(pageNumber, pageSize, status, sortByCreateAtDesc, userId));
    }

    /// <summary>
    ///     (All) Xem chi tiết một đề xuất lịch hẹn.
    /// </summary>
    /// <param name="id">ID của Proposal.</param>
    /// <response code="200">Trả về chi tiết Proposal.</response>
    /// <response code="404">Không tìm thấy.</response>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Collector) Đề xuất lịch hẹn mới (Reschedule).
    /// </summary>
    /// <remarks>
    ///     Dùng khi Collector muốn **đổi giờ** thu gom khác so với Offer ban đầu. <br/>
    ///     **Điều kiện:** Offer gốc phải đang ở trạng thái `Pending`.
    /// </remarks>
    /// <param name="offerId">ID của Offer cần hẹn lại lịch.</param>
    /// <param name="request">Thời gian đề xuất mới và lời nhắn.</param>
    /// <response code="201">Tạo thành công.</response>
    /// <response code="400">Offer không ở trạng thái Pending (Không thể thương lượng nữa).</response>
    /// <response code="403">Bạn không phải chủ Offer này.</response>
    [HttpPost("{offerId:guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromRoute] Guid offerId, [FromBody] ScheduleProposalCreateModel request)
    {
        var userId = GetCurrentUserId();
        var result = await _service.CreateAsync(userId, offerId, request);
        return CreatedAtAction(nameof(GetById), new { id = result.ScheduleProposalId }, result);
    }

    /// <summary>
    ///     (Collector) Sửa nội dung đề xuất lịch hẹn.
    /// </summary>
    /// <remarks>
    ///     Chỉ cho phép sửa khi đề xuất vẫn đang `Pending` (Household chưa trả lời).
    /// </remarks>
    /// <param name="id">ID của Proposal.</param>
    /// <param name="proposedTime">Thời gian mới (Optional).</param>
    /// <param name="responseMessage">Lời nhắn mới (Optional).</param>
    /// <response code="200">Cập nhật thành công.</response>
    /// <response code="400">Không thể sửa vì đề xuất đã được Chấp nhận/Từ chối.</response>
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid id, [FromQuery] DateTime? proposedTime,
        [FromQuery] string? responseMessage)
    {
        var userId = GetCurrentUserId();
        return Ok(await _service.UpdateAsync(userId, id, proposedTime, responseMessage));
    }

    /// <summary>
    ///     (Collector) Hủy hoặc Mở lại đề xuất lịch.
    /// </summary>
    /// <remarks>
    ///     Chuyển đổi trạng thái giữa `Pending` <--> `Canceled`. <br/>
    ///     Dùng khi Collector lỡ gửi nhầm lịch hoặc muốn rút lại đề nghị.
    /// </remarks>
    /// <param name="id">ID của Proposal.</param>
    /// <response code="200">Thành công.</response>
    /// <response code="400">Không thể hủy đề xuất đã được Chấp nhận.</response>
    [HttpPatch("{id:guid}/toggle-cancel")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ToggleCancel(Guid id)
    {
        var userId = GetCurrentUserId();
        await _service.ToggleCancelAsync(userId, id);
        return Ok(new { Message = "Status changed successfully." });
    }

    /// <summary>
    ///     (Household) Chốt lịch (Accept) hoặc Từ chối lịch (Reject).
    /// </summary>
    /// <remarks>
    ///     **Quyết định cuối cùng của Household:** <br/>
    ///     - **Accept:** Chốt thời gian thu gom này -> Cập nhật trạng thái Offer -> Có thể dẫn đến tạo Transaction. <br/>
    ///     - **Reject:** Từ chối thời gian này -> Collector phải đề xuất giờ khác.
    /// </remarks>
    /// <param name="id">ID của Proposal.</param>
    /// <param name="isAccepted">`true` = Đồng ý, `false` = Từ chối.</param>
    /// <response code="200">Thành công.</response>
    /// <response code="400">Đề xuất không ở trạng thái Pending.</response>
    /// <response code="403">Bạn không phải chủ bài đăng của Offer này.</response>
    [HttpPatch("{id:guid}/process")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ProcessProposal(Guid id, [FromQuery] bool isAccepted)
    {
        var userId = GetCurrentUserId();
        await _service.ProcessProposalAsync(userId, id, isAccepted);
        return Ok(new { Message = isAccepted ? "Proposal Accepted" : "Proposal Rejected" });
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}