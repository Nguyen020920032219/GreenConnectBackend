using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Reports;

public class ReportForCollectorModel
{
    public float TotalEarning { get; set; }
    public int TotalFeedbacks { get; set; }
    public float TotalRating { get; set; }
    public int TotalComplaints { get; set; }
    public List<ComplaintsModel> Complaints { get; set; } = new();
    public int TotalAccused { get; set; }
    public List<ComplaintsModel> Accused { get; set; } = new();
    public int TotalOffers { get; set; }
    public List<OffersModel> Offers { get; set; } = new();
    public int TotalTransactions { get; set; }
    public List<TransactionReportModel> Transactions { get; set; } = new();
}

public class OffersModel
{
    public OfferStatus Status { get; set; }
    public int Count { get; set; }
}

public class TransactionReportModel
{
    public TransactionStatus Status { get; set; }
    public int Count { get; set; }
}

public class ComplaintsModel
{
    public ComplaintStatus Status { get; set; }
    public int Count { get; set; }
}