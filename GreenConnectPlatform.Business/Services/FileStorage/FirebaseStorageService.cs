using Google;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;

namespace GreenConnectPlatform.Business.Services.FileStorage;

public class FirebaseStorageService : IFileStorageService
{
    private readonly string _bucketName;
    private readonly UrlSigner _urlSigner;

    public FirebaseStorageService(IConfiguration configuration)
    {
        var serviceAccountPath = Path.Combine(AppContext.BaseDirectory, "Configs", "firebase-service-account.json");
        _urlSigner = UrlSigner.FromCredentialFile(serviceAccountPath);

        _bucketName = configuration["firebase_storage:bucket"]!;
        if (string.IsNullOrEmpty(_bucketName))
            throw new InvalidOperationException("Không tìm thấy 'firebase_storage:bucket' trong cấu hình.");
    }

    public Task<string> GenerateUploadSignedUrlAsync(string objectName, string contentType)
    {
        var requestTemplate = UrlSigner.RequestTemplate
            .FromBucket(_bucketName)
            .WithObjectName(objectName)
            .WithHttpMethod(HttpMethod.Put);

        var options = UrlSigner.Options.FromDuration(TimeSpan.FromMinutes(15))
            .WithSigningVersion(SigningVersion.V4);

        var signedUrl = _urlSigner.Sign(requestTemplate, options);
        return Task.FromResult(signedUrl);
    }

    public Task<string> GetReadSignedUrlAsync(string objectName)
    {
        if (string.IsNullOrEmpty(objectName)) return Task.FromResult("URL_TO_DEFAULT_IMAGE");

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
        var storageClient = StorageClient.Create();
        try
        {
            await storageClient.DeleteObjectAsync(_bucketName, objectName);
        }
        catch (GoogleApiException ex) when (ex.Error.Code == 404)
        {
            Console.WriteLine($"File to delete not found: {objectName}");
        }
    }
}