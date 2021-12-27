namespace ReportsWebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Entities;
using ReportsInfrastructure.Services.Interfaces;
using DataTransferObjects;

[Route("api/[controller]")]
[ApiController]
public class SprintsController : ControllerBase
{
    private readonly ISprintsService _sprintsService;
    private readonly IMapper _mapper;

    public SprintsController(ISprintsService sprintsService, IMapper mapper)
    {
        _sprintsService = sprintsService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<SprintDto>>> GetSprints() =>
        Ok(_mapper.Map<List<SprintDto>>(await _sprintsService.GetSprintsAsync()));

    [HttpGet("{sprintId}", Name = "GetSprintById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SprintDto>> GetSprint([FromRoute] Guid sprintId)
    {
        Sprint sprint = await _sprintsService.GetSprintByIdAsync(sprintId);
        return Ok(_mapper.Map<SprintDto>(sprint));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSprint(DateTime expirationTime, Guid workTeamId, Guid changerId)
    {
        Sprint sprint = await _sprintsService.CreateSprint(expirationTime, workTeamId, changerId);

        return CreatedAtRoute(
                "GetSprintById", new { sprintId = sprint.SprintId }, _mapper.Map<SprintDto>(sprint));
    }

    [HttpPut("{sprintId}/add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTaskToSprint([FromRoute] Guid sprintId, Guid taskId)
    {
        Sprint updatedSprint = await _sprintsService.AddTaskToSprint(sprintId, taskId);
        return Ok(_mapper.Map<SprintDto>(updatedSprint));
    }

    [HttpPut("{sprintId}/remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveTaskFromSprint(Guid sprintId, Guid taskId)
    {
        Sprint removedSprint = await _sprintsService.RemoveTaskFromSprint(sprintId, taskId);
        return Ok(_mapper.Map<SprintDto>(removedSprint));
    }
}