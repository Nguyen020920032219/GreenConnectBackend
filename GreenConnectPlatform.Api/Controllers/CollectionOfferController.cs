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
[Tags("5. Collection Offers (Đề Nghị Thu Gom)")]
public class CollectionOfferController : ControllerBase
{
    private readonly ICollectionOfferService _service;

    public CollectionOfferController(ICollectionOfferService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (Collector) Xem lịch sử các đề nghị thu gom đã gửi.
    /// </summary>
    /// <remarks>
    ///     Giúp Collector quản lý danh sách các Offer mình đã gửi đi. <br />
    ///     Có thể lọc theo trạng thái để xem các Offer đang chờ (`Pending`), đã chốt (`Accepted`) hoặc bị từ chối
    ///     (`Rejected`).
    /// </remarks>
    /// <param name="status">Lọc theo trạng thái Offer (Optional).</param>
    /// <param name="sortByCreateAtDesc">`true`: Mới nhất trước (Mặc định). `false`: Cũ nhất trước.</param>
    /// <param name="pageNumber">Trang số (Bắt đầu từ 1).</param>
    /// <param name="pageSize">Số lượng item/trang.</param>
    /// <response code="200">Thành công. Trả về danh sách phân trang.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Người dùng không phải là Collector.</response>
    [HttpGet]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(PaginatedResult<CollectionOfferOveralForCollectorModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
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
    ///     Trả về đầy đủ thông tin: <br />
    ///     - Thông tin người gửi (Collector). <br />
    ///     - Danh sách chi tiết giá (`OfferDetails`). <br />
    ///     - Lịch sử thương lượng giờ giấc (`ScheduleProposals`).
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <response code="200">Thành công. Trả về chi tiết Offer.</response>
    /// <response code="404">Không tìm thấy Offer.</response>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CollectionOfferModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Collector) Hủy hoặc Mở lại một đề nghị.
    /// </summary>
    /// <remarks>
    ///     **Cơ chế Toggle:** <br />
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
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
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
    ///     Đây là bước quan trọng để bắt đầu giao dịch. <br />
    ///     - **Nếu Chấp nhận (isAccepted = true):** <br />
    ///     1. Trạng thái Offer -> `Accepted`. <br />
    ///     2. Hệ thống **Tự động tạo Giao dịch (Transaction)** mới với trạng thái `Scheduled`. <br />
    ///     3. Trạng thái bài đăng gốc chuyển sang `FullyBooked` (hoặc `PartiallyBooked` tùy logic). <br />
    ///     - **Nếu Từ chối (isAccepted = false):** <br />
    ///     1. Trạng thái Offer -> `Rejected`. <br />
    ///     2. Collector có thể sửa lại giá và gửi lại (nếu muốn).
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <param name="isAccepted">`true`: Đồng ý, `false`: Từ chối.</param>
    /// <response code="200">Thao tác thành công.</response>
    /// <response code="400">Offer không ở trạng thái Pending (Đã xử lý rồi).</response>
    /// <response code="403">Bạn không phải chủ bài đăng.</response>
    [HttpPatch("{id:guid}/process")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
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
    ///     Dùng để bổ sung mặt hàng vào Offer (nếu lúc đầu quên hoặc Household thêm hàng mới). <br />
    ///     **Điều kiện:** <br />
    ///     - Offer phải đang ở trạng thái `Pending` hoặc `Rejected` (để sửa lại). <br />
    ///     - Loại ve chai này phải có trong bài đăng gốc. <br />
    ///     - Loại ve chai này chưa có trong Offer (tránh trùng lặp).
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <param name="request">Thông tin báo giá (Loại rác, Đơn giá, Đơn vị tính).</param>
    /// <response code="201">Thêm thành công (Trả về chi tiết Offer cập nhật).</response>
    /// <response code="400">Loại ve chai không hợp lệ hoặc Offer đã chốt.</response>
    /// <response code="409">Loại ve chai này đã tồn tại trong Offer.</response>
    [HttpPost("{id:guid}/details")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(CollectionOfferModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddDetail(Guid id, [FromBody] OfferDetailCreateModel request)
    {
        var userId = GetCurrentUserId();
        await _service.AddDetailAsync(userId, id, request);
        // Trả về 201 Created và dẫn link về trang chi tiết Offer
        return CreatedAtAction(nameof(GetById), new { id }, await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Collector) Cập nhật báo giá (Sửa đơn giá).
    /// </summary>
    /// <remarks>
    ///     Dùng khi Collector muốn thương lượng lại giá (VD: Household chê rẻ quá). <br />
    ///     Chỉ được sửa khi Offer chưa được Chấp nhận.
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <param name="detailId">ID chi tiết báo giá cần sửa.</param>
    /// <param name="request">Giá mới.</param>
    /// <response code="200">Cập nhật thành công.</response>
    /// <response code="400">Offer đã được chấp nhận, không thể sửa.</response>
    [HttpPut("{id:guid}/details/{detailId:guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(CollectionOfferModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
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
    ///     **Cảnh báo:** Nếu bài đăng gốc yêu cầu `MustTakeAll` (Bán trọn gói), việc xóa bớt item sẽ khiến Offer không hợp lệ
    ///     (thiếu hàng) và hệ thống sẽ chặn hành động này.
    /// </remarks>
    /// <param name="id">ID của Offer.</param>
    /// <param name="detailId">ID chi tiết báo giá cần xóa.</param>
    /// <response code="204">Xóa thành công.</response>
    /// <response code="400">Không thể xóa (Do quy tắc Full-lot hoặc Offer đã Accepted).</response>
    [HttpDelete("{id:guid}/details/{detailId:guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
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