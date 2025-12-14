using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Business.Services.ScrapCategories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GreenConnectPlatform.Tests.Controllers
{
    public class ScrapCategoryControllerTests
    {
        private readonly Mock<IScrapCategoryService> _mockService;
        private readonly ScrapCategoryController _controller;

        public ScrapCategoryControllerTests()
        {
            _mockService = new Mock<IScrapCategoryService>();
            _controller = new ScrapCategoryController(_mockService.Object);
        }

        // ==========================================
        // 1. GET LIST (PST-15)
        // ==========================================
        [Fact] //PST-15 Xem danh sách loại phế liệu với phân trang
        public async Task PST15_GetList_ReturnsPaginatedList_WhenCalled()
        {
            // Arrange
            var categories = new PaginatedResult<ScrapCategoryModel>
            {
                Data = new List<ScrapCategoryModel> 
                { 
                    new ScrapCategoryModel { ScrapCategoryId = 1, CategoryName = "Plastic" } 
                },
                Pagination = new PaginationModel(1, 1, 10)
            };

            // Khớp tham số: pageNumber, pageSize, searchName
            _mockService.Setup(s => s.GetListAsync(1, 10, null))
                .ReturnsAsync(categories);

            // Act
            // Controller: GetList(string? searchName, int pageNumber = 1, int pageSize = 10)
            var result = await _controller.GetList(null, 1, 10);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var data = okResult.Value.Should().BeOfType<PaginatedResult<ScrapCategoryModel>>().Subject;
            data.Data.Should().HaveCount(1);
            data.Data[0].CategoryName.Should().Be("Plastic");
        }

        // ==========================================
        // 2. GET DETAIL (PST-16)
        // ==========================================
        [Fact] //PST-16 Xem chi tiết loại phế liệu theo ID
        public async Task PST16_GetById_ReturnsCategory_WhenIdExists()
        {
            // Arrange
            var category = new ScrapCategoryModel { ScrapCategoryId = 1, CategoryName = "Paper" };

            _mockService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(category);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((ScrapCategoryModel)okResult.Value).CategoryName.Should().Be("Paper");
        }
        
        [Fact] //PST-17 Xem chi tiết loại phế liệu theo ID không tồn tại
        public async Task PST17_GetById_ThrowsNotFound_WhenIdNotExists()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(99))
                .ThrowsAsync(new ApiExceptionModel(404, "NOT_FOUND", "Category not found"));

            // Act & Assert
            await _controller.Invoking(c => c.GetById(99))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 404);
        }
        
        // ==========================================
        // 3. CREATE (ADM-01, ADM-02)
        // ==========================================
        [Fact] //ADM-01 Tạo loại phế liệu mới với dữ liệu hợp lệ
        public async Task ADM01_Create_ReturnsCreated_WhenValid()
        {
            // Arrange
            string name = "Electronics";
            string description = "Electronic devices";
            
            var createdCategory = new ScrapCategoryModel 
            { 
                ScrapCategoryId = 1, 
                CategoryName = name, 
                Description = description 
            };

            // Setup service call với tham số string riêng lẻ
            _mockService.Setup(s => s.CreateAsync(name, description))
                .ReturnsAsync(createdCategory);

            // Act
            // Gọi hàm với tham số string theo định nghĩa Controller
            var result = await _controller.Create(name, description);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(201);
            ((ScrapCategoryModel)createdResult.Value).CategoryName.Should().Be(name);
        }

        [Fact] //ADM-02 Tạo loại phế liệu mới với tên không hợp lệ
        public async Task ADM02_Create_ThrowsBadRequest_WhenNameInvalid()
        {
            // Arrange
            string name = ""; // Invalid case giả định
            string description = "Desc";

            _mockService.Setup(s => s.CreateAsync(name, description))
                .ThrowsAsync(new ApiExceptionModel(400, "VALIDATION_ERROR", "Name is required"));

            // Act & Assert
            await _controller.Invoking(c => c.Create(name, description))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 400);
        }

        // ==========================================
        // 4. UPDATE (ADM-03)
        // ==========================================
        [Fact] //ADM-03 Cập nhật loại phế liệu với dữ liệu hợp lệ
        public async Task ADM03_Update_ReturnsOk_WhenValid()
        {
            // Arrange
            int categoryId = 1;
            string newName = "Updated Name";
            string newDesc = "Updated Desc";
            
            var updatedCategory = new ScrapCategoryModel 
            { 
                ScrapCategoryId = categoryId, 
                CategoryName = newName 
            };

            // Setup service call với tham số string riêng lẻ
            _mockService.Setup(s => s.UpdateAsync(categoryId, newName, newDesc))
                .ReturnsAsync(updatedCategory);

            // Act
            var result = await _controller.Update(categoryId, newName, newDesc);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((ScrapCategoryModel)okResult.Value).CategoryName.Should().Be(newName);
        }

        // ==========================================
        // 5. DELETE (ADM-04)
        // ==========================================
        [Fact] //ADM-04 Xóa loại phế liệu theo ID
        public async Task ADM04_Delete_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            int categoryId = 1;
            _mockService.Setup(s => s.DeleteAsync(categoryId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(categoryId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
        
        [Fact] //ADM-05 Xóa loại phế liệu đang được sử dụng
        public async Task ADM05_Delete_ThrowsBadRequest_WhenCategoryInUse()
        {
            // Arrange
            int categoryId = 1;
            _mockService.Setup(s => s.DeleteAsync(categoryId))
                .ThrowsAsync(new ApiExceptionModel(400, "BAD_REQUEST", "Cannot delete category currently in use"));

            // Act & Assert
            await _controller.Invoking(c => c.Delete(categoryId))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 400 && e.Message.Contains("Cannot delete category"));
        }
    }
}