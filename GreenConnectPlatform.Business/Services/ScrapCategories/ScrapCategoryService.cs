using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.ScrapCategories;

public class ScrapCategoryService : IScrapCategoryService
{
    private readonly IScrapCategoryRepository _repository;
    private readonly IScrapPostRepository _postRepository;
    private readonly IMapper _mapper;

    public ScrapCategoryService(IScrapCategoryRepository repository, IScrapPostRepository postRepository,
        IMapper mapper)
    {
        _repository = repository;
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ScrapCategoryModel>> GetListAsync(int pageNumber, int pageSize,
        string? searchName)
    {
        var (entities, totalRecords) = await _repository.SearchAndPaginateAsync(searchName, pageNumber, pageSize);

        var data = _mapper.Map<List<ScrapCategoryModel>>(entities);

        var pagination = new PaginationModel(totalRecords, pageNumber, pageSize);

        return new PaginatedResult<ScrapCategoryModel>
        {
            Data = data,
            Pagination = pagination
        };
    }

    public async Task<ScrapCategoryModel> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Danh mục không tồn tại.");

        return _mapper.Map<ScrapCategoryModel>(entity);
    }

    public async Task<ScrapCategoryModel> CreateAsync(ScrapCategoryModel request)
    {
        var exists =
            (await _repository.FindAsync(c => c.CategoryName.ToLower() == request.CategoryName.ToLower())).Any();
        if (exists)
            throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409",
                $"Danh mục '{request.CategoryName}' đã tồn tại.");

        var entity = _mapper.Map<ScrapCategory>(request);
        entity.ScrapCategoryId = 0;

        await _repository.AddAsync(entity);
        return _mapper.Map<ScrapCategoryModel>(entity);
    }

    public async Task<ScrapCategoryModel> UpdateAsync(int id, ScrapCategoryModel request)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Danh mục không tồn tại.");

        if (!string.Equals(entity.CategoryName, request.CategoryName, StringComparison.OrdinalIgnoreCase))
        {
            var exists = (await _repository.FindAsync(c => c.CategoryName.ToLower() == request.CategoryName.ToLower()))
                .Any();
            if (exists)
                throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409",
                    $"Danh mục '{request.CategoryName}' đã tồn tại.");
        }

        entity.CategoryName = request.CategoryName;
        entity.Description = request.Description;

        await _repository.UpdateAsync(entity);
        return _mapper.Map<ScrapCategoryModel>(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Danh mục không tồn tại.");

        var isInUse = await _postRepository.IsCategoryInUseAsync(id);
        if (isInUse)
        {
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không thể xóa danh mục này vì đang có bài đăng sử dụng nó. Vui lòng xóa các bài đăng liên quan trước.");
        }

        await _repository.DeleteAsync(entity);
    }
}