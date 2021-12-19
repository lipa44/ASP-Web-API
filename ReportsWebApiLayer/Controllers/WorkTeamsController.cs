using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Entities;
using ReportsWebApiLayer.DataTransferObjects;

namespace ReportsWebApiLayer.Controllers;

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

    // GET: api/WorkTeams
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<WorkTeamDto>>> Get() =>
        _mapper.Map<List<WorkTeamDto>>(await _workTeamsService.GetWorkTeams());

    // GET: api/WorkTeams/1
    [HttpGet("{workTeamId}", Name = "GetWorkTeam")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullWorkTeamDto>> GetWorkTeam(Guid workTeamId)
    {
        try
        {
            WorkTeam workTeam = await _workTeamsService.GetWorkTeamById(workTeamId);
            return _mapper.Map<FullWorkTeamDto>(workTeam);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    // POST: api/WorkTeams?leadId=1&workTeamName=Aboba%20Team
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterWorkTeam(Guid leadId, string workTeamName)
    {
        try
        {
            WorkTeam newWorkTeam = await _workTeamsService.RegisterWorkTeam(leadId, workTeamName);

            return CreatedAtRoute(
                "GetWorkTeam", new { workTeamId = newWorkTeam.Id }, _mapper.Map<WorkTeamDto>(newWorkTeam));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/WorkTeams/1/add?employeeId=2&changerId=3
    [HttpPut("{workTeamId}/add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid workTeamId)
    {
        try
        {
            _workTeamsService.AddEmployeeToTeam(employeeId, changerId, workTeamId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/WorkTeams/1/remove?employeeId=2&changerId=3
    [HttpPut("{workTeamId}/remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid workTeamId)
    {
        try
        {
            _workTeamsService.RemoveEmployeeFromTeam(employeeId, changerId, workTeamId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/WorkTeams/1/sprints/add?changerId=2&sprintExpirationDate=2020-08-10
    [HttpPut("{workTeamId}/sprints/add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AddSprintToWorkTeam(Guid changerId, Guid workTeamId, DateTime sprintExpirationDate)
    {
        try
        {
            _workTeamsService.AddSprintToTeam(workTeamId, changerId, sprintExpirationDate);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/WorkTeams/1/sprints/remove?changerId=2&sprintExpirationDate=2020-08-10
    [HttpPut("{workTeamId}/sprints/remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult RemoveSprintFromWorkTeam(Guid changerId, Guid workTeamId, Guid sprintId)
    {
        try
        {
            _workTeamsService.RemoveSprintFromTeam(workTeamId, changerId, sprintId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}