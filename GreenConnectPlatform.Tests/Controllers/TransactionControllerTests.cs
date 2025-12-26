using System.Security.Claims;
using System.Text.Json;
using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Business.Services.Feedbacks;
using GreenConnectPlatform.Business.Services.Transactions;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class TransactionControllerTests
{
    private readonly TransactionController _controller;
    private readonly Mock<IFeedbackService> _mockFeedbackService;
    private readonly Mock<ITransactionService> _mockService;
    private readonly Guid _testUserId;

    public TransactionControllerTests()
    {
        _mockService = new Mock<ITransactionService>();
        _mockFeedbackService = new Mock<IFeedbackService>();
        _controller = new TransactionController(_mockService.Object, _mockFeedbackService.Object);

        // Giả lập User Context (Role mặc định là Collector cho các case thao tác)
        _testUserId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()),
            new Claim(ClaimTypes.Role, "IndividualCollector")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    // =================================================================
    // GROUP 1: CHECK-IN (TRX-01, TRX-02)
    // =================================================================

    [Fact] // TRX-01: Collector Check-in successfully (Valid GPS)
    public async Task TRX01_CheckIn_ReturnsOk_WhenCoordinatesValid()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var location = new LocationModel { Latitude = 10.762622, Longitude = 106.660172 };

        // Service: CheckInAsync(transactionId, collectorId, location)
        _mockService.Setup(s => s.CheckInAsync(transactionId, _testUserId, location))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CheckIn(transactionId, location);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    [Fact] // TRX-02: Collector Check-in fail - Too Far
    public async Task TRX02_CheckIn_ThrowsBadRequest_WhenTooFar()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var location = new LocationModel { Latitude = 21.028511, Longitude = 105.804817 }; // Far location

        _mockService.Setup(s => s.CheckInAsync(transactionId, _testUserId, location))
            .ThrowsAsync(new ApiExceptionModel(400, "INVALID_LOCATION", "You are too far from the location"));

        // Act & Assert
        await _controller.Invoking(c => c.CheckIn(transactionId, location))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400 && e.Message.Contains("too far"));
    }

    // =================================================================
    // GROUP 2: SUBMIT DETAILS (TRX-03 -> TRX-06)
    // =================================================================
    // NOTE: Service signature changed to: SubmitDetailsAsync(scrapPostId, collectorId, slotId, details)

    [Fact] // TRX-04: Submit Details fail - Negative Price
    public async Task TRX04_SubmitDetails_ThrowsBadRequest_WhenPriceNegative()
    {
        // Arrange
        var scrapPostId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var details = new List<TransactionDetailCreateModel>
        {
            new() { ScrapCategoryId = categoryId, PricePerUnit = -5000 }
        };

        // Update Mock to match 4 arguments: scrapPostId, collectorId, slotId, details
        _mockService.Setup(s => s.SubmitDetailsAsync(scrapPostId, _testUserId, slotId, details))
            .ThrowsAsync(new ApiExceptionModel(400, "INVALID_PRICE", "Unit price must be positive"));

        // Act & Assert
        // Assuming Controller: SubmitDetails(Guid id, Guid slotId, List details) where id is ScrapPostId
        await _controller.Invoking(c => c.SubmitDetails(scrapPostId, slotId, details))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400);
    }

    [Fact] // TRX-05: Submit Details fail - Zero/Negative Weight (Quantity)
    public async Task TRX05_SubmitDetails_ThrowsBadRequest_WhenWeightInvalid()
    {
        // Arrange
        var scrapPostId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var details = new List<TransactionDetailCreateModel>
        {
            new() { Quantity = 0 }
        };

        // Update Mock
        _mockService.Setup(s => s.SubmitDetailsAsync(It.IsAny<Guid>(), _testUserId, It.IsAny<Guid>(), details))
            .ThrowsAsync(new ApiExceptionModel(400, "INVALID_WEIGHT", "Weight must be greater than 0"));

        // Act & Assert
        await _controller.Invoking(c => c.SubmitDetails(scrapPostId, slotId, details))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400);
    }

    [Fact] // TRX-06: Submit Details fail - Empty List
    public async Task TRX06_SubmitDetails_ThrowsBadRequest_WhenListEmpty()
    {
        // Arrange
        var scrapPostId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var emptyList = new List<TransactionDetailCreateModel>();

        // Update Mock
        _mockService.Setup(s => s.SubmitDetailsAsync(It.IsAny<Guid>(), _testUserId, It.IsAny<Guid>(), emptyList))
            .ThrowsAsync(new ApiExceptionModel(400, "EMPTY_LIST", "At least one item is required"));

        // Act & Assert
        await _controller.Invoking(c => c.SubmitDetails(scrapPostId, slotId, emptyList))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400);
    }

    // =================================================================
    // GROUP 3: PROCESS TRANSACTION (TRX-07, TRX-08)
    // =================================================================
    // NOTE: Service signature changed to: 
    // ProcessTransactionAsync(scrapPostId, collectorId, slotId, householdId, isAccepted, paymentMethod)

    [Fact] // TRX-07: Household Confirm Transaction (Success)
    public async Task TRX07_Process_Confirm_ReturnsOk()
    {
        // Arrange - Switch to Household Role
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Household"),
            new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString())
        }, "mock"));
        _controller.ControllerContext.HttpContext.User = user;

        var scrapPostId = Guid.NewGuid();
        var collectorId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var paymentMethod = TransactionPaymentMethod.Cash;

        // Mock Setup: 
        // Household đang login (_testUserId), họ confirm đơn của collectorId
        // Service argument householdId sẽ là _testUserId
        _mockService.Setup(s =>
                s.ProcessTransactionAsync(scrapPostId, collectorId, slotId, _testUserId, true, paymentMethod))
            .Returns(Task.CompletedTask);

        // Act
        // Assuming Controller: Process(Guid id, Guid collectorId, Guid slotId, bool isAccepted, Method)
        var result = await _controller.Process(scrapPostId, collectorId, slotId, true, paymentMethod);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    [Fact] // TRX-08: Household Reject Transaction
    public async Task TRX08_Process_Reject_ReturnsOk()
    {
        // Arrange - Switch to Household Role
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Household"),
            new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString())
        }, "mock"));
        _controller.ControllerContext.HttpContext.User = user;

        var scrapPostId = Guid.NewGuid();
        var collectorId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var paymentMethod = TransactionPaymentMethod.Cash;

        // Update Mock to match 6 args
        _mockService.Setup(s =>
                s.ProcessTransactionAsync(scrapPostId, collectorId, slotId, _testUserId, false, paymentMethod))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Process(scrapPostId, collectorId, slotId, false, paymentMethod);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =================================================================
    // GROUP 4: TOGGLE CANCEL (TRX-09)
    // =================================================================

    [Fact] // TRX-09: Collector Cancel Transaction
    public async Task TRX09_ToggleCancel_ReturnsOk_WhenSuccess()
    {
        // Arrange
        var transactionId = Guid.NewGuid();

        // Service: ToggleCancelAsync(transactionId, collectorId)
        _mockService.Setup(s => s.ToggleCancelAsync(transactionId, _testUserId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ToggleCancel(transactionId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =================================================================
    // GROUP 5: GET LIST & DETAIL (TRX-10, TRX-11, BOK-19)
    // =================================================================

    [Fact] // TRX-10: Get Transaction List (Filter by Status)
    public async Task TRX10_GetList_ReturnsOk_WithPaginatedData()
    {
        // Arrange
        var pagedResult = new PaginatedResult<TransactionOveralModel>
        {
            Data = new List<TransactionOveralModel>
            {
                new() { TransactionId = Guid.NewGuid(), Status = TransactionStatus.Completed }
            },
            Pagination = new PaginationModel(1, 1, 10)
        };

        // Mock service call with role "IndividualCollector" inferred from Claims
        _mockService.Setup(s => s.GetListAsync(_testUserId, "IndividualCollector", 1, 10, true, false))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetList();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var data = okResult.Value.Should().BeOfType<PaginatedResult<TransactionOveralModel>>().Subject;
        data.Data.Should().HaveCount(1);
    }

    [Fact] // TRX-11: Get Transaction Detail
    public async Task TRX11_GetById_ReturnsOk_WhenExists()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var detail = new TransactionModel
        {
            TransactionId = transactionId,
            TotalPrice = 150000,
            Status = TransactionStatus.InProgress
        };

        _mockService.Setup(s => s.GetByIdAsync(transactionId))
            .ReturnsAsync(detail);

        // Act
        var result = await _controller.GetById(transactionId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        ((TransactionModel)okResult.Value).TransactionId.Should().Be(transactionId);
    }

    // =================================================================
    // GROUP 6: QR CODE (TRX-12)
    // =================================================================

    [Fact] // TRX-12: Get Payment QR Code
    public async Task TRX12_GetQrCode_ReturnsUrl_WhenSuccess()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var expectedUrl = "https://img.vietqr.io/image/970436-123456789-compact.png";

        // Service: GetTransactionQrCodeAsync(transactionId, userId)
        _mockService.Setup(s => s.GetTransactionQrCodeAsync(transactionId, _testUserId))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await _controller.GetQrCode(transactionId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        // Verify anonymous object property or string content depending on Controller implementation
        var json = JsonSerializer.Serialize(okResult.Value);
        json.Should().Contain(expectedUrl);
    }
}