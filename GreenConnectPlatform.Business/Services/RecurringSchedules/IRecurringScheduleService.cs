using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.RecurringScheduleDetails;
using GreenConnectPlatform.Business.Models.RecurringSchedules;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.RecurringSchedules;

public interface IRecurringScheduleService
{
    Task<PaginatedResult<RecurringScheduleOverallModel>> GetPagedRecurringSchedulesAsync(int pageNumber, int pageSize,
        bool sortByCreatedAt);

    Task<RecurringScheduleModel> GetRecurringScheduleByIdAsync(Guid id);
    Task<RecurringScheduleModel> CreateRecurringScheduleAsync(Guid userId, RecurringScheduleCreateModel model);
    Task<RecurringScheduleModel> UpdateRecurringScheduleAsync(Guid id, Guid userId, RecurringScheduleUpdateModel model);
    Task ToggleRecurringScheduleAsync(Guid id, Guid userId);
    Task<RecurringScheduleDetailModel> GetRecurringScheduleDetailAsync(Guid id);

    Task<RecurringScheduleDetailModel> AddRecurringScheduleDetailAsync(Guid recurringScheduleId, Guid userId,
        RecurringScheduleDetailCreateModel model);

    Task<RecurringScheduleDetailModel> UpdateRecurringScheduleDetailAsync(Guid recurringScheduleId, Guid id,
        Guid userId, double? quantity, string? amountDescription, ItemTransactionType? type);

    Task DeleteRecurringScheduleDetailAsync(Guid recurringScheduleId, Guid id, Guid userId);
}