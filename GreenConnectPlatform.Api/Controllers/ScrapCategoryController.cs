using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Business.Services.ScrapCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/scrap-categories")]
[Tags("3. Master Data (Category)")]
public class ScrapCategoryController : ControllerBase
{
    private readonly IScrapCategoryService _service;

    public ScrapCategoryController(IScrapCategoryService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (Public) Lấy danh sách danh mục ve chai.
    /// </summary>
    /// <remarks>
    ///     Dùng để hiển thị dropdown chọn loại rác hoặc bộ lọc tìm kiếm. <br />
    ///     Hỗ trợ tìm kiếm tương đối theo tên (VD: "giấy" -> ra "Giấy vụn", "Giấy carton").
    /// </remarks>
    /// <param name="searchName">Từ khóa tìm kiếm (Optional).</param>
    /// <param name="pageNumber">Trang số (Mặc định: 1).</param>
    /// <param name="pageSize">Số dòng/trang (Mặc định: 10).</param>
    /// <response code="200">Thành công.</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedResult<ScrapCategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetList(
        [FromQuery] string? searchName,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetListAsync(pageNumber, pageSize, searchName);
        return Ok(result);
    }

    /// <summary>
    ///     (Public) Xem chi tiết danh mục.
    /// </summary>
    /// <param name="id">ID danh mục.</param>
    /// <response code="200">Trả về thông tin chi tiết.</response>
    /// <response code="404">Không tìm thấy ID này.</response>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ScrapCategoryModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    ///     (Admin) Tạo danh mục ve chai mới.
    /// </summary>
    /// <remarks>
    ///     Tên danh mục (`CategoryName`) là duy nhất, không được trùng lặp (Case-insensitive).
    /// </remarks>
    /// <param name="request">Thông tin danh mục cần tạo.</param>
    /// <response code="201">Tạo thành công.</response>
    /// <response code="409">Lỗi trùng tên danh mục.</response>
    /// <response code="403">Không phải Admin.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ScrapCategoryModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] ScrapCategoryModel request)
    {
        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.ScrapCategoryId }, result);
    }

    /// <summary>
    ///     (Admin) Cập nhật thông tin danh mục.
    /// </summary>
    /// <remarks>
    ///     Cho phép đổi tên hoặc mô tả. Nếu đổi tên, hệ thống sẽ kiểm tra trùng lặp lại.
    /// </remarks>
    /// <param name="id">ID danh mục cần sửa.</param>
    /// <param name="request">Thông tin mới.</param>
    /// <response code="200">Cập nhật thành công.</response>
    /// <response code="409">Tên mới bị trùng với danh mục khác.</response>
    /// <response code="404">Danh mục không tồn tại.</response>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ScrapCategoryModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] ScrapCategoryModel request)
    {
        var result = await _service.UpdateAsync(id, request);
        return Ok(result);
    }

    /// <summary>
    ///     (Admin) Xóa danh mục ve chai.
    /// </summary>
    /// <remarks>
    ///     **QUAN TRỌNG:** Hệ thống sẽ **CHẶN** xóa nếu danh mục này đang được sử dụng trong bất kỳ bài đăng (`ScrapPost`) nào
    ///     để đảm bảo tính toàn vẹn dữ liệu. <br />
    ///     Admin cần xóa các bài đăng liên quan trước, hoặc chỉ được phép ẩn danh mục (chức năng update status - nếu có).
    /// </remarks>
    /// <param name="id">ID danh mục cần xóa.</param>
    /// <response code="204">Xóa thành công.</response>
    /// <response code="400">Không thể xóa vì danh mục đang được sử dụng (Ràng buộc khóa ngoại).</response>
    /// <response code="404">Danh mục không tồn tại.</response>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}