namespace GreenConnectPlatform.Business.Models.AI;

public class ScrapRecognitionResponse
{
    public string ItemName { get; set; } = string.Empty; // Tên món (VD: Vỏ lon bia)
    public double EstimatedQuantity { get; set; } // Số lượng ước tính
    public string Unit { get; set; } = "kg"; // Đơn vị (kg, cái, bao)
    public Guid? SuggestedCategoryId { get; set; } // ID danh mục khớp trong DB
    public string CategoryName { get; set; } = string.Empty; // Tên danh mục hiển thị
    public string Confidence { get; set; } = "Medium"; // Độ tin cậy
    public string? ImageUrl { get; set; } // Ảnh minh họa (sẽ gán ảnh tổng quan vào đây)
}