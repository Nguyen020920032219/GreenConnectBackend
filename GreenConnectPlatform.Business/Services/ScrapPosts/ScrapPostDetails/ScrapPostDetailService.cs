using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.ScrapPosts.ScrapPostDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Business.Services.ScrapPosts.ScrapPostDetails;

public class ScrapPostDetailService : IScrapPostDetailService
{
    private readonly IMapper _mapper;
    private readonly IScrapCategoryRepository _scrapCategoryRepository;
    private readonly IScrapPostDetailRepository _scrapPostDetailRepository;
    private readonly IScrapPostRepository _scrapPostRepository;

    public ScrapPostDetailService(
        IScrapPostDetailRepository scrapPostDetailRepository,
        IScrapPostRepository scrapPostRepository,
        IScrapCategoryRepository scrapCategoryRepository,
        IMapper mapper)
    {
        _scrapPostDetailRepository = scrapPostDetailRepository;
        _scrapPostRepository = scrapPostRepository;
        _scrapCategoryRepository = scrapCategoryRepository;
        _mapper = mapper;
    }

    public async Task<ScrapPostDetailModel> GetScrapPostDetailById(Guid scrapPostId, int scrapCategoryId)
    {
        var detail = await _scrapPostDetailRepository.DbSet()
            .Include(s => s.ScrapCategory)
            .FirstOrDefaultAsync(d => d.ScrapPostId == scrapPostId && d.ScrapCategoryId == scrapCategoryId);
        if (detail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Scrap post detail not found");
        return _mapper.Map<ScrapPostDetailModel>(detail);
    }

    public async Task<ScrapPostDetailModel> AddScrapPostDetail(Guid userId, Guid scrapPostId,
        ScrapPostDetailCreateModel scrapPostDetailCreateModel)
    {
        var scrapPost = await _scrapPostRepository.DbSet()
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Scrap post not found");
        if (scrapPost.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "You can not add details to this scrap post");
        if (scrapPost.Status == PostStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Cannot add details to a completed scrap post");
        var scrapPostDetails = await _scrapPostDetailRepository.DbSet()
            .FirstOrDefaultAsync(d =>
                d.ScrapPostId == scrapPostId && d.ScrapCategoryId == scrapPostDetailCreateModel.ScrapCategoryId);
        if (scrapPostDetails != null)
            throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409", "Scrap post details already exists");

        var scrapCategory = await _scrapCategoryRepository.DbSet()
            .FirstOrDefaultAsync(c => c.ScrapCategoryId == scrapPostDetailCreateModel.ScrapCategoryId);

        if (scrapCategory == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Scrap post category not found");

        scrapPostDetailCreateModel.ScrapPostId = scrapPostId;
        var newScrapDetail = _mapper.Map<ScrapPostDetail>(scrapPostDetailCreateModel);
        var result = await _scrapPostDetailRepository.Add(newScrapDetail);
        if (result == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Failed to create scrap post detail");
        return _mapper.Map<ScrapPostDetailModel>(result);
    }

    public async Task<ScrapPostDetailModel> UpdateScrapPostDetail(Guid userId, Guid scrapPostId, int scrapCategoryId,
        ScrapPostDetailUpdateModel scrapPostDetailUpdateModel)
    {
        var scrapPost = await _scrapPostRepository.DbSet()
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Scrap post not found");
        if (scrapPost.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "You can not update details of this scrap post");
        if (scrapPost.Status == PostStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Cannot update details to a completed scrap post");
        var scrapPostDetails = await _scrapPostDetailRepository.DbSet()
            .FirstOrDefaultAsync(d => d.ScrapPostId == scrapPostId && d.ScrapCategoryId == scrapCategoryId);
        if (scrapPostDetails == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Scrap post detail does not exist");

        if (scrapPostDetails.Status != PostDetailStatus.Available)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Only available scrap post details can be updated");
        _mapper.Map(scrapPostDetailUpdateModel, scrapPostDetails);
        var result = await _scrapPostDetailRepository.Update(scrapPostDetails);
        if (result == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Failed to update scrap post detail");
        return _mapper.Map<ScrapPostDetailModel>(result);
    }

    public async Task DeleteScrapPostDetail(Guid userId, Guid scrapPostId, string userRole, int scrapCategoryId)
    {
        var scrapPost = await _scrapPostRepository.DbSet()
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Scrap post not found");
        if (userRole != "Admin")
            if (scrapPost.HouseholdId != userId)
                throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "You can not delete this details");
        if (scrapPost.Status == PostStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Cannot delete details to a completed scrap post");
        var details = await _scrapPostDetailRepository.DbSet()
            .FirstOrDefaultAsync(d => d.ScrapPostId == scrapPostId && d.ScrapCategoryId == scrapCategoryId);
        if (details == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Scrap post detail does not exist");
        if (details.Status == PostDetailStatus.Booked)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Cannot delete a scrap post detail that is booked");
        await _scrapPostDetailRepository.Delete(details);
    }
}