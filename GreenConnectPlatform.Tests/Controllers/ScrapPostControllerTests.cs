using System.Security.Claims;
using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Services.CollectionOffers;
using GreenConnectPlatform.Business.Services.ScrapPosts;
using GreenConnectPlatform.Business.Services.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class ScrapPostControllerTests
{
    private readonly ScrapPostController _controller;
    private readonly Mock<ICollectionOfferService> _mockCollectionOfferService;
    private readonly Mock<IScrapPostService> _mockScrapPostService;
    private readonly Mock<ITransactionService> _mockTransactionService;
    private readonly Guid _testUserId;

    public ScrapPostControllerTests()
    {
        _mockScrapPostService = new Mock<IScrapPostService>();
        _mockCollectionOfferService = new Mock<ICollectionOfferService>();
        _mockTransactionService = new Mock<ITransactionService>();
        _controller = new ScrapPostController(_mockScrapPostService.Object, _mockCollectionOfferService.Object,
            _mockTransactionService.Object);

        _testUserId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()),
            new Claim(ClaimTypes.Role, "Household")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    // ==========================================
    // 1. CREATE POST (PST-03, PST-04, PST-05)
    // ==========================================

    [Fact] // PST-03: Tạo bài đăng thành công
    public async Task PST03_Create_ReturnsCreated_WhenDataIsValid()
    {
        // Arrange
        var location = new LocationModel
        {
            Longitude = 106.660172,
            Latitude = 10.762622
        };

        var request = new ScrapPostCreateModel
        {
            Title = "Old Papers",
            Location = location,
            Description = "Come after 5PM"
        };

        var createdPost = new ScrapPostModel { Id = Guid.NewGuid(), Title = "Old Papers" };

        _mockScrapPostService.Setup(s => s.CreateAsync(_testUserId, request))
            .ReturnsAsync(createdPost);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        var data = createdResult.Value.Should().BeOfType<ScrapPostModel>().Subject;
        data.Title.Should().Be("Old Papers");
    }

    [Fact] // PST-04: Tạo bài đăng thất bại - Lỗi xác thực (Mock Service Logic)
    public async Task PST04_Create_ThrowsBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new ScrapPostCreateModel { Title = "" }; // Invalid title

        _mockScrapPostService.Setup(s => s.CreateAsync(_testUserId, request))
            .ThrowsAsync(new ApiExceptionModel(400, "VALIDATION_ERROR", "Title is required"));

        // Act & Assert
        await _controller.Invoking(c => c.Create(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400 && e.Message.Contains("Title"));
    }

    [Fact] // PST-05: Tạo bài đăng thất bại - Vị trí không hợp lệ
    public async Task PST05_Create_ThrowsBadRequest_WhenLocationIsInvalid()
    {
        // Arrange
        var request = new ScrapPostCreateModel
            { Location = new LocationModel { Latitude = 0, Longitude = 0 } }; // Invalid GPS

        _mockScrapPostService.Setup(s => s.CreateAsync(_testUserId, request))
            .ThrowsAsync(new ApiExceptionModel(400, "INVALID_LOCATION", "Coordinates are invalid"));

        // Act & Assert
        await _controller.Invoking(c => c.Create(request))
            .Should().ThrowAsync<ApiExceptionModel>();
    }

    // ==========================================
    // 2. SEARCH & GET (PST-06, PST-07, PST-08, PST-09, PST-10)
    // ==========================================

    [Fact] // PST-06: Tìm kiếm bài đăng với bộ lọc danh mục
    public async Task PST06_Search_ReturnsOk_WhenFilteredByCategory()
    {
        // Arrange
        var result = new PaginatedResult<ScrapPostOverralModel>
        {
            Data = new List<ScrapPostOverralModel> { new() { Title = "Plastic Bottles" } },
            Pagination = new PaginationModel(10, 1, 1)
        };

        // Mock service call with categoryId = 1
        _mockScrapPostService.Setup(s => s.SearchPostsAsync(
                It.IsAny<string>(), 1, 10, null, null, false, false, It.IsAny<Guid>()))
            .ReturnsAsync(result);

        // Act
        var response = await _controller.Search(null, null);

        // Assert
        var okResult = response.Should().BeOfType<OkObjectResult>().Subject;
        var data = okResult.Value.Should().BeAssignableTo<PaginatedResult<ScrapPostOverralModel>>().Subject;
        data.Data.Should().HaveCount(1);
    }

    [Fact] // PST-07: Xem bài đăng của tôi
    public async Task PST07_GetMyPosts_ReturnsList_WhenCalled()
    {
        // Arrange
        var myPosts = new PaginatedResult<ScrapPostOverralModel>
        {
            Data = new List<ScrapPostOverralModel> { new() { Title = "My Post" } },
            Pagination = new PaginationModel(10, 1, 1)
        };

        _mockScrapPostService.Setup(s => s.GetMyPostsAsync(1, 10, null, null, _testUserId))
            .ReturnsAsync(myPosts);

        // Act
        var result = await _controller.GetMyPosts(null, null);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(myPosts);
    }

    [Fact] // PST-08: Tìm kiếm bài đăng được sắp xếp theo vị trí
    public async Task PST08_Search_ReturnsOk_WhenSortedByLocation()
    {
        // Arrange
        _mockScrapPostService.Setup(s => s.SearchPostsAsync(
                It.IsAny<string>(), 1, 10, null, null, true, false, It.IsAny<Guid>()))
            .ReturnsAsync(new PaginatedResult<ScrapPostOverralModel>());

        // Act
        var result = await _controller.Search(null, null, true);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact] // PST-09 Tìm kiếm chi tiết bài đăng
    public async Task PST09_GetById_ReturnsOk_WhenPostExists()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var postDetail = new ScrapPostModel { Id = postId, Title = "Detail Post" };

        _mockScrapPostService.Setup(s => s.GetByIdAsync(postId))
            .ReturnsAsync(postDetail);

        // Act
        var result = await _controller.GetById(postId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        ((ScrapPostModel)okResult.Value).Id.Should().Be(postId);
    }

    [Fact] // PST-10 Tìm kiếm chi tiết bài đăng không tồn tại
    public async Task PST10_GetById_ReturnsNotFound_WhenPostDoesNotExists()
    {
        // Arrange
        var postNotFoundId = Guid.NewGuid();

        _mockScrapPostService.Setup(s => s.GetByIdAsync(postNotFoundId))
            .ThrowsAsync(new ApiExceptionModel(404, "NOT_FOUND", "Scrap post not found"));

        await _controller.Invoking(c => c.GetById(postNotFoundId))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 404);
    }

    // ==========================================
    // 3. UPDATE & DELETE (PST-11, PST-12, PST-13, PST-14)
    // ==========================================

    [Fact] // PST-11: Update Post Successfully
    public async Task PST11_Update_ReturnsOk_WhenDataIsValid()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var updateReq = new ScrapPostUpdateModel { Title = "Updated Title" };
        var updatedPost = new ScrapPostModel { Id = postId, Title = "Updated Title" };

        _mockScrapPostService.Setup(s => s.UpdateAsync(_testUserId, postId, updateReq))
            .ReturnsAsync(updatedPost);

        // Act
        var result = await _controller.Update(postId, updateReq);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        ((ScrapPostModel)okResult.Value).Title.Should().Be("Updated Title");
    }

    [Fact] // PST-12: Update Post-Failed - Locked Status
    public async Task PST12_Update_ThrowsException_WhenPostIsLocked()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var updateReq = new ScrapPostUpdateModel();

        _mockScrapPostService.Setup(s => s.UpdateAsync(_testUserId, postId, updateReq))
            .ThrowsAsync(new ApiExceptionModel(400, "POST_LOCKED", "Cannot update post in current status"));

        // Act & Assert
        await _controller.Invoking(c => c.Update(postId, updateReq))
            .Should().ThrowAsync<ApiExceptionModel>();
    }

    [Fact] // PST-13: Toggle Status (Cancel/Delete) Successfully
    public async Task PST13_ToggleStatus_ReturnsOk_WhenActionValid()
    {
        // Arrange
        var postId = Guid.NewGuid();

        _mockScrapPostService.Setup(s => s.ToggleStatusAsync(_testUserId, postId, "Household"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ToggleStatus(postId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    [Fact] // PST-14: Toggle Status Fail - Active Transaction
    public async Task PST14_ToggleStatus_ThrowsException_WhenActiveTransaction()
    {
        // Arrange
        var postId = Guid.NewGuid();

        _mockScrapPostService.Setup(s => s.ToggleStatusAsync(_testUserId, postId, "Household"))
            .ThrowsAsync(new ApiExceptionModel(400, "ACTIVE_TRANSACTION",
                "Cannot delete post with active transaction"));

        // Act & Assert
        await _controller.Invoking(c => c.ToggleStatus(postId))
            .Should().ThrowAsync<ApiExceptionModel>();
    }
}