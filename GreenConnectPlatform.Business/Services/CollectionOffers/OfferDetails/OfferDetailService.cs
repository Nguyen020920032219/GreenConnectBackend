using AutoMapper;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Business.Services.CollectionOffers.OfferDetails;

public class OfferDetailService : IOfferDetailService
{
    private readonly ICollectionOfferRepository _collectionOfferRepository;
    private readonly IMapper _mapper;
    private readonly IOfferDetailRepository _offerDetailRepository;
    private readonly IScrapCategoryRepository _scrapCategoryRepository;

    public OfferDetailService(
        IOfferDetailRepository offerDetailRepository,
        ICollectionOfferRepository collectionOfferRepository,
        IScrapCategoryRepository scrapCategoryRepository,
        IMapper mapper)
    {
        _offerDetailRepository = offerDetailRepository;
        _collectionOfferRepository = collectionOfferRepository;
        _scrapCategoryRepository = scrapCategoryRepository;
        _mapper = mapper;
    }

    public async Task<OfferDetailModel> GetOfferDetail(Guid offerDetailId, Guid offerId)
    {
        var offerDetail = await _offerDetailRepository.DbSet()
            .Include(o => o.ScrapCategory)
            .FirstOrDefaultAsync(o => o.OfferDetailId == offerDetailId && o.CollectionOfferId == offerId);
        if (offerDetail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Offer detail does not exist");
        return _mapper.Map<OfferDetailModel>(offerDetail);
    }

    public async Task<OfferDetailModel> AddOfferDetail(Guid collectorId, Guid collectionOfferId,
        OfferDetailCreateModel offerDetailCreateModel)
    {
        var collectionOffer = await _collectionOfferRepository.DbSet()
            .Include(o => o.ScrapCollector)
            .Include(o => o.OfferDetails)
            .Include(o => o.ScrapPost)
            .ThenInclude(o => o.ScrapPostDetails)
            .FirstOrDefaultAsync(c => c.CollectionOfferId == collectionOfferId);
        if (collectionOffer == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "CollectionOfferId is does not exist");
        if (collectionOffer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "You can not add offer detail to this collection offer");
        if (collectionOffer.Status != OfferStatus.Rejected && collectionOffer.Status != OfferStatus.Canceled)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "You can only add offer details to reject or cancel collection offers");

        var scrapCategory = await _scrapCategoryRepository.DbSet()
            .FirstOrDefaultAsync(s => s.ScrapCategoryId == offerDetailCreateModel.ScrapCategoryId);
        if (scrapCategory == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "ScrapCategoryId is does not exist");

        var allCategoriesIdInPost = collectionOffer.ScrapPost.ScrapPostDetails
            .Select(d => d.ScrapCategoryId)
            .ToHashSet();
        if (!allCategoriesIdInPost.Contains(offerDetailCreateModel.ScrapCategoryId))
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Scrap category does not belong to the original scrap post.");

        var detailAlreadyExists = collectionOffer.OfferDetails
            .Any(d => d.ScrapCategoryId == offerDetailCreateModel.ScrapCategoryId);
        if (detailAlreadyExists)
            throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409",
                "An offer detail for this scrap category already exists in the collection offer.");

        var offerDetailModel = _mapper.Map<OfferDetail>(offerDetailCreateModel);

        offerDetailModel.CollectionOfferId = collectionOfferId;
        offerDetailModel.OfferDetailId = Guid.NewGuid();
        var result = await _offerDetailRepository.Add(offerDetailModel);
        if (result == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Failed to create scrap post detail");
        return _mapper.Map<OfferDetailModel>(result);
    }

    public async Task<OfferDetailModel> UpdateOfferDetail(Guid collectorId, Guid offerDetailId,
        OfferDetailUpdateModel offerDetailUpdateModel)
    {
        var offerDetail = await _offerDetailRepository.DbSet()
            .Include(o => o.CollectionOffer)
            .FirstOrDefaultAsync(o => o.OfferDetailId == offerDetailId);
        if (offerDetail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Offer detail does not exist");
        if (offerDetail.CollectionOffer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "You can not update this offer detail");
        if (offerDetail.CollectionOffer.Status == OfferStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "You can not update offer details of accepted collection offers");
        if (offerDetailUpdateModel.PricePerUnit == null) offerDetailUpdateModel.PricePerUnit = offerDetail.PricePerUnit;
        _mapper.Map(offerDetailUpdateModel, offerDetail);
        var result = await _offerDetailRepository.Update(offerDetail);
        if (result == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Failed to update offer detail");
        return _mapper.Map<OfferDetailModel>(result);
    }

    public async Task DeleteOfferDetail(Guid collectorId, Guid offerDetailId)
    {
        var offerDetail = await _offerDetailRepository.DbSet()
            .Include(d => d.CollectionOffer)
            .FirstOrDefaultAsync(o => o.OfferDetailId == offerDetailId);
        if (offerDetail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Offer detail does not exist");
        if (offerDetail.CollectionOffer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "You can not update this offer detail");
        if (offerDetail.CollectionOffer.Status != OfferStatus.Canceled &&
            offerDetail.CollectionOffer.Status != OfferStatus.Rejected)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "You can only update offer details of cancel or rejected collection offers");
        await _offerDetailRepository.Delete(offerDetail);
    }
}