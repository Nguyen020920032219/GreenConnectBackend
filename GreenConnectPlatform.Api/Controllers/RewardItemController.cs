using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.RewardItems;
using GreenConnectPlatform.Business.Services.RewardItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/reward-items")]
[Tags("15. Reward Items (Phần thưởng)")]
public class RewardItemController(IRewardItemService rewardItemService) : ControllerBase
{
    /// <summary>
    ///     (All) Xem danh sách vật phẩm đổi thưởng.
    /// </summary>
    /// <remarks>
    ///     Lấy danh sách các quà tặng/vật phẩm có sẵn trong hệ thống để người dùng đổi điểm.
    ///     Hỗ trợ phân trang, tìm kiếm theo tên và sắp xếp theo số điểm yêu cầu.
    /// </remarks>
    /// <param name="pageIndex">Số trang hiện tại (Mặc định: 1).</param>
    /// <param name="pageSize">Số lượng vật phẩm trên một trang (Mặc định: 10).</param>
    /// <param name="name">Từ khóa tìm kiếm theo tên vật phẩm (Tùy chọn).</param>
    /// <param name="sortByPoint">`true`: Sắp xếp điểm tăng dần, `false`: Giảm dần (Mặc định: true).</param>
    /// <response code="200">Thành công. Trả về danh sách vật phẩm.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền truy cập.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(RewardItemModel),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetRewardItems([FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? name = null,
        [FromQuery] bool sortByPoint = true)
    {
        var result = await rewardItemService.GetRewardItems(pageIndex, pageSize, name, sortByPoint);
        return Ok(result);
    }
    
    /// <summary>
    ///     (All) Xem chi tiết vật phẩm đổi thưởng.
    /// </summary>
    /// <remarks>
    ///     Lấy thông tin cụ thể của một vật phẩm dựa trên ID.
    ///     Bao gồm: Tên, Mô tả, Số điểm cần đổi, Số lượng còn lại trong kho, Hình ảnh...
    /// </remarks>
    /// <param name="id">ID của vật phẩm (số nguyên).</param>
    /// <response code="200">Thành công. Trả về chi tiết `RewardItemModel`.</response>
    /// <response code="404">Không tìm thấy vật phẩm.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền truy cập.</response>
    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RewardItemModel),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel),StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetRewardItemById([FromRoute] int id)
    {
        var result = await rewardItemService.GetByRewardItemIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    ///     (Admin) Tạo mới vật phẩm đổi thưởng.
    /// </summary>
    /// <remarks>
    ///     Dành cho Admin thêm các món quà mới vào kho đổi thưởng.
    ///     Cần nhập: Tên, Điểm quy đổi, Số lượng tồn kho, Hình ảnh minh họa...
    /// </remarks>
    /// <param name="rewardItem">Dữ liệu tạo vật phẩm (`RewardItemCreateModel`).</param>
    /// <response code="201">Tạo thành công. Trả về vật phẩm vừa tạo.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền Admin.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RewardItemModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> CreateRewardItem([FromBody] RewardItemCreateModel rewardItem)
    {
        var result = await rewardItemService.CreateRewardItemAsync(rewardItem);
        return CreatedAtAction(nameof(GetRewardItemById), new { id = result.RewardItemId }, result);
    }

    /// <summary>
    ///     (Admin) Cập nhật vật phẩm đổi thưởng.
    /// </summary>
    /// <remarks>
    ///     Cho phép Admin chỉnh sửa thông tin vật phẩm (Ví dụ: Thay đổi số điểm yêu cầu, cập nhật số lượng kho, sửa tên/mô tả).
    /// </remarks>
    /// <param name="id">ID của vật phẩm cần sửa.</param>
    /// <param name="rewardItem">Dữ liệu cần cập nhật (`RewardItemUpdateModel`).</param>
    /// <response code="200">Cập nhật thành công.</response>
    /// <response code="404">Không tìm thấy vật phẩm.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền Admin.</response>
    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RewardItemModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateRewardItem([FromRoute] int id, [FromBody] RewardItemUpdateModel rewardItem)
    {
        var result = await rewardItemService.UpdateRewardItemAsync(id, rewardItem);
        return Ok(result);
    }

    /// <summary>
    ///     (Admin) Xóa vật phẩm đổi thưởng.
    /// </summary>
    /// <remarks>
    ///     Xóa vật phẩm khỏi danh sách đổi thưởng. 
    ///     Lưu ý: Nếu vật phẩm đã từng được đổi, hệ thống có thể chỉ xóa mềm (ẩn đi) thay vì xóa vĩnh viễn khỏi Database.
    /// </remarks>
    /// <param name="id">ID của vật phẩm cần xóa.</param>
    /// <response code="200">Xóa thành công.</response>
    /// <response code="404">Không tìm thấy vật phẩm.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền Admin.</response>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteRewardItem([FromRoute] int id)
    {
        await rewardItemService.DeleteRewardItemAsync(id);
        return Ok("Xóa phần thưởng thành công");
    }
}