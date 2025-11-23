using Google;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
// Cần namespace này

namespace GreenConnectPlatform.Business.Services.FileStorage;

public class FirebaseStorageService : IFileStorageService
{
    private readonly string _bucketName;
    private readonly StorageClient _storageClient; // Reuse client connection
    private readonly UrlSigner _urlSigner;

    public FirebaseStorageService(IConfiguration configuration)
    {
        var serviceAccountPath = Path.Combine(AppContext.BaseDirectory, "Configs", "firebase-service-account.json");

        if (!File.Exists(serviceAccountPath))
            throw new FileNotFoundException($"Không tìm thấy file cấu hình Firebase tại: {serviceAccountPath}");

        // 1. Load Credential
        var credential = GoogleCredential.FromFile(serviceAccountPath);

        // 2. Init UrlSigner
        _urlSigner = UrlSigner.FromCredential(credential);

        // 3. Init StorageClient (Singleton pattern for performance)
        _storageClient = StorageClient.Create(credential);

        // 4. Load Bucket Name
        _bucketName = configuration["firebase_storage:bucket"]
                      ?? throw new InvalidOperationException(
                          "Thiếu cấu hình 'firebase_storage:bucket' trong appsettings.");

        // Loại bỏ tiền tố gs:// nếu lỡ tay nhập vào config
        _bucketName = _bucketName.Replace("gs://", "").TrimEnd('/');
    }

    public Task<string> GenerateUploadSignedUrlAsync(string objectName, string contentType)
    {
        // SỬA LỖI: Sử dụng dictionary cho ContentHeaders thay vì .WithContentType()
        var contentHeaders = new Dictionary<string, IEnumerable<string>>
        {
            { "Content-Type", new[] { contentType } }
        };

        var requestTemplate = UrlSigner.RequestTemplate
            .FromBucket(_bucketName)
            .WithObjectName(objectName)
            .WithHttpMethod(HttpMethod.Put)
            .WithContentHeaders(contentHeaders);

        var options = UrlSigner.Options.FromDuration(TimeSpan.FromMinutes(15))
            .WithSigningVersion(SigningVersion.V4);

        return Task.FromResult(_urlSigner.Sign(requestTemplate, options));
    }

    public Task<string> GetReadSignedUrlAsync(string objectName)
    {
        if (string.IsNullOrEmpty(objectName)) return Task.FromResult("");

        // URL xem ảnh sống trong 60 phút (hoặc tùy chỉnh)
        var signedUrl = _urlSigner.Sign(
            _bucketName,
            objectName,
            TimeSpan.FromHours(1),
            HttpMethod.Get,
            SigningVersion.V4
        );

        return Task.FromResult(signedUrl);
    }

    public async Task DeleteFileAsync(string objectName)
    {
        try
        {
            // Dùng _storageClient đã khởi tạo sẵn
            await _storageClient.DeleteObjectAsync(_bucketName, objectName);
        }
        catch (GoogleApiException ex) when (ex.Error.Code == 404)
        {
            // File không tồn tại -> Coi như xóa thành công (Idempotent)
            // Không cần log ra console gây rác log production
        }
        catch (Exception ex)
        {
            // Log error nếu cần thiết (với ILogger)
            throw new Exception($"Lỗi khi xóa file {objectName}: {ex.Message}", ex);
        }
    }
}