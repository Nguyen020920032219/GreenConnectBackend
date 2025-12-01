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
    private readonly ILogger<TransactionAutoCancelBackGroundService> _logger;

    public TransactionAutoCancelBackGroundService(IServiceScopeFactory serviceScopeFactory, 
        ILogger<TransactionAutoCancelBackGroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("--- Robot tự động hủy đơn đã khởi động ---");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOverdueTransactions();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Lỗi khi quét đơn quá hạn.");
            }
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ProcessOverdueTransactions()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GreenConnectDbContext>();
            var now = DateTime.UtcNow;
            var overdueTransactions = await context.Transactions
                .Where(t => t.Status == TransactionStatus.Scheduled
                            && t.ScheduledTime.HasValue
                            && t.ScheduledTime.Value.AddMinutes(30) < now)
                .ToListAsync();

            if (overdueTransactions.Any())
            {
                foreach (var transaction in overdueTransactions)
                {
                    transaction.Status = TransactionStatus.CanceledBySystem;
                    transaction.UpdatedAt = now;
                }
            }
            await context.SaveChangesAsync();
            _logger.LogInformation($"Đã tự động hủy {overdueTransactions.Count} đơn hàng quá hạn.");
        }
    }
}