using Google;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;

namespace GreenConnectPlatform.Business.Services.FileStorage;

public class FirebaseStorageService : IFileStorageService
{
    private readonly string _bucketName;
    private readonly StorageClient _storageClient;
    private readonly UrlSigner _urlSigner;

    public FirebaseStorageService(IConfiguration configuration)
    {
        var serviceAccountPath = Path.Combine(AppContext.BaseDirectory, "Configs", "firebase-service-account.json");

        if (!File.Exists(serviceAccountPath))
            throw new FileNotFoundException($"Không tìm thấy tệp cấu hình Firebase tại: {serviceAccountPath}");

        using var stream = new FileStream(serviceAccountPath, FileMode.Open, FileAccess.Read);
        var credential = GoogleCredential.FromStream(stream)
            .CreateScoped("https://www.googleapis.com/auth/cloud-platform");

        _urlSigner = UrlSigner.FromCredential(credential);
        _storageClient = StorageClient.Create(credential);

        _bucketName = configuration["firebase_storage:bucket"]
                      ?? throw new InvalidOperationException(
                          "Thiếu cấu hình 'firebase_storage:bucket' trong appsettings.");

        _bucketName = _bucketName.Replace("gs://", "").TrimEnd('/');
    }

    public Task<string> GenerateUploadSignedUrlAsync(string objectName, string contentType)
    {
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
            await _storageClient.DeleteObjectAsync(_bucketName, objectName);
        }
        catch (GoogleApiException ex) when (ex.Error.Code == 404)
        {
            // Bỏ qua nếu file không tồn tại (Idempotent)
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi xóa tệp {objectName}: {ex.Message}", ex);
        }
    }

    public async Task<string> UploadFileStreamAsync(Stream fileStream, string objectName, string contentType)
    {
        try
        {
            // Đảm bảo stream ở vị trí đầu
            if (fileStream.Position > 0) fileStream.Position = 0;

            // Upload lên Bucket
            await _storageClient.UploadObjectAsync(_bucketName, objectName, contentType, fileStream);

            // Trả về objectName (FilePath) để lưu vào DB
            return objectName;
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi upload file lên Firebase từ Server: {ex.Message}", ex);
        }
    }
}