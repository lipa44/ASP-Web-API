using DataAccess.DataBase;
using Domain.Entities;
using Domain.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repositories.Reports;

public class ReportsRepository : IReportsRepository
{
    private readonly ReportsDbContext _dbContext;

    public ReportsRepository(ReportsDbContext context) => _dbContext = context;

    public async Task<IReadOnlyCollection<Report>> GetAll()
        => await _dbContext.Reports
            .Include(report => report.Owner)
            .ToListAsync();

    public async Task<Report> FindItem(Guid id)
        => await _dbContext.Reports
            .Include(report => report.Owner)
            .SingleOrDefaultAsync(report => report.Id == id);

    public async Task<Report> GetItem(Guid id)
        => await FindItem(id)
           ?? throw new ReportsException($"Report with id {id} doesn't exist");

    public async Task<Report> Create(Report item)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeReportCreated");

            if (IsReportExistAsync(item.Id).Result)
                throw new ReportsException($"Report {item.Id} to create already exist");

            _dbContext.Reports.Add(item);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return item;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeReportCreated");
            throw;
        }
    }

    public async Task<Report> Update(Report item)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeUpdatingReport");

            if (!IsReportExistAsync(item.Id).Result)
                throw new ReportsException($"Report {item.Id} to update doesn't exist");

            EntityEntry<Report> updatedReport = _dbContext.Reports.Update(item);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return updatedReport.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeUpdatingReport");
            throw;
        }
    }

    public Task<Report> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Report> GetReportByEmployeeId(Guid employeeId)
        => await _dbContext.Reports
            .Include(report => report.Owner)
            .SingleOrDefaultAsync(report => report.OwnerId == employeeId)
        ?? throw new ReportsException($"Employee with id {employeeId} doesn't have a report");

    public async Task<Employee> GetEmployeeByReportId(Guid reportId)
        => (await GetItem(reportId)).Owner;

    public Report GenerateWorkTeamReport(Guid workTeamId, Guid changerId)
    {
        throw new NotImplementedException();
    }

    public Report SetReportAsDone(Guid reportId, Guid changerId)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> IsReportExistAsync(Guid reportId)
        => await _dbContext.Reports.FindAsync(reportId) != null;
}