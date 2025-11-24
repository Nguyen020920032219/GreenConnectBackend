using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PaymentPackages;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.PaymentPackages;

public interface IPaymentPackageService
{
    Task<PaginatedResult<PaymentPackageOverallModel>> GetPaymentPackages(int pageNumber, int pageSize, string? roleName,
        bool? sortByPrice, PackageType? packageType, string? name);
    
    Task<PaymentPackageModel> GetPaymentPackage(Guid packageId);
    
    Task<PaymentPackageModel> CreatePaymentPackage(PaymentPackageCreateModel model);
    
    Task<PaymentPackageModel> UpdatePaymentPackage(Guid packageId, PaymentPackageUpdateModel model);
    
    Task InActivePaymentPackage(Guid packageId);
}