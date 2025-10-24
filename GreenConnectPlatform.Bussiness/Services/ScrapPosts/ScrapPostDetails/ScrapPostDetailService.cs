using AutoMapper;
using GreenConnectPlatform.Bussiness.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.ScrapPosts.ScrapPostDetails;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Bussiness.Services.ScrapPosts.ScrapPostDetails;

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
            .FirstOrDefaultAsync(d => d.ScrapPostId == scrapPostId && d.ScrapCategoryId == scrapCategoryId);
        if (detail == null) throw new KeyNotFoundException("Scrap post detail not found");
        return _mapper.Map<ScrapPostDetailModel>(detail);
    }

    public async Task<ScrapPostDetailModel> CreateScrapPostDetail(Guid userId, Guid scrapPostId,
        ScrapPostDetailCreateModel scrapPostDetailCreateModel)
    {
        var scrapPost = await _scrapPostRepository.DbSet()
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null) throw new KeyNotFoundException("Scrap post not found");
        if (scrapPost.HouseholdId != userId)
            throw new UnauthorizedAccessException("User is not authorized to add details to this scrap post");

        var scrapPostDetails = await _scrapPostDetailRepository.DbSet()
            .FirstOrDefaultAsync(d =>
                d.ScrapPostId == scrapPostId && d.ScrapCategoryId == scrapPostDetailCreateModel.ScrapCategoryId);
        if (scrapPostDetails != null)
            throw new InvalidOperationException("Scrap post detail for this category already exists");

        var scrapCategory = await _scrapCategoryRepository.DbSet()
            .FirstOrDefaultAsync(c => c.ScrapCategoryId == scrapPostDetailCreateModel.ScrapCategoryId);

        if (scrapCategory == null) throw new KeyNotFoundException("Scrap category not exist");

        scrapPostDetailCreateModel.ScrapPostId = scrapPostId;
        var newScrapDetail = _mapper.Map<ScrapPostDetail>(scrapPostDetailCreateModel);
        var result = await _scrapPostDetailRepository.Add(newScrapDetail);
        if (result == null) throw new Exception("Failed to create scrap post detail");
        return _mapper.Map<ScrapPostDetailModel>(result);
    }

    public async Task<ScrapPostDetailModel> UpdateScrapPostDetail(Guid userId, Guid scrapPostId, int scrapCategoryId,
        ScrapPostDetailUpdateModel scrapPostDetailUpdateModel)
    {
        var scrapPost = await _scrapPostRepository.DbSet()
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null) throw new KeyNotFoundException("Scrap post not found");
        if (scrapPost.HouseholdId != userId)
            throw new UnauthorizedAccessException("User is not authorized to add details to this scrap post");

        var scrapPostDetails = await _scrapPostDetailRepository.DbSet()
            .FirstOrDefaultAsync(d => d.ScrapPostId == scrapPostId && d.ScrapCategoryId == scrapCategoryId);
        if (scrapPostDetails == null) throw new KeyNotFoundException("Scrap post detail not found");
        _mapper.Map(scrapPostDetailUpdateModel, scrapPostDetails);
        var result = await _scrapPostDetailRepository.Update(scrapPostDetails);
        if (result == null) throw new Exception("Failed to update scrap post detail");
        return _mapper.Map<ScrapPostDetailModel>(result);
    }

    public async Task<bool> DeleteScrapPostDetail(Guid userId, Guid scrapPostId, int scrapCategoryId)
    {
        var scrapPost = await _scrapPostRepository.DbSet()
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null) throw new KeyNotFoundException("Scrap post not found");
        if (scrapPost.HouseholdId != userId)
            throw new UnauthorizedAccessException("User is not authorized to add details to this scrap post");

        var details = await _scrapPostDetailRepository.DbSet()
            .FirstOrDefaultAsync(d => d.ScrapPostId == scrapPostId && d.ScrapCategoryId == scrapCategoryId);
        if (details == null) throw new KeyNotFoundException("Scrap post detail not found");
        await _scrapPostDetailRepository.Delete(details);
        return true;
    }
}