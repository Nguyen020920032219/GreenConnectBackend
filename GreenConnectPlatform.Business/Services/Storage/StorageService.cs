using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.Transactions;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Storage;

public class StorageService : IStorageService
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IScrapPostRepository _scrapPostRepository;
    private readonly ITransactionRepository _transactionRepository;

    public StorageService(
        IFileStorageService fileStorageService,
        IScrapPostRepository scrapPostRepository,
        ITransactionRepository transactionRepository)
    {
        _fileStorageService = fileStorageService;
        _scrapPostRepository = scrapPostRepository;
        _transactionRepository = transactionRepository;
    }

    // 1. AVATAR: avatars/{userId}/{guid}.ext
    public async Task<FileUploadResponse> GenerateAvatarUploadUrlAsync(Guid userId, FileUploadBaseRequest request)
    {
        var extension = Path.GetExtension(request.FileName);
        var filePath = $"avatars/{userId}/{Guid.NewGuid()}{extension}";

        var signedUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(filePath, request.ContentType);
        return new FileUploadResponse { UploadUrl = signedUrl, FilePath = filePath };
    }

    // 2. VERIFICATION: verifications/{userId}/{guid}.ext
    public async Task<FileUploadResponse> GenerateVerificationUploadUrlAsync(Guid userId, FileUploadBaseRequest request)
    {
        var extension = Path.GetExtension(request.FileName);
        var filePath = $"verifications/{userId}/{Guid.NewGuid()}{extension}";

        var signedUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(filePath, request.ContentType);
        return new FileUploadResponse { UploadUrl = signedUrl, FilePath = filePath };
    }

    // 3. SCRAP POST: scraps/{userId}/{postId}/{guid}.ext
    public async Task<FileUploadResponse> GenerateScrapPostUploadUrlAsync(Guid userId, EntityFileUploadRequest request)
    {
        // Check quyền: Phải là chủ bài đăng
        var post = await _scrapPostRepository.GetByIdAsync(request.EntityId);
        if (post == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");

        if (post.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không có quyền đăng ảnh cho bài viết này.");

        var extension = Path.GetExtension(request.FileName);
        var filePath = $"scraps/{userId}/{request.EntityId}/{Guid.NewGuid()}{extension}";

        var signedUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(filePath, request.ContentType);
        return new FileUploadResponse { UploadUrl = signedUrl, FilePath = filePath };
    }

    // 4. CHECK-IN: checkins/{transactionId}/{guid}.ext
    public async Task<FileUploadResponse> GenerateCheckInUploadUrlAsync(Guid userId, EntityFileUploadRequest request)
    {
        // Check quyền: Phải là Collector của đơn hàng này
        var transaction = await _transactionRepository.GetByIdAsync(request.EntityId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tồn tại.");

        if (transaction.ScrapCollectorId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không phải người thu gom của đơn hàng này.");

        var extension = Path.GetExtension(request.FileName);
        // Path không chứa UserId để Admin/Household dễ truy cập, nhưng logic sinh link đã check quyền UserId
        var filePath = $"checkins/{request.EntityId}/{Guid.NewGuid()}{extension}";

        var signedUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(filePath, request.ContentType);
        return new FileUploadResponse { UploadUrl = signedUrl, FilePath = filePath };
    }

    // DELETE
    public async Task DeleteFileAsync(Guid userId, string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        // Parse path để check quyền cơ bản: avatars/USER_ID/...
        var segments = filePath.Split('/');

        // Nếu là ảnh cá nhân (avatars, verifications, scraps), segment[1] phải là UserId
        if (segments[0] == "avatars" || segments[0] == "verifications" || segments[0] == "scraps")
            if (segments.Length < 2 || segments[1] != userId.ToString())
                throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền xóa file này.");

        // Nếu là checkins, tạm thời chỉ cho phép xóa nếu logic nghiệp vụ cho phép (hoặc chặn luôn để làm bằng chứng)
        if (segments[0] == "checkins")
            // Tạm thời chặn xóa ảnh checkin để lưu bằng chứng
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Không thể xóa ảnh bằng chứng giao dịch.");

        await _fileStorageService.DeleteFileAsync(filePath);
    }
}