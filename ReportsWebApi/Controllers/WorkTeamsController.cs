using AutoMapper;
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

    // GET: api/WorkTeams
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<WorkTeamDto>>> Get() =>
        _mapper.Map<List<WorkTeamDto>>(await _workTeamsService.GetWorkTeams());

    // GET: api/WorkTeams/1
    [HttpGet("{workTeamId}", Name = "GetWorkTeam")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullWorkTeamDto>> GetWorkTeam(Guid workTeamId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

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
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterWorkTeam(Guid leadId, string workTeamName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

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
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid workTeamId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

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
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid workTeamId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

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

    // DELETE: api/Employees/1
    [HttpDelete("{workTeamId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult RemoveEmployee(Guid workTeamId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            _workTeamsService.RemoveWorkTeam(workTeamId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}