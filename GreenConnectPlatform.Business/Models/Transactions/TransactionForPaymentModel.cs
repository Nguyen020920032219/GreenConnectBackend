namespace GreenConnectPlatform.Business.Models.Transactions;

public class TransactionForPaymentModel
{
    public List<TransactionModel> Transactions { get; set; } = new();
    public float AmountDifference { get; set; }
}