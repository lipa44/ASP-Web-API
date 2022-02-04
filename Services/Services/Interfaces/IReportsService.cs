using Domain.Entities;

namespace Services.Services.Interfaces;

public interface IReportsService
{
    Task<IReadOnlyCollection<Report>> GetReports();
    Task<Report> FindReportById(Guid reportId);
    Task<Report> GetReportByEmployeeId(Guid employeeId);
    Task<Report> CommitChangesToReport(Guid ownerId);

    // Task<Report> GenerateWorkTeamReport(Guid workTeamId, Guid changerId);
    Task<Report> SetReportAsDone(Guid reportId, Guid changerId);
}