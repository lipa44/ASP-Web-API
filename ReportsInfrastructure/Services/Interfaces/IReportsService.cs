namespace ReportsInfrastructure.Services.Interfaces;

using ReportsDomain.Entities;

public interface IReportsService
{
    Task<List<Report>> GetReports();
    Task<Report> GetReportById(Guid reportId);
    Task<Report> FindReportById(Guid reportId);
    Task<IReadOnlyCollection<Report>> GetReportsByEmployeeId(Guid employeeId);
    Task<Report> CreateReport(Guid ownerId);
    Task<Report> CommitChangesToReport(Guid ownerId);
    Task<Report> GenerateWorkTeamReport(Guid workTeamId, Guid changerId);
    Task<Report> SetReportAsDone(Guid reportId, Guid changerId);
}