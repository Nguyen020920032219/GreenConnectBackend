using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.PaymentPackages;

public interface IPaymentPackageRepository : IBaseRepository<PaymentPackage, Guid>
{ 
    Task<PaymentPackage?> GetPaymentPackageByIdAsync(Guid paymentPackageId);
    Task<(List<PaymentPackage> Items, int TotalCount)> GetPaymentPackagesAsync(
        int pageIndex,
        int pageSize,
        string? roleName,
        bool? sortByPrice, 
        PackageType? packageType, 
        string? name);
}