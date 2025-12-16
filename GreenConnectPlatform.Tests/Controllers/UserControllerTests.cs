using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.PointHistories;
using GreenConnectPlatform.Business.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace GreenConnectPlatform.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IPointHistoryService> _mockPointHistoryService;
        private readonly UserController _controller;
        private readonly Guid _adminId;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockPointHistoryService = new Mock<IPointHistoryService>();
            
            // [FIX] Constructor nhận 2 tham số
            _controller = new UserController(_mockUserService.Object, _mockPointHistoryService.Object);

            _adminId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _adminId.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        // --- ADM-05: Get User List ---
        [Fact]
        public async Task ADM05_GetUsers_ReturnsOk_WithPaginatedData()
        {
            // Arrange
            var pagedResult = new PaginatedResult<UserModel>
            {
                Data = new List<UserModel> { new UserModel { Id = Guid.NewGuid(), FullName = "User A" } },
                Pagination = new PaginationModel(1, 1, 10)
            };

            _mockUserService.Setup(s => s.GetUsersAsync(1, 10, null, null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetUsers(1, 10, null, null);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var data = okResult.Value.Should().BeOfType<PaginatedResult<UserModel>>().Subject;
            data.Data.Should().HaveCount(1);
        }

        // --- ADM-07, ADM-08: Ban/Unban User (Toggle) ---
        [Fact]
        public async Task ADM07_BanOrUnbanUser_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            _mockUserService.Setup(s => s.BanOrUnbanUserAsync(targetId, _adminId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.BanOrUnbanUser(targetId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Người dùng đã bị cấm hoặc mở lại thành công");
        }
        
        // Note: ADM-06 (Get User Detail) không có endpoint GetById trong UserController.
        // Bạn có thể dùng GetUsers với filter tên để mô phỏng hoặc bỏ qua case này.
    }
}
