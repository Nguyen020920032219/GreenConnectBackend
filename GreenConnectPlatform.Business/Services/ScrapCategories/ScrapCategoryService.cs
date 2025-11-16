using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Business.Services.ScrapCategories;

public class ScrapCategoryService : IScrapCategoryService
{
    private readonly IScrapCategoryRepository _scrapCategoryRepository;
    private readonly IMapper _mapper;

    public ScrapCategoryService(IScrapCategoryRepository scrapCategoryRepository, IMapper mapper)
    {
        _scrapCategoryRepository = scrapCategoryRepository;
        _mapper = mapper;
    }
    
    public async Task<PaginatedResult<ScrapCategoryModel>> GetScrapCategories(int pageNumber, int pageSize, string? categoryName)
    {
        var query = _scrapCategoryRepository.DbSet()
            .AsQueryable()
            .AsNoTracking();
        if(categoryName != null)
            query = query.Where(x => x.CategoryName.ToLower().Contains(categoryName.ToLower()));
        var totalRecords = await query.CountAsync();
        var scrapCategories = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var categoryModel = _mapper.Map<List<ScrapCategoryModel>>(scrapCategories);
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        var paginationModel = new PaginationModel
        {
            TotalRecords = totalRecords,
            CurrentPage = pageNumber,
            TotalPages = totalPages,
            NextPage = pageNumber < totalPages ? pageNumber + 1 : null,
            PrevPage = pageNumber > 1 ? pageNumber - 1 : null,
        };
        return new PaginatedResult<ScrapCategoryModel>
        {
            Data = categoryModel,
            Pagination = paginationModel
        };
    }

    public async Task<ScrapCategoryModel> GetScrapCategory(int scrapCategoryId)
    {
        var scrapCategory = await _scrapCategoryRepository.DbSet()
            .FirstOrDefaultAsync(c => c.ScrapCategoryId == scrapCategoryId);
        if (scrapCategory == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Scrap category does not exist");
        return _mapper.Map<ScrapCategoryModel>(scrapCategory);
    }
}