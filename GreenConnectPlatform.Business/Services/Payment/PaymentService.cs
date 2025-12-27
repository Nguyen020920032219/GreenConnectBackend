using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Payment;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;
using GreenConnectPlatform.Data.Repositories.PaymentPackages;
using GreenConnectPlatform.Data.Repositories.PaymentTransactions;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.UserPackages;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Payment;

public class PaymentService : IPaymentService
{
    private readonly ICreditTransactionHistoryRepository _creditHistoryRepo;
    private readonly IPaymentPackageRepository _packageRepo;
    private readonly IProfileRepository _profileRepo;
    private readonly IPaymentTransactionRepository _txnRepo;
    private readonly IUserPackageRepository _userPackageRepo;
    private readonly IVnPayService _vnPayService;

    public PaymentService(
        IVnPayService vnPayService,
        IPaymentPackageRepository packageRepo,
        IPaymentTransactionRepository txnRepo,
        IUserPackageRepository userPackageRepo,
        IProfileRepository profileRepo,
        ICreditTransactionHistoryRepository creditHistoryRepo)
    {
        _vnPayService = vnPayService;
        _packageRepo = packageRepo;
        _txnRepo = txnRepo;
        _userPackageRepo = userPackageRepo;
        _profileRepo = profileRepo;
        _creditHistoryRepo = creditHistoryRepo;
    }

    public async Task<PaymentLinkResponse> CreatePaymentLinkAsync(Guid userId, CreatePaymentRequest request,
        HttpContext httpContext)
    {
        var package = await _packageRepo.GetByIdAsync(request.PackageId);
        if (package == null || !package.IsActive)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Gói cước không tồn tại.");
        if (package.Price <= 0)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Gói miễn phí không cần thanh toán.");

        var txnRef = DateTime.Now.Ticks.ToString();
        var transaction = new PaymentTransaction
        {
            PaymentId = Guid.NewGuid(),
            UserId = userId,
            PackageId = package.PackageId,
            Amount = package.Price,
            PaymentGateway = "VNPay",
            Status = PaymentStatus.Pending,
            TransactionRef = txnRef,
            CreatedAt = DateTime.Now
        };
        await _txnRepo.AddAsync(transaction);

        var url = _vnPayService.CreatePaymentUrl(httpContext, txnRef, (double)package.Price, $"Mua goi {package.Name}");

        return new PaymentLinkResponse { PaymentUrl = url, TransactionRef = txnRef };
    }

    public async Task ProcessPaymentCallbackAsync(IQueryCollection collections)
    {
        var vnpResponse = _vnPayService.PaymentExecute(collections);

        var txn = await _txnRepo.GetByTransactionRefAsync(vnpResponse.OrderId);

        if (txn == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giao dịch không tồn tại.");
        if (txn.Status == PaymentStatus.Success) return;

        txn.VnpTransactionNo = vnpResponse.PaymentId;
        txn.ResponseCode = vnpResponse.VnPayResponseCode;
        txn.BankCode = vnpResponse.BankCode;
        txn.OrderInfo = vnpResponse.OrderInfo;

        if (!vnpResponse.Success || vnpResponse.VnPayResponseCode != "00")
        {
            txn.Status = PaymentStatus.Failed;
            await _txnRepo.UpdateAsync(txn);
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Thanh toán thất bại hoặc bị hủy.");
        }

        txn.Status = PaymentStatus.Success;
        await _txnRepo.UpdateAsync(txn);

        await ActivatePackageForUser(txn.UserId, txn.PackageId!.Value, txn.PaymentId);
    }

    private async Task ActivatePackageForUser(Guid userId, Guid packageId, Guid transactionId)
    {
        var package = await _packageRepo.GetByIdAsync(packageId);
        if (package == null) return;

        var userPackages = await _userPackageRepo.FindAsync(up => up.UserId == userId);
        var existingUserPackage = userPackages.FirstOrDefault();

        if (existingUserPackage == null)
        {
            var newUserPackage = new UserPackage
            {
                UserPackageId = Guid.NewGuid(),
                UserId = userId,
                PackageId = packageId,
                ActivationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(30),
                RemainingConnections = package.ConnectionAmount
            };
            await _userPackageRepo.AddAsync(newUserPackage);
        }
        else
        {
            existingUserPackage.PackageId = packageId;
            existingUserPackage.ActivationDate = DateTime.Now;
            existingUserPackage.ExpirationDate = DateTime.Now.AddDays(30);
            if (package.ConnectionAmount.HasValue)
                existingUserPackage.RemainingConnections =
                    (existingUserPackage.RemainingConnections ?? 0) + package.ConnectionAmount;
            await _userPackageRepo.UpdateAsync(existingUserPackage);
        }

        var profile = await _profileRepo.GetByUserIdWithRankAsync(userId);
        if (profile != null && package.ConnectionAmount.HasValue)
        {
            profile.CreditBalance += package.ConnectionAmount.Value;
            await _profileRepo.UpdateAsync(profile);

            var history = new CreditTransactionHistory
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Amount = package.ConnectionAmount.Value,
                BalanceAfter = profile.CreditBalance,
                Type = "Purchase",
                ReferenceId = transactionId,
                Description = $"Mua gói {package.Name}",
                CreatedAt = DateTime.Now
            };
            await _creditHistoryRepo.AddAsync(history);
        }
    }
}