using Firebase.Storage;
using Microsoft.Extensions.Configuration;

namespace GreenConnectPlatform.Bussiness.Services.FileStorage;

public class FirebaseStorageService : IFileStorageService
{
    private readonly string? _bucket;

    public FirebaseStorageService(IConfiguration configuration)
    {
        _bucket = configuration["FirebaseStorage:Bucket"];
        if (string.IsNullOrEmpty(_bucket))
        {
            throw new InvalidOperationException("Firebase Storage Bucket is not configured.");
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType,
        CancellationToken cancellationToken = default)
    {
        var storage = new FirebaseStorage(_bucket);

        var task = storage
            .Child("images")
            .Child(fileName)
            .PutAsync(fileStream, cancellationToken, contentType);

        var downloadUrl = await task;
        return downloadUrl;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var storage = new FirebaseStorage(_bucket);

        await storage
            .Child("images")
            .Child(fileName)
            .DeleteAsync();
    }
}