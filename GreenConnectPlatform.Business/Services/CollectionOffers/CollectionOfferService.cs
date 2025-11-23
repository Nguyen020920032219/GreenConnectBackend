using AutoMapper;
using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.Transactions;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.CollectionOffers;

public class CollectionOfferService : ICollectionOfferService
{
    private readonly IScrapCategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ICollectionOfferRepository _offerRepository;
    private readonly IScrapPostRepository _postRepository;
    private readonly ITransactionRepository _transactionRepository;

    public CollectionOfferService(
        ICollectionOfferRepository offerRepository,
        IScrapPostRepository postRepository,
        IScrapCategoryRepository categoryRepository,
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        _offerRepository = offerRepository;
        _postRepository = postRepository;
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<CollectionOfferOveralForCollectorModel>> GetByCollectorAsync(
        int pageNumber, int pageSize, OfferStatus? status, bool sortByCreateAtDesc, Guid collectorId)
    {
        var (items, totalCount) =
            await _offerRepository.GetByCollectorAsync(collectorId, status, sortByCreateAtDesc, pageNumber, pageSize);
        var data = _mapper.Map<List<CollectionOfferOveralForCollectorModel>>(items);
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
        return new PaginatedResult<CollectionOfferOveralForHouseModel>
        {
            Data = data,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<CollectionOfferModel> GetByIdAsync(Guid id)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(id);
        if (offer == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Offer not found.");
        return _mapper.Map<CollectionOfferModel>(offer);
    }

    public async Task<CollectionOfferModel> CreateAsync(Guid collectorId, Guid postId,
        CollectionOfferCreateModel request)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Post not found.");

        if (post.HouseholdId == collectorId)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Cannot create offer for your own post.");

        var offerCategoryIds = request.OfferDetails.Select(d => d.ScrapCategoryId).Distinct().ToList();
        var postCategoryIds = post.ScrapPostDetails.Select(d => d.ScrapCategoryId).ToList();

        if (offerCategoryIds.Except(postCategoryIds).Any())
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Offer contains items not in the post.");

        if (post.MustTakeAll && offerCategoryIds.Count != postCategoryIds.Count)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "This post requires Full-lot purchase (must offer all items).");

        var offer = _mapper.Map<CollectionOffer>(request);
        offer.CollectionOfferId = Guid.NewGuid();
        offer.ScrapPostId = postId;
        offer.ScrapCollectorId = collectorId;
        offer.Status = OfferStatus.Pending;
        offer.CreatedAt = DateTime.UtcNow;

        if (request.ScheduleProposal != null)
        {
            var proposal = _mapper.Map<ScheduleProposal>(request.ScheduleProposal);
            proposal.ScheduleProposalId = Guid.NewGuid();
            proposal.ProposerId = collectorId;
            proposal.Status = ProposalStatus.Pending;
            proposal.CreatedAt = DateTime.UtcNow;
            offer.ScheduleProposals.Add(proposal);
        }

        await _offerRepository.AddAsync(offer);
        return await GetByIdAsync(offer.CollectionOfferId);
    }

    public async Task ProcessOfferAsync(Guid householdId, Guid offerId, bool isAccepted)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        if (offer == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Offer not found.");

        if (offer.ScrapPost.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (offer.Status != OfferStatus.Pending)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Can only accept/reject Pending offers.");

        if (isAccepted)
        {
            offer.Status = OfferStatus.Accepted;

            offer.ScrapPost.Status = PostStatus.FullyBooked;

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
                Quantity = 0, // Chưa cân
                FinalPrice = 0
            }).ToList();

            await _transactionRepository.AddAsync(transaction);
        }
        else
        {
            offer.Status = OfferStatus.Rejected;
        }

        await _offerRepository.UpdateAsync(offer);
    }

    public async Task ToggleCancelAsync(Guid collectorId, Guid offerId)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        if (offer == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Offer not found.");
        if (offer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (offer.Status == OfferStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Cannot cancel an accepted offer.");

        if (offer.Status == OfferStatus.Pending) offer.Status = OfferStatus.Canceled;
        else if (offer.Status == OfferStatus.Canceled) offer.Status = OfferStatus.Pending;

        await _offerRepository.UpdateAsync(offer);
    }

    public async Task AddDetailAsync(Guid collectorId, Guid offerId, OfferDetailCreateModel detailRequest)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        if (offer == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Offer not found.");
        if (offer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (offer.Status == OfferStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Cannot modify accepted offer.");

        if (offer.OfferDetails.Any(d => d.ScrapCategoryId == detailRequest.ScrapCategoryId))
            throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409", "Item exists.");

        var post = await _postRepository.GetByIdWithDetailsAsync(offer.ScrapPostId);
        if (!post!.ScrapPostDetails.Any(d => d.ScrapCategoryId == detailRequest.ScrapCategoryId))
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Item not in original post.");

        var detail = _mapper.Map<OfferDetail>(detailRequest);
        detail.CollectionOfferId = offerId;
        offer.OfferDetails.Add(detail);

        await _offerRepository.UpdateAsync(offer);
    }

    public async Task UpdateDetailAsync(Guid collectorId, Guid offerId, Guid detailId, OfferDetailUpdateModel request)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        var detail = offer!.OfferDetails.FirstOrDefault(d => d.OfferDetailId == detailId);
        if (detail == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Detail not found.");

        _mapper.Map(request, detail);
        await _offerRepository.UpdateAsync(offer);
    }

    public async Task DeleteDetailAsync(Guid collectorId, Guid offerId, Guid detailId)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        var detail = offer!.OfferDetails.FirstOrDefault(d => d.OfferDetailId == detailId);

        var post = await _postRepository.GetByIdWithDetailsAsync(offer.ScrapPostId);
        if (post!.MustTakeAll && offer.OfferDetails.Count <= post.ScrapPostDetails.Count)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Full-lot post requires all items.");

        offer.OfferDetails.Remove(detail!);
        await _offerRepository.UpdateAsync(offer);
    }
}