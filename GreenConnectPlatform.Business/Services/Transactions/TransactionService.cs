using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Business.Services.Notifications;
using GreenConnectPlatform.Business.Services.Payment;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.PointHistories;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.Transactions;
using Microsoft.AspNetCore.Http;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Business.Services.Transactions;

public class TransactionService : ITransactionService
{
    private const double CheckinRadiusMeters = 100.0;
    private readonly IFileStorageService _fileStorageService;
    private readonly GeometryFactory _geometryFactory;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly IPointHistoryRepository _pointHistoryRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IVietQrService _vietQrService;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IPointHistoryRepository pointHistoryRepository,
        IFileStorageService fileStorageService,
        INotificationService notificationService,
        IMapper mapper,
        IVietQrService vietQrService,
        IProfileRepository profileRepository)
    {
        _transactionRepository = transactionRepository;
        _pointHistoryRepository = pointHistoryRepository;
        _fileStorageService = fileStorageService;
        _notificationService = notificationService;
        _mapper = mapper;
        _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
        _vietQrService = vietQrService;
        _profileRepository = profileRepository;
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
        if (transaction.Status == TransactionStatus.CanceledBySystem ||
            transaction.Status == TransactionStatus.CanceledByUser || transaction.Status == TransactionStatus.Completed)
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

        var householdId = transaction.HouseholdId;
        var title = "Người thu gom đã đến!";
        var body = $"Người thu gom {transaction.ScrapCollector?.FullName ?? "ẩn danh"} đã check-in tại điểm thu gom.";
        var data = new Dictionary<string, string> { { "type", "Transaction" }, { "id", transactionId.ToString() } };
        _ = _notificationService.SendNotificationAsync(householdId, title, body, data);
    }

    public async Task<TransactionModel> GetByIdAsync(Guid id)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(id);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tìm thấy.");
        var transactionModel = _mapper.Map<TransactionModel>(transaction);
        foreach (var d in transactionModel.Offer.ScrapPost.ScrapPostDetails)
            if (!string.IsNullOrEmpty(d.ImageUrl))
                d.ImageUrl = await _fileStorageService.GetReadSignedUrlAsync(d.ImageUrl);

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
        foreach (var p in d.Offer.ScrapPost.ScrapPostDetails)
            if (!string.IsNullOrEmpty(p.ImageUrl))
                p.ImageUrl = await _fileStorageService.GetReadSignedUrlAsync(p.ImageUrl);

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
        foreach (var p in d.Offer.ScrapPost.ScrapPostDetails)
            if (!string.IsNullOrEmpty(p.ImageUrl))
                p.ImageUrl = await _fileStorageService.GetReadSignedUrlAsync(p.ImageUrl);


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

        var offerCategoryIds = transaction.Offer.OfferDetails.Select(d => d.ScrapCategoryId).ToList();
        var inputCategoryIds = details.Select(d => d.ScrapCategoryId).ToList();

        if (inputCategoryIds.Except(offerCategoryIds).Any())
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Contains item not in original Offer.");

        transaction.TransactionDetails.Clear();

        var newDetails = _mapper.Map<List<TransactionDetail>>(details);
        foreach (var d in newDetails)
        {
            d.TransactionId = transactionId;
            d.FinalPrice = d.PricePerUnit * (decimal)d.Quantity;
            transaction.TransactionDetails.Add(d);
        }

        transaction.UpdatedAt = DateTime.UtcNow;

        await _transactionRepository.UpdateAsync(transaction);

        var householdId = transaction.HouseholdId;
        var title = "Xác nhận số lượng!";
        var body = "Người thu gom đã cập nhật số lượng và giá. Vui lòng kiểm tra và chốt đơn.";
        var data = new Dictionary<string, string> { { "type", "Transaction" }, { "id", transactionId.ToString() } };
        await _notificationService.SendNotificationAsync(householdId, title, body, data);

        return _mapper.Map<List<TransactionDetailModel>>(transaction.TransactionDetails);
    }

    public async Task ProcessTransactionAsync(Guid transactionId, Guid householdId, bool isAccepted,
        TransactionPaymentMethod paymentMethod)
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
            transaction.TotalAmount = transaction.TransactionDetails.Sum(d => d.FinalPrice);
            transaction.PaymentMethod = paymentMethod;
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
            var transactedCategoryIds = transaction.TransactionDetails
                .Select(td => td.ScrapCategoryId)
                .ToList();
            foreach (var detail in transaction.Offer.ScrapPost.ScrapPostDetails)
                if (transactedCategoryIds.Contains(detail.ScrapCategoryId))
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

            var title = "Giao dịch thành công!";
            var body = "Chúc mừng! Đơn hàng đã hoàn tất. Bạn nhận được +10 điểm thưởng.";
            var data = new Dictionary<string, string> { { "type", "Transaction" }, { "id", transactionId.ToString() } };
            await _notificationService.SendNotificationAsync(transaction.ScrapCollectorId, title, body, data);
        }
        else
        {
            transaction.Status = TransactionStatus.CanceledByUser;

            var title = "Giao dịch bị hủy";
            var body = "Hộ gia đình đã từ chối kết quả cân đo và hủy giao dịch.";
            var data = new Dictionary<string, string> { { "type", "Transaction" }, { "id", transactionId.ToString() } };
            await _notificationService.SendNotificationAsync(transaction.ScrapCollectorId, title, body, data);
        }

        transaction.UpdatedAt = DateTime.UtcNow;
        await _transactionRepository.UpdateAsync(transaction);
    }

    public async Task ToggleCancelAsync(Guid transactionId, Guid collectorId)
    {
        var transaction =
            await _transactionRepository
                .GetByIdWithDetailsAsync(
                    transactionId); // Fix: Dùng GetByIdWithDetailsAsync để lấy HouseholdId cho noti
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tìm thấy.");

        if (transaction.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền");

        if (transaction.Status == TransactionStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể hủy hoặc mở lại giao dịch khi đã hoàn thành.");

        if (transaction.Status == TransactionStatus.CanceledByUser)
        {
            transaction.Status = TransactionStatus.InProgress;
            var title = "Giao dịch được mở lại";
            var body = "Người thu gom đã mở lại giao dịch.";
            var data = new Dictionary<string, string> { { "type", "Transaction" }, { "id", transactionId.ToString() } };
            await _notificationService.SendNotificationAsync(transaction.HouseholdId, title, body, data);
        }
        else
        {
            transaction.Status = TransactionStatus.CanceledByUser;

            var title = "Giao dịch bị hủy";
            var body = "Người thu gom đã hủy giao dịch vì sự cố.";
            var data = new Dictionary<string, string> { { "type", "Transaction" }, { "id", transactionId.ToString() } };
            await _notificationService.SendNotificationAsync(transaction.HouseholdId, title, body, data);
        }

        transaction.UpdatedAt = DateTime.UtcNow;
        await _transactionRepository.UpdateAsync(transaction);
    }

    public async Task<string> GetTransactionQrCodeAsync(Guid transactionId, Guid userId)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tồn tại.");

        // Chỉ Collector hoặc Household của đơn này mới được lấy QR
        if (transaction.HouseholdId != userId && transaction.ScrapCollectorId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền xem mã QR này.");

        // Tính tổng tiền
        var totalAmount = transaction.TransactionDetails.Sum(d => d.FinalPrice);
        if (totalAmount <= 0)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Đơn hàng chưa có giá trị thanh toán.");

        // Lấy thông tin ngân hàng của Household (Người nhận tiền)
        // Lưu ý: Dùng hàm GetByUserIdWithRankAsync hoặc hàm nào đó có load thông tin Bank
        var householdProfile = await _profileRepository.GetByUserIdWithRankAsync(transaction.HouseholdId);

        if (householdProfile == null ||
            string.IsNullOrEmpty(householdProfile.BankCode) ||
            string.IsNullOrEmpty(householdProfile.BankAccountNumber))
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Người bán (Household) chưa cập nhật thông tin tài khoản ngân hàng.");

        // Tạo nội dung chuyển khoản: "Thanh toan don hang {Mã đơn}"
        // Lấy 8 ký tự đầu của ID cho ngắn gọn
        var content = $"GC {transaction.TransactionId.ToString().Substring(0, 8)}";

        // Gọi Service tạo link
        var qrUrl = _vietQrService.GenerateQrUrl(
            householdProfile.BankCode,
            householdProfile.BankAccountNumber,
            householdProfile.BankAccountName ?? "",
            totalAmount,
            content
        );

        return qrUrl;
    }
}