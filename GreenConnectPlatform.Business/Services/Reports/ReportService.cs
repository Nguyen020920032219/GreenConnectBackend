using GreenConnectPlatform.Business.Models.Reports;
using GreenConnectPlatform.Data.Repositories.Complaints;
using GreenConnectPlatform.Data.Repositories.RewardItems;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.Transactions;
using GreenConnectPlatform.Data.Repositories.Users;

namespace GreenConnectPlatform.Business.Services.Reports;

public class ReportService : IReportService
{
    private readonly IRewardItemRepository _itemRepository;
    private readonly IComplaintRepository _complaintRepository;
    private readonly IScrapPostRepository _scrapPostRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITransactionRepository _transactionRepository;

    public ReportService(IRewardItemRepository rewardItemRepository,
        IComplaintRepository complaintRepository,
        IScrapPostRepository scrapPostRepository,
        IUserRepository userRepository,
        ITransactionRepository transactionRepository)
    {
        _itemRepository = rewardItemRepository;
        _complaintRepository = complaintRepository;
        _scrapPostRepository = scrapPostRepository;
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
    }
    
    public async Task<ReportModel> GetReport(DateTime startDate, DateTime endDate)
    {
        var totalRewardItems = await _itemRepository.GetRewardItemsForReport();
        var totalComplaint = await _complaintRepository.GetComplaintsForReport(startDate, endDate);
        var totalPost = await _scrapPostRepository.GetScrapPostForReport(startDate, endDate);
        var totalUser = await _userRepository.GetUsersFroReport(startDate, endDate);
        var totalTransaction = await _transactionRepository.GetTransactionsForReport(startDate, endDate);
        var transactionModel = totalTransaction
            .GroupBy(t => t.Status)
            .Select(g => new TransactionStatusModel
            {
                TransactionStatus = g.Key,
                TotalTransactionStatus = g.Count()
            })
            .ToList();
        return new ReportModel
        {
            TotalRewardItems = totalRewardItems.Count(),
            ActivityComplaint = totalComplaint.Count(),
            TotalPost = totalPost.Count(),
            TotalAllUsers = totalUser.Count(),
            TotalTransaction = totalTransaction.Count(),
            TransactionStatus = transactionModel
        };
    }
}