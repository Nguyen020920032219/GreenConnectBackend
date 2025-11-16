using AutoMapper;
using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.ScrapPosts.ScrapPostDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Business.Services.CollectionOffers;

public class CollectionOfferService : ICollectionOfferService
{
    private readonly ICollectionOfferRepository _collectionOfferRepository;
    private readonly IScrapPostRepository _scrapPostRepository;
    private readonly IScrapCategoryRepository _scrapCategoryRepository;
    private readonly IScrapPostDetailRepository _scrapPostDetailRepository;
    private readonly GreenConnectDbContext _context;
    private readonly IMapper _mapper;

    public CollectionOfferService(
        ICollectionOfferRepository collectionOfferRepository,
        IScrapPostRepository scrapPostRepository,
        IScrapCategoryRepository scrapCategoryRepository, 
        IScrapPostDetailRepository scrapPostDetailRepository,
        GreenConnectDbContext context,
        IMapper mapper
        )
    {
        _collectionOfferRepository = collectionOfferRepository;
        _scrapPostRepository = scrapPostRepository;
        _scrapCategoryRepository = scrapCategoryRepository;
        _scrapPostDetailRepository = scrapPostDetailRepository;
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedResult<CollectionOfferOveralForCollectorModel>> GetCollectionOffersForCollector(
        int pageNumber,
        int pageSize,
        OfferStatus? offerStatus,
        bool? sortByCreateAt,
        Guid collectorId)
    {
        var query = _collectionOfferRepository.DbSet()
            .Where(o => o.ScrapCollectorId == collectorId)
            .AsQueryable()
            .AsNoTracking();
        if(offerStatus != null)
            query = query.Where(o => o.Status == offerStatus);
        if(sortByCreateAt == false)
            query = query.OrderByDescending(o => o.CreatedAt);
        else 
            query = query.OrderBy(o => o.CreatedAt);
        var totalRecords = await query.CountAsync();
        var collectionOffers = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var offerModel = _mapper.Map<List<CollectionOfferOveralForCollectorModel>>(collectionOffers);
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        var paginationModel = new PaginationModel
        {
            TotalRecords = totalRecords,
            CurrentPage = pageNumber,
            TotalPages = totalPages,
            NextPage = pageNumber < totalPages ? pageNumber + 1 : null,
            PrevPage = pageNumber > 1 ? pageNumber - 1 : null,
        };
        return new PaginatedResult<CollectionOfferOveralForCollectorModel>
        {
            Data = offerModel,
            Pagination = paginationModel
        };
    }

    public async Task<PaginatedResult<CollectionOfferOveralForHouseModel>> GetCollectionOffersForHousehold(int pageNumber, int pageSize, OfferStatus? offerStatus, Guid scrapPostId)
    {
        var query = _collectionOfferRepository.DbSet()
            .Include(o => o.ScrapCollector)
            .Where(o => o.ScrapPostId == scrapPostId)
            .AsQueryable()
            .AsNoTracking();
        if(offerStatus != null)
            query = query.Where(o => o.Status == offerStatus);

        var totalRecords = await query.CountAsync();
        
        var collectionOffers = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var offerModel = _mapper.Map<List<CollectionOfferOveralForHouseModel>>(collectionOffers);
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        var paginationModel = new PaginationModel
        {
            TotalRecords = totalRecords,
            CurrentPage = pageNumber,
            TotalPages = totalPages,
            NextPage = pageNumber < totalPages ? pageNumber + 1 : null,
            PrevPage = pageNumber > 1 ? pageNumber - 1 : null,
        };
        return new PaginatedResult<CollectionOfferOveralForHouseModel>
        {
            Data = offerModel,
            Pagination = paginationModel
        };
    }
    

    public async Task<CollectionOfferModel> GetCollectionOffer(Guid scrapPostId, Guid collectionOfferId)
    {
        var collectionOffer = await _collectionOfferRepository.DbSet()
            .Include(o => o.OfferDetails)
            .Include(o => o.ScheduleProposals)
            .FirstOrDefaultAsync(o => o.CollectionOfferId == collectionOfferId && o.ScrapPostId == scrapPostId);
        if(collectionOffer == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Collection offer not found");
        return _mapper.Map<CollectionOfferModel>(collectionOffer);
    }

    public async Task<CollectionOfferModel> CreateCollectionOffer(Guid scrapPostId,Guid scrapCollectorId, CollectionOfferCreateModel model)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var collectionOffer = _mapper.Map<CollectionOffer>(model);
            var scrapPost = await _scrapPostRepository.DbSet()
                .Include(s => s.ScrapPostDetails)
                .FirstOrDefaultAsync(p => p.ScrapPostId == scrapPostId);
            if (scrapPost == null)
                throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Scrap post id does not exist");
            var category = model.OfferDetails
                .Select(c => c.ScrapCategoryId).ToList();

            var distinctCategoryIds = category.Distinct().ToList();

            if (category.Any())
            {
                if (distinctCategoryIds.Count != category.Count)
                    throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409",
                        "Scrap category IDs in the details list cannot be duplicated.");

                var existingCategoryCount = await _scrapCategoryRepository.DbSet()
                    .CountAsync(c => distinctCategoryIds.Contains(c.ScrapCategoryId));

                if (existingCategoryCount != distinctCategoryIds.Count)
                    throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                        "One or more scrap categories do not exist");
                var scrapPostDetails = scrapPost.ScrapPostDetails
                    .Where(d => d.Status == PostDetailStatus.Available)
                    .Select(d => d.ScrapCategoryId)
                    .ToHashSet();

                var allCategoryIdInPost = scrapPost.ScrapPostDetails
                    .Select(c => c.ScrapCategoryId)
                    .ToHashSet();
                
                foreach (var offerCategoryId in distinctCategoryIds)
                {
                    if(!allCategoryIdInPost.Contains(offerCategoryId))
                        throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                            $"Scrap category ID {offerCategoryId} is not part of the scrap post.");
                    if (!scrapPostDetails.Contains(offerCategoryId))
                        throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                            $"Scrap category ID {offerCategoryId} is not available in the scrap post.");
                }
            }

            bool isFullOffer = scrapPost.ScrapPostDetails.Count == distinctCategoryIds.Count;
            if (scrapPost.MustTakeAll && !isFullOffer)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "This scrap post requires all scrap categories to be included in the collection offer.");

            collectionOffer.CollectionOfferId = Guid.NewGuid();
            collectionOffer.CreatedAt = DateTime.UtcNow;
            collectionOffer.ScrapPostId = scrapPostId;
            collectionOffer.Status = OfferStatus.Pending;
            collectionOffer.ScrapCollectorId = scrapCollectorId;

            var scheduleProposal = collectionOffer.ScheduleProposals.FirstOrDefault();
            if (scheduleProposal != null)
            {
                scheduleProposal.ScheduleProposalId = Guid.NewGuid();
                scheduleProposal.CollectionOfferId = collectionOffer.CollectionOfferId;
                scheduleProposal.ProposerId = scrapCollectorId;
                scheduleProposal.CreatedAt = DateTime.UtcNow;
                scheduleProposal.Status = ProposalStatus.Pending;
            }
            else
            {
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Schedule proposal is required.");
            }
            
            _context.CollectionOffers.Add(collectionOffer);
            
            var scrapPostDetailToUpdate = scrapPost.ScrapPostDetails
                .Where(d => distinctCategoryIds.Contains(d.ScrapCategoryId))
                .ToList();
            if (scrapPostDetailToUpdate.Any())
            {
                foreach (var scrapPostDetail in scrapPostDetailToUpdate)
                {
                    scrapPostDetail.Status = PostDetailStatus.Booked;
                }
            }

            scrapPost.Status = isFullOffer ? PostStatus.FullyBooked : PostStatus.PartiallyBooked;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return _mapper.Map<CollectionOfferModel>(collectionOffer);
        }
        catch (ApiExceptionModel)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception )
        {
            await transaction.RollbackAsync();
            throw new ApiExceptionModel(StatusCodes.Status500InternalServerError, "500", "An error occurred while creating the collection offer");
        }
    }

    public async Task RejectOrAcceptCollectionOffer(Guid collectionOfferId,Guid scrapPostId, Guid householdId, bool isAccepted)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var collectionOffer = await _collectionOfferRepository.DbSet()
                .Include(o => o.ScheduleProposals)
                .Include(o => o.ScrapPost)
                .ThenInclude(o => o.ScrapPostDetails)
                .Include(o => o.OfferDetails)
                .FirstOrDefaultAsync(o => o.CollectionOfferId == collectionOfferId && o.ScrapPostId == scrapPostId);
            if(collectionOffer == null) 
                throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Collection offer not found");
            if(collectionOffer.ScrapPost.HouseholdId != householdId)
                throw new ApiExceptionModel(StatusCodes.Status401Unauthorized, "401", "You are not authorized to reject this collection offer");
            if(collectionOffer.Status != OfferStatus.Pending)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Only pending collection offers can be accepted or rejected");
            if (isAccepted)
            {
                collectionOffer.Status = OfferStatus.Accepted;
                var proposalToAccept = collectionOffer.ScheduleProposals
                    .FirstOrDefault(p => p.Status == ProposalStatus.Pending);
                if (proposalToAccept == null)
                    throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Cannot accept offer: Missing a pending schedule proposal.");
                proposalToAccept.Status = ProposalStatus.Accepted;
                var newTransaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    HouseholdId = collectionOffer.ScrapPost.HouseholdId,
                    ScrapCollectorId = collectionOffer.ScrapCollectorId,
                    OfferId = collectionOffer.CollectionOfferId,
                    Status = TransactionStatus.Scheduled,
                    ScheduledTime = proposalToAccept.ProposedTime,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Transactions.Add(newTransaction);
            }
            else if (!isAccepted)
            {
                collectionOffer.Status = OfferStatus.Rejected;
                
                var categoryIdsInOffer = collectionOffer.OfferDetails
                    .Select(od => od.ScrapCategoryId)
                    .ToList();
                
                var scrapPostDetailsToUpdate = collectionOffer.ScrapPost.ScrapPostDetails
                    .Where(d => categoryIdsInOffer.Contains(d.ScrapCategoryId))
                    .ToList();
                
                foreach (var scrapPostDetail in scrapPostDetailsToUpdate)
                {
                    scrapPostDetail.Status = PostDetailStatus.Available;

                }
                
                bool detailsBooked = collectionOffer.ScrapPost.ScrapPostDetails
                    .Any(d => d.Status == PostDetailStatus.Booked);
                collectionOffer.ScrapPost.Status = detailsBooked ? PostStatus.PartiallyBooked : PostStatus.Open;
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }catch (ApiExceptionModel)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new ApiExceptionModel(StatusCodes.Status500InternalServerError, "500", "An error occurred while reject or accept the collection offer");
        }
    }

    public async Task CancelOrReopenCollectionOffer(Guid collectionOfferId,Guid collectorId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var collectionOffer = await _collectionOfferRepository.DbSet()
                .Include(o => o.OfferDetails)
                .Include(o => o.ScrapPost)
                .ThenInclude(o => o.ScrapPostDetails)
                .ThenInclude(o => o.ScrapCategory)
                .FirstOrDefaultAsync(o => o.CollectionOfferId == collectionOfferId);
            if(collectionOffer == null)
                throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Collection offer not found");
            if(collectionOffer.ScrapCollectorId != collectorId)
                throw new ApiExceptionModel(StatusCodes.Status401Unauthorized, "401", "You are not authorized to cancel or reopen this collection offer");
            if(collectionOffer.Status == OfferStatus.Accepted)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Collection offer is in accepted status so it cannot be canceled or reopened");
            if (collectionOffer.Status == OfferStatus.Rejected || collectionOffer.Status == OfferStatus.Canceled)
            {
                var categoryIdsToReopen = collectionOffer.OfferDetails
                    .Select(od => od.ScrapCategoryId)
                    .ToHashSet();
                
                var detailsToCheck = collectionOffer.ScrapPost.ScrapPostDetails
                    .Where(d => categoryIdsToReopen.Contains(d.ScrapCategoryId))
                    .ToList();
                foreach (var detail in detailsToCheck)
                {
                    if (detail.Status == PostDetailStatus.Booked || detail.Status == PostDetailStatus.Collected)
                    {
                        throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                            $"Cannot reopen offer. Scrap category {detail.ScrapCategory.CategoryName} is already {detail.Status}.");
                    }
                }
                foreach (var detail in detailsToCheck)
                {
                    detail.Status = PostDetailStatus.Booked;
                }
                
                bool isFullOffer = collectionOffer.ScrapPost.ScrapPostDetails.Count == detailsToCheck.Count;
                collectionOffer.ScrapPost.Status = isFullOffer ? PostStatus.FullyBooked : PostStatus.PartiallyBooked;
                
                collectionOffer.Status = OfferStatus.Pending;
            }
            else if (collectionOffer.Status == OfferStatus.Pending)
            {
                collectionOffer.Status = OfferStatus.Canceled;
                var scrapPostDetailsToUpdate = await _scrapPostDetailRepository.DbSet()
                    .Where(d => d.ScrapPostId == collectionOffer.ScrapPostId && collectionOffer.OfferDetails
                        .Select(od => od.ScrapCategoryId)
                        .Contains(d.ScrapCategoryId))
                    .ToListAsync();
                foreach (var scrapPostDetail in scrapPostDetailsToUpdate)
                {
                    scrapPostDetail.Status = PostDetailStatus.Available;

                }
                var scrapPost = await _scrapPostRepository.DbSet()
                    .Include(p => p.ScrapPostDetails)
                    .FirstOrDefaultAsync(p => p.ScrapPostId == collectionOffer.ScrapPostId);
                    
                bool detailsBooked = scrapPost.ScrapPostDetails
                    .Any(d => d.Status == PostDetailStatus.Booked);
                scrapPost.Status = detailsBooked ? PostStatus.PartiallyBooked : PostStatus.Open;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }catch (ApiExceptionModel)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception )
        {
            await transaction.RollbackAsync();
            throw new ApiExceptionModel(StatusCodes.Status500InternalServerError, "500", "An error occurred while reopen or cancel the collection offer");
        }
    }
}