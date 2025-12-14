using AutoMapper;
using GreenConnectPlatform.Business.Models.CreditTransactionHistories;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;

namespace GreenConnectPlatform.Business.Services.CreditTransactionHistories;

public class CreditTransactionHistoryService : ICreditTransactionHistoryService
{
    private readonly ICreditTransactionHistoryRepository _creditTransactionHistoryRepository;
    private readonly IMapper _mapper;
    
    public CreditTransactionHistoryService(ICreditTransactionHistoryRepository creditTransactionHistoryRepository, IMapper mapper)
    {
        _creditTransactionHistoryRepository = creditTransactionHistoryRepository;
        _mapper = mapper;
    }
    
    public async Task<PaginatedResult<CreditTransactionHistoryModel>> GetCreditTransactionHistoriesByUserIdAsync(int pageIndex, int pageSize, Guid userId, bool sortByCreatedAt,
        string? type = null)
    {
        var (items, totalCount) = await _creditTransactionHistoryRepository
            .GetCreditTransactionHistoriesByUserId(pageIndex, pageSize, userId, sortByCreatedAt, type);
        var creditTransaction = _mapper.Map<List<CreditTransactionHistoryModel>>(items);
        return new PaginatedResult<CreditTransactionHistoryModel>
        {
            Data = creditTransaction,
            Pagination = new PaginationModel(totalCount, pageIndex, pageSize)
        };
    }
}