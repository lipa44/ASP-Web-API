using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Entities;

namespace ReportsDataAccessLayer.Services.Interfaces;

public interface IWorkTeamService
{
    ActionResult<List<WorkTeam>> GetWorkTeams();
    Task<WorkTeam> GetWorkTeamById(Guid workTeamId);
    Task<WorkTeam> RegisterWorkTeam(Guid leadId, string workTeamName);
    void RemoveWorkTeam(WorkTeam workTeam);
}