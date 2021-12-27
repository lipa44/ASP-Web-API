namespace ReportsInfrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ReportsDataAccess.DataBase;
using ReportsDomain.Employees;
using ReportsDomain.Entities;
using ReportsDomain.Tools;
using Interfaces;

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

    public IReadOnlyCollection<Report> GetReportsByEmployeeIdAsync(Guid employeeId)
        => _dbContext.Reports
            .Include(report => report.Owner)
            .Where(report => report.OwnerId == employeeId).ToList();

    public async Task<Report> CreateReport(Guid ownerId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeReportCreated");

            Employee reportOwner = await GetEmployeeFromDbAsync(ownerId);
            Report newReport = reportOwner.CreateReport();

            _dbContext.Reports.Add(newReport);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return newReport;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeReportCreated");
            throw;
        }
    }

    public async Task<Report> CommitChangesToReport(Guid ownerId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeChangesCommitted");

            Employee employee = await GetEmployeeFromDbAsync(ownerId);
            Report updatedReport = employee.CommitChangesToReport();

            _dbContext.Reports.Update(updatedReport);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return updatedReport;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeChangesCommitted");
            throw;
        }
    }

    public async Task<Report> GenerateWorkTeamReport(Guid workTeamId, Guid changerId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeReportGenerated");

            Employee changerToGenerateReport = await GetEmployeeFromDbAsync(changerId);
            WorkTeam teamToGenerateReport = await GetWorkTeamByIdAsync(workTeamId);

            Report updatedReport = teamToGenerateReport.GenerateReport(changerToGenerateReport);
            updatedReport.SetReportAsDone(changerToGenerateReport);

            _dbContext.Reports.Update(updatedReport);
            _dbContext.WorkTeams.Update(teamToGenerateReport);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return updatedReport;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeReportGenerated");
            throw;
        }
    }

    public async Task<Report> SetReportAsDone(Guid reportId, Guid changerId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeSetAsDone");

            Employee changerToSetReportDone = await GetEmployeeFromDbAsync(changerId);
            Report reportToSetReportAsDone = await GetReportByIdAsync(reportId);

            Report updatedReport = reportToSetReportAsDone.SetReportAsDone(changerToSetReportDone);
            _dbContext.Reports.Update(updatedReport);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return updatedReport;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeSetAsDone");
            throw;
        }
    }

    private async Task<Employee> GetEmployeeFromDbAsync(Guid employeeId) =>
        await _dbContext.Employees
            .Include(employee => employee.Report)
            .Include(employee => employee.Tasks)
            .SingleOrDefaultAsync(employee => employee.Id == employeeId)
        ?? throw new ReportsException($"Employee with Id {employeeId} doesn't exist");

    private async Task<WorkTeam> GetWorkTeamByIdAsync(Guid workTeamId)
        => await _dbContext.WorkTeams
               .SingleOrDefaultAsync(workTeam => workTeam.Id == workTeamId)
           ?? throw new ReportsException($"WorkTeam with Id {workTeamId} doesn't exist");
}