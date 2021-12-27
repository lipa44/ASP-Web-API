using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Entities;
using ReportsInfrastructure.Services.Interfaces;
using ReportsWebApi.DataTransferObjects;
using ReportsWebApi.Extensions;

namespace ReportsWebApi.Controllers;

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

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<WorkTeamDto>>> Get() =>
        Ok(_mapper.Map<List<WorkTeamDto>>(await _workTeamsService.GetWorkTeams()));

    [HttpGet("{workTeamId}", Name = "GetWorkTeam")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullWorkTeamDto>> GetWorkTeam(Guid workTeamId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        WorkTeam workTeam = await _workTeamsService.GetWorkTeamById(workTeamId);
        return Ok(_mapper.Map<FullWorkTeamDto>(workTeam));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterWorkTeam(Guid leadId, string workTeamName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        WorkTeam newWorkTeam = await _workTeamsService.RegisterWorkTeam(leadId, workTeamName);

        return CreatedAtRoute(
                "GetWorkTeam", new { workTeamId = newWorkTeam.Id }, _mapper.Map<WorkTeamDto>(newWorkTeam));
    }

    [HttpPut("{workTeamId}/add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid workTeamId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());
        WorkTeam updatedWorkTeam = await _workTeamsService.AddEmployeeToTeam(employeeId, changerId, workTeamId);
        return Ok(_mapper.Map<FullWorkTeamDto>(updatedWorkTeam));
    }

    [HttpPut("{workTeamId}/remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid workTeamId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        WorkTeam updatedWorkTeam = await _workTeamsService.RemoveEmployeeFromTeam(employeeId, changerId, workTeamId);
        return Ok(_mapper.Map<FullWorkTeamDto>(updatedWorkTeam));
    }

    [HttpDelete("{workTeamId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveEmployee(Guid workTeamId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        WorkTeam removedWorkTeam = await _workTeamsService.RemoveWorkTeam(workTeamId);
        return Ok(_mapper.Map<FullWorkTeamDto>(removedWorkTeam));
    }
}