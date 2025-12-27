using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.RecurringScheduleDetails;
using GreenConnectPlatform.Business.Models.RecurringSchedules;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.RecurringScheduleDetails;
using GreenConnectPlatform.Data.Repositories.RecurringSchedules;
using Microsoft.AspNetCore.Http;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Business.Services.RecurringSchedules;

public class RecurringScheduleService : IRecurringScheduleService
{
    private readonly GeometryFactory _geometryFactory;
    private readonly IMapper _mapper;
    private readonly IRecurringScheduleDetailRepository _recurringScheduleDetailRepository;
    private readonly IRecurringScheduleRepository _recurringScheduleRepository;

    public RecurringScheduleService(IRecurringScheduleRepository recurringScheduleRepository,
        IRecurringScheduleDetailRepository recurringScheduleDetailRepository,
        IMapper mapper)
    {
        _recurringScheduleRepository = recurringScheduleRepository;
        _recurringScheduleDetailRepository = recurringScheduleDetailRepository;
        _mapper = mapper;
        _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
    }

    public async Task<PaginatedResult<RecurringScheduleOverallModel>> GetPagedRecurringSchedulesAsync(Guid userId,
        int pageNumber,
        int pageSize, bool sortByCreatedAt)
    {
        var (items, totalCount) =
            await _recurringScheduleRepository.GetPagedRecurringSchedulesAsync(userId, pageNumber, pageSize,
                sortByCreatedAt);
        var models = _mapper.Map<List<RecurringScheduleOverallModel>>(items);
        return new PaginatedResult<RecurringScheduleOverallModel>
        {
            Data = models,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<RecurringScheduleModel> GetRecurringScheduleByIdAsync(Guid id)
    {
        var recurringSchedule = await _recurringScheduleRepository.GetRecurringScheduleByIdAsync(id);
        if (recurringSchedule == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Recurring schedule không tồn tại");
        return _mapper.Map<RecurringScheduleModel>(recurringSchedule);
    }

    public async Task<RecurringScheduleModel> CreateRecurringScheduleAsync(Guid userId,
        RecurringScheduleCreateModel model)
    {
        if (model.DayOfWeek < 0 || model.DayOfWeek > 6)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "DayOfWeek phải nằm trong khoảng từ 0 đến 6 tương với thứ 2 đến chủ nhật và bắt đầu với 0 là chủ nhật và 1 là thứ 2");
        if (model.StartTime >= model.EndTime)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "StartTime phải nhỏ hơn EndTime");
        var recurringSchedule = _mapper.Map<RecurringSchedule>(model);
        recurringSchedule.Id = Guid.NewGuid();
        recurringSchedule.HouseholdId = userId;
        recurringSchedule.IsActive = true;
        recurringSchedule.CreatedAt = DateTime.Now;

        var vnNow = DateTime.Now;
        var currentDayOfWeek = (int)vnNow.DayOfWeek;
        var currentTimeOnly = TimeOnly.FromDateTime(vnNow);
        if (model.DayOfWeek == currentDayOfWeek && model.StartTime > currentTimeOnly)
            recurringSchedule.LastRunDate = vnNow.AddDays(-1);
        else
            recurringSchedule.LastRunDate = vnNow;
        if (model.Location != null && model.Location.Latitude.HasValue && model.Location.Longitude.HasValue)
        {
            if (model.Location.Latitude < -90 || model.Location.Latitude > 90)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Vĩ độ (Latitude) không hợp lệ. Phải nằm trong khoảng từ -90 đến 90.");

            if (model.Location.Longitude < -180 || model.Location.Longitude > 180)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Kinh độ (Longitude) không hợp lệ. Phải nằm trong khoảng từ -180 đến 180.");
            recurringSchedule.Location
                = _geometryFactory.CreatePoint(new Coordinate(model.Location.Longitude.Value,
                    model.Location.Latitude.Value));
        }

        foreach (var detail in recurringSchedule.ScheduleDetails)
        {
            detail.Id = Guid.NewGuid();
            detail.RecurringScheduleId = recurringSchedule.Id;
        }

        await _recurringScheduleRepository.AddAsync(recurringSchedule);
        return await GetRecurringScheduleByIdAsync(recurringSchedule.Id);
    }

    public async Task<RecurringScheduleModel> UpdateRecurringScheduleAsync(Guid id, Guid userId,
        RecurringScheduleUpdateModel model)
    {
        var recurringSchedule = await _recurringScheduleRepository.GetRecurringScheduleByIdAsync(id);
        if (recurringSchedule == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Recurring schedule không tồn tại");
        if (recurringSchedule.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không có quyền cập nhật lịch trình định kỳ này");
        if (model.DayOfWeek <= 2 || model.DayOfWeek >= 8)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "DayOfWeek phải nằm trong khoảng từ thứ 2 đến chủ nhật");
        if (model.StartTime > recurringSchedule.EndTime)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Ngày bắt đầu mới phải nhỏ ngày kết thúc cũ");
        if (model.EndTime < recurringSchedule.StartTime)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Ngày kết thúc mới phải lớn ngày bắt đầu cũ");
        if (model.StartTime != null && model.EndTime != null && model.StartTime > model.EndTime)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Ngày bắt đầu mới phải nhỏ ngày kết thúc mới");
        if (model.DayOfWeek == null)
            model.DayOfWeek = recurringSchedule.DayOfWeek;
        if (model.StartTime != null)
            recurringSchedule.StartTime = model.StartTime.Value;
        if (model.EndTime != null)
            recurringSchedule.EndTime = model.EndTime.Value;
        if (!string.IsNullOrEmpty(model.Address))
        {
            recurringSchedule.Address = model.Address;
            if (model.Location != null && model.Location.Latitude.HasValue && model.Location.Longitude.HasValue)
            {
                if (model.Location.Latitude < -90 || model.Location.Latitude > 90)
                    throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                        "Vĩ độ (Latitude) không hợp lệ. Phải nằm trong khoảng từ -90 đến 90.");

                if (model.Location.Longitude < -180 || model.Location.Longitude > 180)
                    throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                        "Kinh độ (Longitude) không hợp lệ. Phải nằm trong khoảng từ -180 đến 180.");
                recurringSchedule.Location
                    = _geometryFactory.CreatePoint(new Coordinate(model.Location.Longitude.Value,
                        model.Location.Latitude.Value));
            }
            else
            {
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Nếu bạn cập nhật địa chỉ thì phải cập nhật luôn cả vị trí");
            }
        }

        _mapper.Map(model, recurringSchedule);
        await _recurringScheduleRepository.UpdateAsync(recurringSchedule);
        return await GetRecurringScheduleByIdAsync(id);
    }

