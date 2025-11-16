using System.Security.Claims;
using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Business.Services.CollectionOffers;
using GreenConnectPlatform.Business.Services.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Services.ScheduleProposals;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/offers")]
[ApiController]
[Tags("Collection Offers")]
public class CollectionOfferController(
    ICollectionOfferService collectionOfferService, 
    IOfferDetailService offerDetailService, 
    IScheduleProposalService scheduleProposalService
    ) : ControllerBase
{
    /// <summary>
    ///    Scrap Collector can get all collection offers they made for scrap posts with pagination and filtering by offer status.
    /// </summary>
    /// <param name="offerStatus">Status of offer for scrap post</param>
    /// <param name="sortByCreateAt">Collector can sort asc or des list offer by create date</param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    [HttpGet]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(CollectionOfferOveralForCollectorModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CollectionOfferOveralForCollectorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(CollectionOfferOveralForCollectorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCollectionOffersForCollector(
        [FromQuery] OfferStatus? offerStatus,
        [FromQuery] bool? sortByCreateAt = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        var offers = await collectionOfferService.GetCollectionOffersForCollector(pageNumber, pageSize, offerStatus, sortByCreateAt, userIdParsed);
        return Ok(offers);
    }
    
    /// <summary>
    ///    Scrap Collector can cancel or reopen their collection offer for scrap post.
    /// </summary>
    /// <param name="offerId">ID of collection offer</param>
    [HttpPatch("{offerId:Guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CancelOrReopenCollectionOffer(
        [FromRoute] Guid offerId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        await collectionOfferService.CancelOrReopenCollectionOffer(offerId, userIdParsed);
        return Ok("Update successfully");
    }
    
    /// <summary>
    ///    User can get details of offer detail.
    /// </summary>
    /// <param name="offerId">ID of collection offer</param>
    /// <param name="offerDetailId">ID of offer detail</param>
    [HttpGet("{offerId:Guid}/details{offerDetailId:Guid}")]
    [Authorize]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOfferDetail(
        [FromRoute] Guid offerId,
        [FromRoute] Guid offerDetailId)
    {
        var offerDetail = await offerDetailService.GetOfferDetail(offerDetailId, offerId);
        return Ok(offerDetail);
    }
    
    /// <summary>
    ///    Crap Collector can add offer detail to their collection offer.
    /// </summary>
    /// <param name="offerId">ID of collection offer</param>
    /// <param name="offerDetailCreateModel">Information of offer detail</param>
    [HttpPost("{offerId:Guid}/details")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AddOfferDetail(
        [FromRoute] Guid offerId,
        [FromBody] OfferDetailCreateModel offerDetailCreateModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        var createdOfferDetail = await offerDetailService.AddOfferDetail(userIdParsed, offerId, offerDetailCreateModel);
        return CreatedAtAction(nameof(GetOfferDetail), new { offerId, offerDetailId = createdOfferDetail.OfferDetailId }, createdOfferDetail);
    }
    /// <summary>
    ///    Household can get all schedule proposals they made for collection offer with pagination and filtering by offer status.
    /// </summary>
    /// <param name="offerId">ID of collection offer</param>
    /// <param name="status">Status of offer for a schedule proposal</param>
    /// <param name="sortByCreateAt">Household can sort asc or des list schedule by create date</param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    
    [HttpGet("{offerId:Guid}/schedules")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(List<ScheduleProposalModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ScheduleProposalModel>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(List<ScheduleProposalModel>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(List<ScheduleProposalModel>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScheduleProposalsForOffer(
        [FromRoute] Guid offerId,
        [FromQuery] ProposalStatus? status,
        [FromQuery] bool? sortByCreateAt = true,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var proposals = await scheduleProposalService.GetScheduleProposalsByCollectionOfferId(pageNumber, pageSize, status, sortByCreateAt, offerId);
        return Ok(proposals);
    }
    
    /// <summary>
    ///    User can get detail schedule proposals
    /// </summary>
    /// <param name="offerId">ID of collection offer</param>
    /// <param name="scheduleId">ID of a schedule proposal</param>
    [HttpGet("{offerId:Guid}/schedules/{scheduleId:Guid}")]
    [Authorize(Roles = "Household, IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScheduleProposalForOffer(
        [FromRoute] Guid offerId,
        [FromRoute] Guid scheduleId)
    {
        var proposal = await scheduleProposalService.GetScheduleProposal(offerId, scheduleId);
        return Ok(proposal);
    }
    /// <summary>
    ///    User can reschedule a schedule proposal for a collection offer.
    /// </summary>
    /// <param name="offerId">ID of collection offer</param>
    /// <param name="model">Information of a model schedule proposal</param>
    [HttpPost("{offerId:Guid}/reschedules")]
    [Authorize(Roles = "Household, IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReScheduleProposalForOffer(
        [FromRoute] Guid offerId,
        [FromBody] ScheduleProposalCreateModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        var proposal = await scheduleProposalService.ReScheduleProposal(offerId, userIdParsed, model);
        return CreatedAtAction(nameof(GetScheduleProposalForOffer), new { offerId, scheduleId = proposal.ScheduleProposalId }, proposal);
    }

    /// <summary>
    ///    Household can accept or reject a schedule proposal for a collection offer.
    /// </summary>
    /// <param name="offerId">ID of collection offer</param>
    /// <param name="scheduleId">ID of a schedule proposal</param>
    /// <param name="isAccepted">True is Household accept schedule, False is Household reject schedule </param>
    [HttpPatch("{offerId:Guid}/schedules/{scheduleId:Guid}")]
    [Authorize(Roles = "Household")]
    public async Task<IActionResult> AcceptOrRejectScheduleProposal([FromRoute]Guid offerId,
        [FromRoute]Guid scheduleId, 
        [FromQuery] bool isAccepted)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        if (isAccepted)
        {
            await scheduleProposalService.RejectOrAcceptScheduleProposal(scheduleId, offerId, userIdParsed, isAccepted);
            return Ok("Schedule proposal accepted successfully");
        }
        else
        {
            await scheduleProposalService.RejectOrAcceptScheduleProposal(scheduleId, offerId, userIdParsed, isAccepted);
            return Ok("Schedule proposal rejected successfully");
        }
    }
}