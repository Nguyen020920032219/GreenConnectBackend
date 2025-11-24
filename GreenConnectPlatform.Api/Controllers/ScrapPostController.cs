using System.Security.Claims;
using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Services.CollectionOffers;
using GreenConnectPlatform.Business.Services.ScrapPosts;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/posts")]
[Tags("4. Scrap Posts (Bài Đăng Thu Gom)")]
[ApiController]
public class ScrapPostController : ControllerBase
{
    private readonly IScrapPostService _service;
    private readonly ICollectionOfferService _collectionOfferService;

    public ScrapPostController(IScrapPostService service, ICollectionOfferService collectionOfferService)
    {
        _service = service;
        _collectionOfferService = collectionOfferService;
    }

    /// <summary>
    ///     (Collector/Admin) Tìm kiếm bài đăng thu gom với bộ lọc nâng cao.
    /// </summary>
    /// <remarks>
    ///     API chính dành cho **Người thu gom (Collector)** để tìm kiếm các đơn hàng tiềm năng xung quanh họ. <br />
    ///     **Logic nghiệp vụ:** <br />
    ///     - **Phân quyền:** Collector chỉ thấy các bài đang `Open` hoặc `PartiallyBooked`. Admin thấy tất cả. <br />
    ///     - **Sắp xếp vị trí (`sortByLocation=true`):** Hệ thống sẽ tính khoảng cách từ tọa độ (GPS) trong Profile của người
    ///     dùng đang gọi API tới từng bài đăng, sau đó sắp xếp từ **gần nhất đến xa nhất**. <br />
    ///     - **Lưu ý:** Để dùng tính năng sắp xếp vị trí, User bắt buộc phải cập nhật tọa độ trong Profile trước.
    /// </remarks>
    /// <param name="categoryName">Tìm kiếm theo tên loại ve chai (VD: "Giấy", "Nhựa").</param>
    /// <param name="status">Lọc theo trạng thái bài đăng (Open, Completed...).</param>
    /// <param name="sortByLocation">`true`: Sắp xếp theo khoảng cách gần nhất. `false`: Sắp xếp mặc định.</param>
    /// <param name="sortByCreateAt">`true`: Cũ nhất trước. `false`: Mới nhất trước (Mặc định).</param>
    /// <param name="pageNumber">Trang hiện tại (Bắt đầu từ 1).</param>
    /// <param name="pageSize">Số lượng bài trên mỗi trang.</param>
    /// <response code="200">Thành công. Trả về danh sách phân trang.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Household không được dùng API này.</response>
    [HttpGet]
    [Authorize(Roles = "Admin, IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(PaginatedResult<ScrapPostOverralModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
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
    ///     Giúp **Hộ gia đình (Household)** quản lý các bài mình đã đăng. <br />
    ///     Có thể lọc để xem lại các bài đã hoàn thành (`Completed`) hoặc các bài đang chờ (`Open`).
    /// </remarks>
    /// <param name="title">Tìm theo tiêu đề bài đăng.</param>
    /// <param name="status">Lọc theo trạng thái.</param>
    /// <response code="200">Trả về danh sách bài đăng của chính user đó.</response>
    [HttpGet("my-posts")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(PaginatedResult<ScrapPostOverralModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
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
    ///     - **Thông tin người đăng:** Tên, SĐT, Avatar, Rank. <br />
    ///     - **Chi tiết ve chai:** Danh sách các loại rác, khối lượng ước lượng, ảnh. <br />
    ///     - **MustTakeAll:** Cờ đánh dấu bài đăng này có bắt buộc mua trọn gói hay không.
    /// </remarks>
    /// <param name="id">ID của bài đăng (GUID).</param>
    /// <response code="200">Thành công. Trả về object `ScrapPostModel`.</response>
    /// <response code="404">Không tìm thấy bài đăng.</response>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    ///     (Household) Tạo bài đăng bán ve chai mới.
    /// </summary>
    /// <remarks>
    ///     **Quy tắc quan trọng:** <br />
    ///     - `Location` (Tọa độ): Bắt buộc phải có để hiển thị trên bản đồ cho Collector tìm kiếm. <br />
    ///     - `MustTakeAll = true`: Nếu bật, Collector bắt buộc phải thu gom **TẤT CẢ** các mục trong danh sách (Full-lot
    ///     purchase). <br />
    ///     - `ScrapPostDetails`: Danh sách các loại rác và ảnh minh họa.
    /// </remarks>
    /// <param name="request">Thông tin bài đăng mới.</param>
    /// <response code="201">Tạo thành công (Trả về chi tiết bài vừa tạo).</response>
    /// <response code="400">Dữ liệu không hợp lệ (Thiếu tọa độ, danh mục rác bị trùng hoặc không tồn tại...).</response>
    [HttpPost]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
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
    ///     Chỉ cho phép cập nhật khi bài đăng đang ở trạng thái **Mở (Open)**. <br />
    ///     Nếu bài đăng đã có người đặt (`PartiallyBooked`) hoặc đã xong (`Completed`), hệ thống sẽ từ chối cập nhật để bảo
    ///     toàn dữ liệu.
    /// </remarks>
    /// <param name="id">ID bài đăng.</param>
    /// <param name="request">Các trường thông tin cần sửa.</param>
    /// <response code="200">Cập nhật thành công.</response>
    /// <response code="400">Bài đăng không ở trạng thái Open.</response>
    /// <response code="403">Bạn không phải chủ bài đăng này.</response>
    /// <response code="404">Không tìm thấy bài đăng.</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ScrapPostUpdateModel request)
    {
        var userId = GetCurrentUserId();
        return Ok(await _service.UpdateAsync(userId, id, request));
    }

    /// <summary>
    ///     (Household/Admin) Đổi trạng thái bài đăng (Mở <-> Hủy).
    /// </summary>
    /// <remarks>
    ///     Dùng để **Hủy (Cancel)** bài đăng nếu người dùng không muốn bán nữa, hoặc **Mở lại (Reopen)** bài đã hủy. <br />
    ///     **Lưu ý:** Không thể tác động vào các bài đã hoàn thành (`Completed`).
    /// </remarks>
    /// <param name="id">ID bài đăng.</param>
    /// <response code="200">Đổi trạng thái thành công.</response>
    /// <response code="400">Trạng thái hiện tại không cho phép đổi (VD: Đang xử lý giao dịch).</response>
    [HttpPatch("{id:guid}/toggle")]
    [Authorize(Roles = "Household, Admin")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var userId = GetCurrentUserId();
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        await _service.ToggleStatusAsync(userId, id, role);
        return Ok(new { Message = "Trạng thái bài đăng đã được thay đổi thành công." });
    }

    // --- DETAIL APIS (Quản lý từng món hàng trong bài đăng) ---

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
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status409Conflict)]
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
    ///     Ví dụ: Sửa mô tả "10kg giấy" thành "15kg giấy". <br />
    ///     Không thể sửa các món hàng đã được người mua đặt cọc hoặc đã thu gom.
    /// </remarks>
    /// <param name="id">ID bài đăng.</param>
    /// <param name="categoryId">ID loại ve chai cần sửa.</param>
    /// <param name="request">Thông tin cập nhật.</param>
    [HttpPut("{id:guid}/details/{categoryId:int}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
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
    ///     Chỉ xóa được nếu món hàng đó chưa được ai đặt mua (`Available`). <br />
    ///     Nếu bài đăng đang có trạng thái `MustTakeAll` (Bán trọn gói), việc xóa item có thể bị hạn chế tùy logic.
    /// </remarks>
    /// <param name="id">ID bài đăng.</param>
    /// <param name="categoryId">ID loại ve chai cần xóa.</param>
    /// <response code="204">Xóa thành công.</response>
    /// <response code="400">Không thể xóa vì hàng đã được đặt hoặc thu gom.</response>
    /// <response code="404">Không tìm thấy món hàng này.</response>
    [HttpDelete("{id:guid}/details/{categoryId:int}")]
    [Authorize(Roles = "Household, Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
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
    
    /// <summary>
    ///     (IndividualCollector/BusinessCollector) Tạo đề nghị thu gom cho một bài đăng.
    /// </summary>
    /// <remarks>
    ///     Chỉ tạo đề nghị thu gom nếu bài đăng đang ở trạng thái Open hoặc PartiallyBooked
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <response code="200">Thêm offer dành cho bài đăng(trả về đề nghị dành cho bài đăng)</response>
    /// <response code="400">Không thể thêm lời đề nghị vì bài đăng đã hoàn thành.....</response>
    /// <response code="404">Không tìm thấy bài đăng này.</response>
    [HttpPost("{id:guid}/offers")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(CollectionOfferModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddOffer(Guid id, [FromBody] CollectionOfferCreateModel request)
    {
        var userId = GetCurrentUserId();
        var result = await _collectionOfferService.CreateAsync(userId, id, request);
        return Ok(await _collectionOfferService.GetByIdAsync(result.CollectionOfferId));
    }
}