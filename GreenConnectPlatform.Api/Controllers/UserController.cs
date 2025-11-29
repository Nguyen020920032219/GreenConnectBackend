using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PointHistories;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.PointHistories;
using GreenConnectPlatform.Business.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/users")]
[Tags("14. Users (Người dùng)")]
public class UserController(IUserService userService, IPointHistoryService pointHistoryService) : ControllerBase
{
    /// <summary>
    ///     Admin có lấy danh sách tất cả người dùng trong hệ thống
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="roleId">Lấy danh sách các user theo role</param>
    /// <param name="fullName">Tìm kiếm người dùng bằng tên</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedResult<UserModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? roleId = null,
        [FromQuery] string? fullName = null)
    {
        var result = await userService.GetUsersAsync(pageIndex, pageSize, roleId, fullName);
        return Ok(result);
    }

    /// <summary>
    ///     Admin có cấm hoặc mở lại tài khoản cho người dùng
    /// </summary>
    /// <param name="userId">Id của người dùng</param>
    /// <returns></returns>
    [HttpPatch("{userId:Guid}/ban-toggle")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BanOrUnbanUser([FromRoute] Guid userId)
    {
        var currentUserId = GetCurrentUserId();
        await userService.BanOrUnbanUserAsync(userId, currentUserId);
        return Ok("Người dùng đã bị cấm hoặc mở lại thành công");
    }
    
    /// <summary>
    ///     Người dùng có thể xem danh sách lịch sử thay đổi điểm của mình hoặc (Admin) có thể xem của người dùng trong hệ thống 
    /// </summary>
    /// <param name="userId">Admin có thể xem danh sách lịch sử thay đổi điểm của người dùng</param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortByCreatAt">Có thể sắp xếp theo ngày tạo</param>
    /// <returns></returns>
    [HttpGet("points")]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedResult<PointHistoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserPointHistories(
        [FromQuery] Guid? userId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool sortByCreatAt = true)
    {
        var currentUserId = GetCurrentUserId();
        var result = await pointHistoryService.GetPointHistoriesAsync(userId,currentUserId, pageIndex, pageSize, sortByCreatAt);
        return Ok(result);
    }
    
    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}