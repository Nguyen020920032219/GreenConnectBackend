namespace GreenConnectPlatform.Business.Services.FileStorage;

public interface IFileStorageService
{
    /// <summary>
    ///     Tạo Signed URL để Client upload file (PUT).
    /// </summary>
    Task<string> GenerateUploadSignedUrlAsync(string objectName, string contentType);

    /// <summary>
    ///     Tạo Signed URL để Client xem file (GET).
    /// </summary>
    Task<string> GetReadSignedUrlAsync(string objectName);

    /// <summary>
    ///     Xóa file khỏi Bucket.
    /// </summary>
    Task DeleteFileAsync(string objectName);
}