using System.Security.Claims;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Services.ScrapPosts;
using GreenConnectPlatform.Business.Services.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

/// <summary>
///     Everything about Scrap Posts and Scrap Post Details
/// </summary>
[Route("api/v1/posts")]
[Tags("Scrap Posts")]
[ApiController]
public class ScrapPostController(
    IScrapPostService scrapPostService,
    IScrapPostDetailService scrapPostDetailService
)
    : ControllerBase
{
    /// <summary>
    ///     Get a list of scrap posts with pagination, filtering by category name and sorting by location options
    /// </summary>
    /// <param name="categoryName">Search by category name</param>
    /// <param name="status">Sort by status of scrap post, for only Admin</param>
    /// <param name="sortByLocation">Sort by location, for only Scrap Collector</param>
    /// <param name="sortByCreateAt">Sort by creation date of scrap post</param>
    /// <param name="sortByUpdateAt">Sort by update date of scrap post</param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    [HttpGet]
    [Authorize(Roles = "Admin, ScrapCollector")]
    [ProducesResponseType(typeof(ScrapPostOverralModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostOverralModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostOverralModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPosts(
        [FromQuery] string? categoryName,
        [FromQuery] PostStatus? status,
        [FromQuery] bool sortByLocation = false,
        [FromQuery] bool sortByCreateAt = false,
        [FromQuery] bool sortByUpdateAt = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var posts = await scrapPostService.GetPosts(pageNumber, pageSize, userIdParsed, userRole, categoryName, status,
            sortByLocation, sortByCreateAt, sortByUpdateAt);
        return Ok(posts);
    }

    /// <summary>
    ///     Get lists of your scrap posts with pagination, filter by category name and sort by position option
    /// </summary>
    /// <param name="title"></param>
    /// <param name="status"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    [HttpGet("my-posts")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(ScrapPostOverralModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostOverralModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostOverralModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPostsByHousehold(
        [FromQuery] string? title,
        [FromQuery] PostStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        Guid? userIdParsed = null;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        userIdParsed = Guid.Parse(userId);
        var posts = await scrapPostService.GetPostsByHousehold(pageNumber, pageSize, userIdParsed, title, status);
        return Ok(posts);
    }

    /// <summary>
    ///     Get scrap posts detail with id of scrap post
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    [HttpGet("{postId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPost([FromRoute] Guid postId)
    {
        try
        {
            var post = await scrapPostService.GetPost(postId);
            return Ok(post);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    ///     Create new Scrap Post with scrap post information and list of scrap post details
    /// </summary>
    [Authorize(Roles = "Household")]
    [HttpPost]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateScrapPost([FromBody] ScrapPostCreateModel scrapPostCreateModel)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            scrapPostCreateModel.HouseholdId = Guid.Parse(householdId);
            var post = await scrapPostService.CreateScrapPost(scrapPostCreateModel);
            return CreatedAtAction(nameof(GetPost), new { postId = post.ScrapPostId }, post);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Household Update base information of scrap post
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="scrapPostRequestModel">Base information of scrap post</param>
    [Authorize(Roles = "Household")]
    [HttpPatch("{postId:guid}")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateScrapPost([FromRoute] Guid postId,
        [FromBody] ScrapPostUpdateModel scrapPostRequestModel)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = await scrapPostService.UpdateScrapPost(Guid.Parse(householdId), postId,
                scrapPostRequestModel);
            return Ok(post);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Admin and Household can toggle scrap post between Open and Closed status
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    [Authorize(Roles = "Household, Admin")]
    [HttpPatch("{postId:guid}/toggle")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ToggleScrapPost([FromRoute] Guid postId)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var result = await scrapPostService.ToggleScrapPost(Guid.Parse(householdId), postId, userRole);
            if (!result) return BadRequest("Failed to delete scrap post");
            return Ok("Toggle successfully");
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Can get scrap post detail by id of scrap post and scrap category
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="scrapCategoryId">ID of scrap category</param>
    [HttpGet("{postId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScrapPostDetailById([FromRoute] Guid postId,
        [FromQuery] int scrapCategoryId)
    {
        try
        {
            var detail = await scrapPostDetailService.GetScrapPostDetailById(postId, scrapCategoryId);
            return Ok(detail);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    ///     Household can create new scrap post detail for their scrap post
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="scrapPostDetailCreateModel">New information for scrap post detail</param>
    [Authorize(Roles = "Household")]
    [HttpPost("{postId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateScrapPostDetail([FromRoute] Guid postId,
        [FromBody] ScrapPostDetailCreateModel scrapPostDetailCreateModel)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var detail = await scrapPostDetailService.CreateScrapPostDetail(Guid.Parse(householdId), postId,
                scrapPostDetailCreateModel);
            return CreatedAtAction(nameof(GetScrapPostDetailById),
                new { postId, scrapCategoryId = detail.ScrapCategoryId }, detail);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Household can update base information of scrap post detail
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="scrapCategoryId">ID of scrap category</param>
    /// <param name="scrapPostDetailUpdateModel">Base information update for scrap post detail</param>
    [Authorize(Roles = "Household")]
    [HttpPatch("{postId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateScrapPostDetail([FromRoute] Guid postId,
        [FromQuery] int scrapCategoryId,
        [FromBody] ScrapPostDetailUpdateModel scrapPostDetailUpdateModel)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var detail = await scrapPostDetailService.UpdateScrapPostDetail(Guid.Parse(householdId), postId,
                scrapCategoryId, scrapPostDetailUpdateModel);
            return Ok(detail);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Admin and Household can delete scrap post detail
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="scrapCategoryId">ID of scrap category</param>
    [Authorize(Roles = "Household, Admin")]
    [HttpDelete("{postId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteScrapPostDetail([FromRoute] Guid postId,
        [FromQuery] int scrapCategoryId)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var result =
                await scrapPostDetailService.DeleteScrapPostDetail(Guid.Parse(householdId), postId, userRole,
                    scrapCategoryId);
            if (!result) return BadRequest("Failed to delete scrap post detail");
            return Ok("Delete successfully");
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}