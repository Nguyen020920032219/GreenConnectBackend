using GreenConnectPlatform.Business.Models.Reports;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.Complaints;
using GreenConnectPlatform.Data.Repositories.Feedbacks;
using GreenConnectPlatform.Data.Repositories.RewardItems;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.Transactions;
using GreenConnectPlatform.Data.Repositories.Users;

namespace GreenConnectPlatform.Business.Services.Reports;

public class ReportService : IReportService
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IRewardItemRepository _itemRepository;
    private readonly ICollectionOfferRepository _offerRepository;
    private readonly IScrapPostRepository _scrapPostRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    public ReportService(IRewardItemRepository rewardItemRepository,
        IComplaintRepository complaintRepository,
        IScrapPostRepository scrapPostRepository,
        IUserRepository userRepository,
        ITransactionRepository transactionRepository,
        IFeedbackRepository feedbackRepository,
        ICollectionOfferRepository offerRepository)
    {
        _itemRepository = rewardItemRepository;
        _complaintRepository = complaintRepository;
        _scrapPostRepository = scrapPostRepository;
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
        _feedbackRepository = feedbackRepository;
        _offerRepository = offerRepository;
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

    public async Task<ReportForCollectorModel> GetReportForCollector(Guid userId, DateTime startDate, DateTime endDate)
    {
        var earning = await _transactionRepository.GetEarningForCollectorReport(userId, startDate, endDate);
        var feedbacks = await _feedbackRepository.GetAllFeedbacks(userId, startDate, endDate);
        var complaints = await _complaintRepository.GetComplaintsForCollectorReport(userId, startDate, endDate);
        var complaintModel = complaints
            .GroupBy(t => t.Status)
            .Select(c => new ComplaintsModel
            {
                Status = c.Key,
                Count = c.Count()
            }).ToList();
        var accused = await _complaintRepository.GetAccusedForCollectorReport(userId, startDate, endDate);
        var accusedModel = accused
            .GroupBy(t => t.Status)
            .Select(c => new ComplaintsModel
            {
                Status = c.Key,
                Count = c.Count()
            }).ToList();
        var offers = await _offerRepository.GetOffersForReport(userId, startDate, endDate);
        var offerModel = offers
            .GroupBy(t => t.Status)
            .Select(o => new OffersModel
            {
                Status = o.Key,
                Count = o.Count()
            }).ToList();
        var transactions = await _transactionRepository.GetTransactionsForCollectorReport(userId, startDate, endDate);
        var transactionModel = transactions
            .GroupBy(t => t.Status)
            .Select(tr => new TransactionReportModel
            {
                Status = tr.Key,
                Count = tr.Count()
            })
            .ToList();

        return new ReportForCollectorModel
        {
            TotalEarning = earning.Sum(t => t.TransactionDetails.Sum(d => (float)d.FinalPrice * d.Quantity)),
            TotalFeedbacks = feedbacks.Count,
            TotalRating = feedbacks.Count > 0 ? feedbacks.Sum(f => f.Rate) / feedbacks.Count : 0,
            TotalComplaints = complaints.Count,
            Complaints = complaintModel,
            TotalAccused = accused.Count,
            Accused = accusedModel,
            TotalOffers = offers.Count,
            Offers = offerModel,
            TotalTransactions = transactions.Count,
            Transactions = transactionModel
        };
    }
}