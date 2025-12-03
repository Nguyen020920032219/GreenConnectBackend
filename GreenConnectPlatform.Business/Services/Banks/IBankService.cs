using GreenConnectPlatform.Business.Models.Banks;

namespace GreenConnectPlatform.Business.Services.Banks;

public interface IBankService
{
    Task<List<BankModel>> GetSupportedBanksAsync();
}