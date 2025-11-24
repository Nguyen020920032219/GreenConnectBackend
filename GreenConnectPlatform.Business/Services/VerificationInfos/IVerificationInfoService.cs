using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.VerificationInfos;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.VerificationInfos;

public interface IVerificationInfoService
{
    Task<PaginatedResult<VerificationInfoOveralModel>> GetVerificationInfos(int pageNumber, int pageSize, bool sortBySubmittedAt, VerificationStatus? sortByStatus);
    Task<VerificationInfoModel> GetVerificationInfo(Guid userId);
    Task<VerificationInfoModel> GetMyVerificationInfo(Guid userId);
    Task VerifyCollector(Guid userId, Guid reviewerId, bool isAccepted, string? reviewerNotes);
    
    Task<VerificationInfoModel> UpdateVerificationInfo(Guid userId,BuyerType? buyerType, string? documentFrontUrl, string? documentBackUrl);
}