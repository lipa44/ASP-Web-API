using DataAccess.DataBase;
using Domain.Entities;
using Domain.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repositories.Sprints;

public class SprintsRepository : ISprintsRepository
{
    private readonly ReportsDbContext _dbContext;

    public SprintsRepository(ReportsDbContext context)
    {
        _dbContext = context;
    }

    public async Task<IReadOnlyCollection<Sprint>> GetAll()
        => await _dbContext.Sprints.ToListAsync();

    public async Task<Sprint> FindItem(Guid id)
        => await _dbContext.Sprints
            .Include(sprint => sprint.Tasks)
            .SingleOrDefaultAsync(sprint => sprint.Id == id);

    public async Task<Sprint> GetItem(Guid id)
        => await FindItem(id)
           ?? throw new ReportsException($"Sprint with id {id} doesn't exist");

    public async Task<Sprint> Create(Sprint item)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeSprintCreated");

            if (IsSprintExistAsync(item.Id).Result)
                throw new ReportsException($"Sprint {item.Id} to create already exist");

            EntityEntry<Sprint> newSprint = _dbContext.Sprints.Add(item);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return newSprint.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeSprintCreated");
            throw;
        }
    }

    public Task<Sprint> Update(Sprint item)
    {
        throw new NotImplementedException();
    }

    public Task<Sprint> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> IsSprintExistAsync(Guid sprintId)
        => await _dbContext.Sprints.FindAsync(sprintId) != null;
}