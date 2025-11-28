using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Feedbacks;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Feedbacks;
using GreenConnectPlatform.Data.Repositories.Transactions;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Feedbacks;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;

    public FeedbackService(IFeedbackRepository feedbackRepository, ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        _feedbackRepository = feedbackRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<FeedbackModel>> GetFeedbacksAsync(int pageNumber, int pageSize,
        Guid transactionId, bool sortByCreatAt)
    {
        var (items, totalCount) =
            await _feedbackRepository.GetFeedbackByTransactionId(pageNumber, pageSize, transactionId, sortByCreatAt);
        var feedbackModels = _mapper.Map<List<FeedbackModel>>(items);
        return new PaginatedResult<FeedbackModel>
        {
            Data = feedbackModels,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<PaginatedResult<FeedbackModel>> GetFeedbacksByUserIdAsync(int pageNumber, int pageSize,
        Guid userId, string roleName, bool sortByCreatAt)
    {
        var (items, totalCount) =
            await _feedbackRepository.GetMyFeedback(pageNumber, pageSize, userId, roleName, sortByCreatAt);
        var feedbackModels = _mapper.Map<List<FeedbackModel>>(items);
        return new PaginatedResult<FeedbackModel>
        {
            Data = feedbackModels,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<FeedbackModel> GetFeedbackByIdAsync(Guid id)
    {
        var feedback = await _feedbackRepository.GetFeedbackById(id);
        if (feedback == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy nhận xét nào");
        return _mapper.Map<FeedbackModel>(feedback);
    }

    public async Task<FeedbackModel> CreateFeedbackAsync(Guid userId, FeedbackCreateModel feedback)
    {
        var transaction = await _transactionRepository.GetByIdAsync(feedback.TransactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "TransactionId baạn điền vào không tồn tại");
        if (transaction.Status != TransactionStatus.Completed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể nhận xét khi chưa hoàn thành giao dịch");
        if (transaction.HouseholdId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không thể nhận xét vì bạn không thuộc giao dịch này");
        var newFeedback = _mapper.Map<Feedback>(feedback);
        newFeedback.FeedbackId = Guid.NewGuid();
        newFeedback.ReviewerId = userId;
        newFeedback.RevieweeId = transaction.ScrapCollectorId;
        newFeedback.CreatedAt = DateTime.UtcNow;
        await _feedbackRepository.AddAsync(newFeedback);
        return _mapper.Map<FeedbackModel>(newFeedback);
    }

    public async Task<FeedbackModel> UpdateFeedbackAsync(Guid id, Guid userId, int? rate, string? comment)
    {
        var feedback = await _feedbackRepository.GetFeedbackById(id);
        if (feedback == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Không tìm thấy nhận xét này");
        if (feedback.ReviewerId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không thể cập nhật nhận xét không phải của bạn");
        if (rate != null)
        {
            if (rate > 1 || rate < 5)
                feedback.Rate = rate.Value;
            else
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Bạn chỉ có thể rate từ 1 -5");
        }

        if (!string.IsNullOrWhiteSpace(comment)) feedback.Comment = comment;
        await _feedbackRepository.UpdateAsync(feedback);
        return _mapper.Map<FeedbackModel>(feedback);
    }

    public async Task DeleteFeedbackAsync(Guid id, Guid userId)
    {
        var feedback = await _feedbackRepository.GetFeedbackById(id);
        if (feedback == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Không tìm thấy nhận xét này");
        if (feedback.ReviewerId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không thể cập nhật nhận xét không phải của bạn");
        await _feedbackRepository.DeleteAsync(feedback);
    }
}