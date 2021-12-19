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
        => await _dbContext.Reports.ToListAsync();

    public Task<Report> FindReportByIdAsync(Guid reportId)
        => _dbContext.Reports.SingleOrDefaultAsync(report => report.Id == reportId);

    public Task<Report> GetReportByIdAsync(Guid reportId)
    {
        if (!IsReportExistAsync(reportId).Result)
            throw new Exception($"Report to with id {reportId} doesn't exist");

        return _dbContext.Reports
            .SingleAsync(report => report.Id == reportId);
    }

    public async Task<Report> CreateReport(Guid ownerId)
    {
        if (!IsEmployeeExistAsync(ownerId).Result)
            throw new Exception($"Employee {ownerId} to create report doesn't exist");

        Employee reportOwner = await GetEmployeeFromDbAsync(ownerId);
        Report newReport = reportOwner.CreateReport();

        _dbContext.Reports.Add(newReport);

        // _dbContext.Employees.Update(reportOwner);
        await _dbContext.SaveChangesAsync();

        return newReport;
    }

    public async Task<Report> CommitChangesToReport(Guid ownerId)
    {
        Employee employee = await GetEmployeeFromDbAsync(ownerId);
        Report updatedReport = employee.CommitChangesToReport();

        _dbContext.Reports.Update(updatedReport);

        // _dbContext.Employees.Update(employee);
        await _dbContext.SaveChangesAsync();

        return updatedReport;
    }

    public async Task<Report> GenerateWorkTeamReport(Guid workTeamId, Guid changerId)
    {
        Employee teamLeadToGenerateReport = await GetEmployeeFromDbAsync(changerId);
        WorkTeam teamToGenerateReport = await GetWorkTeamByIdAsync(workTeamId);

        if (teamToGenerateReport.TeamLeadId != changerId)
            throw new PermissionDeniedException("Only team lead can add employees to the work team");

        Report updatedReport = teamToGenerateReport.GenerateReport(teamLeadToGenerateReport);

        _dbContext.Reports.Update(updatedReport);

        await _dbContext.SaveChangesAsync();

        return updatedReport;
    }

    private async Task<bool> IsReportExistAsync(Guid reportId)
        => await _dbContext.Reports.AnyAsync(report => report.Id == reportId);

    private async Task<bool> IsEmployeeExistAsync(Guid employeeId)
        => await _dbContext.Employees.AnyAsync(employee => employee.Id == employeeId);

    private async Task<Employee> GetEmployeeFromDbAsync(Guid employeeId) =>
        await _dbContext.Employees
            .Include(e => e.Report)
            .Include(e => e.Tasks)
            .SingleAsync(e => e.Id == employeeId);

    private async Task<WorkTeam> GetWorkTeamByIdAsync(Guid workTeamId)
        => await _dbContext.WorkTeams.SingleAsync(workTeam => workTeam.Id == workTeamId);
}