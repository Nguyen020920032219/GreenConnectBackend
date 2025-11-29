namespace GreenConnectPlatform.Business.Models.AI;

public class ScrapRecognitionResponse
{
    // --- Thông tin từ AI ---
    public string ItemName { get; set; } = string.Empty; // Tên vật phẩm (VD: Lon Coca)
    public string Category { get; set; } = string.Empty; // Loại rác (VD: Lon nhôm)
    public string Material { get; set; } = string.Empty; // Chất liệu (VD: Nhôm)
    public bool IsRecyclable { get; set; } // Có tái chế được không

    // [ĐÃ SỬA] Ước lượng số lượng/kích thước (VD: "Khoảng 2kg", "3 túi lớn")
    public string EstimatedAmount { get; set; } = string.Empty;

    public string Advice { get; set; } = string.Empty; // Lời khuyên xử lý
    public double Confidence { get; set; } // Độ tin cậy

    // --- Thông tin từ Server ---
    public string SavedImageFilePath { get; set; } = string.Empty;
    public string SavedImageUrl { get; set; } = string.Empty;
}