namespace GreenConnectPlatform.Business.Services.FileStorage;

public interface IFileStorageService
{
    Task<string> GenerateUploadSignedUrlAsync(string objectName, string contentType);
    Task<string> GetReadSignedUrlAsync(string objectName);
    Task DeleteFileAsync(string objectName);
    Task<string> UploadFileStreamAsync(Stream fileStream, string objectName, string contentType);
}