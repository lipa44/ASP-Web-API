using Microsoft.EntityFrameworkCore;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tools;

namespace ReportsDataAccessLayer.Services;

public class ReportsService : IReportsService
{
    private readonly ReportsDbContext _dbContext;

    public ReportsService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<Report>> GetReportsAsync()
        => await _dbContext.Reports
            .Include(report => report.Owner)
            .ToListAsync();

    public Task<Report> FindReportByIdAsync(Guid reportId)
        => _dbContext.Reports.SingleOrDefaultAsync(report => report.Id == reportId);

    public async Task<Report> GetReportByIdAsync(Guid reportId)
        => await _dbContext.Reports
            .Include(report => report.Owner)
            .SingleOrDefaultAsync(report => report.Id == reportId)
           ?? throw new ReportsException($"Report with id {reportId} doesn't exist");

    public async Task<Report> CreateReport(Guid ownerId)
    {
        Employee reportOwner = await GetEmployeeFromDbAsync(ownerId);
        Report newReport = reportOwner.CreateReport();

        _dbContext.Reports.Add(newReport);

        await _dbContext.SaveChangesAsync();

        return newReport;
    }

    public async Task<Report> CommitChangesToReport(Guid ownerId)
    {
        Employee employee = await GetEmployeeFromDbAsync(ownerId);
        Report updatedReport = employee.CommitChangesToReport();

        _dbContext.Reports.Update(updatedReport);

        await _dbContext.SaveChangesAsync();

        return updatedReport;
    }

    public async Task<Report> GenerateWorkTeamReport(Guid workTeamId, Guid changerId)
    {
        Employee changerToGenerateReport = await GetEmployeeFromDbAsync(changerId);
        WorkTeam teamToGenerateReport = await GetWorkTeamByIdAsync(workTeamId);

        Report updatedReport = teamToGenerateReport.GenerateReport(changerToGenerateReport);

        _dbContext.Reports.Update(updatedReport);

        await _dbContext.SaveChangesAsync();

        return updatedReport;
    }

    public async Task<Report> SetReportAsDone(Guid workTeamId, Guid changerId)
    {
        Employee changerToSetReportDone = await GetEmployeeFromDbAsync(changerId);
        WorkTeam teamToSetReportAsDone = await GetWorkTeamByIdAsync(workTeamId);

        Report updatedReport = teamToSetReportAsDone.Report.SetReportAsDone(changerToSetReportDone);

        _dbContext.Reports.Update(updatedReport);

        await _dbContext.SaveChangesAsync();

        return updatedReport;
    }

    private async Task<Employee> GetEmployeeFromDbAsync(Guid employeeId) =>
        await _dbContext.Employees
            .Include(employee => employee.Report)
            .Include(employee => employee.Tasks)
            .SingleOrDefaultAsync(employee => employee.Id == employeeId)
        ?? throw new ReportsException($"Employee with Id {employeeId} doesn't exist");

    private async Task<WorkTeam> GetWorkTeamByIdAsync(Guid workTeamId)
        => await _dbContext.WorkTeams.SingleOrDefaultAsync(workTeam => workTeam.Id == workTeamId)
           ?? throw new ReportsException($"WorkTeam with Id {workTeamId} doesn't exist");
}