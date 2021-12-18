using ReportsLibrary.Entities;

namespace ReportsDataAccessLayer.Services.Interfaces;

public interface IWorkTeamsService
{
    Task<List<WorkTeam>> GetWorkTeams();
    Task<WorkTeam> GetWorkTeamById(Guid workTeamId);
    Task<WorkTeam> RegisterWorkTeam(Guid leadId, string workTeamName);
    void AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid teamId);
    void RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid teamId);
    Task AddSprintToTeam(Guid workTeamId, Guid changerId, DateTime sprintExpirationDate);
    Task RemoveSprintFromTeam(Guid workTeamId, Guid changerId, Guid sprintId);
    void RemoveWorkTeam(WorkTeam workTeam);
    Task<WorkTeam> GenerateReport(Guid workTeamId, Guid changerId);
}