using GreenConnectPlatform.Business.Models.Reports;

namespace GreenConnectPlatform.Business.Services.Reports;

public interface IReportService
{
    Task<ReportModel> GetReport(DateTime startDate, DateTime endDate);

    Task<ReportForCollectorModel> GetReportForCollector(Guid userId, DateTime startDate, DateTime endDate);
}