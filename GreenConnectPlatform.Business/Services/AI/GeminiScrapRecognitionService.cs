using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using GreenConnectPlatform.Business.Models.AI;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GreenConnectPlatform.Business.Services.AI;

public class GeminiScrapRecognitionService : IScrapRecognitionService
{
    private readonly string _apiKey;
    private readonly IScrapCategoryRepository _categoryRepository;
    private readonly HttpClient _httpClient;
    private readonly string _modelId;

    public GeminiScrapRecognitionService(
        HttpClient httpClient,
        IConfiguration configuration,
        IScrapCategoryRepository categoryRepository)
    {
        _httpClient = httpClient;
        _categoryRepository = categoryRepository;
        _apiKey = configuration["GoogleAI:ApiKey"] ?? throw new InvalidOperationException("Thiếu Google AI API Key.");

        // [QUAN TRỌNG] Quay về dùng Flash để lấy tốc độ
        _modelId = configuration["GoogleAI:ModelId"] ?? "gemini-flash-latest";
    }

    public async Task<ScrapRecognitionResponse> RecognizeScrapImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Vui lòng gửi file ảnh.");

        // 1. Lấy danh sách Category
        var categories = await _categoryRepository.GetAllAsync();
        var categoryNames = string.Join(", ", categories.Select(c => $"'{c.CategoryName}'"));

        // 2. Chuyển ảnh sang Base64
        string base64Image;
        using (var ms = new MemoryStream())
        {
            await imageFile.CopyToAsync(ms);
            base64Image = Convert.ToBase64String(ms.ToArray());
        }

        // 3. Định nghĩa Schema (Giữ nguyên để đảm bảo format)
        var responseSchema = new
        {
            type = "OBJECT",
            properties = new Dictionary<string, object>
            {
                { "itemName", new { type = "STRING" } },
                { "category", new { type = "STRING" } },
                { "material", new { type = "STRING" } },
                { "isRecyclable", new { type = "BOOLEAN" } },
                { "estimatedAmount", new { type = "STRING" } },
                { "advice", new { type = "STRING" } },
                { "confidence", new { type = "NUMBER" } }
            },
            required = new[]
                { "itemName", "category", "material", "isRecyclable", "estimatedAmount", "advice", "confidence" }
        };

        // 4. [TỐI ƯU PROMPT CHO FLASH]
        // Thêm Persona (Vai trò) + Ví dụ cụ thể để Flash "khôn" hơn
        var prompt = $@"
            You are an expert in scrap recycling and waste management in Vietnam. 
            Analyze this image and identify the scrap item.

            *** CONSTRAINTS ***
            1. CATEGORY: Must be exactly one of: [{categoryNames}]. If unclear, return empty string.
            2. LANGUAGE: Respond entirely in VIETNAMESE.

            *** INSTRUCTIONS FOR FIELDS ***
            - ItemName: Specific name (e.g., 'Vỏ lon bia Tiger', 'Thùng carton cũ').
            - EstimatedAmount: Estimate quantity/weight visually (e.g., 'Khoảng 2kg', 'Một túi lớn đầy', '50 vỏ lon').
            - Advice: Provide actionable, specific preparation tips to increase scrap value.
              - BAD Advice: 'Thu gom và tái chế.' (Too generic)
              - GOOD Advice: 'Nên đổ hết nước thừa, đạp dẹp lon để tiết kiệm diện tích. Nếu là chai nhựa thì tháo nắp riêng.'

            Return the result in valid JSON matching the schema.
        ";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = prompt },
                        new
                        {
                            inline_data = new
                            {
                                mime_type = imageFile.ContentType,
                                data = base64Image
                            }
                        }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 0.1, // Vẫn giữ thấp để ổn định format JSON
                response_mime_type = "application/json",
                response_schema = responseSchema
            }
        };

        // 5. Gọi API
        // Lưu ý: Đảm bảo URL trỏ đúng modelId (gemini-1.5-flash)
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
            throw new ApiExceptionModel(StatusCodes.Status500InternalServerError, "AI_ERROR", $"Lỗi AI: {error}");
        }

        // 6. Parse kết quả
        var jsonResponse = await response.Content.ReadAsStringAsync();

        try
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            var textResult = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            var result = JsonSerializer.Deserialize<ScrapRecognitionResponse>(textResult!, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? throw new Exception("Kết quả rỗng");
        }
        catch
        {
            return new ScrapRecognitionResponse
            {
                ItemName = "Không xác định",
                Category = "",
                EstimatedAmount = "Không rõ",
                Advice = "Vui lòng nhập thông tin thủ công.",
                Confidence = 0
            };
        }
    }
}