using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.RewardItems;
using GreenConnectPlatform.Business.Services.RewardItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/rewards")]
[ApiController]
[Authorize]
[Tags("15. Gamification (Đổi Thưởng)")]
public class RewardItemController : ControllerBase
{
    private readonly IRewardItemService _service;

    public RewardItemController(IRewardItemService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (All) Lấy danh sách quà tặng có thể đổi.
    /// </summary>
    /// <remarks>
    ///     Trả về danh sách các item (Credit, Package...) kèm số điểm yêu cầu.
    /// </remarks>
    [HttpGet]
    [AllowAnonymous] // Cho khách xem để kích thích
    [ProducesResponseType(typeof(List<RewardItemModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRewards()
    {
        return Ok(await _service.GetAvailableRewardsAsync());
    }

    /// <summary>
    ///     (All) Xem lịch sử đổi quà của tôi.
    /// </summary>
    [HttpGet("my-history")]
    [ProducesResponseType(typeof(List<RedemptionHistoryModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyHistory()
    {
        var userId = GetCurrentUserId();
        return Ok(await _service.GetMyRedemptionHistoryAsync(userId));
    }

    /// <summary>
    ///     (All) Đổi điểm lấy quà.
    /// </summary>
    /// <remarks>
    ///     **Quy trình:** <br />
    ///     1. Kiểm tra số dư điểm (`PointBalance`). <br />
    ///     2. Trừ điểm tương ứng. <br />
    ///     3. Cộng quà vào tài khoản (Tăng Credit hoặc Kích hoạt Gói). <br />
    ///     4. Ghi lịch sử giao dịch.
    /// </remarks>
    /// <param name="id">ID của món quà (`RewardItemId`).</param>
    /// <response code="200">Đổi quà thành công.</response>
    /// <response code="400">Không đủ điểm hoặc quà không hợp lệ.</response>
    [HttpPost("{id:int}/redeem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Redeem(int id)
    {
        var userId = GetCurrentUserId();
        await _service.RedeemRewardAsync(userId, id);
        return Ok(new { Message = "Đổi quà thành công! Vui lòng kiểm tra tài khoản." });
    }

    /// <summary>
    ///     (Admin) Tạo món quà mới.
    /// </summary>
    /// <remarks>
    ///     Admin tạo các gói Credit hoặc gói Package để User đổi điểm.
    /// </remarks>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RewardItemModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] RewardItemCreateModel request)
    {
        var result = await _service.CreateRewardItemAsync(request);
        // Trả về 201 Created
        // Lưu ý: Nếu có API GetById cho RewardItem thì dùng CreatedAtAction, ở đây ta chưa làm GetById cho RewardItem (chỉ có GetAll), nên trả Ok hoặc Created kèm data.
        return Created("", result);
    }

    /// <summary>
    ///     (Admin) Cập nhật thông tin món quà.
    /// </summary>
    /// <remarks>
    ///     Sửa tên, mô tả, giá điểm, hoặc loại quà.
    /// </remarks>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RewardItemModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] RewardItemUpdateModel request)
    {
        var result = await _service.UpdateRewardItemAsync(id, request);
        return Ok(result);
    }

    /// <summary>
    ///     (Admin) Xóa món quà.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteRewardItemAsync(id);
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}