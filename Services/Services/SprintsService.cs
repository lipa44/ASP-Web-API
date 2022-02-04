using DataAccess.Repositories.Employees;
using DataAccess.Repositories.Sprints;
using DataAccess.Repositories.Tasks;
using DataAccess.Repositories.WorkTeams;
using Domain.Entities;
using Domain.Entities.Tasks;
using Services.Services.Interfaces;

namespace Services.Services;

public class SprintsService : ISprintsService
{
    private readonly ISprintsRepository _sprintsRepository;
    private readonly IWorkTeamsRepository _workTeamsRepository;
    private readonly IEmployeesRepository _employeesRepository;
    private readonly IReportTasksRepository _reportTasksRepository;

    public SprintsService(
        ISprintsRepository sprintsRepository,
        IWorkTeamsRepository workTeamsRepository,
        IEmployeesRepository employeesRepository,
        IReportTasksRepository reportTasksRepository)
    {
        _sprintsRepository = sprintsRepository;
        _workTeamsRepository = workTeamsRepository;
        _employeesRepository = employeesRepository;
        _reportTasksRepository = reportTasksRepository;
    }

    public async Task<IReadOnlyCollection<Sprint>> GetSprints()
        => await _sprintsRepository.GetAll();

    public async Task<Sprint> FindSprintById(Guid sprintId)
        => await _sprintsRepository.FindItem(sprintId);

    public async Task<Sprint> CreateSprint(DateTime expirationDate, Guid workTeamId, Guid changerId)
    {
        Sprint newSprint = new (expirationDate, workTeamId);
        WorkTeam workTeam = await _workTeamsRepository.GetItem(workTeamId);
        Employee changer = await _employeesRepository.GetItem(changerId);

        workTeam.AddSprint(changer, newSprint);

        await _sprintsRepository.Create(newSprint);
        await _workTeamsRepository.Update(workTeam);

        return newSprint;
    }

    public async Task<Sprint> AddTaskToSprint(Guid changerId, Guid sprintId, Guid taskId)
    {
        Sprint sprint = await _sprintsRepository.GetItem(sprintId);
        Employee changer = await _employeesRepository.GetItem(changerId);
        ReportsTask task = await _reportTasksRepository.GetItem(taskId);

        task.SetSprint(changer, sprint);
        sprint.AddTask(task);

        await _reportTasksRepository.Update(task);
        await _sprintsRepository.Update(sprint);

        return sprint;
    }

    public async Task<Sprint> RemoveTaskFromSprint(Guid sprintId, Guid taskId)
    {
        Sprint sprint = await _sprintsRepository.GetItem(sprintId);
        ReportsTask task = await _reportTasksRepository.GetItem(taskId);

        sprint.RemoveTask(task);

        // TODO: to think about reportsTask updating
        await _sprintsRepository.Update(sprint);

        return sprint;
    }
}