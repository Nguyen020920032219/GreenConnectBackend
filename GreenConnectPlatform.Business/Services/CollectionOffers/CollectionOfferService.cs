using AutoMapper;
using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Business.Services.Notifications;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Chatrooms;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.Transactions;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.CollectionOffers;

public class CollectionOfferService : ICollectionOfferService
{
    private readonly ICreditTransactionHistoryRepository _creditTransactionHistoryRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly ICollectionOfferRepository _offerRepository;
    private readonly IScrapPostRepository _postRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IChatRoomRepository _roomRepository;
    private readonly IScrapCategoryRepository _scrapCategoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    public CollectionOfferService(
        ICollectionOfferRepository offerRepository,
        IScrapPostRepository postRepository,
        ITransactionRepository transactionRepository,
        IChatRoomRepository roomRepository,
        IFileStorageService fileStorageService,
        IScrapCategoryRepository scrapCategoryRepository,
        IMapper mapper,
        INotificationService notificationService,
        IProfileRepository profileRepository,
        ICreditTransactionHistoryRepository creditTransactionHistoryRepository)
    {
        _offerRepository = offerRepository;
        _postRepository = postRepository;
        _transactionRepository = transactionRepository;
        _roomRepository = roomRepository;
        _fileStorageService = fileStorageService;
        _scrapCategoryRepository = scrapCategoryRepository;
        _mapper = mapper;
        _notificationService = notificationService;
        _profileRepository = profileRepository;
        _creditTransactionHistoryRepository = creditTransactionHistoryRepository;
    }

