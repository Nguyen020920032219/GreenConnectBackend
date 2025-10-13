using AutoMapper;
using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Bussiness.Services.ScrapPosts;

public class ScrapPostService : IScrapPostService
{
    private readonly IScrapPostRepository _scrapPostRepository;
    private readonly IMapper _mapper;
    public ScrapPostService(IScrapPostRepository scrapPostRepository, IMapper mapper)
    {
        _scrapPostRepository = scrapPostRepository;
        _mapper = mapper;
    }
    public async Task<List<ScrapPostModel>> GetPosts(int pageNumber, int pageSize)
    {
        var scrapPosts = await _scrapPostRepository.DbSet()
            .Where(s => s.Status == PostStatus.Open)
            .OrderByDescending(s => s.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return _mapper.Map<List<ScrapPostModel>>(scrapPosts);
    }

    public async Task<ScrapPostModel> GetPost(Guid scrapPostId)
    {
        var scrapPost = await _scrapPostRepository.DbSet().FirstOrDefaultAsync(s => s.ScrapPostId == scrapPostId);
        if (scrapPost == null)
            throw new KeyNotFoundException("Scrap post not found");
        return _mapper.Map<ScrapPostModel>(scrapPost);
    }

    public async Task<ScrapPostModel> CreateScrapPost(ScrapPostRequest scrapPostRequest)
    {
        scrapPostRequest.HouseholdId = new Guid("a1b2c3d4-e5f6-7788-9900-aabbccddeeff");
        var scrapPost = _mapper.Map<ScrapPost>(scrapPostRequest);
        var result = await _scrapPostRepository.Add(scrapPost);
        if(result == null) throw new Exception("Scrap post not created");
        return _mapper.Map<ScrapPostModel>(result);
    }
}