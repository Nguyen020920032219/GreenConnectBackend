using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Services.CollectionOffers;
using GreenConnectPlatform.Business.Services.Transactions;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace GreenConnectPlatform.Tests.Controllers
{
    public class CollectionOfferControllerTests
    {
        private readonly Mock<ICollectionOfferService> _mockOfferService;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly CollectionOfferController _controller;
        private readonly Guid _testUserId;

        public CollectionOfferControllerTests()
        {
            _mockOfferService = new Mock<ICollectionOfferService>();
            _mockTransactionService = new Mock<ITransactionService>();
            _controller = new CollectionOfferController(_mockOfferService.Object, _mockTransactionService.Object);

            _testUserId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()),
                new Claim(ClaimTypes.Role, "IndividualCollector")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        // ==========================================
        // 1. GET OFFERS (BOK-01)
        // ==========================================

        [Fact] // BOK-01: Collector views list of sent offers
        public async Task BOK01_GetMyOffers_ReturnsOk_WithPaginatedData()
        {
            // Arrange
            var pagedResult = new PaginatedResult<CollectionOfferOveralForCollectorModel>
            {
                Data = new List<CollectionOfferOveralForCollectorModel> 
                { 
                    new CollectionOfferOveralForCollectorModel { CollectionOfferId  = Guid.NewGuid(), Status = OfferStatus.Pending } 
                },
                Pagination = new PaginationModel(1, 1, 10)
            };

            _mockOfferService.Setup(s => s.GetByCollectorAsync(1, 10, OfferStatus.Pending, true, _testUserId))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetMyOffers(OfferStatus.Pending, true, 1, 10);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnData = okResult.Value.Should().BeOfType<PaginatedResult<CollectionOfferOveralForCollectorModel>>().Subject;
            returnData.Data.Should().HaveCount(1);
        }

        // ==========================================
        // 2. GET DETAIL (BOK-02, BOK-03)
        // ==========================================

        [Fact] // BOK-02: View offer details
        public async Task BOK02_GetById_ReturnsOk_WhenExists()
        {
            // Arrange
            var offerId = Guid.NewGuid();
            var detail = new CollectionOfferModel { CollectionOfferId = offerId };
            _mockOfferService.Setup(s => s.GetByIdAsync(offerId)).ReturnsAsync(detail);

            // Act
            var result = await _controller.GetById(offerId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((CollectionOfferModel)okResult.Value).CollectionOfferId.Should().Be(offerId);
        }

        [Fact] // BOK-03: View details failed - Not found
        public async Task BOK03_GetById_ThrowsNotFound_WhenMissing()
        {
            // Arrange
            var offerId = Guid.NewGuid();
            _mockOfferService.Setup(s => s.GetByIdAsync(offerId))
                .ThrowsAsync(new ApiExceptionModel(404, "NOT_FOUND", "Offer not found"));

            // Act & Assert
            await _controller.Invoking(c => c.GetById(offerId))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 404);
        }

        // ==========================================
        // 3. TOGGLE CANCEL (BOK-04, BOK-05, BOK-06)
        // ==========================================

        [Fact] // BOK-04: Collector cancels Offer
        public async Task BOK04_ToggleCancel_ReturnsOk_WhenPending()
        {
            // Arrange
            var offerId = Guid.NewGuid();
            _mockOfferService.Setup(s => s.ToggleCancelAsync(_testUserId, offerId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ToggleCancel(offerId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact] // BOK-05: Collector re-opens Offer
        public async Task BOK05_ToggleCancel_ReturnsOk_WhenCanceled()
        {
            // Arrange
            var offerId = Guid.NewGuid();
            _mockOfferService.Setup(s => s.ToggleCancelAsync(_testUserId, offerId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ToggleCancel(offerId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }
        [Fact] // BOK-06: Cancel failed - Already accepted
        public async Task BOK06_ToggleCancel_ThrowsBadRequest_WhenAccepted()
        {
            // Arrange
            var offerId = Guid.NewGuid();
            _mockOfferService.Setup(s => s.ToggleCancelAsync(_testUserId, offerId))
                .ThrowsAsync(new ApiExceptionModel(400, "INVALID_STATUS", "Cannot cancel accepted offer"));

            // Act & Assert
            await _controller.Invoking(c => c.ToggleCancel(offerId))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 400);
        }

        // ==========================================
        // 4. PROCESS OFFER (BOK-07, BOK-08, BOK-09)
        // ==========================================

        [Fact] // BOK-07: Household accepts offer
        public async Task BOK07_ProcessOffer_Accept_ReturnsOk()
        {
            // Arrange - Switch to Household context
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] 
            { 
                new Claim(ClaimTypes.Role, "Household"), 
                new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()) 
            }, "mock"));
            _controller.ControllerContext.HttpContext.User = user;

            var offerId = Guid.NewGuid();
            _mockOfferService.Setup(s => s.ProcessOfferAsync(_testUserId, offerId, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ProcessOffer(offerId, true);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact] // BOK-08: Household rejects offer
        public async Task BOK08_ProcessOffer_Reject_ReturnsOk()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] 
            { 
                new Claim(ClaimTypes.Role, "Household"), 
                new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()) 
            }, "mock"));
            _controller.ControllerContext.HttpContext.User = user;

            var offerId = Guid.NewGuid();
            _mockOfferService.Setup(s => s.ProcessOfferAsync(_testUserId, offerId, false)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ProcessOffer(offerId, false);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact] // BOK-09: Processing failed - Not the owner
        public async Task BOK09_ProcessOffer_ThrowsForbidden_WhenNotOwner()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] 
            { 
                new Claim(ClaimTypes.Role, "Household"), 
                new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()) 
            }, "mock"));
            _controller.ControllerContext.HttpContext.User = user;

            var offerId = Guid.NewGuid();
            _mockOfferService.Setup(s => s.ProcessOfferAsync(_testUserId, offerId, true))
                .ThrowsAsync(new ApiExceptionModel(403, "FORBIDDEN", "You do not have permission"));

            // Act & Assert
            await _controller.Invoking(c => c.ProcessOffer(offerId, true))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 403);
        }

        // ==========================================
        // 5. MANAGE DETAILS (BOK-10, BOK-11, BOK-12)
        // ==========================================

        [Fact] // BOK-10: Collector adds item to quotation
        public async Task BOK10_AddDetail_ReturnsCreated_WhenValid()
        {
            // Arrange
            var offerId = Guid.NewGuid();
            var request = new OfferDetailCreateModel 
            { 
                ScrapCategoryId = 1, 
                PricePerUnit = 5000, 
                Unit = "kg" 
            };
            var updatedOffer = new CollectionOfferModel { CollectionOfferId = offerId };

            _mockOfferService.Setup(s => s.AddDetailAsync(_testUserId, offerId, request)).Returns(Task.CompletedTask);
            _mockOfferService.Setup(s => s.GetByIdAsync(offerId)).ReturnsAsync(updatedOffer);

            // Act
            var result = await _controller.AddDetail(offerId, request);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(201);
        }

        [Fact] // BOK-11: Collector updates price (Negotiation)
        public async Task BOK11_UpdateDetail_ReturnsOk_WhenValid()
        {
            // Arrange
            var offerId = Guid.NewGuid();
            var detailId = Guid.NewGuid();
            var request = new OfferDetailUpdateModel { PricePerUnit = 6000 };

            _mockOfferService.Setup(s => s.UpdateDetailAsync(_testUserId, offerId, detailId, request)).Returns(Task.CompletedTask);
            _mockOfferService.Setup(s => s.GetByIdAsync(offerId)).ReturnsAsync(new CollectionOfferModel());

            // Act
            var result = await _controller.UpdateDetail(offerId, detailId, request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact] // BOK-12: Collector removes item from quotation
        public async Task BOK12_DeleteDetail_ReturnsNoContent_WhenValid()
        {
            // Arrange
            var offerId = Guid.NewGuid();
            var detailId = Guid.NewGuid();

            _mockOfferService.Setup(s => s.DeleteDetailAsync(_testUserId, offerId, detailId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteDetail(offerId, detailId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        // ==========================================
        // 6. TRANSACTIONS (BOK-19)
        // ==========================================

        [Fact] // BOK-19: View transactions related to Offer
        public async Task BOK19_GetTransactions_ReturnsOk_WithData()
        {
            // Arrange
            var offerId = Guid.NewGuid();
            var pagedResult = new PaginatedResult<TransactionOveralModel>
            {
                Data = new List<TransactionOveralModel> { new TransactionOveralModel { TransactionId = Guid.NewGuid() } },
                Pagination = new PaginationModel(1, 1, 10)
            };

            _mockTransactionService.Setup(s => s.GetByOfferIdAsync(offerId, null, true, true, 1, 10))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetTransactions(offerId, null);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var data = okResult.Value.Should().BeOfType<PaginatedResult<TransactionOveralModel>>().Subject;
            data.Data.Should().HaveCount(1);
        }
    }
}