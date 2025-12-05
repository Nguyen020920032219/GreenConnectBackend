using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.VerificationInfos;
using GreenConnectPlatform.Business.Services.VerificationInfos;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/admin/verifications")]
[Authorize(Roles = "Admin")]
[Tags("99. Admin - Verification Management")]
public class AdminVerificationController : ControllerBase
{
    private readonly IVerificationInfoService _service;

    public AdminVerificationController(IVerificationInfoService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (Admin) Lấy danh sách đơn xác minh.
    /// </summary>
    /// <remarks>
    ///     Dành cho Admin duyệt đơn thủ công (trong trường hợp AI duyệt sót hoặc cần kiểm tra lại). <br />
    ///     Hỗ trợ lọc theo trạng thái (`PendingReview`, `Approved`...) và sắp xếp ngày gửi.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<VerificationInfoOveralModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] VerificationStatus? status = null,
        [FromQuery] bool sortBySubmittedAt = true)
    {
        return Ok(await _service.GetVerificationInfos(pageNumber, pageSize, sortBySubmittedAt, status));
    }

    /// <summary>
    ///     (Admin) Xem chi tiết đơn xác minh.
    /// </summary>
    /// <remarks>
    ///     Trả về đầy đủ thông tin định danh (Số CCCD, Ngày cấp...) và thông tin User liên quan.
    /// </remarks>
    /// <param name="userId">ID của User nộp đơn.</param>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(VerificationInfoModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetail(Guid userId)
    {
        return Ok(await _service.GetVerificationInfo(userId));
    }

    /// <summary>
    ///     (Admin) Duyệt hoặc Từ chối đơn xác minh.
    /// </summary>
    /// <remarks>
    ///     **Duyệt (True):** Nâng cấp User thành Collector, kích hoạt tài khoản, gửi Noti. <br />
    ///     **Từ chối (False):** Gửi Noti kèm lý do từ chối. User phải nộp lại.
    /// </remarks>
    /// <param name="userId">ID của User.</param>
    /// <param name="isAccepted">`true` = Duyệt, `false` = Từ chối.</param>
    /// <param name="reviewerNote">Ghi chú lý do (Bắt buộc nếu từ chối).</param>
    [HttpPatch("{userId:guid}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] Guid userId,
        [FromQuery] bool isAccepted,
        [FromQuery] string? reviewerNote)
    {
        var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _service.VerifyCollector(userId, adminId, isAccepted, reviewerNote);

        return Ok(new { Message = isAccepted ? "Đã duyệt thành công." : "Đã từ chối hồ sơ." });
    }
}