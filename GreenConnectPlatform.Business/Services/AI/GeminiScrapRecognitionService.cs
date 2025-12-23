using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using GreenConnectPlatform.Business.Models.AI;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GreenConnectPlatform.Business.Services.AI;

public class GeminiScrapRecognitionService : IScrapRecognitionService
{
    private readonly string _apiKey;
    private readonly IScrapCategoryRepository _categoryRepo;
    private readonly HttpClient _httpClient;
    private readonly string _modelId;
    private readonly IFileStorageService _storageService;

    public GeminiScrapRecognitionService(
        IConfiguration configuration,
        IScrapCategoryRepository categoryRepo,
        HttpClient httpClient,
        IFileStorageService storageService)
    {
        _httpClient = httpClient;
        _categoryRepo = categoryRepo;
        _storageService = storageService;

        _apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException("Thiếu Gemini API Key.");
        _modelId = configuration["Gemini:ModelId"] ?? "gemini-1.5-flash";
    }

    public async Task<ScrapPostAiSuggestion> AnalyzeImageAsync(IFormFile imageFile, Guid userId)
    {
        if (imageFile == null || imageFile.Length == 0)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "FILE_MISSING",
                "Vui lòng tải lên một hình ảnh.");

        // --- BƯỚC 1: Upload ảnh & Lấy Link Xem ---
        using var ms = new MemoryStream();
        await imageFile.CopyToAsync(ms);

        // Reset stream để đọc
        ms.Position = 0;

        var extension = Path.GetExtension(imageFile.FileName);
        var fileName = $"ai_scan_{Guid.NewGuid()}{extension}";
        var objectName = $"scrap-posts/{fileName}"; // Đây là đường dẫn lưu trong Bucket

        // 1. Upload lên Firebase
        await _storageService.UploadFileStreamAsync(ms, objectName, imageFile.ContentType);

        // 2. [FIX] Gọi hàm lấy URL (Signed URL) để Client xem được ảnh
        var viewableUrl = await _storageService.GetReadSignedUrlAsync(objectName);

        // --- BƯỚC 2: Chuẩn bị AI ---
        ms.Position = 0;
        var imageBytes = ms.ToArray();
        var base64Image = Convert.ToBase64String(imageBytes);

        var categories = await _categoryRepo.GetAllAsync();
        var categoryContext = string.Join(", ", categories.Select(c => $"{{Id: '{c.Id}', Name: '{c.Name}'}}"));

        // --- BƯỚC 3: Cấu hình Schema ---
        var responseSchema = new
        {
            type = "OBJECT",
            properties = new Dictionary<string, object>
            {
                { "suggestedTitle", new { type = "STRING" } },
                { "suggestedDescription", new { type = "STRING" } },
                {
                    "identifiedItems", new
                    {
                        type = "ARRAY",
                        items = new
                        {
                            type = "OBJECT",
                            properties = new Dictionary<string, object>
                            {
                                { "itemName", new { type = "STRING" } },
                                { "estimatedQuantity", new { type = "NUMBER" } },
                                { "unit", new { type = "STRING" } },
                                { "suggestedCategoryId", new { type = "STRING" } },
                                { "categoryName", new { type = "STRING" } },
                                { "confidence", new { type = "STRING" } }
                            },
                            required = new[] { "itemName", "estimatedQuantity", "unit" }
                        }
                    }
                }
            },
            required = new[] { "suggestedTitle", "suggestedDescription", "identifiedItems" }
        };

        // --- BƯỚC 4: Prompt "Gom Nhóm Thông Minh" ---
        var promptText = $@"
            Bạn là trợ lý phân loại ve chai thông minh. Hãy phân tích ảnh và gom nhóm các vật phẩm hợp lý.

            *** DANH MỤC HỆ THỐNG (CONTEXT) ***
            [{categoryContext}]

            *** NGUYÊN TẮC GOM NHÓM (QUAN TRỌNG) ***
            1. **GOM THEO LOẠI & HÌNH DẠNG (TYPE & FORM):** - Gom tất cả vật phẩm có cùng chất liệu và hình dạng tương tự vào một dòng.
               - **KHÔNG tách lẻ theo màu sắc** (Ví dụ: Đừng ghi 'Chai xanh', 'Chai trắng' -> Hãy ghi chung là **'Vỏ chai nhựa'**).
               - **KHÔNG tách lẻ theo kích thước nhỏ** (Ví dụ: Đừng ghi 'Hộp tròn', 'Hộp vuông' -> Hãy ghi chung là **'Hộp/Khay nhựa'**).
            
            2. **VÍ DỤ GOM NHÓM:**
               - (1 chai Pepsi + 1 chai Nước suối + 1 chai Dầu ăn) -> Gom thành: **'Vỏ chai nhựa các loại'** (Số lượng: 3).
               - (3 lon Bia + 2 lon Coca) -> Gom thành: **'Vỏ lon nhôm'** (Số lượng: 5).
               - (Nhiều bìa carton + Hộp giấy) -> Gom thành: **'Giấy bìa carton'**.
               - (Chai nhựa + Hộp cơm nhựa) -> Đây là 2 dạng khác nhau, hãy để 2 dòng: 'Vỏ chai nhựa' và 'Hộp nhựa'.

            3. **BỎ QUA CHI TIẾT THỪA:**
               - Bỏ qua nắp chai, nhãn mác, ống hút trừ khi chúng chiếm số lượng rất lớn.

            4. **TITLE & DESCRIPTION:**
               - Tạo tiêu đề hấp dẫn, ngắn gọn bao quát các nhóm chính (VD: 'Thanh lý 5kg Nhựa và Vỏ lon').

            5. **MAPPING:** Chọn Category ID phù hợp nhất cho nhóm đã gom.

            Hãy trả về JSON đúng format.
        ";

        // --- BƯỚC 5: Gọi AI ---
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = promptText },
                        new { inline_data = new { mime_type = imageFile.ContentType, data = base64Image } }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 0.2,
                response_mime_type = "application/json",
                response_schema = responseSchema
            }
        };

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_modelId}:generateContent?key={_apiKey}";

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var response = await _httpClient.PostAsJsonAsync(url, requestBody, jsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new ApiExceptionModel(StatusCodes.Status500InternalServerError, "AI_ERROR",
                $"Lỗi kết nối AI: {error}");
        }

        // --- BƯỚC 6: Xử lý kết quả ---
        var resultString = await response.Content.ReadAsStringAsync();

        try
        {
            using var doc = JsonDocument.Parse(resultString);
            var candidates = doc.RootElement.GetProperty("candidates");
            if (candidates.GetArrayLength() == 0) throw new Exception("No candidates");

            var textResult = candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text")
                .GetString();

            var resultObj = JsonSerializer.Deserialize<ScrapPostAiSuggestion>(textResult!, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new ScrapPostAiSuggestion();

            // [FIX] Gán đúng giá trị
            resultObj.SavedImageFilePath = objectName; // Path để lưu vào DB (khi CreatePost)
            resultObj.SavedImageUrl = viewableUrl; // URL để hiển thị lên App (có token)

            if (resultObj.IdentifiedItems != null)
                foreach (var item in resultObj.IdentifiedItems)
                    item.ImageUrl = viewableUrl; // Các item con cũng dùng link xem được này

            return resultObj;
        }
        catch (Exception)
        {
            return new ScrapPostAiSuggestion
            {
                SuggestedTitle = "Đã nhận diện ảnh",
                SuggestedDescription = "Vui lòng kiểm tra lại chi tiết.",
                SavedImageFilePath = objectName,
                SavedImageUrl = viewableUrl
            };
        }
    }
}