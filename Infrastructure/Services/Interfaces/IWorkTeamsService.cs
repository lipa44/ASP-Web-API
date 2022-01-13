using Domain.Entities;

namespace Infrastructure.Services.Interfaces;

public interface IWorkTeamsService
{
    Task<List<WorkTeam>> GetWorkTeams();
    Task<WorkTeam> GetWorkTeamById(Guid workTeamId);
    Task<WorkTeam> FindWorkTeamById(Guid workTeamId);
    Task<WorkTeam> CreateWorkTeam(Guid leadId, string workTeamName);
    Task<WorkTeam> AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid teamId);
    Task<WorkTeam> RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid teamId);
    Task<WorkTeam> RemoveWorkTeam(Guid workTeamId);
}