namespace ReportsInfrastructure.Services.Interfaces;

using ReportsDomain.Entities;

public interface ISprintsService
{
    Task<List<Sprint>> GetSprintsAsync();
    Task<Sprint> FindSprintByIdAsync(Guid sprintId);
    Task<Sprint> GetSprintByIdAsync(Guid sprintId);
    Task<Sprint> CreateSprint(DateTime expirationDate, Guid workTeamId, Guid changerId);
    Task<Sprint> AddTaskToSprint(Guid sprintId, Guid taskId);
    Task<Sprint> RemoveTaskFromSprint(Guid sprintId, Guid taskId);
}