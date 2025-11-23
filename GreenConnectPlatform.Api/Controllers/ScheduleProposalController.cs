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
    ///     (Collector) Lấy lịch sử các đề xuất lịch hẹn của tôi.
    /// </summary>
    /// <remarks>
    ///     Giúp Collector xem lại các lịch hẹn mình đã gửi cho các Offer khác nhau.
    /// </remarks>
    /// <param name="status">Lọc theo trạng thái (Pending, Accepted...).</param>
    /// <param name="sortByCreateAtDesc">Sắp xếp mới nhất (Mặc định: true).</param>
    /// <param name="pageNumber">Trang số.</param>
    /// <param name="pageSize">Số lượng item.</param>
    /// <response code="200">Thành công.</response>
    [HttpGet]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(PaginatedResult<ScheduleProposalModel>), StatusCodes.Status200OK)]
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
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Collector) Tạo đề xuất lịch hẹn mới (Reschedule).
    /// </summary>
    /// <remarks>
    ///     Dùng khi Collector muốn đổi giờ hẹn khác cho một Offer. <br />
    ///     Chỉ được tạo khi Offer đang ở trạng thái `Pending`.
    /// </remarks>
    /// <param name="offerId">ID của Offer cần hẹn lịch.</param>
    /// <param name="request">Thời gian đề xuất và lời nhắn.</param>
    /// <response code="201">Tạo thành công.</response>
    /// <response code="400">Offer không ở trạng thái Pending.</response>
    [HttpPost("{offerId:guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromRoute] Guid offerId, [FromBody] ScheduleProposalCreateModel request)
    {
        var userId = GetCurrentUserId();
        var result = await _service.CreateAsync(userId, offerId, request);
        return CreatedAtAction(nameof(GetById), new { id = result.ScheduleProposalId }, result);
    }

    /// <summary>
    ///     (Collector) Cập nhật nội dung đề xuất lịch hẹn.
    /// </summary>
    /// <remarks>
    ///     Chỉ sửa được khi đề xuất chưa được chấp nhận (`Accepted`).
    /// </remarks>
    /// <param name="id">ID của đề xuất.</param>
    /// <param name="proposedTime">Thời gian mới (Optional).</param>
    /// <param name="responseMessage">Lời nhắn mới (Optional).</param>
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromQuery] DateTime? proposedTime,
        [FromQuery] string? responseMessage)
    {
        var userId = GetCurrentUserId();
        return Ok(await _service.UpdateAsync(userId, id, proposedTime, responseMessage));
    }

    /// <summary>
    ///     (Collector) Hủy hoặc Mở lại đề xuất lịch hẹn.
    /// </summary>
    /// <remarks>
    ///     Chuyển trạng thái qua lại giữa `Pending` và `Canceled`.
    /// </remarks>
    [HttpPatch("{id:guid}/toggle-cancel")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    public async Task<IActionResult> ToggleCancel(Guid id)
    {
        var userId = GetCurrentUserId();
        await _service.ToggleCancelAsync(userId, id);
        return Ok(new { Message = "Status changed successfully." });
    }

    /// <summary>
    ///     (Household) Chấp nhận hoặc Từ chối lịch hẹn.
    /// </summary>
    /// <param name="id">ID của đề xuất.</param>
    /// <param name="isAccepted">`true` = Đồng ý, `false` = Từ chối.</param>
    [HttpPatch("{id:guid}/process")]
    [Authorize(Roles = "Household")]
    public async Task<IActionResult> ProcessProposal(Guid id, [FromQuery] bool isAccepted)
    {
        var userId = GetCurrentUserId();
        await _service.ProcessProposalAsync(userId, id, isAccepted);
        return Ok(new { Message = isAccepted ? "Accepted" : "Rejected" });
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}