using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Feedbacks;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Business.Services.Feedbacks;
using GreenConnectPlatform.Business.Services.Transactions;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/transactions")]
[ApiController]
[Tags("07. Transactions (Giao Dịch & Thanh Toán)")]
public class TransactionController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;
    private readonly ITransactionService _service;

    public TransactionController(ITransactionService service, IFeedbackService feedbackService)
    {
        _service = service;
        _feedbackService = feedbackService;
    }

    /// <summary>
    ///     (Collector) Check-in GPS tại điểm thu gom.
    /// </summary>
    /// <remarks>
    ///     **Bắt buộc:** Collector phải Check-in khi đến nơi để bắt đầu giao dịch. <br />
    ///     **Logic:** Hệ thống tính khoảng cách từ tọa độ gửi lên (`location`) đến tọa độ của Bài đăng. <br />
    ///     Nếu khoảng cách **&lt; 100m**, trạng thái chuyển từ `Scheduled` -> `InProgress`.
    /// </remarks>
    /// <param name="id">ID Giao dịch.</param>
    /// <param name="location">Tọa độ hiện tại (Lat, Long).</param>
    /// <response code="200">Check-in thành công.</response>
    /// <response code="400">Khoảng cách quá xa hoặc sai trạng thái.</response>
    [HttpPatch("{id:guid}/check-in")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CheckIn([FromRoute] Guid id, [FromBody] LocationModel location)
    {
        var userId = GetCurrentUserId();
        await _service.CheckInAsync(id, userId, location);
        return Ok(new { Message = "Check-in thành công. Bạn có thể bắt đầu nhập hàng." });
    }

    /// <summary>
    ///     (Collector) Nhập số lượng ve chai thực tế.
    /// </summary>
    /// <remarks>
    ///     Sau khi cân đo, Collector nhập số liệu vào App. <br />
    ///     Hệ thống sẽ cập nhật danh sách chi tiết (`TransactionDetails`) và tính lại **Tổng tiền**. <br />
    ///     **Lưu ý:** Hành động này sẽ **ghi đè** toàn bộ danh sách chi tiết cũ (nếu đã nhập trước đó).
    /// </remarks>
    /// <param name="scrapPostId">ID Giao dịch.</param>
    /// <param name="slotId"></param>
    /// <param name="details">Danh sách hàng hóa và số lượng thực tế.</param>
    /// <response code="200">Cập nhật thành công. Trả về danh sách chi tiết mới.</response>
    /// <response code="400">Chưa Check-in hoặc dữ liệu không hợp lệ (Sai loại ve chai).</response>
    [HttpPost("details")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(List<TransactionDetailModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitDetails([FromQuery] Guid scrapPostId,
        [FromQuery] Guid slotId,
        [FromBody] List<TransactionDetailCreateModel> details)
    {
        var userId = GetCurrentUserId();
        var result = await _service.SubmitDetailsAsync(scrapPostId, userId, slotId, details);
        return Ok(result);
    }

    /// <summary>
    ///     (Household) Xác nhận Hoàn tất (Chốt đơn) hoặc Hủy.
    /// </summary>
    /// <remarks>
    ///     Đây là bước cuối cùng của quy trình thu gom. <br />
    ///     - **Accept (true):** Household đồng ý với số lượng và số tiền -> Giao dịch `Completed` -> Cộng điểm thưởng. <br />
    ///     - **Reject (false):** Household không đồng ý -> Giao dịch `Canceled`.
    /// </remarks>
    /// <param name="scrapPostId">ID bài đăng.</param>
    /// <param name="collectorId"></param>
    /// <param name="slotId"></param>
    /// <param name="isAccepted">`true` = Chốt đơn, `false` = Hủy.</param>
    /// <param name="paymentMethod">Thanh toán bằng hình thức gì</param>
    /// <response code="200">Thành công.</response>
    /// <response code="400">Collector chưa nhập số liệu (Không thể chốt đơn trống).</response>
    [HttpPatch("process")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Process([FromQuery] Guid scrapPostId, [FromQuery] Guid collectorId,
        [FromQuery] Guid slotId, [FromQuery] bool isAccepted,
        [FromQuery] TransactionPaymentMethod paymentMethod)
    {
        var userId = GetCurrentUserId();
        await _service.ProcessTransactionAsync(scrapPostId, collectorId, slotId, userId, isAccepted, paymentMethod);
        return Ok(new { Message = isAccepted ? "Giao dịch thành công!" : "Giao dịch đã hủy." });
    }

    /// <summary>
    ///     (All) Lấy danh sách giao dịch của tôi.
    /// </summary>
    /// <remarks>
    ///     Lấy lịch sử giao dịch (Đang xử lý, Hoàn tất, Đã hủy...). Hỗ trợ phân trang và sắp xếp.
    /// </remarks>
    /// <param name="sortByCreateAt">Sắp xếp theo ngày tạo.</param>
    /// <param name="sortByUpdateAt">Sắp xếp theo ngày cập nhật.</param>
    /// <param name="pageNumber">Trang số.</param>
    /// <param name="pageSize">Số lượng item/trang.</param>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedResult<TransactionOveralModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] bool sortByCreateAt = true,
        [FromQuery] bool sortByUpdateAt = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        return Ok(await _service.GetListAsync(userId, role, pageNumber, pageSize, sortByCreateAt, sortByUpdateAt));
    }

    /// <summary>
    ///     (All) Xem chi tiết một giao dịch.
    /// </summary>
    /// <param name="id">ID Transaction.</param>
    /// <response code="200">Trả về thông tin chi tiết (Kèm Offer, User, Details).</response>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(TransactionModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Collector) Hủy hoặc Mở lại giao dịch (Sự cố).
    /// </summary>
    /// <remarks>
    ///     Dùng trong các trường hợp bất khả kháng (xe hỏng, không liên lạc được...). <br />
    ///     Chuyển đổi trạng thái giữa `InProgress` và `Canceled`.
    /// </remarks>
    [HttpPatch("{id:guid}/toggle-cancel")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ToggleCancel(Guid id)
    {
        var userId = GetCurrentUserId();
        await _service.ToggleCancelAsync(id, userId);
        return Ok(new { Message = "Đổi trạng thái giao dịch thành công." });
    }

    /// <summary>
    ///     (IndividualCollector, BusinessCollector, Household) có thể lấy danh sách nhận xét dành cho giao dịch của mình
    /// </summary>
    /// <param name="id">Id của nhận xét</param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortByCreateAt">Có thể sắp xếp theo ngày tạo nhận xét</param>
    [HttpGet("{id:guid}/feedbacks")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector, Household")]
    [ProducesResponseType(typeof(FeedbackModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeedbacks([FromRoute] Guid id,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool sortByCreateAt = true)
    {
        return Ok(await _feedbackService.GetFeedbacksAsync(pageNumber, pageSize, id, sortByCreateAt));
    }

    /// <summary>
    ///     (All) Lấy mã VietQR để thanh toán cho đơn hàng.
    /// </summary>
    /// <remarks>
    ///     Tạo mã QR chuyển khoản tự động dựa trên tổng tiền đơn hàng và số tài khoản của Household hoặc Collector. <br />
    ///     Collector hoặc Household quét mã này để chuyển tiền nhanh chóng. <br />
    ///     **Lưu ý:** Household hoặc Collector phải cập nhật thông tin ngân hàng trong Profile trước thì mới tạo được mã.
    /// </remarks>
    /// <param name="id">ID Bài post.</param>
    /// <param name="totalAmount">Tổng giá tiền (số dương)</param>
    /// <response code="200">Thành công. Trả về URL ảnh QR.</response>
    /// <response code="400">Household hoặc Collector chưa có số tài khoản hoặc đơn hàng 0 đồng.</response>
    [HttpGet("{id:guid}/qr-code")]
    [Authorize]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetQrCode(Guid id, [FromQuery] decimal totalAmount)
    {
        var userId = GetCurrentUserId();
        var qrUrl = await _service.GetTransactionQrCodeAsync(userId, id, totalAmount);
        return Ok(new { QrUrl = qrUrl });
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}