namespace ReportsInfrastructure.Services.Interfaces;

using ReportsDomain.Entities;

public interface IReportsService
{
    Task<List<Report>> GetReportsAsync();
    Task<Report> FindReportByIdAsync(Guid reportId);
    Task<Report> GetReportByIdAsync(Guid reportId);
    IReadOnlyCollection<Report> GetReportsByEmployeeIdAsync(Guid employeeId);
    Task<Report> CreateReport(Guid ownerId);
    Task<Report> CommitChangesToReport(Guid ownerId);
    Task<Report> GenerateWorkTeamReport(Guid workTeamId, Guid changerId);
    Task<Report> SetReportAsDone(Guid reportId, Guid changerId);
}