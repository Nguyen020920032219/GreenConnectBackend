using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PaymentPackages;
using GreenConnectPlatform.Business.Services.PaymentPackages;
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
    public class PaymentPackageControllerTests
    {
        private readonly Mock<IPaymentPackageService> _mockService;
        private readonly PaymentPackageController _controller;
        private readonly Guid _adminId;

        public PaymentPackageControllerTests()
        {
            _mockService = new Mock<IPaymentPackageService>();
            _controller = new PaymentPackageController(_mockService.Object);

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
        // PAY-01: Get Packages List
        // ==========================================
        [Fact]
        public async Task PAY01_GetList_ReturnsOk_WithPaginatedData()
        {
            // Arrange
            var pagedResult = new PaginatedResult<PaymentPackageOverallModel>
            {
                Data = new List<PaymentPackageOverallModel>
                {
                    new PaymentPackageOverallModel { PackageId = Guid.NewGuid(), Name = "Premium" }
                },
                Pagination = new PaginationModel(1, 1, 10)
            };

            _mockService.Setup(s => s.GetPaymentPackages(1, 10, null, null, null, null))
                .ReturnsAsync(pagedResult);

            // Act
            // [FIX] Tên hàm đúng trong Controller là GetPaymentPackages
            var result = await _controller.GetPaymentPackages(1, 10, null, null, null);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var data = okResult.Value.Should().BeOfType<PaginatedResult<PaymentPackageOverallModel>>().Subject;
            data.Data.Should().HaveCount(1);
        }

        // ==========================================
        // PAY-02: Get Package Detail
        // ==========================================
        [Fact]
        public async Task PAY02_GetById_ReturnsOk_WhenExists()
        {
            // Arrange
            var packageId = Guid.NewGuid();
            var detail = new PaymentPackageModel { PackageId = packageId, Name = "Gold", Price = 100000 };

            _mockService.Setup(s => s.GetPaymentPackage(packageId)).ReturnsAsync(detail);

            // Act
            // [FIX] Tên hàm đúng là GetPaymentPackage
            var result = await _controller.GetPaymentPackage(packageId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((PaymentPackageModel)okResult.Value).PackageId.Should().Be(packageId);
        }

        // ==========================================
        // PAY-03: Create Package
        // ==========================================
        [Fact]
        public async Task PAY03_Create_ReturnsCreated_WhenValid()
        {
            // Arrange
            var request = new PaymentPackageCreateModel
            {
                Name = "Diamond",
                Price = 500000,
                ConnectionAmount = 100,
                Description = "Best value",
                PackageType = 1
            };
            var createdPackage = new PaymentPackageModel { PackageId = Guid.NewGuid(), Name = "Diamond" };

            _mockService.Setup(s => s.CreatePaymentPackage(request)).ReturnsAsync(createdPackage);

            // Act
            // [FIX] Tên hàm đúng là CreatePaymentPackage
            var result = await _controller.CreatePaymentPackage(request);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(201);
            ((PaymentPackageModel)createdResult.Value).Name.Should().Be("Diamond");
        }

        // ==========================================
        // PAY-04: Update Package
        // ==========================================
        [Fact]
        public async Task PAY04_Update_ReturnsOk_WhenValid()
        {
            // Arrange
            var packageId = Guid.NewGuid();
            var request = new PaymentPackageUpdateModel { Name = "Diamond Plus" };
            var updatedPackage = new PaymentPackageModel { PackageId = packageId, Name = "Diamond Plus" };

            _mockService.Setup(s => s.UpdatePaymentPackage(packageId, request)).ReturnsAsync(updatedPackage);

            // Act
            // [FIX] Tên hàm đúng là UpdatePaymentPackage
            var result = await _controller.UpdatePaymentPackage(packageId, request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((PaymentPackageModel)okResult.Value).Name.Should().Be("Diamond Plus");
        }

        // ==========================================
        // PAY-05: Delete Package (Inactivate)
        // ==========================================
        [Fact]
        public async Task PAY05_Delete_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var packageId = Guid.NewGuid();

            _mockService.Setup(s => s.InActivePaymentPackage(packageId)).Returns(Task.CompletedTask);

            // Act
            // [FIX] Tên hàm đúng là InactivatePaymentPackage
            var result = await _controller.InactivatePaymentPackage(packageId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be("Đã vô hiệu hóa gói thanh toán thành công");
        }
    }
}