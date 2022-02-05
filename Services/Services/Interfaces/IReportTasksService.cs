using Domain.Entities.Tasks;
using Domain.Entities.Tasks.TaskChangeCommands;

namespace Services.Services.Interfaces;

public interface IReportTasksService
{
    Task<IReadOnlyCollection<ReportsTask>> GetTasks();
    Task<ReportsTask> FindTaskById(Guid taskId);
    Task<ReportsTask> CreateTask(string taskName);
    Task<ReportsTask> RemoveTaskById(Guid taskId);
    Task<ReportsTask> UseChangeTaskCommand(Guid taskId, Guid changerId, ITaskCommand command);
    Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime);
    Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationDate(DateTime modificationTime);
    Task<IReadOnlyCollection<ReportsTask>> FindTasksByEmployeeId(Guid employeeId);
    Task<IReadOnlyCollection<ReportsTask>> FindsTaskModifiedByEmployeeId(Guid employeeId);
    Task<IReadOnlyCollection<ReportsTask>> FindTasksCreatedByEmployeeSubordinates(Guid employeeId);
}