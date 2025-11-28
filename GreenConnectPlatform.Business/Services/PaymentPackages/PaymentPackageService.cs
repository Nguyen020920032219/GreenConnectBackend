using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PaymentPackages;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.PaymentPackages;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.PaymentPackages;

public class PaymentPackageService : IPaymentPackageService
{
    private readonly IMapper _mapper;
    private readonly IPaymentPackageRepository _paymentPackageRepository;

    public PaymentPackageService(IPaymentPackageRepository paymentPackageRepository, IMapper mapper)
    {
        _paymentPackageRepository = paymentPackageRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<PaymentPackageOverallModel>> GetPaymentPackages(int pageNumber, int pageSize,
        string? roleName,
        bool? sortByPrice, PackageType? packageType, string? name)
    {
        var (items, totalCount) = await _paymentPackageRepository.GetPaymentPackagesAsync(
            pageNumber,
            pageSize,
            roleName,
            sortByPrice,
            packageType,
            name);
        var paymentPackageOverallModels = _mapper.Map<List<PaymentPackageOverallModel>>(items);
        return new PaginatedResult<PaymentPackageOverallModel>
        {
            Data = paymentPackageOverallModels,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<PaymentPackageModel> GetPaymentPackage(Guid packageId)
    {
        var paymentPackage = await _paymentPackageRepository.GetPaymentPackageByIdAsync(packageId);
        if (paymentPackage == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Gói cước không tồn tại");
        return _mapper.Map<PaymentPackageModel>(paymentPackage);
    }

    public async Task<PaymentPackageModel> CreatePaymentPackage(PaymentPackageCreateModel model)
    {
        var paymentPackage = _mapper.Map<PaymentPackage>(model);
        paymentPackage.PackageId = Guid.NewGuid();
        paymentPackage.IsActive = true;
        await _paymentPackageRepository.AddAsync(paymentPackage);
        return await GetPaymentPackage(paymentPackage.PackageId);
    }

    public async Task<PaymentPackageModel> UpdatePaymentPackage(Guid packageId, PaymentPackageUpdateModel model)
    {
        var paymentPackage = await _paymentPackageRepository.GetPaymentPackageByIdAsync(packageId);
        if (paymentPackage == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Gói cước không tồn tại");
        if (model.Price == null) model.Price = paymentPackage.Price;
        if (model.ConnectionAmount == null) model.ConnectionAmount = paymentPackage.ConnectionAmount;
        if (model.PackageType == null) model.PackageType = (int)paymentPackage.PackageType;
        _mapper.Map(model, paymentPackage);
        await _paymentPackageRepository.UpdateAsync(paymentPackage);
        return _mapper.Map<PaymentPackageModel>(paymentPackage);
    }

    public async Task InActivePaymentPackage(Guid packageId)
    {
        var paymentPackage = await _paymentPackageRepository.GetPaymentPackageByIdAsync(packageId);
        if (paymentPackage == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Gói cước không tồn tại");
        paymentPackage.IsActive = false;
        await _paymentPackageRepository.UpdateAsync(paymentPackage);
    }
}