using GreenConnectPlatform.Business.Models.Files;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Storage;

public interface IStorageService
{
    Task<FileUploadResponse> GenerateAvatarUploadUrlAsync(Guid userId, FileUploadBaseRequest request);
    Task<FileUploadResponse> GenerateVerificationUploadUrlAsync(Guid userId, FileUploadBaseRequest request);
    Task<FileUploadResponse> GenerateScrapPostUploadUrlAsync(Guid userId, FileUploadBaseRequest request);
    Task<FileUploadResponse> GenerateComplaintImageUploadUrlAsync(Guid userId, EntityFileUploadRequest request);
    Task<string> GetFileReadUrlAsync(Guid userId, string role, string filePath);
    Task DeleteFileAsync(Guid userId, string filePath);
    Task<string> UploadScrapImageDirectAsync(Guid userId, IFormFile file);
}