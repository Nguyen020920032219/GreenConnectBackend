using GreenConnectPlatform.Business.Models.Files;

namespace GreenConnectPlatform.Business.Services.Storage;

public interface IStorageService
{
    Task<FileUploadResponse> GenerateAvatarUploadUrlAsync(Guid userId, FileUploadBaseRequest request);
    Task<FileUploadResponse> GenerateVerificationUploadUrlAsync(Guid userId, FileUploadBaseRequest request);
    Task<FileUploadResponse> GenerateScrapPostUploadUrlAsync(Guid userId, EntityFileUploadRequest request);
    Task<FileUploadResponse> GenerateCheckInUploadUrlAsync(Guid userId, EntityFileUploadRequest request);
    Task DeleteFileAsync(Guid userId, string filePath);
}