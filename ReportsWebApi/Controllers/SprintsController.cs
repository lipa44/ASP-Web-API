namespace ReportsWebApi.Controllers;

using Extensions;
using Filters;
using AutoMapper;
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<SprintDto>>> GetSprints(
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        List<Sprint> sprints = await _sprintsService.GetSprints();

        var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

        return Ok(IndexViewModelExtensions<SprintDto>
            .ToIndexViewModel(_mapper.Map<List<SprintDto>>(sprints), paginationFilter));
    }

    [HttpGet("{sprintId:guid}", Name = "GetSprintById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SprintFullDto>> GetSprint([FromRoute] Guid sprintId)
    {
        Sprint sprint = await _sprintsService.FindSprintById(sprintId);

        if (sprint is null) return NotFound();

        return Ok(_mapper.Map<SprintFullDto>(sprint));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSprint(
        [FromQuery] DateTime expirationTime,
        [FromQuery] Guid workTeamId,
        [FromQuery] Guid changerId)
    {
        Sprint sprint = await _sprintsService.CreateSprint(expirationTime, workTeamId, changerId);

        return CreatedAtRoute(
                "GetSprintById", new { sprintId = sprint.Id }, _mapper.Map<SprintDto>(sprint));
    }

    [HttpPut("{sprintId:guid}/add/{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTaskToSprint(
        [FromQuery] Guid changerId,
        [FromRoute] Guid sprintId,
        [FromRoute] Guid taskId)
    {
        Sprint updatedSprint = await _sprintsService.AddTaskToSprint(changerId, sprintId, taskId);

        return Ok(_mapper.Map<SprintDto>(updatedSprint));
    }

    [HttpPut("{sprintId:guid}/remove/{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveTaskFromSprint([FromRoute] Guid sprintId, [FromRoute] Guid taskId)
    {
        Sprint removedSprint = await _sprintsService.RemoveTaskFromSprint(sprintId, taskId);

        return Ok(_mapper.Map<SprintDto>(removedSprint));
    }
}