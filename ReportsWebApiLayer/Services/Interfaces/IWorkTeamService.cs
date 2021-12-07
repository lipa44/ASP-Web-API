using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Entities;

namespace ReportsWebApiLayer.Services.Interfaces;

public interface IWorkTeamService
{
    ActionResult<List<WorkTeam>> GetWorkTeams();
    Task<WorkTeam> RegisterWorkTeam(WorkTeam workTeam);
    void RemoveWorkTeam(WorkTeam workTeam);
}