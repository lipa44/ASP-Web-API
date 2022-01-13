using DataAccess.DataBase;
using Domain.Entities;
using Domain.Tools;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services;

public class ReportsService : IReportsService
{
    private readonly ReportsDbContext _dbContext;

    public ReportsService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<Report>> GetReports()
        => await _dbContext.Reports
            .Include(report => report.Owner)
            .ToListAsync();

    public async Task<Report> GetReportById(Guid reportId)
        => await FindReportById(reportId)
           ?? throw new ReportsException($"Report with id {reportId} doesn't exist");

    public async Task<Report> FindReportById(Guid reportId)
        => await _dbContext.Reports
            .Include(report => report.Owner)
            .SingleOrDefaultAsync(report => report.Id == reportId);

    public async Task<IReadOnlyCollection<Report>> GetReportsByEmployeeId(Guid employeeId)
        => await _dbContext.Reports
            .Include(report => report.Owner)
            .Where(report => report.OwnerId == employeeId).ToListAsync();

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
            Report reportToSetReportAsDone = await GetReportById(reportId);

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
               .Include(workTeam => workTeam.Employees)
               .ThenInclude(e => e.Report)
               .SingleOrDefaultAsync(workTeam => workTeam.Id == workTeamId)
           ?? throw new ReportsException($"WorkTeam with Id {workTeamId} doesn't exist");
}