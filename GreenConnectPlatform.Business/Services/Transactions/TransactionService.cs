using AutoMapper;
using GeoCoordinatePortable;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Transaction = GreenConnectPlatform.Data.Entities.Transaction;
using TransactionStatus = GreenConnectPlatform.Data.Enums.TransactionStatus;

namespace GreenConnectPlatform.Business.Services.Transactions;

public class TransactionService : ITransactionService
{
    private const double CHECKIN_RADIUS_METERS = 50.0;
    private readonly GreenConnectDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository, GreenConnectDbContext context,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task CheckIn(LocationModel location, Guid transactionId, Guid userId)
    {
        var transaction = await _transactionRepository.DbSet()
            .Include(t => t.Offer)
            .ThenInclude(t => t.ScrapPost)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Transaction does not exist");
        if (transaction.ScrapCollectorId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "You are not authorized to check in for this transaction");
        if (transaction.Status != TransactionStatus.Scheduled)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Cannot checkin for this transaction when Household has not accepted the offer");

        var pointFromDb = transaction.Offer.ScrapPost.Location;
        var latA = pointFromDb.Y;
        var lonA = pointFromDb.X;

        var latB = location.Latitude.Value;
        var lonB = location.Longitude.Value;

        var geoCoordA = new GeoCoordinate(latA, lonA);
        var geoCoordB = new GeoCoordinate(latB, lonB);

        var distance = geoCoordA.GetDistanceTo(geoCoordB);

        if (distance <= CHECKIN_RADIUS_METERS)
        {
            transaction.CheckInTime = DateTime.UtcNow;
            transaction.Status = TransactionStatus.InProgress;
            await _transactionRepository.Update(transaction);
        }
        else
        {
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "You are too far away and cannot check in, you can only check in within 50 meters");
        }
    }

    public async Task<TransactionModel> GetTransaction(Guid transactionId)
    {
        var transaction = await _transactionRepository.DbSet()
            .Include(t => t.TransactionDetails)
            .ThenInclude(t => t.ScrapCategory)
            .Include(q => q.Household)
            .Include(q => q.ScrapCollector)
            .Include(q => q.Offer)
            .ThenInclude(o => o.OfferDetails)
            .Include(q => q.Offer)
            .ThenInclude(o => o.ScheduleProposals)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Transaction does not exist");
        return _mapper.Map<TransactionModel>(transaction);
    }

    public async Task<PaginatedResult<TransactionOveralModel>> GetTransactionsByUserId(Guid userId, string roleName,
        int pageNumber, int pageSize, bool sortByCreateAt = false, bool sortByUpdateAt = false)
    {
        var query = _transactionRepository.DbSet()
            .AsNoTracking()
            .AsQueryable();
        if (roleName == "Household")
            query = query.Where(t => t.HouseholdId == userId);
        else if (roleName == "IndividualCollector" || roleName == "BusinessCollector")
            query = query.Where(t => t.ScrapCollectorId == userId);
        var totalRecords = await query.CountAsync();
        IOrderedQueryable<Transaction> orderedQuery;
        if (sortByCreateAt)
            orderedQuery = query.OrderBy(t => t.CreatedAt);
        else
            orderedQuery = query.OrderByDescending(t => t.CreatedAt);

        orderedQuery = sortByUpdateAt
            ? orderedQuery.ThenBy(t => t.UpdatedAt)
            : orderedQuery.ThenByDescending(t => t.UpdatedAt);

        query = orderedQuery;

        var transactions = await query
            .Include(q => q.Household)
            .Include(q => q.ScrapCollector)
            .Include(q => q.Offer)
            .ThenInclude(o => o.OfferDetails)
            .Include(q => q.Offer)
            .ThenInclude(o => o.ScheduleProposals)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var transactionModel = _mapper.Map<List<TransactionOveralModel>>(transactions);
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        var paginationModel = new PaginationModel
        {
            TotalRecords = totalRecords,
            CurrentPage = pageNumber,
            TotalPages = totalPages,
            NextPage = pageNumber < totalPages ? pageNumber + 1 : null,
            PrevPage = pageNumber > 1 ? pageNumber - 1 : null
        };
        return new PaginatedResult<TransactionOveralModel>
        {
            Data = transactionModel,
            Pagination = paginationModel
        };
    }

    public async Task CancelOrAcceptTransaction(Guid transactionId, Guid userId, bool isAccept)
    {
        await using var dbTransaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var transaction = await _transactionRepository.DbSet()
                .Include(t => t.TransactionDetails)
                .Include(t => t.Offer)
                .ThenInclude(o => o.ScrapPost)
                .ThenInclude(s => s.ScrapPostDetails)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
            if (transaction == null)
                throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Transaction does not exist");
            if (transaction.HouseholdId != userId)
                throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                    "You are not authorized to respond to this transaction");
            if (transaction.Status != TransactionStatus.InProgress)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "you can cancel or accept only transactions that are in progress");
            if (isAccept)
            {
                transaction.Status = TransactionStatus.Completed;
                var scrapPost = transaction.Offer.ScrapPost;
                if (scrapPost == null)
                    throw new ApiExceptionModel(500, "500", "Data integrity error: Transaction is missing ScrapPost.");
                var completedCategoryIds = transaction.TransactionDetails
                    .Select(td => td.ScrapCategoryId)
                    .ToHashSet();
                foreach (var scrapPostDetail in scrapPost.ScrapPostDetails)
                    if (completedCategoryIds.Contains(scrapPostDetail.ScrapCategoryId))
                        scrapPostDetail.Status = PostDetailStatus.Collected;

                var allCollected = scrapPost.ScrapPostDetails
                    .All(spd => spd.Status == PostDetailStatus.Collected);
                if (allCollected)
                    scrapPost.Status = PostStatus.Completed;
            }
            else
            {
                transaction.Status = TransactionStatus.CanceledByUser;
            }

            await _context.SaveChangesAsync();
            await dbTransaction.CommitAsync();
        }
        catch (ApiExceptionModel)
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
        catch (Exception)
        {
            await dbTransaction.RollbackAsync();
            throw new ApiExceptionModel(StatusCodes.Status500InternalServerError, "500",
                "An error occurred while processing the transaction");
        }
    }

    public async Task CancelOrReopenTransaction(Guid transactionId, Guid userId)
    {
        var transaction = await _transactionRepository.DbSet()
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Transaction does not exist");
        if (transaction.ScrapCollectorId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "You are not authorized to respond to this transaction");
        if (transaction.Status == TransactionStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "You can not cancel or reopen a completed transaction");
        if (transaction.Status == TransactionStatus.InProgress || transaction.Status == TransactionStatus.Scheduled)
            transaction.Status = TransactionStatus.CanceledByUser;
        else if (transaction.Status == TransactionStatus.CanceledByUser ||
                 transaction.Status == TransactionStatus.CanceledBySystem)
            transaction.Status = TransactionStatus.InProgress;
        await _transactionRepository.Update(transaction);
    }
}