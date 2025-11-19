using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Business.Services.Transactions;
using GreenConnectPlatform.Business.Services.Transactions.TransactionDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/transactions")]
[ApiController]
[Tags("Transactions")]
public class TransactionController(
    ITransactionService transactionService,
    ITransactionDetailService transactionDetailService) : ControllerBase
{
    /// <summary>
    ///     IndividualCollector, BusinessCollector can check in at the collection location for a transaction.
    /// </summary>
    /// <param name="transactionId">ID of transaction</param>
    /// <param name="location">Location include Longitude and Latitude</param>
    [HttpPatch("{transactionId:Guid}/check-in")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckIn(Guid transactionId, LocationModel location)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await transactionService.CheckIn(location, transactionId, Guid.Parse(userId));
        return Ok("Check-in successful");
    }

    /// <summary>
    ///     User can get transaction by transactionId.
    /// </summary>
    /// <param name="transactionId">ID of transaction</param>
    [HttpGet("{transactionId:Guid}")]
    [Authorize]
    [ProducesResponseType(typeof(TransactionModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransaction(Guid transactionId)
    {
        var transaction = await transactionService.GetTransaction(transactionId);
        return Ok(transaction);
    }

    /// <summary>
    ///     IndividualCollector, BusinessCollector and Household can get transactions by userId with pagination and sorting.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortByCreateAt">User can sort list transaction by creation date</param>
    /// <param name="sortByUpdateAt">User can sort list transaction by update date</param>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<TransactionOveralModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTransactionsByUserId(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool sortByCreateAt = false,
        [FromQuery] bool sortByUpdateAt = false)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var roleName = User.FindFirstValue(ClaimTypes.Role);
        var transactions = await transactionService.GetTransactionsByUserId(
            Guid.Parse(userId), roleName, pageNumber, pageSize, sortByCreateAt, sortByUpdateAt);
        return Ok(transactions);
    }


    [HttpPatch("{transactionId:Guid}/accept-cancel")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CancelOrAcceptTransaction(
        [FromRoute] Guid transactionId,
        [FromQuery] bool isAccept)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (isAccept)
        {
            await transactionService.CancelOrAcceptTransaction(transactionId, Guid.Parse(userId), isAccept);
            return Ok("Transaction accepted successfully");
        }

        await transactionService.CancelOrAcceptTransaction(transactionId, Guid.Parse(userId), isAccept);
        return Ok("Transaction rejected successfully");
    }

    [HttpPatch("{transactionId:Guid}/toggle")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CancelOrReopoenTransaction(
        [FromRoute] Guid transactionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await transactionService.CancelOrReopenTransaction(transactionId, Guid.Parse(userId));
        return Ok("Transaction status updated successfully");
    }

    /// <summary>
    ///     IndividualCollector, BusinessCollector can submit transaction details for a transaction.
    /// </summary>
    /// <param name="transactionId">ID of transaction</param>
    /// <param name="transactionDetailCreateModels">List of transaction details that the Household will submit for transaction</param>
    [HttpPost("{transactionId:Guid}/submit-details")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(List<TransactionDetailModel>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SubmitTransactionDetails(
        [FromRoute] Guid transactionId,
        [FromBody] List<TransactionDetailCreateModel> transactionDetailCreateModels)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var transactionDetails = await transactionDetailService.CreateTransactionDetails(
            transactionId, Guid.Parse(userId), transactionDetailCreateModels);
        return CreatedAtAction(nameof(GetTransaction), new { transactionId }, transactionDetails);
    }

    /// <summary>
    ///     IndividualCollector, BusinessCollector can update transaction detail for a transaction.
    /// </summary>
    /// <param name="transactionId">ID of transaction</param>
    /// <param name="scrapCategoryId">ID of scrap category in transaction detail</param>
    /// <param name="transactionDetailUpdateModel">Information of transaction details to update</param>
    [HttpPatch("{transactionId:Guid}/details")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(TransactionDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateTransactionDetail(
        [FromRoute] Guid transactionId,
        [FromQuery] int scrapCategoryId,
        [FromBody] TransactionDetailUpdateModel transactionDetailUpdateModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var transactionDetail = await transactionDetailService.UpdateTransactionDetail(
            Guid.Parse(userId), transactionId, scrapCategoryId, transactionDetailUpdateModel);
        return Ok(transactionDetail);
    }
}