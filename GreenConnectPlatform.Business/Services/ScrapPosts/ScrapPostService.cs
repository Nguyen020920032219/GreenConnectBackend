using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Models.ScrapPostTimeSlots;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using GreenConnectPlatform.Data.Repositories.ScrapPosts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Business.Services.ScrapPosts;

public class ScrapPostService : IScrapPostService
{
    private readonly IScrapCategoryRepository _categoryRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly GeometryFactory _geometryFactory;
    private readonly IMapper _mapper;
    private readonly IScrapPostRepository _postRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly UserManager<User> _userManager;

    public ScrapPostService(
        IScrapPostRepository postRepository,
        IScrapCategoryRepository categoryRepository,
        IProfileRepository profileRepository,
        IFileStorageService fileStorageService,
        UserManager<User> userManager,
        IMapper mapper)
    {
        _postRepository = postRepository;
        _categoryRepository = categoryRepository;
        _profileRepository = profileRepository;
        _fileStorageService = fileStorageService;
        _userManager = userManager;
        _mapper = mapper;
        _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
    }

    public async Task<PaginatedResult<ScrapPostOverralModel>> SearchPostsAsync(
        string roleName,
        int pageNumber, int pageSize, Guid? categoryId, PostStatus? status,
        bool sortByLocation, bool sortByCreateAt, Guid currentUserId)
    {
        Point? userLocation = null;
        if (sortByLocation)
        {
            var profile = await _profileRepository.GetByIdAsync(currentUserId);
            userLocation = profile?.Location;
        }

        var (items, totalCount) = await _postRepository.SearchAsync(
            roleName,
            categoryId, status, userLocation,
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
        var roles = await _userManager.GetRolesAsync(post.Household);

        var scrapPostDetails = new List<ScrapPostDetailModel>();

        if (post.ScrapPostDetails != null)
            foreach (var detail in post.ScrapPostDetails)
            {
                string? detailImageUrl = null;
                if (!string.IsNullOrEmpty(detail.ImageUrl))
                    detailImageUrl = await _fileStorageService.GetReadSignedUrlAsync(detail.ImageUrl);

                scrapPostDetails.Add(new ScrapPostDetailModel
                {
                    ScrapCategoryId = detail.ScrapCategoryId,
                    ScrapCategory = new ScrapCategoryModel
                    {
                        Id = detail.ScrapCategory.Id,
                        Name = detail.ScrapCategory.Name,
                        ImageUrl = detail.ScrapCategory.ImageUrl
                    },
                    AmountDescription = detail.AmountDescription,
                    ImageUrl = detailImageUrl,
                    Status = detail.Status,
                    Type = detail.Type
                });
            }

        var timeSlots = new List<ScrapPostTimeSlotModel>();
        foreach (var slot in post.TimeSlots)
            timeSlots.Add(new ScrapPostTimeSlotModel
            {
                Id = slot.Id,
                ScrapPostId = post.Id,
                IsBooked = slot.IsBooked,
                SpecificDate = slot.SpecificDate,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime
            });
        var postModel = new ScrapPostModel
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Address = post.Address,
            Status = post.Status,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            HouseholdId = post.HouseholdId,
            Household = new UserViewModel
            {
                Id = post.Household.Id,
                FullName = post.Household.FullName,
                PhoneNumber = post.Household.PhoneNumber,
                AvatarUrl = post.Household.Profile.AvatarUrl != null
                    ? await _fileStorageService.GetReadSignedUrlAsync(post.Household.Profile.AvatarUrl)
                    : null,
                PointBalance = post.Household.Profile.PointBalance,
                Rank = post.Household.Profile.Rank.ToString(),
                Roles = roles
            },
            MustTakeAll = post.MustTakeAll,
            ScrapPostDetails = scrapPostDetails,
            TimeSlots = timeSlots
        };
        return postModel;
    }

    public async Task<ScrapPostModel> CreateAsync(Guid householdId, ScrapPostCreateModel request)
    {
        var categoryIds = request.ScrapPostDetails.Select(d => d.ScrapCategoryId).Distinct().ToList();
        foreach (var catId in categoryIds)
            if (await _categoryRepository.GetByIdAsync(catId) == null)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    $"Loại ve chai ID {catId} không hợp lệ.");
        var allCategoryIds = request.ScrapPostDetails.Select(d => d.ScrapCategoryId).ToList();
        if (allCategoryIds.Count != categoryIds.Count)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không được lặp lại loại ve chai trong chi tiết ve chai.");
        if (!request.ScrapPostDetails.Any())
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Phải có ít nhất một chi tiết ve chai trong bài đăng.");
        if (!request.ScrapPostTimeSlots.Any())
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Phải có ít nhất một khung thời gian trong bài đăng.");
        var post = _mapper.Map<ScrapPost>(request);
        post.Id = Guid.NewGuid();
        post.HouseholdId = householdId;
        post.Status = PostStatus.Open;
        post.CreatedAt = DateTime.Now;
        post.UpdatedAt = DateTime.Now;

        if (request.Location != null && request.Location.Latitude.HasValue && request.Location.Longitude.HasValue)
        {
            if (request.Location.Latitude < -90 || request.Location.Latitude > 90)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Vĩ độ (Latitude) không hợp lệ. Phải nằm trong khoảng từ -90 đến 90.");

            if (request.Location.Longitude < -180 || request.Location.Longitude > 180)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Kinh độ (Longitude) không hợp lệ. Phải nằm trong khoảng từ -180 đến 180.");
            post.Location =
                _geometryFactory.CreatePoint(new Coordinate(request.Location.Longitude.Value,
                    request.Location.Latitude.Value));
        }

