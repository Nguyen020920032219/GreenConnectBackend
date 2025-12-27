using GreenConnectPlatform.Business.Services.Notifications;
using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GreenConnectPlatform.Business.Services.ScrapPosts;

public class ScrapPostAuToCreateBackGround : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ScrapPostAuToCreateBackGround(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessRecurringSchedulesAsync();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ProcessRecurringSchedulesAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GreenConnectDbContext>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var vnTime = DateTime.UtcNow.AddHours(7);
            var todayDate = vnTime.Date;
            var currentTimeOnly = TimeOnly.FromDateTime(vnTime);

            var currentDayOfWeek = (int)vnTime.DayOfWeek;
            var schedules = await context.RecurringSchedules
                .Include(s => s.ScheduleDetails)
                .Where(s => s.IsActive
                            && s.DayOfWeek == currentDayOfWeek
                            && s.StartTime <= currentTimeOnly
                            && s.LastRunDate.Date < todayDate)
                .ToListAsync();
            if (schedules.Any())
            {
                foreach (var schedule in schedules)
                {
                    var newPost = new ScrapPost
                    {
                        Id = Guid.NewGuid(),
                        HouseholdId = schedule.HouseholdId,
                        Title = schedule.Title,
                        Description = schedule.Description,
                        Address = schedule.Address,
                        MustTakeAll = schedule.MustTakeAll,
                        Location = schedule.Location,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Status = PostStatus.Open,
                        TimeSlots = new List<ScrapPostTimeSlot>(),
                        ScrapPostDetails = new List<ScrapPostDetail>()
                    };
                    newPost.TimeSlots.Add(new ScrapPostTimeSlot
                    {
                        Id = Guid.NewGuid(),
                        ScrapPostId = newPost.Id,
                        SpecificDate = DateOnly.FromDateTime(todayDate),
                        StartTime = schedule.StartTime,
                        EndTime = schedule.EndTime,
                        IsBooked = false
                    });
                    foreach (var scheduleDetail in schedule.ScheduleDetails)
                        newPost.ScrapPostDetails.Add(new ScrapPostDetail
                        {
                            ScrapPostId = newPost.Id,
                            ScrapCategoryId = scheduleDetail.ScrapCategoryId,
                            Quantity = scheduleDetail.Quantity,
                            AmountDescription = scheduleDetail.AmountDescription,
                            Unit = scheduleDetail.Unit,
                            Type = scheduleDetail.Type,
                            Status = PostDetailStatus.Available
                        });
                    context.ScrapPosts.Add(newPost);
                    schedule.LastRunDate = vnTime;
                }

                await context.SaveChangesAsync();
                var title = "Bài đăng đã được tạo tự động từ lịch trình";


                var body = $"Bài đăng '{schedules.First().Title}' đã được tạo tự động cho ngày {todayDate:dd/MM/yyyy}.";

                var data = new Dictionary<string, string>
                {
                    { "type", "Recurring schedule" },
                    { "id", schedules.First().Id.ToString() }
                };
                await notificationService.SendNotificationAsync(schedules.First().HouseholdId, title, body, data);
            }
        }
    }
}