    public async Task ToggleRecurringScheduleAsync(Guid id, Guid userId)
    {
        var recurringSchedule = await _recurringScheduleRepository.GetRecurringScheduleByIdAsync(id);
        if (recurringSchedule == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Recurring schedule không tồn tại");
        if (recurringSchedule.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không có quyền cập nhật lịch trình định kỳ này");
        if (recurringSchedule.IsActive)
            recurringSchedule.IsActive = false;
        else
            recurringSchedule.IsActive = true;
        await _recurringScheduleRepository.UpdateAsync(recurringSchedule);
    }

    public async Task<RecurringScheduleDetailModel> GetRecurringScheduleDetailAsync(Guid id)
    {
        var recurringScheduleDetail = await _recurringScheduleDetailRepository.GetByIdAsync(id);
        if (recurringScheduleDetail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Recurring schedule detail không tồn tại");
        return _mapper.Map<RecurringScheduleDetailModel>(recurringScheduleDetail);
    }

    public async Task<RecurringScheduleDetailModel> AddRecurringScheduleDetailAsync(Guid recurringScheduleId,
        Guid userId, RecurringScheduleDetailCreateModel model)
    {
        var recurringSchedule = await _recurringScheduleRepository.GetRecurringScheduleByIdAsync(recurringScheduleId);
        if (recurringSchedule == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Recurring schedule không tồn tại");
        if (recurringSchedule.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không có quyền thêm mới chi tiết cho lịch trình định kỳ này");
        var recurringScheduleDetail = _mapper.Map<RecurringScheduleDetail>(model);
        recurringScheduleDetail.Id = Guid.NewGuid();
        recurringScheduleDetail.RecurringScheduleId = recurringScheduleId;
        await _recurringScheduleDetailRepository.AddAsync(recurringScheduleDetail);
        return await GetRecurringScheduleDetailAsync(recurringScheduleDetail.Id);
    }

    public async Task<RecurringScheduleDetailModel> UpdateRecurringScheduleDetailAsync(Guid recurringScheduleId,
        Guid id, Guid userId, double? quantity, string? amountDescription,
        ItemTransactionType? type)
    {
        var recurringScheduleDetail =
            await _recurringScheduleDetailRepository.GetByRecurringScheduleIdAsync(id, recurringScheduleId);
        if (recurringScheduleDetail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Recurring schedule detail không tồn tại");
        if (recurringScheduleDetail.RecurringSchedule.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không có quyền cập nhật chi tiết cho lịch trình định kỳ này");
        if (quantity != null)
            recurringScheduleDetail.Quantity = quantity.Value;
        if (!string.IsNullOrWhiteSpace(amountDescription))
            recurringScheduleDetail.AmountDescription = amountDescription;
        if (type != null)
            recurringScheduleDetail.Type = type.Value;
        await _recurringScheduleDetailRepository.UpdateAsync(recurringScheduleDetail);
        return await GetRecurringScheduleDetailAsync(id);
    }

    public async Task DeleteRecurringScheduleDetailAsync(Guid recurringScheduleId, Guid id, Guid userId)
    {
        var recurringScheduleDetail =
            await _recurringScheduleDetailRepository.GetByRecurringScheduleIdAsync(id, recurringScheduleId);
        if (recurringScheduleDetail == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Recurring schedule detail không tồn tại");
        if (recurringScheduleDetail.RecurringSchedule.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không có quyền xóa chi tiết cho lịch trình định kỳ này");
        await _recurringScheduleDetailRepository.DeleteAsync(recurringScheduleDetail);
    }
}