using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using WebApi.DataTransferObjects;
using WebApi.Extensions;
using WebApi.Filters;

namespace WebApi.Controllers;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<WorkTeamDto>>> Get(
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        List<WorkTeam> workTeams = await _workTeamsService.GetWorkTeams();

        var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

        return Ok(IndexViewModelExtensions<WorkTeamDto>
            .ToIndexViewModel(_mapper.Map<List<WorkTeamDto>>(workTeams), paginationFilter));
    }

    [HttpGet("{workTeamId:guid}", Name = "GetWorkTeam")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkTeamFullDto>> GetWorkTeam([FromRoute] Guid workTeamId)
    {
        WorkTeam workTeam = await _workTeamsService.FindWorkTeamById(workTeamId);

        if (workTeam is null) return NotFound();

        return Ok(_mapper.Map<WorkTeamFullDto>(workTeam));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateWorkTeam([FromQuery] Guid leadId, [FromQuery] string workTeamName)
    {
        WorkTeam newWorkTeam = await _workTeamsService.CreateWorkTeam(leadId, workTeamName);

        return CreatedAtRoute(
            "GetWorkTeam", new { workTeamId = newWorkTeam.Id }, _mapper.Map<WorkTeamDto>(newWorkTeam));
    }

    [HttpPut("{workTeamId:guid}/add/{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddEmployeeToTeam(
        [FromRoute] Guid workTeamId,
        [FromQuery] Guid employeeId,
        [FromQuery] Guid changerId)
    {
        WorkTeam updatedWorkTeam
            = await _workTeamsService.AddEmployeeToTeam(employeeId, changerId, workTeamId);

        return Ok(_mapper.Map<WorkTeamFullDto>(updatedWorkTeam));
    }

    [HttpPut("{workTeamId:guid}/remove/{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveEmployeeFromTeam(
        [FromRoute] Guid workTeamId,
        [FromQuery] Guid employeeId,
        [FromQuery] Guid changerId)
    {
        WorkTeam updatedWorkTeam
            = await _workTeamsService.RemoveEmployeeFromTeam(employeeId, changerId, workTeamId);

        return Ok(_mapper.Map<WorkTeamFullDto>(updatedWorkTeam));
    }

    [HttpDelete("{workTeamId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveWorkTeam([FromRoute] Guid workTeamId)
    {
        await _workTeamsService.RemoveWorkTeam(workTeamId);

        return NoContent();
    }
}