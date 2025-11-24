using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Reports;

public class ReportModel
{
    public int TotalRewardItems {get; set;}
    public int ActivityComplaint {get; set;}
    public int TotalPost{get; set;}
    public int TotalAllUsers {get; set;}
    public int TotalTransaction {get; set;}
    public List<TransactionStatusModel> TransactionStatus { get; set; } = new();
}

public class TransactionStatusModel
{
    public TransactionStatus TransactionStatus {get; set;}
    public int TotalTransactionStatus {get; set;}
}