    public async Task<PaginatedResult<CollectionOfferOveralForCollectorModel>> GetByCollectorAsync(
        int pageNumber, int pageSize, OfferStatus? status, bool sortByCreateAtDesc, Guid collectorId)
    {
        var (items, totalCount) =
            await _offerRepository.GetByCollectorAsync(collectorId, status, sortByCreateAtDesc, pageNumber, pageSize);
        var data = _mapper.Map<List<CollectionOfferOveralForCollectorModel>>(items);
        foreach (var d in data)
        foreach (var detail in d.ScrapPost.ScrapPostDetails)
            if (!string.IsNullOrEmpty(detail.ImageUrl))
                detail.ImageUrl = await _fileStorageService.GetReadSignedUrlAsync(detail.ImageUrl);

        return new PaginatedResult<CollectionOfferOveralForCollectorModel>
        {
            Data = data,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<PaginatedResult<CollectionOfferOveralForHouseModel>> GetByPostAsync(
        int pageNumber, int pageSize, OfferStatus? status, Guid postId)
    {
        var (items, totalCount) = await _offerRepository.GetByPostIdAsync(postId, status, pageNumber, pageSize);
        var data = _mapper.Map<List<CollectionOfferOveralForHouseModel>>(items);
        foreach (var d in data)
        foreach (var detail in d.ScrapPost.ScrapPostDetails)
            if (!string.IsNullOrEmpty(detail.ImageUrl))
                detail.ImageUrl = await _fileStorageService.GetReadSignedUrlAsync(detail.ImageUrl);

        return new PaginatedResult<CollectionOfferOveralForHouseModel>
        {
            Data = data,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<CollectionOfferModel> GetByIdAsync(Guid id)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(id);
        if (offer == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Đề nghị thu gom không tìm thấy.");
        var offerModel = _mapper.Map<CollectionOfferModel>(offer);
        foreach (var detail in offerModel.ScrapPost.ScrapPostDetails)
            if (!string.IsNullOrEmpty(detail.ImageUrl))
                detail.ImageUrl = await _fileStorageService.GetReadSignedUrlAsync(detail.ImageUrl);

        return offerModel;
    }

    public async Task<CollectionOfferModel> CreateAsync(Guid collectorId, Guid postId,
        CollectionOfferCreateModel request)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tìm thấy.");

        if (post.HouseholdId == collectorId)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không thể tự tạo đề nghị thu gom cho bài đăng của chính mình.");

        var offerCategoryIds = request.OfferDetails.Select(d => d.ScrapCategoryId).Distinct().ToList();
        var postCategoryIds = post.ScrapPostDetails.Select(d => d.ScrapCategoryId).ToList();

        if (offerCategoryIds.Except(postCategoryIds).Any())
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Đề nghị không thể bao gồm các mục không có trong bài đăng.");

        if (post.MustTakeAll && offerCategoryIds.Count != postCategoryIds.Count)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bài đăng yêu cầu thu gom toàn bộ lô hàng.");

        var unavailableItems = post.ScrapPostDetails
            .Where(detail => offerCategoryIds.Contains(detail.ScrapCategoryId)
                             && detail.Status != PostDetailStatus.Available)
            .ToList();
        if (unavailableItems.Any())
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Có mục trong đề nghị không còn sẵn sàng để thu gom.");

        var profile = await _profileRepository.GetByUserIdWithRankAsync(collectorId);
        if (profile == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Hồ sơ người thu gom không tìm thấy.");
        if (profile.CreditBalance < 10)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn cần có ít nhất 10 điểm tín dụng để tạo đề nghị thu gom.");

        var offer = _mapper.Map<CollectionOffer>(request);
        offer.CollectionOfferId = Guid.NewGuid();
        offer.ScrapPostId = postId;
        offer.ScrapCollectorId = collectorId;
        offer.Status = OfferStatus.Pending;
        offer.CreatedAt = DateTime.UtcNow;

        foreach (var detail in offer.OfferDetails)
        {
            detail.OfferDetailId = Guid.NewGuid();
            detail.CollectionOfferId = offer.CollectionOfferId;
            var matchingPostDetail = post.ScrapPostDetails
                .FirstOrDefault(d => d.ScrapCategoryId == detail.ScrapCategoryId);
            if(matchingPostDetail != null)
                detail.Type = matchingPostDetail.Type;
        }
        
        if (request.ScheduleProposal != null)
        {
            var proposal = _mapper.Map<ScheduleProposal>(request.ScheduleProposal);
            proposal.CollectionOfferId = offer.CollectionOfferId;
            proposal.ScheduleProposalId = Guid.NewGuid();
            proposal.ProposerId = collectorId;
            proposal.Status = ProposalStatus.Pending;
            proposal.CreatedAt = DateTime.UtcNow;
            offer.ScheduleProposals.Add(proposal);
        }
        else
        {
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Lịch hẹn đề xuất bắt buộc phải có.");
        }

        var scrapPostDetailToUpdate = post.ScrapPostDetails
            .Where(d => offerCategoryIds.Contains(d.ScrapCategoryId))
            .ToList();

        if (scrapPostDetailToUpdate.Any())
        {
            foreach (var detail in scrapPostDetailToUpdate) detail.Status = PostDetailStatus.Booked;
            var isFullOffer = offerCategoryIds.Count == postCategoryIds.Count;
            post.Status = isFullOffer ? PostStatus.FullyBooked : PostStatus.PartiallyBooked;
            await _postRepository.UpdateAsync(post);
        }

        await _offerRepository.AddAsync(offer);
        profile.CreditBalance -= 10;
        await _profileRepository.UpdateAsync(profile);
        var creditHistory = new CreditTransactionHistory
        {
            Id = Guid.NewGuid(),
            UserId = collectorId,
            Amount = -10,
            BalanceAfter = profile.CreditBalance,
            Type = "Usage",
            ReferenceId = offer.CollectionOfferId,
            CreatedAt = DateTime.UtcNow,
            Description = $"Tạo đề nghị thu gom cho bài đăng '{post.Title}'."
        };
        await _creditTransactionHistoryRepository.AddAsync(creditHistory);

        var notiTitle = "Đề nghị thu gom mới!";
        var notiBody = $"Có người vừa báo giá cho bài đăng '{post.Title}' của bạn.";
        var notiData = new Dictionary<string, string>
        {
            { "type", "Offer" },
            { "id", offer.CollectionOfferId.ToString() },
            { "postId", post.Id.ToString() }
        };
        await _notificationService.SendNotificationAsync(post.HouseholdId, notiTitle, notiBody, notiData);

        return await GetByIdAsync(offer.CollectionOfferId);
    }

    public async Task ProcessOfferAsync(Guid householdId, Guid offerId, bool isAccepted)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        if (offer == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Đề nghị thu gom không tìm thấy.");

        if (offer.ScrapPost.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (offer.Status != OfferStatus.Pending)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Chỉ có thể xử lý đề nghị ở trạng thái Pending.");

        if (isAccepted)
        {
            offer.Status = OfferStatus.Accepted;
            var pendingProposal = offer.ScheduleProposals.FirstOrDefault(p => p.Status == ProposalStatus.Pending);
            if (pendingProposal != null) pendingProposal.Status = ProposalStatus.Accepted;

            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                HouseholdId = householdId,
                ScrapCollectorId = offer.ScrapCollectorId,
                OfferId = offerId,
                Status = TransactionStatus.Scheduled,
                ScheduledTime = pendingProposal?.ProposedTime,
                CreatedAt = DateTime.UtcNow
            };

            transaction.TransactionDetails = offer.OfferDetails.Select(od => new TransactionDetail
            {
                TransactionId = transaction.TransactionId,
                ScrapCategoryId = od.ScrapCategoryId,
                PricePerUnit = od.PricePerUnit,
                Unit = od.Unit ?? "kg",
                Quantity = 0,
                FinalPrice = 0
            }).ToList();

            await _transactionRepository.AddAsync(transaction);

            var chatRoom = new ChatRoom
            {
                ChatRoomId = Guid.NewGuid(),
                TransactionId = transaction.TransactionId,
                CreatedAt = DateTime.UtcNow
            };
            chatRoom.ChatParticipants.Add(new ChatParticipant
            {
                UserId = householdId,
                ChatRoomId = chatRoom.ChatRoomId,
                JoinedAt = DateTime.UtcNow
            });
            chatRoom.ChatParticipants.Add(new ChatParticipant
            {
                UserId = offer.ScrapCollectorId,
                ChatRoomId = chatRoom.ChatRoomId,
                JoinedAt = DateTime.UtcNow
            });
            await _roomRepository.AddAsync(chatRoom);
            var detailsBooked = offer.ScrapPost.ScrapPostDetails
                .Any(d => d.Status == PostDetailStatus.Available);
            offer.ScrapPost.Status = detailsBooked ? PostStatus.PartiallyBooked : PostStatus.FullyBooked;

            var notiTitle = "Đề nghị được chấp nhận!";
            var notiBody = "Chúc mừng! Hộ gia đình đã đồng ý bán cho bạn. Giao dịch đã được tạo.";
            var notiData = new Dictionary<string, string>
            {
                { "type", "Transaction" },
                { "id", transaction.TransactionId.ToString() }
            };
            await _notificationService.SendNotificationAsync(offer.ScrapCollectorId, notiTitle, notiBody, notiData);
        }
        else
        {
            offer.Status = OfferStatus.Rejected;
            var schedule = offer.ScheduleProposals.Where(s => s.Status == ProposalStatus.Pending).ToList();
            foreach (var s in schedule)
                s.Status = ProposalStatus.Rejected;
            var offerCategoryIds = offer.OfferDetails.Select(d => d.ScrapCategoryId).ToList();
            var post = offer.ScrapPost.ScrapPostDetails
                .Where(d => offerCategoryIds.Contains(d.ScrapCategoryId)).ToList();

            foreach (var detail in post)
                detail.Status = PostDetailStatus.Available;

            var detailsBooked = offer.ScrapPost.ScrapPostDetails
                .Any(d => d.Status == PostDetailStatus.Booked || d.Status == PostDetailStatus.Collected);
            offer.ScrapPost.Status = detailsBooked ? PostStatus.PartiallyBooked : PostStatus.Open;
            await _postRepository.UpdateAsync(offer.ScrapPost);

            var notiTitle = "Đề nghị bị từ chối";
            var notiBody = $"Hộ gia đình đã từ chối đề nghị của bạn cho bài đăng '{offer.ScrapPost.Title}'.";
            var notiData = new Dictionary<string, string>
            {
                { "type", "Offer" },
                { "id", offer.CollectionOfferId.ToString() }
            };
            await _notificationService.SendNotificationAsync(offer.ScrapCollectorId, notiTitle, notiBody, notiData);
        }

        await _offerRepository.UpdateAsync(offer);
    }

    public async Task ToggleCancelAsync(Guid collectorId, Guid offerId)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        if (offer == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Đề nghị thu gom không tìm thấy.");
        if (offer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (offer.Status == OfferStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể hủy hoặc mở lại đề nghị đã được chấp nhận.");

        var offerCategoryIds = offer.OfferDetails.Select(d => d.ScrapCategoryId).ToList();
        var post = offer.ScrapPost.ScrapPostDetails
            .Where(d => offerCategoryIds.Contains(d.ScrapCategoryId)).ToList();
        var detailsBooked = offer.ScrapPost.ScrapPostDetails
            .Any(d => d.Status == PostDetailStatus.Booked);

        if (offer.Status == OfferStatus.Pending)
        {
            offer.Status = OfferStatus.Canceled;

            foreach (var detail in post)
                detail.Status = PostDetailStatus.Available;

            offer.ScrapPost.Status = detailsBooked ? PostStatus.PartiallyBooked : PostStatus.Open;
            await _postRepository.UpdateAsync(offer.ScrapPost);

            var notiTitle = "Đề nghị bị hủy";
            var notiBody = $"Người thu gom đã hủy báo giá cho bài đăng '{offer.ScrapPost.Title}'.";
            await _notificationService.SendNotificationAsync(offer.ScrapPost.HouseholdId, notiTitle, notiBody);
        }
        else if (offer.Status == OfferStatus.Canceled || offer.Status == OfferStatus.Rejected)
        {
            offer.Status = OfferStatus.Pending;
            var profile = await _profileRepository.GetByUserIdWithRankAsync(collectorId);
            if (profile == null)
                throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                    "Hồ sơ người thu gom không tìm thấy.");
            if (profile.CreditBalance < 10)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Bạn cần có ít nhất 10 điểm tín dụng để mở lại đề nghị thu gom.");
            profile.CreditBalance -= 10;
            await _profileRepository.UpdateAsync(profile);
            var creditHistory = new CreditTransactionHistory
            {
                Id = Guid.NewGuid(),
                UserId = collectorId,
                Amount = -10,
                BalanceAfter = profile.CreditBalance,
                Type = "Usage",
                ReferenceId = offer.CollectionOfferId,
                CreatedAt = DateTime.UtcNow,
                Description = $"Mở lại đề nghị thu gom cho bài đăng '{offer.ScrapPost.Title}'."
            };
            await _creditTransactionHistoryRepository.AddAsync(creditHistory);
            foreach (var detail in post)
            {
                if (detail.Status == PostDetailStatus.Booked || detail.Status == PostDetailStatus.Collected)
                    throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                        $"Không thể mở lại đề nghị thu gom cho {detail.ScrapCategory.Name} vì đang có trạng thái là {detail.Status}.");
                detail.Status = PostDetailStatus.Booked;
            }

            var isFullBooked = offerCategoryIds.Count == offer.ScrapPost.ScrapPostDetails.Count;
            offer.ScrapPost.Status = isFullBooked ? PostStatus.FullyBooked : PostStatus.PartiallyBooked;
            await _postRepository.UpdateAsync(offer.ScrapPost);
            var notiTitle = "Đề nghị đã được mở lại";
            var notiBody = $"Người thu gom đã mở lại báo giá cho bài đăng '{offer.ScrapPost.Title}'.";
            await _notificationService.SendNotificationAsync(offer.ScrapPost.HouseholdId, notiTitle, notiBody);
        }

        await _offerRepository.UpdateAsync(offer);
    }

    public async Task AddDetailAsync(Guid collectorId, Guid offerId, OfferDetailCreateModel detailRequest)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        if (offer == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Đề nghị thu gom không tìm thấy.");
        if (offer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (offer.Status == OfferStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể thêm mới khi đề nghị đã được chấp nhận.");

        var scrapCategory = await _scrapCategoryRepository.GetByIdAsync(detailRequest.ScrapCategoryId);
        if (scrapCategory == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Loại ve chai không tồn tại.");

        if (offer.OfferDetails.Any(d => d.ScrapCategoryId == detailRequest.ScrapCategoryId))
            throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409", "Đề nghị cho loại này đã có.");

        var post = await _postRepository.GetByIdWithDetailsAsync(offer.ScrapPostId);
        if (!post!.ScrapPostDetails.Any(d => d.ScrapCategoryId == detailRequest.ScrapCategoryId))
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Loại này không thuộc bài đăng này.");

        var detail = _mapper.Map<OfferDetail>(detailRequest);
        detail.CollectionOfferId = offerId;
        offer.OfferDetails.Add(detail);
        await _offerRepository.UpdateAsync(offer);

        var postDetail = post.ScrapPostDetails
            .First(d => d.ScrapCategoryId == detailRequest.ScrapCategoryId);
        postDetail.Status = PostDetailStatus.Booked;

        var isFullOffer = offer.OfferDetails.Select(d => d.ScrapCategoryId).Distinct().Count()
                          == post.ScrapPostDetails.Count;
        post.Status = isFullOffer ? PostStatus.FullyBooked : PostStatus.PartiallyBooked;
        await _postRepository.UpdateAsync(post);
    }

    public async Task UpdateDetailAsync(Guid collectorId, Guid offerId, Guid detailId, OfferDetailUpdateModel request)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        var detail = offer!.OfferDetails.FirstOrDefault(d => d.OfferDetailId == detailId);
        if (detail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Đề nghị thu gom không tìm thấy.");

        _mapper.Map(request, detail);
        await _offerRepository.UpdateAsync(offer);
    }

    public async Task DeleteDetailAsync(Guid collectorId, Guid offerId, Guid detailId)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        if (offer == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Đề nghị thu gom không tìm thấy.");
        if (offer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");
        var detail = offer.OfferDetails.FirstOrDefault(d => d.OfferDetailId == detailId);
        if (detail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Đề nghị thu gom chi tiết không tìm thấy.");
        if (offer.Status == OfferStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể xóa khi đề nghị đã được chấp nhận.");

        var post = await _postRepository.GetByIdWithDetailsAsync(offer.ScrapPostId);
        if (post!.MustTakeAll && offer.OfferDetails.Count <= post.ScrapPostDetails.Count)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể xóa chi tiết đề nghị vì bài đăng yêu cầu thu gom toàn bộ lô hàng.");

        offer.OfferDetails.Remove(detail);
        await _offerRepository.UpdateAsync(offer);

        var postDetail = post.ScrapPostDetails
            .First(d => d.ScrapCategoryId == detail.ScrapCategoryId);
        postDetail.Status = PostDetailStatus.Available;
        post.Status = PostStatus.PartiallyBooked;
        await _postRepository.UpdateAsync(post);
    }
}

