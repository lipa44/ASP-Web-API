namespace ReportsInfrastructure.Services.Interfaces;

using ReportsDomain.Entities;

public interface IWorkTeamsService
{
    Task<List<WorkTeam>> GetWorkTeams();
    Task<WorkTeam> GetWorkTeamById(Guid workTeamId);
    Task<WorkTeam> RegisterWorkTeam(Guid leadId, string workTeamName);
    Task<WorkTeam> AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid teamId);
    Task<WorkTeam> RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid teamId);
    Task<WorkTeam> RemoveWorkTeam(Guid workTeamId);
}