        foreach (var slots in post.TimeSlots)
        {
            if (slots.SpecificDate < DateOnly.FromDateTime(DateTime.Now))
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Khung thời gian không thể có ngày cụ thể trong quá khứ.");
            if (slots.StartTime >= slots.EndTime)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Thời gian bắt đầu phải trước thời gian kết thúc trong khung thời gian.");
            if (slots.SpecificDate == DateOnly.FromDateTime(DateTime.Now))
            {
                if (slots.StartTime < TimeOnly.FromDateTime(DateTime.Now))
                    throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                        "Nếu khung thời gian có ngày cụ thể là hôm nay, thời gian bắt đầu phải lớn hơn thời gian hiện tại.");
            }
            slots.Id = Guid.NewGuid();
            slots.ScrapPostId = post.Id;
            slots.IsBooked = false;
        }

        await _postRepository.AddAsync(post);

        return await GetByIdAsync(post.Id);
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
        post.UpdatedAt = DateTime.Now;

        if (request.Location != null && request.Location.Latitude.HasValue && request.Location.Longitude.HasValue)
        {
            if (request.Location.Latitude < -90 || request.Location.Latitude > 90)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Vĩ độ (Latitude) không hợp lệ. Phải nằm trong khoảng từ -90 đến 90.");

            if (request.Location.Longitude < -180 || request.Location.Longitude > 180)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Kinh độ (Longitude) không hợp lệ. Phải nằm trong khoảng từ -180 đến 180.");
            post.Location =
                _geometryFactory.CreatePoint(new Coordinate(request.Location.Longitude.Value,
                    request.Location.Latitude.Value));
            post.Location =
                _geometryFactory.CreatePoint(new Coordinate(request.Location.Longitude.Value,
                    request.Location.Latitude.Value));
        }

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

        post.UpdatedAt = DateTime.Now;
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

    public async Task UpdateDetailAsync(Guid householdId, Guid postId, Guid categoryId,
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
        if (detailRequest.Type != null)
            detail.Type = detailRequest.Type.Value;
        _mapper.Map(detailRequest, detail);
        await _postRepository.UpdateAsync(post);
    }

    public async Task DeleteDetailAsync(Guid userId, Guid postId, Guid categoryId, string userRole)
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

    public async Task AddTimeSlotAsync(Guid householdId, Guid postId, ScrapPostTimeSlotCreateModel timeSlotRequest)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");
        if (post.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Không có quyền.");
        if (post.Status == PostStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                " thể thêm khung thời gian khi bài đăng đã hoàn thành.");
        if(timeSlotRequest.SpecificDate < DateOnly.FromDateTime(DateTime.Now))
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Khung thời gian không thể có ngày cụ thể trong quá khứ.");
        if (timeSlotRequest.StartTime >= timeSlotRequest.EndTime)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Thời gian bắt đầu phải trước thời gian kết thúc trong khung thời gian.");
        if (timeSlotRequest.SpecificDate == DateOnly.FromDateTime(DateTime.Now))
        {
            if(timeSlotRequest.StartTime < TimeOnly.FromDateTime(DateTime.Now))
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Nếu khung thời gian có ngày cụ thể là hôm nay, thời gian bắt đầu phải lớn hơn thời gian hiện tại.");
        }
        var timeSlot = _mapper.Map<ScrapPostTimeSlot>(timeSlotRequest);
        timeSlot.ScrapPostId = postId;
        post.TimeSlots.Add(timeSlot);
        await _postRepository.UpdateAsync(post);
    }

    public async Task UpdateTimeSlotAsync(Guid householdId, Guid postId, Guid timeSlotId, DateOnly? specificDate,
        TimeOnly? startTime,
        TimeOnly? endTime)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");
        if (post.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Không có quyền.");
        if(specificDate < DateOnly.FromDateTime(DateTime.Now))
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Khung thời gian không thể có ngày cụ thể trong quá khứ.");
        if (startTime >= endTime)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Thời gian bắt đầu phải trước thời gian kết thúc trong khung thời gian.");
        if (specificDate == DateOnly.FromDateTime(DateTime.Now))
        {
            if(startTime < TimeOnly.FromDateTime(DateTime.Now))
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Nếu khung thời gian có ngày cụ thể là hôm nay, thời gian bắt đầu phải lớn hơn thời gian hiện tại.");
        }
        var timeSlot = post.TimeSlots.FirstOrDefault(ts => ts.Id == timeSlotId);
        if (timeSlot == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Khung thời gian không tồn tại.");
        if (specificDate.HasValue)
            timeSlot.SpecificDate = specificDate.Value;
        if (startTime.HasValue)
            timeSlot.StartTime = startTime.Value;
        if (endTime.HasValue)
            timeSlot.EndTime = endTime.Value;
        await _postRepository.UpdateAsync(post);
    }

    public async Task DeleteTimeSlotAsync(Guid householdId, Guid postId, Guid timeSlotId)
    {
        var post = await _postRepository.GetByIdWithDetailsAsync(postId);
        if (post == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bài đăng không tồn tại.");
        if (post.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Không có quyền.");
        var timeSlot = post.TimeSlots.FirstOrDefault(ts => ts.Id == timeSlotId);
        if (timeSlot == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Khung thời gian không tồn tại.");
        if (timeSlot.IsBooked)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không thể xóa khung thời gian đã được đặt.");
        post.TimeSlots.Remove(timeSlot);
        await _postRepository.UpdateAsync(post);
    }
}