namespace GreenConnectPlatform.Business.Models.AI;

public class ScrapPostAiSuggestion
{
    // AI tự đặt tiêu đề (VD: "Dọn kho: 20kg giấy và vỏ lon")
    public string SuggestedTitle { get; set; } = string.Empty;

    // AI viết mô tả ngắn gọn
    public string SuggestedDescription { get; set; } = string.Empty;

    // Đường dẫn ảnh đã upload lên Firebase (để App hiển thị preview ngay)
    public string? SavedImageUrl { get; set; }

    // Đường dẫn file lưu trong DB (để App gửi lại API CreatePost vào trường ThumbnailUrl)
    public string? SavedImageFilePath { get; set; }

    // Danh sách các món rác đã nhận diện
    public List<ScrapRecognitionResponse> IdentifiedItems { get; set; } = new();
}