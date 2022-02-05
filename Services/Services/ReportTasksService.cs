using DataAccess.Repositories.Employees;
using DataAccess.Repositories.Tasks;
using Domain.Entities;
using Domain.Entities.Tasks;
using Domain.Entities.Tasks.TaskChangeCommands;
using Services.Services.Interfaces;

namespace Services.Services;

public class ReportTasksService : IReportTasksService
{
    private readonly IReportTasksRepository _reportTasksRepository;
    private readonly IEmployeesRepository _employeesRepository;

    public ReportTasksService(IReportTasksRepository reportTasksRepository, IEmployeesRepository employeesRepository)
    {
        _reportTasksRepository = reportTasksRepository;
        _employeesRepository = employeesRepository;
    }

    public async Task<IReadOnlyCollection<ReportsTask>> GetTasks()
        => await _reportTasksRepository.GetAll();

    public async Task<ReportsTask> FindTaskById(Guid taskId)
        => await _reportTasksRepository.FindItem(taskId);

    public async Task<ReportsTask> CreateTask(string taskName)
    {
        ReportsTask newTask = new (taskName);

        return await _reportTasksRepository.Create(newTask);
    }

    public async Task<ReportsTask> RemoveTaskById(Guid taskId)
    {
        return await _reportTasksRepository.Delete(taskId);
    }

    public async Task<ReportsTask> UseChangeTaskCommand(Guid taskId, Guid changerId, ITaskCommand command)
    {
        ReportsTask taskToUpdate = await _reportTasksRepository.GetItem(taskId);
        Employee changerToUpdateTask = await _employeesRepository.GetItem(taskId);

        if (command is SetTaskOwnerCommand setOwnerCommand)
        {
            Employee newTaskOwner = await _employeesRepository.GetItem(setOwnerCommand.NewImplementorId);
            setOwnerCommand.NewImplementor = newTaskOwner;
            command.Execute(changerToUpdateTask, taskToUpdate);
            newTaskOwner.AddTask(taskToUpdate);

            await _employeesRepository.Update(newTaskOwner);
        }
        else
        {
            command.Execute(changerToUpdateTask, taskToUpdate);
        }

        await _reportTasksRepository.Update(taskToUpdate);

        return taskToUpdate;
    }

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime)
        => await _reportTasksRepository.FindTasksByCreationTime(creationTime);

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationDate(DateTime modificationTime)
        => await _reportTasksRepository.FindTasksByModificationDate(modificationTime);

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByEmployeeId(Guid employeeId)
        => await _reportTasksRepository.FindTasksByEmployeeId(employeeId);

    public async Task<IReadOnlyCollection<ReportsTask>> FindsTaskModifiedByEmployeeId(Guid employeeId)
        => await _reportTasksRepository.FindsTaskModifiedByEmployeeId(employeeId);

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksCreatedByEmployeeSubordinates(Guid employeeId)
    {
        Employee employee = await _employeesRepository.GetItem(employeeId);
        return await _reportTasksRepository.FindTasksCreatedByEmployeeSubordinates(employee);
    }
}