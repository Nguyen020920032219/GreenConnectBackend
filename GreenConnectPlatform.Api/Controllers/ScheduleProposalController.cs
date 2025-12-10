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
[Tags("06. Schedule Proposals (Thương Lượng Lịch Hẹn)")]
public class ScheduleProposalController : ControllerBase
{
    private readonly IScheduleProposalService _service;

    public ScheduleProposalController(IScheduleProposalService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (Collector) Xem lịch sử các đề xuất lịch hẹn của tôi.
    /// </summary>
    /// <remarks>
    ///     Giúp Người thu gom theo dõi các lịch hẹn mình đã gửi đi cho các đơn hàng khác nhau. <br />
    ///     **Bộ lọc:** <br />
    ///     - `status`: Xem các lịch đang chờ (`Pending`), đã chốt (`Accepted`) hoặc bị từ chối (`Rejected`). <br />
    ///     - `sortByCreateAtDesc`: Sắp xếp mới nhất/cũ nhất.
    /// </remarks>
    /// <param name="status">Lọc theo trạng thái đề xuất (Optional).</param>
    /// <param name="sortByCreateAtDesc">`true`: Mới nhất trước (Mặc định). `false`: Cũ nhất trước.</param>
    /// <param name="pageNumber">Trang số (Bắt đầu từ 1).</param>
    /// <param name="pageSize">Số lượng item/trang.</param>
    /// <response code="200">Thành công. Trả về danh sách phân trang.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Người dùng không phải là Collector.</response>
    [HttpGet]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(PaginatedResult<ScheduleProposalModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
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
    /// <remarks>
    ///     Lấy thông tin chi tiết bao gồm: Thời gian đề xuất, Lời nhắn, Trạng thái, và thông tin Offer liên quan.
    /// </remarks>
    /// <param name="id">ID của Proposal.</param>
    /// <response code="200">Thành công. Trả về chi tiết Proposal.</response>
    /// <response code="404">Không tìm thấy đề xuất.</response>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Collector) Tạo đề xuất lịch hẹn mới (Đổi lịch).
    /// </summary>
    /// <remarks>
    ///     Dùng khi Collector muốn **hẹn lại giờ** thu gom khác so với Offer ban đầu (Reschedule). <br />
    ///     **Điều kiện:** <br />
    ///     - Offer gốc phải đang ở trạng thái `Pending` (Chưa được Household chốt). <br />
    ///     - Nếu Offer đã chốt (`Accepted`), không thể dùng API này (phải liên hệ trực tiếp hoặc hủy kèo).
    /// </remarks>
    /// <param name="offerId">ID của Offer cần hẹn lại lịch.</param>
    /// <param name="request">Thời gian đề xuất mới và lời nhắn kèm theo.</param>
    /// <response code="201">Tạo thành công.</response>
    /// <response code="400">Offer không ở trạng thái Pending (Không thể thương lượng nữa).</response>
    /// <response code="403">Bạn không phải chủ nhân của Offer này.</response>
    [HttpPost("{offerId:guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector, Household")]
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
    ///     Cho phép sửa thời gian hoặc lời nhắn của một đề xuất đã gửi. <br />
    ///     **Lưu ý:** Chỉ sửa được khi đề xuất đó vẫn đang `Pending` (Household chưa phản hồi).
    /// </remarks>
    /// <param name="id">ID của Proposal cần sửa.</param>
    /// <param name="proposedTime">Thời gian mới (Optional).</param>
    /// <param name="responseMessage">Lời nhắn mới (Optional).</param>
    /// <response code="200">Cập nhật thành công.</response>
    /// <response code="400">Không thể sửa vì đề xuất đã được Chấp nhận hoặc Từ chối.</response>
    /// <response code="403">Không có quyền sửa đề xuất này.</response>
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
    ///     Chuyển đổi trạng thái qua lại giữa `Pending` và `Canceled`. <br />
    ///     Dùng khi Collector lỡ gửi nhầm lịch hoặc muốn rút lại đề nghị trước khi Household kịp xem.
    /// </remarks>
    /// <param name="id">ID của Proposal.</param>
    /// <response code="200">Đổi trạng thái thành công.</response>
    /// <response code="400">Không thể hủy đề xuất đã được Chấp nhận.</response>
    /// <response code="403">Không có quyền thao tác.</response>
    [HttpPatch("{id:guid}/toggle-cancel")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ToggleCancel(Guid id)
    {
        var userId = GetCurrentUserId();
        await _service.ToggleCancelAsync(userId, id);
        return Ok(new { Message = "Thay đổi trạng thái đề xuất thành công." });
    }

    /// <summary>
    ///     (Household) Chốt lịch (Đồng ý) hoặc Từ chối lịch.
    /// </summary>
    /// <remarks>
    ///     **Quyết định cuối cùng của Household:** <br />
    ///     - **Accept (`isAccepted = true`):** Đồng ý với thời gian này. Trạng thái Proposal chuyển sang `Accepted`. Các
    ///     Proposal khác (nếu có) sẽ bị hủy. <br />
    ///     - **Reject (`isAccepted = false`):** Từ chối thời gian này. Trạng thái chuyển sang `Rejected`. Collector phải đề
    ///     xuất giờ khác.
    /// </remarks>
    /// <param name="id">ID của Proposal.</param>
    /// <param name="isAccepted">`true` = Đồng ý, `false` = Từ chối.</param>
    /// <param name="responseMessage">Household gửi lí do từ chối và gợi ý thời gian để thu gom</param>
    /// <response code="200">Thao tác thành công.</response>
    /// <response code="400">Đề xuất không ở trạng thái Pending (Đã xử lý rồi).</response>
    /// <response code="403">Bạn không phải chủ bài đăng của Offer này.</response>
    [HttpPatch("{id:guid}/process")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ProcessProposal(Guid id, [FromQuery] bool isAccepted, [FromQuery] string? responseMessage)
    {
        var userId = GetCurrentUserId();
        await _service.ProcessProposalAsync(userId, id, isAccepted, responseMessage);
        return Ok(new { Message = isAccepted ? "Đã đồng ý lịch hẹn." : "Đã từ chối lịch hẹn." });
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}