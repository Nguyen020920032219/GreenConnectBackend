using AutoMapper;
using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Bussiness.Services.ScrapPosts;

public class ScrapPostService : IScrapPostService
{
    private readonly GeometryFactory _geometryFactory;
    private readonly IMapper _mapper;
    private readonly IProfileRepository _profileRepository;
    private readonly IScrapCategoryRepository _scrapCategoryRepository;
    private readonly IScrapPostRepository _scrapPostRepository;

    public ScrapPostService(
        IScrapPostRepository scrapPostRepository,
        IScrapCategoryRepository scrapCategoryRepository,
        IProfileRepository profileRepository,
        IMapper mapper)
    {
        _scrapPostRepository = scrapPostRepository;
        _scrapCategoryRepository = scrapCategoryRepository;
        _profileRepository = profileRepository;
        _mapper = mapper;
        _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
    }

    public async Task<List<ScrapPostOverralModel>> GetPosts(
        int pageNumber,
        int pageSize,
        Guid? userId,
        string? categoryName,
        bool sortByLocation = false)
    {
        Point? userLocation = null;
        if (sortByLocation && userId.HasValue)
        {
            var userProfile = await _profileRepository.DbSet().AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (userProfile != null && userProfile.Location != null) userLocation = userProfile.Location;
        }

        var query = _scrapPostRepository.DbSet()
            .Where(s => s.Status == PostStatus.Open);

        if (sortByLocation && userLocation != null)
            query = query
                .Where(post => post.Location != null)
                .OrderBy(post => post.Location!.Distance(userLocation));
        else
            query = query.OrderByDescending(s => s.CreatedAt);

        if (!string.IsNullOrWhiteSpace(categoryName))
            query = query
                .Include(post => post.ScrapPostDetails)
                .ThenInclude(detail => detail.ScrapCategory)
                .Where(post => post.ScrapPostDetails.Any(detail =>
                    detail.ScrapCategory.CategoryName.ToLower().Contains(categoryName.ToLower())));

        var scrapPosts = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return _mapper.Map<List<ScrapPostOverralModel>>(scrapPosts);
    }

    public async Task<List<ScrapPostOverralModel>> GetPostsByHousehold(int pageNumber, int pageSize, Guid? userId,
        string? title, PostStatus? status)
    {
        var query = _scrapPostRepository.DbSet()
            .Where(s => s.HouseholdId == userId);
        if (!string.IsNullOrWhiteSpace(title)) query = query.Where(s => s.Title.ToLower().Contains(title.ToLower()));

        if (status != null) query = query.Where(s => s.Status == status);
        var scrapPosts = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return _mapper.Map<List<ScrapPostOverralModel>>(scrapPosts);
    }

    public async Task<ScrapPostModel> GetPost(Guid scrapPostId)
    {
        var scrapPost = await _scrapPostRepository.DbSet().Include(s => s.ScrapPostDetails)
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null)
            throw new KeyNotFoundException("Scrap post not found");
        return _mapper.Map<ScrapPostModel>(scrapPost);
    }

    public async Task<ScrapPostModel> CreateScrapPost(ScrapPostCreateModel scrapPostCreateModel)
    {
        Point? userLocation = null;
        var category = scrapPostCreateModel.ScrapPostDetails
            .Select(c => c.ScrapCategoryId).ToList();

        if (category.Any())
        {
            var distinctCategoryIds = category.Distinct().ToList();

            if (distinctCategoryIds.Count != category.Count)
                throw new ArgumentException("Scrap category IDs in the details list cannot be duplicated.");

            var existingCategoryCount = await _scrapCategoryRepository.DbSet()
                .CountAsync(c => distinctCategoryIds.Contains(c.ScrapCategoryId));

            if (existingCategoryCount != distinctCategoryIds.Count)
                throw new KeyNotFoundException("One or more scrap categories do not exist");
        }

        var scrapPost = _mapper.Map<ScrapPost>(scrapPostCreateModel);
        if (scrapPostCreateModel.Location.Latitude.HasValue && scrapPostCreateModel.Location.Longitude.HasValue)
            userLocation = _geometryFactory.CreatePoint(new Coordinate(
                scrapPostCreateModel.Location.Longitude.Value,
                scrapPostCreateModel.Location.Latitude.Value));
        scrapPost.Location = userLocation;
        scrapPost.ScrapPostId = Guid.NewGuid();
        scrapPost.CreatedAt = DateTime.UtcNow;
        var result = await _scrapPostRepository.Add(scrapPost);
        if (result == null) throw new Exception("Scrap post not created");
        return _mapper.Map<ScrapPostModel>(result);
    }

    public async Task<ScrapPostModel> UpdateScrapPost(Guid userId, Guid scrapPostId,
        ScrapPostUpdateModel scrapPostUpdateModel)
    {
        var scrapPost = await _scrapPostRepository.DbSet()
            .Include(s => s.ScrapPostDetails)
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);

        if (scrapPost == null)
            throw new KeyNotFoundException("Scrap post not found");

        if (scrapPost.HouseholdId != userId)
            throw new UnauthorizedAccessException("You are not authorized to update this scrap post");

        _mapper.Map(scrapPostUpdateModel, scrapPost);
        scrapPost.UpdatedAt = DateTime.UtcNow;
        Point? location;
        if (scrapPostUpdateModel.Location != null)
        {
            if (scrapPostUpdateModel.Location.Latitude.HasValue && scrapPostUpdateModel.Location.Longitude.HasValue)
                location = _geometryFactory.CreatePoint(new Coordinate(
                    scrapPostUpdateModel.Location.Longitude.Value,
                    scrapPostUpdateModel.Location.Latitude.Value));
            else
                scrapPostUpdateModel.Location = null;
        }
        else
        {
            location = scrapPost.Location;
        }

        var result = await _scrapPostRepository.Update(scrapPost);
        if (result == null) throw new Exception("Scrap post not updated");
        return _mapper.Map<ScrapPostModel>(result);
    }

    public async Task<bool> ToggleScrapPost(Guid userId, Guid scrapPostId)
    {
        var scrapPost = await _scrapPostRepository.DbSet()
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null)
            throw new KeyNotFoundException("Scrap post not found");
        if (scrapPost.HouseholdId != userId)
            throw new UnauthorizedAccessException("You are not authorized to delete this scrap post");
        scrapPost.UpdatedAt = DateTime.UtcNow;
        if (scrapPost.Status != PostStatus.Canceled)
            scrapPost.Status = PostStatus.Canceled;
        else
            scrapPost.Status = PostStatus.Open;
        var result = await _scrapPostRepository.Update(scrapPost);
        return true;
    }
}