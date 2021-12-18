using ReportsLibrary.Entities;

namespace ReportsDataAccessLayer.Services.Interfaces;

public interface IReportsService
{
    Task<List<Report>> GetReportsAsync();
    Task<Report> FindReportByIdAsync(Guid reportId);
    Task<Report> GetReportByIdAsync(Guid reportId);
    Task<Report> CreateReport(Guid ownerId);
    Task<Report> CommitChangesToReport(Guid ownerId);
    Task<Report> GenerateWorkTeamReport(Guid workTeamId, Guid changerId);
}