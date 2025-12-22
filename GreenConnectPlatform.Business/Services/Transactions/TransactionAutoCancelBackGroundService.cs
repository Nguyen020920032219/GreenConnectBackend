using GreenConnectPlatform.Business.Services.Notifications;
using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GreenConnectPlatform.Business.Services.Transactions;

public class TransactionAutoCancelBackGroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TransactionAutoCancelBackGroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
                await ProcessOverdueTransactions();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ProcessOverdueTransactions()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GreenConnectDbContext>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var now = DateTime.UtcNow;
            var overdueTransactions = await context.Transactions
                .Where(t => t.Status == TransactionStatus.Scheduled
                            && t.TimeSlot.SpecificDate.ToDateTime(t.TimeSlot.EndTime).AddMinutes(30) < now)
                .ToListAsync();

            if (overdueTransactions.Any())
                foreach (var transaction in overdueTransactions)
                {
                    transaction.Status = TransactionStatus.CanceledBySystem;
                    transaction.UpdatedAt = now;
                }

            await context.SaveChangesAsync();
            foreach (var transaction in overdueTransactions)
            {
                var title = "Giao dịch đã bị hủy tự động";
                var endTimeStr = transaction.TimeSlot.EndTime.ToString("HH:mm"); 
                var dateStr = transaction.TimeSlot.SpecificDate.ToString("dd/MM/yyyy");
                        
                var body = $"Giao dịch ngày {dateStr} lúc {endTimeStr} đã bị hệ thống hủy do quá hạn check-in.";
                        
                var data = new Dictionary<string, string> 
                { 
                    { "type", "Transaction" }, 
                    { "id", transaction.TransactionId.ToString() } 
                };
                await notificationService.SendNotificationAsync(transaction.ScrapCollectorId, title, body, data);
            }
        }
    }
}