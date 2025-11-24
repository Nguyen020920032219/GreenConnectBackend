using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Business.Services.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/transactions")]
[ApiController]
[Tags("7. Transactions (Check-in & Payment)")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionController(ITransactionService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (Collector) Check-in tại địa điểm thu gom (GPS).
    /// </summary>
    /// <remarks>
    ///     Bước bắt buộc trước khi bắt đầu giao dịch. <br />
    ///     Collector phải gửi tọa độ GPS hiện tại. Hệ thống sẽ tính khoảng cách tới địa chỉ bài đăng. <br />
    ///     Nếu khoảng cách &lt; 100m (hoặc 50m tùy config), trạng thái chuyển từ `Scheduled` -> `InProgress`.
    /// </remarks>
    /// <param name="id">ID của Transaction.</param>
    /// <param name="location">Tọa độ hiện tại (Lat, Long).</param>
    /// <response code="200">Check-in thành công.</response>
    /// <response code="400">Khoảng cách quá xa hoặc trạng thái không hợp lệ.</response>
    [HttpPatch("{id:guid}/check-in")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CheckIn([FromRoute] Guid id, [FromBody] LocationModel location)
    {
        var userId = GetCurrentUserId();
        await _service.CheckInAsync(id, userId, location);
        return Ok(new { Message = "Check-in successful. You can now submit details." });
    }

    /// <summary>
    ///     (Collector) Nhập số lượng/khối lượng ve chai thực tế.
    /// </summary>
    /// <remarks>
    ///     Sau khi Check-in, Collector cân đo và nhập số liệu vào App. <br />
    ///     API này sẽ cập nhật danh sách chi tiết (`TransactionDetails`) và tính tổng tiền. <br />
    ///     **Lưu ý:** Hành động này sẽ ghi đè các chi tiết cũ nếu submit lại.
    /// </remarks>
    /// <param name="id">ID của Transaction.</param>
    /// <param name="details">Danh sách các loại ve chai và số lượng thực tế.</param>
    /// <response code="200">Cập nhật thành công. Trả về danh sách chi tiết đã lưu.</response>
    /// <response code="400">Chưa check-in hoặc dữ liệu không khớp với Offer ban đầu.</response>
    [HttpPost("{id:guid}/details")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(List<TransactionDetailModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitDetails([FromRoute] Guid id,
        [FromBody] List<TransactionDetailCreateModel> details)
    {
        var userId = GetCurrentUserId();
        var result = await _service.SubmitDetailsAsync(id, userId, details);
        return Ok(result);
    }

    /// <summary>
    ///     (Household) Xác nhận hoàn tất (Accept) hoặc Hủy giao dịch.
    /// </summary>
    /// <remarks>
    ///     Đây là bước cuối cùng ("Chốt đơn"). <br />
    ///     - **Accept (true):** Đồng ý với số lượng và số tiền Collector đã nhập -> Giao dịch `Completed` -> Cộng điểm thưởng.
    ///     <br />
    ///     - **Reject (false):** Không đồng ý -> Giao dịch `Canceled`.
    /// </remarks>
    /// <param name="id">ID của Transaction.</param>
    /// <param name="isAccepted">`true` = Chốt đơn, `false` = Hủy.</param>
    /// <response code="200">Thành công.</response>
    /// <response code="400">Collector chưa nhập số liệu (Empty details).</response>
    [HttpPatch("{id:guid}/process")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Process([FromRoute] Guid id, [FromQuery] bool isAccepted)
    {
        var userId = GetCurrentUserId();
        await _service.ProcessTransactionAsync(id, userId, isAccepted);
        return Ok(new { Message = isAccepted ? "Transaction Completed" : "Transaction Canceled" });
    }

    /// <summary>
    ///     (All) Lấy danh sách giao dịch của tôi.
    /// </summary>
    /// <param name="sortByCreateAt">Sắp xếp theo ngày tạo (Mặc định: Newest).</param>
    /// <param name="sortByUpdateAt">Sắp xếp theo ngày cập nhật.</param>
    /// <param name="pageNumber">Trang số.</param>
    /// <param name="pageSize">Số lượng item.</param>
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
    ///     (All) Xem chi tiết giao dịch.
    /// </summary>
    /// <param name="id">ID Transaction.</param>
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
    [HttpPatch("{id:guid}/toggle-cancel")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    public async Task<IActionResult> ToggleCancel(Guid id)
    {
        var userId = GetCurrentUserId();
        await _service.ToggleCancelAsync(id, userId);
        return Ok(new { Message = "Status toggled." });
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}