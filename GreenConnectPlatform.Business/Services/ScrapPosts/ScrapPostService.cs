using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using Microsoft.AspNetCore.Http;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Business.Services.ScrapPosts;

public class ScrapPostService : IScrapPostService
{
    private readonly IScrapCategoryRepository _categoryRepository;
    private readonly GeometryFactory _geometryFactory;
    private readonly IMapper _mapper;
    private readonly IScrapPostRepository _postRepository;
    private readonly IProfileRepository _profileRepository;

    public ScrapPostService(
        IScrapPostRepository postRepository,
        IScrapCategoryRepository categoryRepository,
        IProfileRepository profileRepository,
        IMapper mapper)
    {
        _postRepository = postRepository;
        _categoryRepository = categoryRepository;
        _profileRepository = profileRepository;
        _mapper = mapper;
        _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
    }

    public async Task<PaginatedResult<ScrapPostOverralModel>> SearchPostsAsync(
        int pageNumber, int pageSize, string? categoryName, PostStatus? status,
        bool sortByLocation, bool sortByCreateAt, Guid currentUserId)
    {
        Point? userLocation = null;
        if (sortByLocation)
        {
            var profile = await _profileRepository.GetByIdAsync(currentUserId);
            userLocation = profile?.Location;
        }

        var (items, totalCount) = await _postRepository.SearchAsync(
            categoryName, status, userLocation,
            sortByLocation, sortByCreateAt, false,
            pageNumber, pageSize);

        var data = _mapper.Map<List<ScrapPostOverralModel>>(items);

        return new PaginatedResult<ScrapPostOverralModel>
        {
            Data = data,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<PaginatedResult<ScrapPostOverralModel>> GetMyPostsAsync(
        int pageNumber, int pageSize, string? title, PostStatus? status, Guid householdId)
    {
        var (items, totalCount) = await _postRepository.GetByHouseholdAsync(
            householdId, title, status, pageNumber, pageSize);

        var data = _mapper.Map<List<ScrapPostOverralModel>>(items);

        return new PaginatedResult<ScrapPostOverralModel>
        {
            Data = data,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<ScrapPostModel> GetByIdAsync(Guid id)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(id);
        if (post == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");

        return _mapper.Map<ScrapPostModel>(post);
    }

    public async Task<ScrapPostModel> CreateAsync(Guid householdId, ScrapPostCreateModel request)
    {
        var categoryIds = request.ScrapPostDetails.Select(d => d.ScrapCategoryId).Distinct().ToList();
        foreach (var catId in categoryIds)
            if (await _categoryRepository.GetByIdAsync(catId) == null)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    $"Loại ve chai ID {catId} không hợp lệ.");

        var post = _mapper.Map<ScrapPost>(request);
        post.ScrapPostId = Guid.NewGuid();
        post.HouseholdId = householdId;
        post.Status = PostStatus.Open;
        post.CreatedAt = DateTime.UtcNow;

        if (request.Location != null && request.Location.Latitude.HasValue && request.Location.Longitude.HasValue)
            post.Location =
                _geometryFactory.CreatePoint(new Coordinate(request.Location.Longitude.Value,
                    request.Location.Latitude.Value));

        await _postRepository.AddAsync(post);

        return await GetByIdAsync(post.ScrapPostId);
    }

    public async Task<ScrapPostModel> UpdateAsync(Guid householdId, Guid postId, ScrapPostUpdateModel request)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");

        if (post.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền chỉnh sửa bài này.");

        if (post.Status != PostStatus.Open)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Chỉ có thể chỉnh sửa bài đăng khi trạng thái là Open.");

        _mapper.Map(request, post);
        post.UpdatedAt = DateTime.UtcNow;

        if (request.Location != null && request.Location.Latitude.HasValue && request.Location.Longitude.HasValue)
            post.Location =
                _geometryFactory.CreatePoint(new Coordinate(request.Location.Longitude.Value,
                    request.Location.Latitude.Value));

        await _postRepository.UpdateAsync(post);
        return _mapper.Map<ScrapPostModel>(post);
    }

    public async Task ToggleStatusAsync(Guid userId, Guid postId, string userRole)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");

        if (userRole != "Admin" && post.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (post.Status == PostStatus.Open) post.Status = PostStatus.Canceled;
        else if (post.Status == PostStatus.Canceled) post.Status = PostStatus.Open;
        else
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không thể đổi trạng thái bài đăng này (đang xử lý hoặc đã xong).");

        post.UpdatedAt = DateTime.UtcNow;
        await _postRepository.UpdateAsync(post);
    }

    public async Task AddDetailAsync(Guid householdId, Guid postId, ScrapPostDetailCreateModel detailRequest)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");
        if (post.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Không có quyền.");

        if (post.Status == PostStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bài đăng đã hoàn thành, không thể thêm.");

        if (post.ScrapPostDetails.Any(d => d.ScrapCategoryId == detailRequest.ScrapCategoryId))
            throw new ApiExceptionModel(StatusCodes.Status409Conflict, "409",
                "Loại ve chai này đã có trong danh sách.");

        var detail = _mapper.Map<ScrapPostDetail>(detailRequest);
        detail.ScrapPostId = postId;
        post.ScrapPostDetails.Add(detail);

        await _postRepository.UpdateAsync(post);
    }

    public async Task UpdateDetailAsync(Guid householdId, Guid postId, int categoryId,
        ScrapPostDetailUpdateModel detailRequest)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");
        if (post.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Không có quyền.");

        var detail = post.ScrapPostDetails.FirstOrDefault(d => d.ScrapCategoryId == categoryId);
        if (detail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Chi tiết ve chai không tồn tại.");

        if (detail.Status != PostDetailStatus.Available)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không thể sửa món hàng đã được đặt/thu gom.");

        _mapper.Map(detailRequest, detail);
        await _postRepository.UpdateAsync(post);
    }

    public async Task DeleteDetailAsync(Guid userId, Guid postId, int categoryId, string userRole)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");

        if (userRole != "Admin" && post.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Không có quyền.");

        var detail = post.ScrapPostDetails.FirstOrDefault(d => d.ScrapCategoryId == categoryId);
        if (detail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Chi tiết không tồn tại.");

        if (detail.Status != PostDetailStatus.Available)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không thể xóa món hàng đã được đặt/thu gom.");

        post.ScrapPostDetails.Remove(detail);
        await _postRepository.UpdateAsync(post);
    }
}