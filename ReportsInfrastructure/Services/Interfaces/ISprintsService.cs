namespace ReportsInfrastructure.Services.Interfaces;

using ReportsDomain.Entities;

public interface ISprintsService
{
    Task<List<Sprint>> GetSprints();
    Task<Sprint> GetSprintById(Guid sprintId);
    Task<Sprint> FindSprintById(Guid sprintId);
    Task<Sprint> CreateSprint(DateTime expirationDate, Guid workTeamId, Guid changerId);
    Task<Sprint> AddTaskToSprint(Guid changerId, Guid sprintId, Guid taskId);
    Task<Sprint> RemoveTaskFromSprint(Guid sprintId, Guid taskId);
}