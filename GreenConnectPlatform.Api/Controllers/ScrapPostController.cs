using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Bussiness.Services.ScrapPosts;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/scrap-posts")]
[ApiController]
public class ScrapPostController(IScrapPostService scrapPostService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var posts = await scrapPostService.GetPosts(pageNumber, pageSize);
        return Ok(posts);
    }
    [HttpGet("{scrapPostId:guid}")]
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
    [HttpPost]
    public async Task<IActionResult> CreateScrapPost([FromBody] ScrapPostRequest scrapPostRequest)
    {
        try
        {
            var post = await scrapPostService.CreateScrapPost(scrapPostRequest);
            return CreatedAtAction(nameof(GetPost), new { scrapPostId = post.ScrapPostId }, post);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}