using DataAccess.DataBase;
using Domain.Entities;
using Domain.Entities.Tasks;
using Domain.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repositories.Tasks;

public class ReportTasksRepository : IReportTasksRepository
{
    private readonly ReportsDbContext _dbContext;

    public ReportTasksRepository(ReportsDbContext context) => _dbContext = context;

    public async Task<IReadOnlyCollection<ReportsTask>> GetAll()
        => await _dbContext.Tasks.ToListAsync();

    public async Task<ReportsTask> GetItem(Guid id)
        => await FindItem(id)
           ?? throw new ReportsException($"Task with id {id} doesn't exist");

    public async Task<ReportsTask> FindItem(Guid id)
        => await _dbContext.Tasks
            .SingleOrDefaultAsync(task => task.Id == id);

    public async Task<ReportsTask> Create(ReportsTask item)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeSetAsDone");

            if (IsReportsTaskExistAsync(item.Id).Result)
                throw new ReportsException($"Task with {item.Id} to create already exist");

            EntityEntry<ReportsTask> newTaskEntry = _dbContext.Tasks.Add(item);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return newTaskEntry.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeSetAsDone");
            throw;
        }
    }

    public Task<ReportsTask> Update(ReportsTask item)
    {
        throw new NotImplementedException();
    }

    public async Task<ReportsTask> Delete(Guid id)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeTaskRemoved");

            ReportsTask taskToRemove = await GetItem(id);

            _dbContext.Tasks.Remove(taskToRemove);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return taskToRemove;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeTaskRemoved");
            throw;
        }
    }

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime)
        => (await GetAll())
            .Where(t => t.CreationTime.Date == creationTime.Date).ToList();

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationDate(DateTime modificationTime)
        => (await GetAll())
            .Where(t => t.ModificationTime.Date == modificationTime.Date).ToList();

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByEmployeeId(Guid employeeId)
        => (await GetAll())
            .Where(task => employeeId == task.OwnerId).ToList();

    public async Task<IReadOnlyCollection<ReportsTask>> FindsTaskModifiedByEmployeeId(Guid employeeId)
        => (await GetAll())
            .Where(t => t.Modifications
                .Any(m => m.ChangerId == employeeId)).ToList();

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksCreatedByEmployeeSubordinates(Employee employee)
        => (await GetAll())
            .Where(t => employee.Subordinates.Any(e => e.Id == t.OwnerId)).ToList();

    private async Task<bool> IsReportsTaskExistAsync(Guid taskId)
        => await _dbContext.Tasks.FindAsync(taskId) != null;
}