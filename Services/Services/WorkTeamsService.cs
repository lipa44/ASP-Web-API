using DataAccess.Repositories.Employees;
using DataAccess.Repositories.WorkTeams;
using Domain.Entities;
using Services.Services.Interfaces;

namespace Services.Services;

public class WorkTeamsService : IWorkTeamsService
{
    private readonly IWorkTeamsRepository _workTeamsRepository;
    private readonly IEmployeesRepository _employeesRepository;

    public WorkTeamsService(IWorkTeamsRepository workTeamsRepository, IEmployeesRepository employeesRepository)
    {
        _workTeamsRepository = workTeamsRepository;
        _employeesRepository = employeesRepository;
    }

    public async Task<IReadOnlyCollection<WorkTeam>> GetWorkTeams()
        => await _workTeamsRepository.GetAll();

    public async Task<WorkTeam> FindWorkTeamById(Guid workTeamId)
        => await _workTeamsRepository.FindItem(workTeamId);

    public async Task<WorkTeam> CreateWorkTeam(Guid leadId, string workTeamName)
    {
        Employee teamLead = await _employeesRepository.GetItem(leadId);
        var workTeam = new WorkTeam(teamLead, workTeamName);

        return await _workTeamsRepository.Create(workTeam);
    }

    public async Task<WorkTeam> AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid teamId)
    {
        Employee employeeToAdd = await _employeesRepository.GetItem(employeeId);
        Employee changer = await _employeesRepository.GetItem(changerId);
        WorkTeam workTeam = await _workTeamsRepository.GetItem(teamId);

        employeeToAdd.SetWorkTeam(workTeam);
        workTeam.AddEmployee(employeeToAdd, changer);

        await _employeesRepository.Update(employeeToAdd);
        await _workTeamsRepository.Update(workTeam);

        return workTeam;
    }

    public async Task<WorkTeam> RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid teamId)
    {
        Employee employeeToRemove = await _employeesRepository.GetItem(employeeId);
        Employee changer = await _employeesRepository.GetItem(changerId);
        WorkTeam workTeam = await _workTeamsRepository.GetItem(teamId);

        employeeToRemove.RemoveWorkTeam();
        workTeam.RemoveEmployee(employeeToRemove, changer);

        // TODO: add IUnitOfWork to work with EmployeeRepository
        await _employeesRepository.Update(employeeToRemove);
        await _workTeamsRepository.Update(workTeam);

        return workTeam;
    }

    public async Task<WorkTeam> RemoveWorkTeam(Guid workTeamId)
    {
        return await _workTeamsRepository.Delete(workTeamId);
    }
}