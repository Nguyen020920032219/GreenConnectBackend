using AutoMapper;
using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Bussiness.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using GreenConnectPlatform.Data.Repositories.ScrapPosts.ScrapPostDetails;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Bussiness.Services.ScrapPosts;

public class ScrapPostService : IScrapPostService
{
    private readonly IScrapPostRepository _scrapPostRepository;
    private readonly IScrapPostDetailRepository _scrapPostDetailRepository;
    private readonly IScrapCategoryRepository _scrapCategoryRepository;
    private readonly IMapper _mapper;
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
        var scrapPost = await _scrapPostRepository.DbSet().Include(s => s.ScrapPostDetails).FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null)
            throw new KeyNotFoundException("Scrap post not found");
        return _mapper.Map<ScrapPostModel>(scrapPost);
    }

    public async Task<ScrapPostModel> CreateScrapPost(ScrapPostRequest scrapPostRequest)
    {
        scrapPostRequest.ScrapPostId = Guid.NewGuid();
        var scrapPost = _mapper.Map<ScrapPost>(scrapPostRequest);
        scrapPost.ScrapPostDetails = new List<ScrapPostDetail>();
        var result = await _scrapPostRepository.Add(scrapPost);
        if(result == null) throw new Exception("Scrap post not created");
        foreach (var details in scrapPostRequest.ScrapPostDetails)
        {
            var scrapPostDetails = _mapper.Map<ScrapPostDetail>(details);
            var category = await _scrapCategoryRepository.DbSet().FirstOrDefaultAsync(c => c.ScrapCategoryId == scrapPostDetails.ScrapCategoryId);
            if (category == null) throw new Exception("Category is not exist!");
            scrapPostDetails.ScrapPostId = scrapPost.ScrapPostId;
            var newScrapPostDetails = await _scrapPostDetailRepository.Add(scrapPostDetails);
            if (newScrapPostDetails == null) throw new Exception("Scrap post details not created");
        }
        return _mapper.Map<ScrapPostModel>(result);
    }
}