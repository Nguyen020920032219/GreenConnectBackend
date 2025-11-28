using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Data.Repositories.Complaints;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.Transactions;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Storage;

public class StorageService : IStorageService
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IScrapPostRepository _scrapPostRepository;
    private readonly ITransactionRepository _transactionRepository;

    public StorageService(
        IFileStorageService fileStorageService,
        IScrapPostRepository scrapPostRepository,
        ITransactionRepository transactionRepository,
        IComplaintRepository complaintRepository)
    {
        _fileStorageService = fileStorageService;
        _scrapPostRepository = scrapPostRepository;
        _transactionRepository = transactionRepository;
        _complaintRepository = complaintRepository;
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

    // 3. SCRAP POST: scraps/{userId}/{guid}.ext
    public async Task<FileUploadResponse> GenerateScrapPostUploadUrlAsync(Guid userId, FileUploadBaseRequest request)
    {
        var extension = Path.GetExtension(request.FileName);
        var filePath = $"scraps/{userId}/{Guid.NewGuid()}{extension}";

        var signedUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(filePath, request.ContentType);
        return new FileUploadResponse { UploadUrl = signedUrl, FilePath = filePath };
    }

    // 4. CHECK-IN: checkins/{transactionId}/{guid}.ext
    public async Task<FileUploadResponse> GenerateCheckInUploadUrlAsync(Guid userId, EntityFileUploadRequest request)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.EntityId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tồn tại.");

        if (transaction.ScrapCollectorId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không phải người thu gom của đơn hàng này.");

        var extension = Path.GetExtension(request.FileName);
        var filePath = $"checkins/{request.EntityId}/{Guid.NewGuid()}{extension}";

        var signedUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(filePath, request.ContentType);
        return new FileUploadResponse { UploadUrl = signedUrl, FilePath = filePath };
    }

    // [MỚI] 5. COMPLAINT: complaints/{complaintId}/{guid}.ext
    public async Task<FileUploadResponse> GenerateComplaintImageUploadUrlAsync(Guid userId,
        EntityFileUploadRequest request)
    {
        // 1. Check quyền: Phải là người tạo khiếu nại (Complainant)
        var complaint = await _complaintRepository.GetByIdAsync(request.EntityId);

        if (complaint == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Khiếu nại không tồn tại.");

        if (complaint.ComplainantId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không phải người tạo khiếu nại này.");

        // 2. Tạo đường dẫn
        var extension = Path.GetExtension(request.FileName);
        var filePath = $"complaints/{request.EntityId}/{Guid.NewGuid()}{extension}";

        var signedUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(filePath, request.ContentType);
        return new FileUploadResponse { UploadUrl = signedUrl, FilePath = filePath };
    }

    // 5. GET READ URL
    public async Task<string> GetFileReadUrlAsync(string filePath)
    {
        return await _fileStorageService.GetReadSignedUrlAsync(filePath);
    }

    // 6. DELETE
    public async Task DeleteFileAsync(Guid userId, string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        var segments = filePath.Split('/');
        if (segments.Length < 2) return;

        // Check quyền sở hữu dựa trên cấu trúc thư mục: {type}/{userId}/...
        if (segments[0] != "checkins")
        {
            if (segments[1] != userId.ToString())
                throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền xóa file này.");
        }
        else
        {
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Không thể xóa ảnh bằng chứng giao dịch.");
        }

        await _fileStorageService.DeleteFileAsync(filePath);
    }
}