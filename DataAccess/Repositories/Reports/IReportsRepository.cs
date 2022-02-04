using Domain.Entities;

namespace DataAccess.Repositories.Reports;

public interface IReportsRepository : IRepository<Report>
{
    Task<Report> GetReportByEmployeeId(Guid employeeId);
    Task<Employee> GetEmployeeByReportId(Guid reportId);
    Report GenerateWorkTeamReport(Guid workTeamId, Guid changerId);
    Report SetReportAsDone(Guid reportId, Guid changerId);
}