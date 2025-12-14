using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.VerificationInfos;
using GreenConnectPlatform.Business.Services.VerificationInfos;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;


namespace GreenConnectPlatform.Tests.Controllers
{
    public class AdminVerificationControllerTests
    {
        private readonly Mock<IVerificationInfoService> _mockService;
        private readonly AdminVerificationController _controller;
        private readonly Guid _adminId;

        public AdminVerificationControllerTests()
        {
            _mockService = new Mock<IVerificationInfoService>();
            _controller = new AdminVerificationController(_mockService.Object);

            // Giả lập Admin đang đăng nhập
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

        // ==========================================
        // GROUP 5: Admin Get List (ADM-01, ADM-02)
        // ==========================================

        [Fact] // ADM-01: Lấy danh sách hồ sơ (Phân trang)
        public async Task ADM01_GetList_ReturnsOk_WithPaginatedData()
        {
            // Arrange
            var pagedResult = new PaginatedResult<VerificationInfoOveralModel>
            {
                Data = new List<VerificationInfoOveralModel> 
                { 
                    new VerificationInfoOveralModel { UserId = Guid.NewGuid() } 
                },
                Pagination = new PaginationModel(10,1,1)
            };

            _mockService.Setup(s => s.GetVerificationInfos(1, 10, true, null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetList(1, 10, null, true);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var data = okResult.Value.Should().BeAssignableTo<PaginatedResult<VerificationInfoOveralModel>>().Subject;
            data.Pagination.Should().Be(pagedResult.Pagination);
        }

        [Fact] // ADM-02: Lọc danh sách theo trạng thái (Pending)
        public async Task ADM02_GetList_ReturnsOk_WithStatusFilter()
        {
            // Arrange
            var status = VerificationStatus.PendingReview;
            var resultList = new PaginatedResult<VerificationInfoOveralModel>();

            _mockService.Setup(s => s.GetVerificationInfos(1, 10, true, status))
                .ReturnsAsync(resultList);

            // Act
            var result = await _controller.GetList(1, 10, status, true);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        // ==========================================
        // GROUP 6: Admin Get Detail (ADM-03, ADM-04)
        // ==========================================

        [Fact] // ADM-03: Xem chi tiết hồ sơ thành công
        public async Task ADM03_GetDetail_ReturnsOk_WhenExists()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();
            var detail = new VerificationInfoModel 
            { 
                UserId = targetUserId, 
                IdentityNumber = "079123456789" 
            };

            _mockService.Setup(s => s.GetVerificationInfo(targetUserId))
                .ReturnsAsync(detail);

            // Act
            var result = await _controller.GetDetail(targetUserId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var data = okResult.Value.Should().BeOfType<VerificationInfoModel>().Subject;
            data.IdentityNumber.Should().Be("079123456789");
        }

        [Fact] // ADM-04: Xem chi tiết thất bại - Không tìm thấy
        public async Task ADM04_GetDetail_ThrowsNotFound_WhenMissing()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();

            _mockService.Setup(s => s.GetVerificationInfo(targetUserId))
                .ThrowsAsync(new ApiExceptionModel(404, "NOT_FOUND", "Verification info not found"));

            // Act & Assert
            await _controller.Invoking(c => c.GetDetail(targetUserId))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 404);
        }

        // ==========================================
        // GROUP 7: Admin Action (ADM-05, ADM-06, ADM-07)
        // ==========================================

        [Fact] // ADM-05: Duyệt hồ sơ (Approve)
        public async Task ADM05_UpdateStatus_Approve_ReturnsOk()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();

            _mockService.Setup(s => s.VerifyCollector(targetUserId, _adminId, true, null))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateStatus(targetUserId, true, null);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact] // ADM-06: Từ chối hồ sơ (Reject) có lý do
        public async Task ADM06_UpdateStatus_Reject_ReturnsOk_WithReason()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();
            string reason = "Ảnh bị mờ";

            _mockService.Setup(s => s.VerifyCollector(targetUserId, _adminId, false, reason))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateStatus(targetUserId, false, reason);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact] // ADM-07: Từ chối thất bại - Thiếu lý do (Service ném lỗi)
        public async Task ADM07_UpdateStatus_Reject_ThrowsException_WhenReasonMissing()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();

            _mockService.Setup(s => s.VerifyCollector(targetUserId, _adminId, false, null))
                .ThrowsAsync(new ApiExceptionModel(400, "MISSING_REASON", "Reason is required"));

            // Act & Assert
            await _controller.Invoking(c => c.UpdateStatus(targetUserId, false, null))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 400);
        }
    }
}