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
    private readonly IMapper _mapper;
    private readonly IScrapPostRepository _postRepository;
    private readonly IScrapCategoryRepository _repository;

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

    public async Task<ScrapCategoryModel> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Danh mục không tồn tại.");

        return _mapper.Map<ScrapCategoryModel>(entity);
    }

    public async Task<ScrapCategoryModel> CreateAsync(string categoryName, string imageUrl)
    {
        var exists =
            (await _repository.FindAsync(c => c.Name.ToLower() == categoryName.ToLower())).Any();
        if (exists)
            throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409",
                $"Danh mục '{categoryName}' đã tồn tại.");
        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Tên của loại ve chai không thể để trống");
        var entity = new ScrapCategory
        {
            Id = Guid.NewGuid(),
            Name = categoryName,
            ImageUrl = imageUrl
        };

        await _repository.AddAsync(entity);
        return _mapper.Map<ScrapCategoryModel>(entity);
    }



    public async Task<ScrapCategoryModel> UpdateAsync(Guid id, string? categoryName, string? imageUrl)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Danh mục không tồn tại.");

        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            if (!string.Equals(entity.Name, categoryName, StringComparison.OrdinalIgnoreCase))
            {
                var exists = (await _repository.FindAsync(c => c.Name.ToLower() == categoryName.ToLower()))
                    .Any();
                if (exists)
                    throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409",
                        $"Danh mục '{categoryName}' đã tồn tại.");
            }

            entity.Name = categoryName;
        }

        if (!string.IsNullOrWhiteSpace(imageUrl)) entity.ImageUrl = imageUrl;
        await _repository.UpdateAsync(entity);
        return _mapper.Map<ScrapCategoryModel>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Danh mục không tồn tại.");

        var isInUse = await _postRepository.IsCategoryInUseAsync(id);
        if (isInUse)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không thể xóa danh mục này vì đang có bài đăng sử dụng nó. Vui lòng xóa các bài đăng liên quan trước.");

        await _repository.DeleteAsync(entity);
    }
}

