using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Transactions;
using GreenConnectPlatform.Data.Repositories.Transactions.TransactionDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Business.Services.Transactions.TransactionDetails;

public class TransactionDetailService : ITransactionDetailService
{
    private readonly ITransactionDetailRepository _transactionDetailRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly GreenConnectDbContext _context;
    private readonly IMapper _mapper;
    public TransactionDetailService(
        ITransactionDetailRepository transactionDetailRepository,
        ITransactionRepository transactionRepository, 
        GreenConnectDbContext context,
        IMapper mapper)
    {
        _transactionDetailRepository = transactionDetailRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }
    
    public async Task<List<TransactionDetailModel>> CreateTransactionDetails(Guid transactionId,Guid scrapCollectorId, List<TransactionDetailCreateModel> transactionDetailCreateModels)
    {
        var transaction = await _transactionRepository.DbSet()
            .Include(t => t.ScrapCollector) 
            .Include(t => t.TransactionDetails) 

            .Include(t => t.Offer) 
            .ThenInclude(o => o.OfferDetails) 

            .Include(t => t.Offer) 
            .ThenInclude(o => o.ScrapPost) 
            .ThenInclude(p => p.ScrapPostDetails) 
            .ThenInclude(spd => spd.ScrapCategory) 
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Transaction does not exist");
        if(transaction.ScrapCollectorId != scrapCollectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "You can not add details to this transaction");
        if(transaction.Status == TransactionStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Cannot add details to a completed transaction");
        if(transaction.CheckInTime == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Cannot add details to a transaction that has not been checked in");

        var categoryInScrapPost = transaction.Offer.ScrapPost.ScrapPostDetails
            .Select(c => c.ScrapCategoryId)
            .ToHashSet();

        var categoryInModels = transactionDetailCreateModels
            .Select(c => c.ScrapCategoryId)
            .ToHashSet();
        var distinctCategoryIds = categoryInModels.Distinct().ToList();
        if (categoryInModels.Count != distinctCategoryIds.Count)
        {
            throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409", "Duplicate scrap category in transaction details");
        }
        foreach (var detail in transactionDetailCreateModels)
        {
            var exists = categoryInScrapPost.Contains(detail.ScrapCategoryId);
            if (!exists)
            {
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", $"Scrap category with ID {detail.ScrapCategoryId} does not exist in the related scrap post");
            }
        }
        
        var categoryInOfferDetails = transaction.Offer.OfferDetails
            .Select(c => c.ScrapCategoryId)
            .ToHashSet();
        if (!categoryInOfferDetails.SetEquals(categoryInModels))
        {
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Transaction details do not match the accepted offer.");
        }
        var transactionDetails = _mapper.Map<List<TransactionDetail>>(transactionDetailCreateModels);
        
        
        foreach (var detail in transactionDetails)
        {
            detail.FinalPrice = detail.PricePerUnit * (decimal)detail.Quantity;
            detail.TransactionId = transactionId;
        }
        var result = await _transactionDetailRepository.AddRange(transactionDetails);

        return _mapper.Map<List<TransactionDetailModel>>(result);
        
    }

    public async Task<TransactionDetailModel> UpdateTransactionDetail(Guid scrapCollectorId, Guid transactionId, int scrapCategoryId,
        TransactionDetailUpdateModel transactionDetailUpdateModel)
    {
        var transactionDetail = await _transactionDetailRepository.DbSet()
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId && t.ScrapCategoryId == scrapCategoryId);
        if (transactionDetail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Transaction does not exist");
        if(transactionDetailUpdateModel.PricePerUnit == null) transactionDetailUpdateModel.PricePerUnit = transactionDetail.PricePerUnit;
        if(transactionDetailUpdateModel.Quantity == null) transactionDetailUpdateModel.Quantity = transactionDetail.Quantity;
        _mapper.Map(transactionDetailUpdateModel, transactionDetail);
        transactionDetail.FinalPrice = transactionDetail.PricePerUnit * (decimal)transactionDetail.Quantity;
        
        var result = await _transactionDetailRepository.Update(transactionDetail);
        return _mapper.Map<TransactionDetailModel>(result);
    }
}