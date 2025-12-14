using AutoMapper;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PaymentTransactions;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.PaymentTransactions;

namespace GreenConnectPlatform.Business.Services.PaymentTransactions;

public class PaymentTransactionService : IPaymentTransactionService
{
    private readonly IPaymentTransactionRepository _paymentTransactionRepository;
    private readonly IMapper _mapper;

    public PaymentTransactionService(IPaymentTransactionRepository paymentTransactionRepository, IMapper mapper)
    {
        _paymentTransactionRepository = paymentTransactionRepository;
        _mapper = mapper;
    }
    
    public async Task<PaginatedResult<PaymentTransactionModel>> GetPaymentTransactionsByUserAsync(int pageIndex, int pageSize, Guid userId, bool sortByCreatedAt,
        PaymentStatus? status = null)
    {
        var (items, totalCount) = await _paymentTransactionRepository
            .GetPaymentTransactionsByUserId(pageIndex, pageSize, userId, sortByCreatedAt, status);
        var models = _mapper.Map<List<PaymentTransactionModel>>(items);
        return new PaginatedResult<PaymentTransactionModel>
        {
            Data = models,
            Pagination = new  PaginationModel(totalCount, pageIndex, pageSize)
        };
    }

    public async Task<PaginatedResult<PaymentTransactionModel>> GetPaymentTransactionsAsync(int pageIndex, int pageSize, bool sortByCreatedAt, PaymentStatus? status,
        DateTime startDate, DateTime endDate)
    {
        var (items, totalCount) = await _paymentTransactionRepository
            .GetAllPaymentTransactions(pageIndex, pageSize, sortByCreatedAt, status, startDate, endDate);
        var models = _mapper.Map<List<PaymentTransactionModel>>(items);
        return new PaginatedResult<PaymentTransactionModel>
        {
            Data = models,
            Pagination = new  PaginationModel(totalCount, pageIndex, pageSize)
        };
    }
}