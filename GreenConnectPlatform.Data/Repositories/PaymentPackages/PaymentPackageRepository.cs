using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.PaymentPackages;

public class PaymentPackageRepository : BaseRepository<GreenConnectDbContext, PaymentPackage, Guid>,
    IPaymentPackageRepository
{
    public PaymentPackageRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaymentPackage?> GetPaymentPackageByIdAsync(Guid paymentPackageId)
    {
        return await _dbSet
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.PackageId == paymentPackageId);
    }

    public async Task<(List<PaymentPackage> Items, int TotalCount)> GetPaymentPackagesAsync(int pageIndex, int pageSize,
        string? roleName, bool? sortByPrice, PackageType? packageType,
        string? name)
    {
        var query = _dbSet.AsNoTracking();
        if (roleName != "Admin") query = query.Where(p => p.IsActive == true);
        if (!string.IsNullOrEmpty(name)) query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));
        if (packageType.HasValue) query = query.Where(p => p.PackageType == packageType.Value);
        if (sortByPrice.HasValue)
            query = query.OrderByDescending(p => p.Price);
        else
            query = query.OrderBy(p => p.Price);
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }
}