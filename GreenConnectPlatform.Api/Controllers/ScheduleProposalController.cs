using System.Security.Claims;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Business.Services.ScheduleProposals;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/schedules")]
[ApiController]
[Tags("Schedule Proposals")]
public class ScheduleProposalController(IScheduleProposalService scheduleProposalService) : ControllerBase
{
    /// <summary>
    ///   IndividualCollector and BusinessCollector can get all schedule proposals they made for collection offer with pagination and filtering by offer status.
    /// </summary>
    /// <param name="status">Status of offer for a schedule proposal</param>
    /// <param name="sortByCreateAt">Household can sort asc or des list schedule by create date</param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    [HttpGet]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(List<ScheduleProposalModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ScheduleProposalModel>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(List<ScheduleProposalModel>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(List<ScheduleProposalModel>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScheduleProposalsByCollectorId(
        [FromQuery] ProposalStatus? status, 
        [FromQuery] bool? sortByCreateAt = true,
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        var result = await scheduleProposalService.GetScheduleProposalsByCollectorId(pageNumber, pageSize, status, sortByCreateAt, userIdParsed);
        return Ok(result);
    }
    
    /// <summary>
    ///   IndividualCollector and BusinessCollector can update information of a schedule proposal they made.
    /// </summary>
    /// <param name="scheduleId">ID of a schedule proposal</param>
    /// <param name="proposedTime">Time you want to update for this schedule</param>
    /// <param name="responseMessage">Message for schedule</param>
    [HttpPatch("{scheduleId:Guid}")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateScheduleProposal(
        [FromRoute] Guid scheduleId,
        [FromQuery] DateTime? proposedTime,
        [FromQuery] string? responseMessage)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        var result = await scheduleProposalService.UpdateScheduleProposal(userIdParsed, scheduleId, proposedTime, responseMessage);
        return Ok(result);
    }
    
    /// <summary>
    ///   IndividualCollector and BusinessCollector can cancel or reopen of a schedule proposal they made.
    /// </summary>
    /// <param name="scheduleId">ID of a schedule proposal</param>
    [HttpPatch("{scheduleId:Guid}/toggle")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ScheduleProposalModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelOrReopenScheduleProposal(
        [FromRoute] Guid scheduleId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = Guid.Parse(userId);
        await scheduleProposalService.CancelOrReopenScheduleProposal(scheduleId, userIdParsed);
        return Ok("Toggle successfully");
    }
}