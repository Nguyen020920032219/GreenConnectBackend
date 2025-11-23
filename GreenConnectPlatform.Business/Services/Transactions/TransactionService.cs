using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
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

    public TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
    }

    public async Task CheckInAsync(Guid transactionId, Guid collectorId, LocationModel currentLocation)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Transaction not found.");

        if (transaction.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (transaction.Status != TransactionStatus.Scheduled)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Can only check-in for Scheduled transactions.");

        var postLocation = transaction.Offer.ScrapPost.Location;
        if (postLocation == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Scrap post has no location data.");

        if (currentLocation.Longitude == null || currentLocation.Latitude == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Invalid current location.");

        var collectorPoint =
            _geometryFactory.CreatePoint(
                new Coordinate(currentLocation.Longitude.Value, currentLocation.Latitude.Value));

        // Tính khoảng cách (Độ -> Mét, xấp xỉ)
        var distanceInDegrees = postLocation.Distance(collectorPoint);
        var distanceInMeters = distanceInDegrees * 111319.9;

        if (distanceInMeters > CheckinRadiusMeters)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                $"You are too far ({distanceInMeters:F0}m). Must be within {CheckinRadiusMeters}m.");

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
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Transaction not found.");
        return _mapper.Map<TransactionModel>(transaction);
    }

    public async Task<PaginatedResult<TransactionOveralModel>> GetListAsync(
        Guid userId, string role, int pageNumber, int pageSize, bool sortByCreateAt, bool sortByUpdateAt)
    {
        var (items, total) = await _transactionRepository.GetByUserIdAsync(
            userId, role, sortByCreateAt, sortByUpdateAt, pageNumber, pageSize);

        var data = _mapper.Map<List<TransactionOveralModel>>(items);
        return new PaginatedResult<TransactionOveralModel>
            { Data = data, Pagination = new PaginationModel(total, pageNumber, pageSize) };
    }

    public async Task<List<TransactionDetailModel>> SubmitDetailsAsync(
        Guid transactionId, Guid collectorId, List<TransactionDetailCreateModel> details)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Transaction not found.");

        if (transaction.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (transaction.Status != TransactionStatus.InProgress)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Must Check-in before submitting details.");

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
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Transaction not found.");

        if (transaction.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (transaction.Status != TransactionStatus.InProgress)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Transaction is not in progress.");

        if (isAccepted && !transaction.TransactionDetails.Any())
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Collector hasn't submitted any items yet.");

        if (isAccepted)
        {
            transaction.Status = TransactionStatus.Completed;
            transaction.Offer.ScrapPost.Status = PostStatus.Completed;

            foreach (var detail in transaction.Offer.ScrapPost.ScrapPostDetails)
                detail.Status = PostDetailStatus.Collected;
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
        if (transaction == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Not found.");

        if (transaction.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (transaction.Status == TransactionStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Cannot cancel completed transaction.");

        if (transaction.Status == TransactionStatus.CanceledByUser)
            transaction.Status = TransactionStatus.InProgress;
        else
            transaction.Status = TransactionStatus.CanceledByUser;

        transaction.UpdatedAt = DateTime.UtcNow;
        await _transactionRepository.UpdateAsync(transaction);
    }
}