using System.Security.Claims;
using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Services.CollectionOffers;
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
    IScrapPostDetailService scrapPostDetailService,
    ICollectionOfferService collectionOfferService
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
    [Authorize(Roles = "Admin, IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ScrapPostOverralModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
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
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
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
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPost([FromRoute] Guid postId)
    {
        var post = await scrapPostService.GetPost(postId);
        return Ok(post);
    }

    /// <summary>
    ///     Create new Scrap Post with scrap post information and list of scrap post details
    /// </summary>
    [Authorize(Roles = "Household")]
    [HttpPost]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateScrapPost([FromBody] ScrapPostCreateModel scrapPostCreateModel)
    {
        var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        scrapPostCreateModel.HouseholdId = Guid.Parse(householdId);
        var post = await scrapPostService.CreateScrapPost(scrapPostCreateModel);
        return CreatedAtAction(nameof(GetPost), new { postId = post.ScrapPostId }, post);
    }

    /// <summary>
    ///     Household Update base information of scrap post
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="scrapPostRequestModel">Base information of scrap post</param>
    [Authorize(Roles = "Household")]
    [HttpPatch("{postId:guid}")]
    [ProducesResponseType(typeof(ScrapPostModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateScrapPost([FromRoute] Guid postId,
        [FromBody] ScrapPostUpdateModel scrapPostRequestModel)
    {
        var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var post = await scrapPostService.UpdateScrapPost(Guid.Parse(householdId), postId,
            scrapPostRequestModel);
        return Ok(post);
    }

    /// <summary>
    ///     Admin and Household can toggle scrap post between Open and Closed status
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    [Authorize(Roles = "Household, Admin")]
    [HttpPatch("{postId:guid}/toggle")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ToggleScrapPost([FromRoute] Guid postId)
    {
        var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var result = await scrapPostService.ToggleScrapPost(Guid.Parse(householdId), postId, userRole);
        if (!result) return BadRequest("Failed to delete scrap post");
        return Ok("Toggle successfully");
    }

    /// <summary>
    ///     Can get scrap post detail by id of scrap post and scrap category
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="scrapCategoryId">ID of scrap category</param>
    [HttpGet("{postId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateScrapPostDetail([FromRoute] Guid postId,
        [FromBody] ScrapPostDetailCreateModel scrapPostDetailCreateModel)
    {
        var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var detail = await scrapPostDetailService.AddScrapPostDetail(Guid.Parse(householdId), postId,
            scrapPostDetailCreateModel);
        return CreatedAtAction(nameof(GetScrapPostDetailById),
            new { postId, scrapCategoryId = detail.ScrapCategoryId }, detail);
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
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateScrapPostDetail([FromRoute] Guid postId,
        [FromQuery] int scrapCategoryId,
        [FromBody] ScrapPostDetailUpdateModel scrapPostDetailUpdateModel)
    {
        var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var detail = await scrapPostDetailService.UpdateScrapPostDetail(Guid.Parse(householdId), postId,
            scrapCategoryId, scrapPostDetailUpdateModel);
        return Ok(detail);
    }

    /// <summary>
    ///     Admin and Household can delete scrap post detail
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="scrapCategoryId">ID of scrap category</param>
    [Authorize(Roles = "Household, Admin")]
    [HttpDelete("{postId:guid}/details")]
    [ProducesResponseType(typeof(ScrapPostDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteScrapPostDetail([FromRoute] Guid postId,
        [FromQuery] int scrapCategoryId)
    {
        var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        await scrapPostDetailService.DeleteScrapPostDetail(Guid.Parse(householdId), postId, userRole,
            scrapCategoryId);
        return Ok("Delete successfully");
    }

    /// <summary>
    ///     Household can get collection offers for their scrap post with pagination and filter by offer status
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="offerStatus">Status of offer for scrap post</param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    [HttpGet("{postId:guid}/offers")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(CollectionOfferOveralForHouseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCollectionOffers(
        [FromRoute] Guid postId,
        [FromQuery] OfferStatus? offerStatus,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await collectionOfferService.GetCollectionOffersForHousehold(pageNumber, pageSize, offerStatus,
            postId));
    }

    /// <summary>
    ///     User can get detail collection offers
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="offerId">ID of collection offer</param>
    [HttpGet("{postId:guid}/offers/{offerId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CollectionOfferModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCollectionOffer(
        [FromRoute] Guid postId,
        [FromRoute] Guid offerId)
    {
        var offer = await collectionOfferService.GetCollectionOffer(postId, offerId);
        return Ok(offer);
    }

    /// <summary>
    ///     IndividualCollector, BusinessCollector can create a collection offer for scrap post
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="collectionOfferCreateModel">Information of collection offer when collector creates offer for scrap post</param>
    [HttpPost("{postId:guid}/offers")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(CollectionOfferModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateCollectionOffer(
        [FromRoute] Guid postId,
        [FromBody] CollectionOfferCreateModel collectionOfferCreateModel)
    {
        var collectorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var offer = await collectionOfferService.CreateCollectionOffer(postId, Guid.Parse(collectorId),
            collectionOfferCreateModel);
        return CreatedAtAction(nameof(GetCollectionOffers), new { postId, offerId = offer.CollectionOfferId }, offer);
    }

    /// <summary>
    ///     Household can accept or reject collection offer for their scrap post
    /// </summary>
    /// <param name="postId">ID of scrap post</param>
    /// <param name="offerId">ID of collection offer</param>
    /// <param name="isAccepted">true is accepted, false is rejected</param>
    [HttpPatch("{postId:guid}/offers/{offerId:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RejectOrAcceptCollectionOffer(
        [FromRoute] Guid postId,
        [FromRoute] Guid offerId,
        [FromQuery] bool isAccepted)
    {
        var householdId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (isAccepted)
        {
            await collectionOfferService.RejectOrAcceptCollectionOffer(offerId, postId, Guid.Parse(householdId),
                isAccepted);
            return Ok("Collection offer accepted successfully");
        }

        await collectionOfferService.RejectOrAcceptCollectionOffer(offerId, postId, Guid.Parse(householdId),
            isAccepted);
        return Ok("Collection offer rejected successfully");
    }
}