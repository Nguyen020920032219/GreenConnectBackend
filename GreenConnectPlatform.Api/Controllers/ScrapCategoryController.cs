using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Business.Services.ScrapCategories;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;
[Route("api/categories")]
[ApiController]
[Tags("Scrap Categories")]
public class ScrapCategoryController(IScrapCategoryService scrapCategoryService) : ControllerBase
{
    /// <summary>
    ///     Can get all scrap categories with pagination and filtering by category name.
    /// </summary>
    /// <param name="categoryName">ID of scrap category</param>
    /// <param name="pageNumber"></param>
    /// /// <param name="pageSize"></param>
    [HttpGet]
    [ProducesResponseType(typeof(ScrapCategoryModel), StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ScrapCategoryModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScrapCategories(
        [FromQuery] string? categoryName,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
        )
    {
        var result = await scrapCategoryService.GetScrapCategories(pageNumber, pageSize, categoryName);
        return Ok(result);
    }
    /// <summary>
    ///     Can get detail scrap categories 
    /// </summary>
    /// <param name="categoryId">ID of scrap category</param>
    [HttpGet("{categoryId:int}")]
    [ProducesResponseType(typeof(ScrapCategoryModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapCategoryModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapCategoryModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScrapCategory([FromRoute] int categoryId)
    {
        var result = await scrapCategoryService.GetScrapCategory(categoryId);
        return Ok(result);
    }
}