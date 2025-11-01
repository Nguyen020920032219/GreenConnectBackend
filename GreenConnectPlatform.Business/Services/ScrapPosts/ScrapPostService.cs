using AutoMapper;
using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.ScrapPosts.ScrapPostDetails;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Bussiness.Services.ScrapPosts;

public class ScrapPostService : IScrapPostService
{
    private readonly IMapper _mapper;
    private readonly IScrapCategoryRepository _scrapCategoryRepository;
    private readonly IScrapPostDetailRepository _scrapPostDetailRepository;
    private readonly IScrapPostRepository _scrapPostRepository;

    public ScrapPostService(IScrapPostRepository scrapPostRepository,
        IScrapPostDetailRepository scrapPostDetailRepository,
        IScrapCategoryRepository scrapCategoryRepository,
        IMapper mapper)
    {
        _scrapPostRepository = scrapPostRepository;
        _scrapPostDetailRepository = scrapPostDetailRepository;
        _scrapCategoryRepository = scrapCategoryRepository;
        _mapper = mapper;
    }

    public async Task<List<ScrapPostOverral>> GetPosts(int pageNumber, int pageSize)
    {
        var scrapPosts = await _scrapPostRepository.DbSet()
            .Include(s => s.ScrapPostDetails)
            .Where(s => s.Status == PostStatus.Open)
            .OrderByDescending(s => s.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return _mapper.Map<List<ScrapPostOverral>>(scrapPosts);
    }

    public async Task<ScrapPostModel> GetPost(Guid scrapPostId)
    {
        var scrapPost = await _scrapPostRepository.DbSet().Include(s => s.ScrapPostDetails)
            .FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null)
            throw new KeyNotFoundException("Scrap post not found");
        return _mapper.Map<ScrapPostModel>(scrapPost);
    }

    public async Task<ScrapPostModel> CreateScrapPost(ScrapPostRequest scrapPostRequest)
    {
        var scrapPost = _mapper.Map<ScrapPost>(scrapPostRequest);
        scrapPost.ScrapPostId = Guid.NewGuid();
        scrapPost.CreatedAt = DateTime.UtcNow;
        var result = await _scrapPostRepository.Add(scrapPost);
        if (result == null) throw new Exception("Scrap post not created");
        return _mapper.Map<ScrapPostModel>(result);
    }
}