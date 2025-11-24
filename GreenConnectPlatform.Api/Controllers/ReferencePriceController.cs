using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.ReferencePrices;
using GreenConnectPlatform.Business.Services.ReferencePrices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/prices")]
[Tags("11. Reference Prices (Giá tham khảo)")]
public class ReferencePriceController(IReferencePriceService priceService) : ControllerBase
{
    /// <summary>
    /// Người dùng có thể lấy danh sách giá tham khảo
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="categoryName">Tìm kiếm giá tham khảo bằng tên vật liệu</param>
    /// <param name="sortByPrice">Sắp xếp theo giá</param>
    /// <param name="sortByUpdateAt">Sắp xếp theo ngày đã được cập nhật</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ReferencePriceModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReferencePrices(
        int pageNumber = 1,
        int pageSize = 10,
        string? categoryName = null,
        bool? sortByPrice = null,
        bool sortByUpdateAt = true
        )
    {
        var result = await priceService.GetReferencePrices(pageNumber, pageSize, categoryName, sortByPrice, sortByUpdateAt);
        return Ok(result);
    }
    
    /// <summary>
    ///     Người dùng có thể lấy thông tin giá tham khảo theo Id
    /// </summary>
    /// <param name="priceId">ID của giá tham khảo</param>
    /// <returns></returns>
    [HttpGet("{priceId:Guid}")]
    [ProducesResponseType(typeof(ReferencePriceModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReferencePrice([FromRoute] Guid priceId)
    {
        var result =  await priceService.GetReferencePrice(priceId);
        return Ok(result);
    }
    
    /// <summary>
    ///     Admin có thể tạo giá tham khảo mới
    /// </summary>
    /// <param name="scrapCategoryId">ID của vật liệu</param>
    /// <param name="pricePerKg">Giá tham khảo dành cho 1 kg</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ReferencePriceModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateReferencePrice([FromQuery] int scrapCategoryId, [FromQuery] decimal pricePerKg)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await priceService.CreateReferencePrice(scrapCategoryId, pricePerKg, Guid.Parse(userId));
        return CreatedAtAction(nameof(GetReferencePrice), new { priceId = result.ReferencePriceId }, result);
    }
    
    /// <summary>
    ///     Admin có thể cập nhật lại giá cho giá tham khảo
    /// </summary>
    /// <param name="priceId">ID của giá tham khảo</param>
    /// <param name="pricePerKg">Giá mới để cập nhật</param>
    /// <returns></returns>
    [HttpPatch("{priceId:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ReferencePriceModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateReferencePrice(
        [FromRoute] Guid priceId,
        [FromQuery] decimal? pricePerKg)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await priceService.UpdateReferencePrice(priceId, pricePerKg, Guid.Parse(userId));
        return Ok(result);
    }
    
    /// <summary>
    ///     Admin có thể xóa giá tham khảo
    /// </summary>
    /// <param name="priceId">ID của giá tham khảo</param>
    /// <returns></returns>
    [HttpDelete("{priceId:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteReferencePrice([FromRoute] Guid priceId)
    {
        await priceService.DeleteReferencePrice(priceId);
        return Ok("Đã xóa giá tham khảo thành công");
    }
}