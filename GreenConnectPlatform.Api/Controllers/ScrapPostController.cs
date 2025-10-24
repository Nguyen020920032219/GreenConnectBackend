using System.Security.Claims;
using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Bussiness.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Bussiness.Services.ScrapPosts;
using GreenConnectPlatform.Bussiness.Services.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/scrap-posts")]
[ApiController]
public class ScrapPostController(IScrapPostService scrapPostService, IScrapPostDetailService scrapPostDetailService)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ScrapPostOverralModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPosts(
        [FromQuery] string? categoryName,
        [FromQuery] bool sortByLocation = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        Guid? userIdParsed = null;
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            userIdParsed = Guid.Parse(userId);
        }

        var posts = await scrapPostService.GetPosts(pageNumber, pageSize, userIdParsed, categoryName, sortByLocation);
        return Ok(posts);
    }

    [HttpGet("my-scrap-posts")]
    [ProducesResponseType(typeof(ScrapPostOverralModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostsByHousehold(
        [FromQuery] string? title,
        [FromQuery] PostStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        Guid? userIdParsed = null;
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            userIdParsed = Guid.Parse(userId);
        }

        var posts = await scrapPostService.GetPostsByHousehold(pageNumber, pageSize, userIdParsed, title, status);
        return Ok(posts);
    }

    [HttpGet("{scrapPostId:guid}")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPost([FromRoute] Guid scrapPostId)
    {
        try
        {
            var post = await scrapPostService.GetPost(scrapPostId);
            return Ok(post);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [Authorize(Roles = "Household")]
    [HttpPost]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateScrapPost([FromBody] ScrapPostCreateModel scrapPostCreateModel)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            scrapPostCreateModel.HouseholdId = Guid.Parse(householdId);
            var post = await scrapPostService.CreateScrapPost(scrapPostCreateModel);
            return CreatedAtAction(nameof(GetPost), new { scrapPostId = post.ScrapPostId }, post);
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

    [Authorize(Roles = "Household")]
    [HttpPut("{scrapPostId:guid}")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateScrapPost([FromRoute] Guid scrapPostId,
        [FromBody] ScrapPostUpdateModel scrapPostRequestModel)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = await scrapPostService.UpdateScrapPost(Guid.Parse(householdId), scrapPostId,
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

    [Authorize(Roles = "Household")]
    [HttpPatch("{scrapPostId:guid}/toggle")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ToggleScrapPost([FromRoute] Guid scrapPostId)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await scrapPostService.ToggleScrapPost(Guid.Parse(householdId), scrapPostId);
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

    [HttpGet("{scrapPostId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScrapPostDetailById([FromRoute] Guid scrapPostId,
        [FromQuery] int scrapCategoryId)
    {
        try
        {
            var detail = await scrapPostDetailService.GetScrapPostDetailById(scrapPostId, scrapCategoryId);
            return Ok(detail);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [Authorize(Roles = "Household")]
    [HttpPost("{scrapPostId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateScrapPostDetail([FromRoute] Guid scrapPostId,
        [FromBody] ScrapPostDetailCreateModel scrapPostDetailCreateModel)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var detail = await scrapPostDetailService.CreateScrapPostDetail(Guid.Parse(householdId), scrapPostId,
                scrapPostDetailCreateModel);
            return CreatedAtAction(nameof(GetScrapPostDetailById),
                new { scrapPostId, scrapCategoryId = detail.ScrapCategoryId }, detail);
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

    [Authorize(Roles = "Household")]
    [HttpPut("{scrapPostId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateScrapPostDetail([FromRoute] Guid scrapPostId,
        [FromQuery] int scrapCategoryId,
        [FromBody] ScrapPostDetailUpdateModel scrapPostDetailUpdateModel)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var detail = await scrapPostDetailService.UpdateScrapPostDetail(Guid.Parse(householdId), scrapPostId,
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

    [Authorize(Roles = "Household")]
    [HttpDelete("{scrapPostId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteScrapPostDetail([FromRoute] Guid scrapPostId,
        [FromQuery] int scrapCategoryId)
    {
        try
        {
            var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result =
                await scrapPostDetailService.DeleteScrapPostDetail(Guid.Parse(householdId), scrapPostId,
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