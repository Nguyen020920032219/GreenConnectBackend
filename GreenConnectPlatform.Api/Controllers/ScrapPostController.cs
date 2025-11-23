using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Services.ScrapPosts;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/posts")]
[Tags("4. Scrap Posts")]
[ApiController]
public class ScrapPostController : ControllerBase
{
    private readonly IScrapPostService _service;

    public ScrapPostController(IScrapPostService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (Collector/Admin) Tìm kiếm bài đăng thu gom (Scrap Posts).
    /// </summary>
    /// <remarks>
    ///     API này dành cho **Người thu gom (Collector)** tìm kiếm mối hàng hoặc **Admin** quản lý. <br />
    ///     **Logic quan trọng:** <br />
    ///     - **Quyền xem:** Collector chỉ thấy các bài `Open` hoặc `PartiallyBooked`. Admin thấy tất cả. <br />
    ///     - **Sắp xếp theo vị trí (`sortByLocation=true`):** Hệ thống sẽ tính khoảng cách từ vị trí (GPS) trong Profile của
    ///     người dùng đang đăng nhập đến vị trí bài đăng, và trả về danh sách từ gần nhất đến xa nhất. <br />
    ///     - **Lọc:** Có thể lọc theo tên loại ve chai (`categoryName`) hoặc trạng thái (`status`).
    /// </remarks>
    /// <param name="categoryName">Tìm theo tên loại ve chai (VD: "Giấy", "Sắt").</param>
    /// <param name="status">Lọc theo trạng thái bài đăng (Open, Completed, Canceled...).</param>
    /// <param name="sortByLocation">
    ///     Nếu `true`, sắp xếp bài đăng gần người dùng nhất (Yêu cầu User phải có tọa độ trong
    ///     Profile).
    /// </param>
    /// <param name="sortByCreateAt">Sắp xếp theo ngày tạo (Mặc định là Mới nhất).</param>
    /// <param name="pageNumber">Trang số mấy (Bắt đầu từ 1).</param>
    /// <param name="pageSize">Số lượng bài trên mỗi trang.</param>
    /// <response code="200">Thành công. Trả về danh sách phân trang `PaginatedResult`.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Bạn là Household (Không được dùng API này để tìm bài của người khác).</response>
    [HttpGet]
    [Authorize(Roles = "Admin, IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(PaginatedResult<ScrapPostOverralModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Search(
        [FromQuery] string? categoryName,
        [FromQuery] PostStatus? status,
        [FromQuery] bool sortByLocation = false,
        [FromQuery] bool sortByCreateAt = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        var result = await _service.SearchPostsAsync(pageNumber, pageSize, categoryName, status, sortByLocation,
            sortByCreateAt, userId);
        return Ok(result);
    }

    /// <summary>
    ///     (Household) Xem lịch sử bài đăng của tôi.
    /// </summary>
    /// <remarks>
    ///     API này giúp **Hộ gia đình (Household)** quản lý các bài mình đã đăng. <br />
    ///     Có thể dùng để xem lại các đơn đã hoàn thành (`Completed`) hoặc đang chờ (`Open`).
    /// </remarks>
    /// <param name="title">Tìm theo tiêu đề bài đăng.</param>
    /// <param name="status">Lọc theo trạng thái.</param>
    /// <response code="200">Trả về danh sách bài đăng của chính user đó.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Người dùng không phải là Household.</response>
    [HttpGet("my-posts")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(PaginatedResult<ScrapPostOverralModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMyPosts(
        [FromQuery] string? title,
        [FromQuery] PostStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        var result = await _service.GetMyPostsAsync(pageNumber, pageSize, title, status, userId);
        return Ok(result);
    }

    /// <summary>
    ///     (All) Xem chi tiết một bài đăng.
    /// </summary>
    /// <remarks>
    ///     Trả về thông tin đầy đủ của bài đăng bao gồm: <br />
    ///     - Thông tin người đăng (Household Info). <br />
    ///     - Danh sách các loại ve chai chi tiết (`ScrapPostDetails`). <br />
    ///     - Cờ `MustTakeAll` (Bán trọn gói hay bán lẻ).
    /// </remarks>
    /// <param name="id">ID của bài đăng (GUID).</param>
    /// <response code="200">Trả về object `ScrapPostModel`.</response>
    /// <response code="404">Không tìm thấy bài đăng.</response>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Household) Tạo bài đăng bán ve chai mới.
    /// </summary>
    /// <remarks>
    ///     **Quy tắc nghiệp vụ:** <br />
    ///     - `MustTakeAll = true`: Yêu cầu người mua phải gom HẾT tất cả các mục trong danh sách (Full-lot). <br />
    ///     - `Location`: Bắt buộc phải có tọa độ (Lat/Long) để hiển thị trên bản đồ. <br />
    ///     - `ScrapPostDetails`: Danh sách các loại rác muốn bán (Giấy, Nhựa...).
    /// </remarks>
    /// <param name="request">Thông tin bài đăng.</param>
    /// <response code="201">Tạo thành công (Trả về chi tiết bài vừa tạo).</response>
    /// <response code="400">Dữ liệu không hợp lệ (Thiếu tọa độ, danh mục không tồn tại...).</response>
    [HttpPost]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ScrapPostCreateModel request)
    {
        var userId = GetCurrentUserId();
        var result = await _service.CreateAsync(userId, request);
        return CreatedAtAction(nameof(GetById), new { id = result.ScrapPostId }, result);
    }

    /// <summary>
    ///     (Household) Cập nhật thông tin bài đăng.
    /// </summary>
    /// <remarks>
    ///     Chỉ cho phép cập nhật khi bài đăng đang ở trạng thái **Open**. <br />
    ///     Nếu bài đăng đã có người đặt hoặc đã hoàn thành, API sẽ trả về lỗi 400.
    /// </remarks>
    /// <param name="id">ID bài đăng.</param>
    /// <param name="request">Các trường cần sửa (Tiêu đề, địa chỉ, Full-lot...).</param>
    /// <response code="200">Cập nhật thành công.</response>
    /// <response code="400">Bài đăng không ở trạng thái Open.</response>
    /// <response code="403">Bạn không phải chủ bài đăng này.</response>
    /// <response code="404">Không tìm thấy bài đăng.</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ScrapPostUpdateModel request)
    {
        var userId = GetCurrentUserId();
        return Ok(await _service.UpdateAsync(userId, id, request));
    }

    /// <summary>
    ///     (Household/Admin) Đổi trạng thái bài đăng (Open <-> Canceled).
    /// </summary>
    /// <remarks>
    ///     Dùng để **Hủy** bài đăng nếu không muốn bán nữa, hoặc **Mở lại** bài đăng đã hủy. <br />
    ///     Không thể tác động vào các bài đã hoàn thành (`Completed`).
    /// </remarks>
    /// <param name="id">ID bài đăng.</param>
    /// <response code="200">Đổi trạng thái thành công.</response>
    /// <response code="400">Trạng thái hiện tại không cho phép đổi.</response>
    [HttpPatch("{id:guid}/toggle")]
    [Authorize(Roles = "Household, Admin")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var userId = GetCurrentUserId();
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        await _service.ToggleStatusAsync(userId, id, role);
        return Ok(new { Message = "Trạng thái bài đăng đã được thay đổi thành công." });
    }

    /// <summary>
    ///     (Household) Thêm một loại rác vào bài đăng có sẵn.
    /// </summary>
    /// <remarks>
    ///     Ví dụ: Đã đăng bán "Giấy", giờ muốn bán thêm "Vỏ lon" vào cùng bài đó. <br />
    ///     Không thể thêm nếu bài đăng đã hoàn thành.
    /// </remarks>
    /// <param name="id">ID bài đăng.</param>
    /// <param name="request">Thông tin loại rác mới.</param>
    /// <response code="200">Thêm thành công (Trả về bài đăng cập nhật).</response>
    /// <response code="409">Loại rác này đã tồn tại trong bài đăng rồi.</response>
    [HttpPost("{id:guid}/details")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddDetail(Guid id, [FromBody] ScrapPostDetailCreateModel request)
    {
        var userId = GetCurrentUserId();
        await _service.AddDetailAsync(userId, id, request);
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Household) Sửa thông tin một món hàng trong bài đăng.
    /// </summary>
    /// <remarks>
    ///     Ví dụ: Sửa "10kg giấy" thành "15kg giấy".
    /// </remarks>
    /// <param name="id">ID bài đăng.</param>
    /// <param name="categoryId">ID loại ve chai cần sửa.</param>
    /// <param name="request">Thông tin cập nhật.</param>
    [HttpPut("{id:guid}/details/{categoryId:int}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDetail(Guid id, int categoryId,
        [FromBody] ScrapPostDetailUpdateModel request)
    {
        var userId = GetCurrentUserId();
        await _service.UpdateDetailAsync(userId, id, categoryId, request);
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Household/Admin) Xóa một món hàng khỏi bài đăng.
    /// </summary>
    /// <remarks>
    ///     Chỉ xóa được nếu món hàng đó chưa được ai đặt mua (`Available`).
    /// </remarks>
    /// <param name="id">ID bài đăng.</param>
    /// <param name="categoryId">ID loại ve chai cần xóa.</param>
    /// <response code="204">Xóa thành công.</response>
    /// <response code="400">Không thể xóa vì hàng đã được đặt.</response>
    [HttpDelete("{id:guid}/details/{categoryId:int}")]
    [Authorize(Roles = "Household, Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDetail(Guid id, int categoryId)
    {
        var userId = GetCurrentUserId();
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        await _service.DeleteDetailAsync(userId, id, categoryId, role);
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}