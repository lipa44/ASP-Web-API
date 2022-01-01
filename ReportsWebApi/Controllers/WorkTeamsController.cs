namespace ReportsWebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Entities;
using ReportsInfrastructure.Services.Interfaces;
using DataTransferObjects;

[Route("api/[controller]")]
[ApiController]
public class WorkTeamsController : ControllerBase
{
    private readonly IWorkTeamsService _workTeamsService;
    private readonly IMapper _mapper;

    public WorkTeamsController(IWorkTeamsService workTeamsService, IMapper mapper)
    {
        _workTeamsService = workTeamsService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<WorkTeamDto>>> Get() =>
        Ok(_mapper.Map<List<WorkTeamDto>>(await _workTeamsService.GetWorkTeams()));

    [HttpGet("{workTeamId}", Name = "GetWorkTeam")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkTeamFullDto>> GetWorkTeam([FromRoute] Guid workTeamId)
    {
        WorkTeam workTeam = await _workTeamsService.GetWorkTeamById(workTeamId);
        return Ok(_mapper.Map<WorkTeamFullDto>(workTeam));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterWorkTeam([FromQuery] Guid leadId, [FromQuery] string workTeamName)
    {
        WorkTeam newWorkTeam = await _workTeamsService.RegisterWorkTeam(leadId, workTeamName);

        return CreatedAtRoute(
            "GetWorkTeam", new { workTeamId = newWorkTeam.Id }, _mapper.Map<WorkTeamDto>(newWorkTeam));
    }

    [HttpPut("{workTeamId}/add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddEmployeeToTeam(
        [FromRoute] Guid workTeamId,
        [FromQuery] Guid employeeId,
        [FromQuery] Guid changerId)
    {
        WorkTeam updatedWorkTeam = await _workTeamsService.AddEmployeeToTeam(employeeId, changerId, workTeamId);
        return Ok(_mapper.Map<WorkTeamFullDto>(updatedWorkTeam));
    }

    [HttpPut("{workTeamId}/remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveEmployeeFromTeam(
        [FromRoute] Guid workTeamId,
        [FromQuery] Guid employeeId,
        [FromQuery] Guid changerId)
    {
        WorkTeam updatedWorkTeam = await _workTeamsService.RemoveEmployeeFromTeam(employeeId, changerId, workTeamId);
        return Ok(_mapper.Map<WorkTeamFullDto>(updatedWorkTeam));
    }

    [HttpDelete("{workTeamId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveEmployee([FromRoute] Guid workTeamId)
    {
        WorkTeam removedWorkTeam = await _workTeamsService.RemoveWorkTeam(workTeamId);
        return Ok(_mapper.Map<WorkTeamFullDto>(removedWorkTeam));
    }
}