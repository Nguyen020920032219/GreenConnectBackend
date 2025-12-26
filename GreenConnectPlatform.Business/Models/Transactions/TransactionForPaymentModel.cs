using System.Transactions;
using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.ScrapPostTimeSlots;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Business.Models.Users;

namespace GreenConnectPlatform.Business.Models.Transactions;

public class TransactionForPaymentModel
{
    public List<TransactionModel> Transactions { get; set; } = new();
    public float AmountDifference { get; set; }
}