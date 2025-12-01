using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.PointHistories;
using GreenConnectPlatform.Data.Repositories.Transactions;
using Microsoft.AspNetCore.Http;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Business.Services.Transactions;

public class TransactionService : ITransactionService
{
    private const double CheckinRadiusMeters = 100.0;
    private readonly GeometryFactory _geometryFactory;
    private readonly IMapper _mapper;

    private readonly ITransactionRepository _transactionRepository;
    private readonly IPointHistoryRepository _pointHistoryRepository;
    private readonly IFileStorageService _fileStorageService;
    
    public TransactionService(
        ITransactionRepository transactionRepository,
        IPointHistoryRepository pointHistoryRepository,
        IFileStorageService fileStorageService,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _pointHistoryRepository = pointHistoryRepository;
        _fileStorageService = fileStorageService;
        _mapper = mapper;
        _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
    }

    public async Task CheckInAsync(Guid transactionId, Guid collectorId, LocationModel currentLocation)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tìm thấy.");

        if (transaction.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (transaction.Status == TransactionStatus.InProgress)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn đã checkin rồi.");
        if(transaction.Status == TransactionStatus.CanceledBySystem || transaction.Status == TransactionStatus.CanceledByUser || transaction.Status == TransactionStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Giao dịch đã bị huỷ hoặc hoàn thành rồi không thể checkin.");

        var postLocation = transaction.Offer.ScrapPost.Location;
        if (postLocation == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Bài đăng không có vị trí.");

        if (currentLocation.Longitude == null || currentLocation.Latitude == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Vị trí gửi không hợp lệ.");

        var collectorPoint =
            _geometryFactory.CreatePoint(
                new Coordinate(currentLocation.Longitude.Value, currentLocation.Latitude.Value));

        // Tính khoảng cách (Độ -> Mét, xấp xỉ)
        var distanceInDegrees = postLocation.Distance(collectorPoint);
        var distanceInMeters = distanceInDegrees * 111319.9;

        if (distanceInMeters > CheckinRadiusMeters)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                $"Bạn đang ở cách xa ({distanceInMeters:F0}m). Phải ở trong khoảng {CheckinRadiusMeters}m.");

        transaction.CheckInTime = DateTime.UtcNow;
        transaction.CheckInLocation = collectorPoint;
        transaction.Status = TransactionStatus.InProgress;
        transaction.UpdatedAt = DateTime.UtcNow;

        await _transactionRepository.UpdateAsync(transaction);
    }

    public async Task<TransactionModel> GetByIdAsync(Guid id)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(id);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tìm thấy.");
        var transactionModel = _mapper.Map<TransactionModel>(transaction);
        foreach (var d in transactionModel.Offer.ScrapPost.ScrapPostDetails)
        {
            if (!string.IsNullOrEmpty(d.ImageUrl))
            {
                d.ImageUrl = await _fileStorageService.GetReadSignedUrlAsync(d.ImageUrl);
            }
        }
        transactionModel.TotalPrice = transaction.TransactionDetails.Sum(d => d.FinalPrice);
        return transactionModel;
    }

    public async Task<PaginatedResult<TransactionOveralModel>> GetByOfferIdAsync(Guid offerId,
        TransactionStatus? status,
        bool sortByCreateAtDesc,
        bool sortByUpdateAtDesc,
        int pageIndex,
        int pageSize)
    {
        var (items, totalCount) = await _transactionRepository.GetByOfferIdAsync(offerId, status,
            sortByCreateAtDesc, sortByUpdateAtDesc, pageIndex, pageSize);
        var data = _mapper.Map<List<TransactionOveralModel>>(items);
        foreach (var d in data)
        {
            foreach (var p in d.Offer.ScrapPost.ScrapPostDetails)
            {
                if (!string.IsNullOrEmpty(p.ImageUrl))
                {
                    p.ImageUrl = await _fileStorageService.GetReadSignedUrlAsync(p.ImageUrl);
                }
            }
        }
        return new PaginatedResult<TransactionOveralModel>
        {
            Data = data,
            Pagination = new PaginationModel(totalCount, pageIndex, pageSize)
        };
    }

    public async Task<PaginatedResult<TransactionOveralModel>> GetListAsync(
        Guid userId, string role, int pageNumber, int pageSize, bool sortByCreateAt, bool sortByUpdateAt)
    {
        var (items, total) = await _transactionRepository.GetByUserIdAsync(
            userId, role, sortByCreateAt, sortByUpdateAt, pageNumber, pageSize);

        var data = _mapper.Map<List<TransactionOveralModel>>(items);
        foreach (var d in data)
        {
            foreach (var p in d.Offer.ScrapPost.ScrapPostDetails)
            {
                if (!string.IsNullOrEmpty(p.ImageUrl))
                {
                    p.ImageUrl = await _fileStorageService.GetReadSignedUrlAsync(p.ImageUrl);
                }
            }
        }
        return new PaginatedResult<TransactionOveralModel>
            { Data = data, Pagination = new PaginationModel(total, pageNumber, pageSize) };
    }

    public async Task<List<TransactionDetailModel>> SubmitDetailsAsync(
        Guid transactionId, Guid collectorId, List<TransactionDetailCreateModel> details)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tìm thấy.");

        if (transaction.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (transaction.Status != TransactionStatus.InProgress)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Phải check-in trước khi gửi chi tiết thu gom.");

        // Validate: Categories có khớp với Offer đã chốt không?
        var offerCategoryIds = transaction.Offer.OfferDetails.Select(d => d.ScrapCategoryId).ToList();
        var inputCategoryIds = details.Select(d => d.ScrapCategoryId).ToList();

        if (inputCategoryIds.Except(offerCategoryIds).Any())
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Contains item not in original Offer.");

        // --- LOGIC MỚI: Update trực tiếp trên List TransactionDetails ---

        // 1. Xóa các item cũ
        transaction.TransactionDetails.Clear();

        // 2. Thêm item mới
        var newDetails = _mapper.Map<List<TransactionDetail>>(details);
        foreach (var d in newDetails)
        {
            d.TransactionId = transactionId;
            d.FinalPrice = d.PricePerUnit * (decimal)d.Quantity;
            transaction.TransactionDetails.Add(d); // Add vào Collection của Parent
        }

        transaction.UpdatedAt = DateTime.UtcNow;

        // 3. Lưu Parent (EF Core sẽ tự xử lý Children)
        await _transactionRepository.UpdateAsync(transaction);

        // Vì Repository AddAsync/UpdateAsync có thể không return full details ngay
        // Nên map lại từ danh sách vừa add
        return _mapper.Map<List<TransactionDetailModel>>(transaction.TransactionDetails);
    }

    public async Task ProcessTransactionAsync(Guid transactionId, Guid householdId, bool isAccepted)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tìm thấy.");

        if (transaction.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (transaction.Status != TransactionStatus.InProgress)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Trạng thái giao dịch không phải là progress.");

        if (isAccepted && !transaction.TransactionDetails.Any())
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Người thu gom vẫn chưa gửi bất kỳ vật phẩm nào.");
        
        if (isAccepted)
        {
            transaction.Status = TransactionStatus.Completed;
            transaction.ScrapCollector.Profile.PointBalance += 10;
            var pointCollectorHistory = new PointHistory
            {
                PointHistoryId = Guid.NewGuid(),
                UserId = transaction.ScrapCollectorId,
                PointChange = 10,
                Reason = "Hoàn thành thu gom vật phẩm.",
                CreatedAt = DateTime.UtcNow
            };
            await _pointHistoryRepository.AddAsync(pointCollectorHistory);
            foreach (var detail in transaction.Offer.ScrapPost.ScrapPostDetails)
                detail.Status = PostDetailStatus.Collected;
            var scrapPosts = transaction.Offer.ScrapPost.ScrapPostDetails
                .Where(s => s.Status == PostDetailStatus.Available || s.Status == PostDetailStatus.Booked)
                .ToList();
            if (!scrapPosts.Any())
            {
                transaction.Offer.ScrapPost.Status = PostStatus.Completed;
                transaction.Household.Profile.PointBalance += 10;
                var pointHistory = new PointHistory
                {
                    PointHistoryId = Guid.NewGuid(),
                    UserId = transaction.HouseholdId,
                    PointChange = 10,
                    Reason = "Hoàn thành thu gom tất cả vật phẩm trong bài đăng.",
                    CreatedAt = DateTime.UtcNow
                };
                await _pointHistoryRepository.AddAsync(pointHistory);
            }
        }
        else
        {
            transaction.Status = TransactionStatus.CanceledByUser;
        }

        transaction.UpdatedAt = DateTime.UtcNow;
        await _transactionRepository.UpdateAsync(transaction);
    }

    public async Task ToggleCancelAsync(Guid transactionId, Guid collectorId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tìm thấy.");

        if (transaction.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền");

        if (transaction.Status == TransactionStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể hủy hoặc mở lại giao dịch khi đã hoàn thành.");

        if (transaction.Status == TransactionStatus.CanceledByUser)
            transaction.Status = TransactionStatus.InProgress;
        else
            transaction.Status = TransactionStatus.CanceledByUser;

        transaction.UpdatedAt = DateTime.UtcNow;
        await _transactionRepository.UpdateAsync(transaction);
    }
}