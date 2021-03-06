using Domain.Entities;

namespace Services.Services.Interfaces;

public interface IWorkTeamsService
{
    Task<IReadOnlyCollection<WorkTeam>> GetWorkTeams();
    Task<WorkTeam> FindWorkTeamById(Guid workTeamId);
    Task<WorkTeam> CreateWorkTeam(Guid leadId, string workTeamName);
    Task<WorkTeam> AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid teamId);
    Task<WorkTeam> RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid teamId);
    Task<WorkTeam> RemoveWorkTeam(Guid workTeamId);
}