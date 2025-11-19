using System.Security.Claims;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Services.CollectionOffers.OfferDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/offer-details")]
[ApiController]
[Tags("Offer Details")]
public class OfferDetailController(IOfferDetailService offerDetailService) : ControllerBase
{
    /// <summary>
    ///     Crap Collector can update offers detail to their collection offer.
    /// </summary>
    /// <param name="offerDetailId">ID of offer detail</param>
    /// <param name="offerDetailUpdateModel">Information of offer detail</param>
    [HttpPatch("{offerDetailId:Guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(OfferDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateOfferDetail(
        [FromRoute] Guid offerDetailId,
        [FromBody] OfferDetailUpdateModel offerDetailUpdateModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        var updatedOfferDetail =
            await offerDetailService.UpdateOfferDetail(userIdParsed, offerDetailId, offerDetailUpdateModel);
        return Ok(updatedOfferDetail);
    }

    /// <summary>
    ///     Crap Collector can delete offer detail to their collection offer.
    /// </summary>
    /// <param name="offerDetailId">ID of collection offer</param>
    [HttpDelete("{offerDetailId:Guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteOfferDetail([FromRoute] Guid offerDetailId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        await offerDetailService.DeleteOfferDetail(userIdParsed, offerDetailId);
        return Ok("Delete successful");
    }
}