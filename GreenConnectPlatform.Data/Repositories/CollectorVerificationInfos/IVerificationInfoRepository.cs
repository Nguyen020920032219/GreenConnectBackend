using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;

public interface IVerificationInfoRepository : IBaseRepository<CollectorVerificationInfo, Guid>
{
    Task<CollectorVerificationInfo?> GetByUserIdAsync(Guid userId);

    Task<(List<CollectorVerificationInfo> Items, int TotalCount)> SearchAsync(
        bool sortBySubmittedAt,
        VerificationStatus? status,
        int pageIndex,
        int pageSize);
}