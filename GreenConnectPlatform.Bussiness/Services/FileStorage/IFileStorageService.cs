namespace GreenConnectPlatform.Bussiness.Services.FileStorage;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType,
        CancellationToken cancellationToken = default);

    Task DeleteFileAsync(string fileName);
}