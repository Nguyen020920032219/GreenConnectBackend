using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Business.Services.ScrapCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/scrap-categories")]
[Tags("3. Master Data (Danh Mục Ve Chai)")]
public class ScrapCategoryController : ControllerBase
{
    private readonly IScrapCategoryService _service;

    public ScrapCategoryController(IScrapCategoryService service)
    {
        _service = service;
    }

    /// <summary>
    ///     (Public) Lấy danh sách các loại ve chai.
    /// </summary>
    /// <remarks>
    ///     API này cung cấp dữ liệu Master Data cho toàn bộ hệ thống. <br />
    ///     **Ứng dụng:** <br />
    ///     - **Household:** Dùng để chọn loại rác khi tạo bài đăng mới. <br />
    ///     - **Collector:** Dùng để lọc bài đăng theo loại rác quan tâm. <br />
    ///     - **Search Bar:** Hỗ trợ tìm kiếm tương đối theo tên (VD: Nhập "nhựa" -> Trả về "Chai nhựa", "Nhựa cứng").
    /// </remarks>
    /// <param name="searchName">Từ khóa tìm kiếm theo tên danh mục (Không phân biệt hoa thường).</param>
    /// <param name="pageNumber">Trang hiện tại (Mặc định: 1).</param>
    /// <param name="pageSize">Số lượng danh mục trên mỗi trang (Mặc định: 10).</param>
    /// <response code="200">Thành công. Trả về danh sách có phân trang.</response>
    /// <response code="500">Lỗi server nội bộ.</response>
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
    ///     (Public) Xem chi tiết một danh mục ve chai.
    /// </summary>
    /// <remarks>
    ///     Lấy thông tin chi tiết bao gồm Tên, Mô tả, Hình ảnh minh họa (nếu có) của một loại rác cụ thể.
    /// </remarks>
    /// <param name="id">ID của danh mục (Số nguyên).</param>
    /// <response code="200">Thành công. Trả về object `ScrapCategoryModel`.</response>
    /// <response code="404">Không tìm thấy danh mục với ID này.</response>
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
    ///     (Admin) Tạo mới một danh mục ve chai.
    /// </summary>
    /// <remarks>
    ///     Chỉ dành cho Quản trị viên hệ thống. <br />
    ///     **Quy tắc Validation:** <br />
    ///     - `CategoryName`: Bắt buộc và **Duy nhất**. Nếu trùng tên với danh mục đã có (không phân biệt hoa thường) sẽ báo
    ///     lỗi `409 Conflict`.
    /// </remarks>
    /// <param name="request">Thông tin danh mục mới (Tên, Mô tả).</param>
    /// <response code="201">Tạo thành công.</response>
    /// <response code="400">Dữ liệu không hợp lệ (Thiếu tên...).</response>
    /// <response code="409">Tên danh mục đã tồn tại.</response>
    /// <response code="403">Không có quyền Admin.</response>
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
    ///     Cho phép Admin sửa tên hoặc mô tả của danh mục. <br />
    ///     **Lưu ý:** Nếu đổi tên, hệ thống vẫn sẽ kiểm tra trùng lặp với các danh mục khác.
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
    ///     (Admin) Xóa một danh mục ve chai.
    /// </summary>
    /// <remarks>
    ///     **Cơ chế an toàn:** <br />
    ///     Hệ thống sẽ **CHẶN (Block)** hành động xóa nếu danh mục này đang được sử dụng trong bất kỳ bài đăng (`ScrapPost`)
    ///     nào. <br />
    ///     Điều này nhằm đảm bảo lịch sử giao dịch và dữ liệu bài đăng không bị lỗi. <br />
    ///     *Giải pháp:* Nếu muốn ngừng sử dụng, hãy cân nhắc tính năng "Ẩn" danh mục (Soft Delete) thay vì xóa cứng (trong
    ///     tương lai).
    /// </remarks>
    /// <param name="id">ID danh mục cần xóa.</param>
    /// <response code="204">Xóa thành công.</response>
    /// <response code="400">Không thể xóa do danh mục đang được sử dụng (Ràng buộc dữ liệu).</response>
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