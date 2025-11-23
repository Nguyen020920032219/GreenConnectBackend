using System.Security.Claims;
using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.CollectionOffers;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/offers")]
[ApiController]
[Tags("5. Collection Offers")]
public class CollectionOfferController : ControllerBase
{
    private readonly ICollectionOfferService _service;

    public CollectionOfferController(ICollectionOfferService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (Collector) Lấy lịch sử các đề nghị thu gom đã gửi.
    /// </summary>
    /// <remarks>
    ///     Dùng để Collector xem lại các Offer mình đã gửi. <br />
    ///     Có thể lọc theo trạng thái (VD: Chỉ xem các Offer đang `Pending` hoặc đã `Accepted`).
    /// </remarks>
    /// <param name="status">Trạng thái Offer (Pending, Accepted, Rejected, Canceled).</param>
    /// <param name="sortByCreateAtDesc">Sắp xếp theo ngày tạo mới nhất (Mặc định: true).</param>
    /// <param name="pageNumber">Trang số mấy.</param>
    /// <param name="pageSize">Số lượng item trên mỗi trang.</param>
    /// <response code="200">Trả về danh sách phân trang `PaginatedResult`.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Người dùng không phải là Collector (Individual/Business).</response>
    [HttpGet]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(PaginatedResult<CollectionOfferOveralForCollectorModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMyOffers(
        [FromQuery] OfferStatus? status,
        [FromQuery] bool sortByCreateAtDesc = true,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        return Ok(await _service.GetByCollectorAsync(pageNumber, pageSize, status, sortByCreateAtDesc, userId));
    }

    /// <summary>
    ///     (All) Xem chi tiết một đề nghị thu gom.
    /// </summary>
    /// <remarks>
    ///     Trả về thông tin đầy đủ của Offer bao gồm: <br />
    ///     - Thông tin Collector (Người gửi). <br />
    ///     - Chi tiết giá từng loại ve chai (`OfferDetails`). <br />
    ///     - Lịch sử thương lượng giờ giấc (`ScheduleProposals`).
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <response code="200">Trả về object `CollectionOfferModel`.</response>
    /// <response code="404">Không tìm thấy Offer.</response>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CollectionOfferModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Collector) Hủy hoặc Mở lại một đề nghị.
    /// </summary>
    /// <remarks>
    ///     **Logic Toggle:** <br />
    ///     - Nếu đang `Pending` -> Chuyển thành `Canceled` (Hủy kèo). <br />
    ///     - Nếu đang `Canceled` -> Chuyển thành `Pending` (Mở lại kèo). <br />
    ///     **Lưu ý:** Không thể hủy nếu Offer đã được Household chấp nhận (`Accepted`).
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <response code="200">Thao tác thành công.</response>
    /// <response code="400">Không thể hủy Offer đã được chấp nhận.</response>
    /// <response code="403">Bạn không phải chủ nhân của Offer này.</response>
    [HttpPatch("{id:guid}/toggle-cancel")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ToggleCancel(Guid id)
    {
        var userId = GetCurrentUserId();
        await _service.ToggleCancelAsync(userId, id);
        return Ok(new { Message = "Trạng thái Offer đã được thay đổi thành công." });
    }

    /// <summary>
    ///     (Household) Chấp nhận hoặc Từ chối một đề nghị.
    /// </summary>
    /// <remarks>
    ///     **Side Effect quan trọng:** <br />
    ///     - Nếu **Accepted**: Hệ thống sẽ tự động tạo một `Transaction` (Giao dịch) mới để bắt đầu quy trình thu gom. Trạng
    ///     thái bài đăng sẽ chuyển sang `FullyBooked` (hoặc xử lý logic tương ứng). <br />
    ///     - Nếu **Rejected**: Trạng thái Offer chuyển sang `Rejected`. Collector có thể cập nhật lại giá và gửi lại.
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <param name="isAccepted">`true` = Đồng ý, `false` = Từ chối.</param>
    /// <response code="200">Thao tác thành công.</response>
    /// <response code="400">Offer không ở trạng thái Pending (Đã xử lý rồi).</response>
    /// <response code="403">Bạn không phải chủ bài đăng.</response>
    [HttpPatch("{id:guid}/process")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ProcessOffer(Guid id, [FromQuery] bool isAccepted)
    {
        var userId = GetCurrentUserId();
        await _service.ProcessOfferAsync(userId, id, isAccepted);
        return Ok(new { Message = isAccepted ? "Đã chấp nhận đề nghị. Giao dịch được tạo." : "Đã từ chối đề nghị." });
    }

    // --- NESTED DETAILS (Quản lý chi tiết giá trong Offer) ---

    /// <summary>
    ///     (Collector) Thêm báo giá cho một loại ve chai vào Offer.
    /// </summary>
    /// <remarks>
    ///     Chỉ được phép thêm khi Offer đang ở trạng thái `Pending` hoặc `Rejected` (để sửa lại báo giá). <br />
    ///     Không thể sửa Offer đã `Accepted`.
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <param name="request">Thông tin báo giá (Category, Unit Price).</param>
    /// <response code="200">Thêm thành công (Trả về Offer cập nhật).</response>
    /// <response code="400">Loại ve chai không có trong bài đăng gốc.</response>
    /// <response code="409">Loại ve chai này đã được báo giá trong Offer rồi.</response>
    [HttpPost("{id:guid}/details")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(CollectionOfferModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddDetail(Guid id, [FromBody] OfferDetailCreateModel request)
    {
        var userId = GetCurrentUserId();
        await _service.AddDetailAsync(userId, id, request);
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Collector) Cập nhật báo giá (Sửa đơn giá).
    /// </summary>
    /// <param name="id">ID của Offer.</param>
    /// <param name="detailId">ID chi tiết báo giá cần sửa.</param>
    /// <param name="request">Giá mới.</param>
    [HttpPut("{id:guid}/details/{detailId:guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(CollectionOfferModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDetail(Guid id, Guid detailId, [FromBody] OfferDetailUpdateModel request)
    {
        var userId = GetCurrentUserId();
        await _service.UpdateDetailAsync(userId, id, detailId, request);
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Collector) Xóa một mục báo giá khỏi Offer.
    /// </summary>
    /// <remarks>
    ///     **Lưu ý:** Nếu bài đăng gốc yêu cầu `MustTakeAll` (Full-lot), việc xóa item có thể khiến Offer trở nên không hợp lệ
    ///     (thiếu item). Service sẽ chặn hành động này.
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <param name="detailId">ID chi tiết báo giá cần xóa.</param>
    /// <response code="204">Xóa thành công.</response>
    /// <response code="400">Không thể xóa (Do quy tắc Full-lot hoặc Offer đã Accepted).</response>
    [HttpDelete("{id:guid}/details/{detailId:guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDetail(Guid id, Guid detailId)
    {
        var userId = GetCurrentUserId();
        await _service.DeleteDetailAsync(userId, id, detailId);
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}