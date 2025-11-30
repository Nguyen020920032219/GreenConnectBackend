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

    // --- 1. UPLOAD LOGIC ---

    public async Task<FileUploadResponse> GenerateAvatarUploadUrlAsync(Guid userId, FileUploadBaseRequest request)
    {
        var ext = Path.GetExtension(request.FileName);
        var path = $"avatars/{userId}/{Guid.NewGuid()}{ext}";
        var url = await _fileStorageService.GenerateUploadSignedUrlAsync(path, request.ContentType);
        return new FileUploadResponse { UploadUrl = url, FilePath = path };
    }

    public async Task<FileUploadResponse> GenerateVerificationUploadUrlAsync(Guid userId, FileUploadBaseRequest request)
    {
        var ext = Path.GetExtension(request.FileName);
        var path = $"verifications/{userId}/{Guid.NewGuid()}{ext}";
        var url = await _fileStorageService.GenerateUploadSignedUrlAsync(path, request.ContentType);
        return new FileUploadResponse { UploadUrl = url, FilePath = path };
    }

    public async Task<FileUploadResponse> GenerateScrapPostUploadUrlAsync(Guid userId, FileUploadBaseRequest request)
    {
        var ext = Path.GetExtension(request.FileName);
        var path = $"scraps/{userId}/{Guid.NewGuid()}{ext}";
        var url = await _fileStorageService.GenerateUploadSignedUrlAsync(path, request.ContentType);
        return new FileUploadResponse { UploadUrl = url, FilePath = path };
    }

    public async Task<FileUploadResponse> GenerateComplaintImageUploadUrlAsync(Guid userId,
        FileUploadBaseRequest request)
    {
        var extension = Path.GetExtension(request.FileName);
        var filePath = $"complaints/{userId}/{Guid.NewGuid()}{extension}";
        var signedUrl = await _fileStorageService.GenerateUploadSignedUrlAsync(filePath, request.ContentType);
        return new FileUploadResponse { UploadUrl = signedUrl, FilePath = filePath };
    }

    // --- 2. READ LOGIC (Lấy link xem ảnh) ---

    public async Task<string> GetFileReadUrlAsync(Guid userId, string role, string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return "";

        var segments = filePath.Split('/');
        if (segments.Length < 2) return "";

        var folderType = segments[0]; // avatars, verifications, scraps, checkins, complaints

        switch (folderType)
        {
            case "verifications":
                // CHỈ CHO PHÉP: Admin hoặc Chính chủ
                var ownerId = segments[1];
                if (role != "Admin" && ownerId != userId.ToString())
                    throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                        "Bạn không có quyền xem tài liệu này.");
                break;

            case "checkins":
                // CHỈ CHO PHÉP: User đã đăng nhập (Tốt nhất nên check thêm xem có liên quan Transaction không)
                // Ở mức MVP, yêu cầu đăng nhập là tạm ổn.
                if (userId == Guid.Empty)
                    throw new ApiExceptionModel(StatusCodes.Status401Unauthorized, "401", "Vui lòng đăng nhập để xem.");
                break;

            case "complaints":
                // [MỚI] Check quyền xem bằng chứng khiếu nại
                var complaintIdStr = segments[1];
                if (Guid.TryParse(complaintIdStr, out var complaintId))
                {
                    var complaint = await _complaintRepository.GetByIdAsync(complaintId);
                    if (complaint != null)
                        // CHỈ CHO PHÉP: Admin, Người tố cáo, Người bị tố cáo
                        if (role != "Admin" &&
                            complaint.ComplainantId != userId &&
                            complaint.AccusedId != userId)
                            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                                "Bạn không có quyền xem bằng chứng này.");
                }

                break;
        }

        return await _fileStorageService.GetReadSignedUrlAsync(filePath);
    }

    // --- 3. DELETE LOGIC ---

    public async Task DeleteFileAsync(Guid userId, string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        var segments = filePath.Split('/');
        if (segments.Length < 2) return;

        var folderType = segments[0];

        // 1. Chặn xóa các file bằng chứng quan trọng
        if (folderType == "checkins" || folderType == "complaints")
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Không thể xóa bằng chứng giao dịch/khiếu nại.");

        // 2. Check quyền sở hữu cho các file cá nhân
        if (folderType == "avatars" || folderType == "verifications" || folderType == "scraps")
            if (segments[1] != userId.ToString())
                throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền xóa file này.");

        await _fileStorageService.DeleteFileAsync(filePath);
    }

    public async Task<string> UploadScrapImageDirectAsync(Guid userId, IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName);
        var objectName = $"scraps/{userId}/{Guid.NewGuid()}{ext}";
        using var stream = file.OpenReadStream();
        await _fileStorageService.UploadFileStreamAsync(stream, objectName, file.ContentType);
        return objectName;
    }
}