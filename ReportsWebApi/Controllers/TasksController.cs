namespace ReportsWebApi.Controllers;

using Extensions;
using Filters;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Enums;
using ReportsDomain.Tasks;
using ReportsDomain.Tasks.TaskChangeCommands;
using ReportsInfrastructure.Services.Interfaces;
using DataTransferObjects;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITasksService _tasksService;
    private readonly IMapper _mapper;

    public TasksController(ITasksService tasksService, IMapper mapper)
    {
        _tasksService = tasksService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ReportsTaskDto>>> GetTasks(
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        List<ReportsTask> reportsTasks = await _tasksService.GetTasks();

        var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

        return Ok(IndexViewModelExtensions<ReportsTaskDto>
            .ToIndexViewModel(_mapper.Map<List<ReportsTaskDto>>(reportsTasks), paginationFilter));
    }

    [HttpGet("{taskId:guid}", Name = "GetTaskById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportsTaskFullDto>> GetTask([FromRoute] Guid taskId)
    {
        ReportsTask reportsTask = await _tasksService.FindTaskById(taskId);

        if (reportsTask is null) return NotFound();

        return Ok(_mapper.Map<ReportsTaskFullDto>(reportsTask));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTask([FromQuery] string taskName)
    {
        ReportsTask reportsTask = await _tasksService.CreateTask(taskName);

        return CreatedAtRoute(
            "GetTaskById", new { taskId = reportsTask.Id }, _mapper.Map<ReportsTaskDto>(reportsTask));
    }

    [HttpGet("byCreationTime", Name = "GetTasksByCreationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReportsTaskFullDto>>> FindTasksByCreationTime(
        [FromQuery] DateTime creationTime,
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        IReadOnlyCollection<ReportsTask> tasksByCreationTime = await _tasksService.FindTasksByCreationTime(creationTime);

        if (tasksByCreationTime == null || tasksByCreationTime.Count == 0)
            return NotFound();

        var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

        return Ok(IndexViewModelExtensions<ReportsTaskFullDto>
            .ToIndexViewModel(_mapper.Map<List<ReportsTaskFullDto>>(tasksByCreationTime), paginationFilter));
    }

    [HttpGet("byModificationTime", Name = "GetTasksByModificationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReportsTaskFullDto>>> FindTasksByModificationTime(
        [FromQuery] DateTime modificationTime,
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        IReadOnlyCollection<ReportsTask> tasksByModificationTime =
            await _tasksService.FindTasksByModificationDate(modificationTime);

        if (tasksByModificationTime == null || tasksByModificationTime.Count == 0)
            return NotFound();

        var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

        return Ok(IndexViewModelExtensions<ReportsTaskFullDto>
            .ToIndexViewModel(_mapper.Map<List<ReportsTaskFullDto>>(tasksByModificationTime), paginationFilter));
    }

    [HttpGet("byEmployee", Name = "GetTasksByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReportsTaskFullDto>>> GetTasksByEmployee(
        [FromQuery] Guid employeeId,
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        IReadOnlyCollection<ReportsTask> employeeTasks = await _tasksService.FindTasksByEmployeeId(employeeId);

        if (employeeTasks == null || employeeTasks.Count == 0)
            return NotFound();

        var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

        return Ok(IndexViewModelExtensions<ReportsTaskFullDto>
            .ToIndexViewModel(_mapper.Map<List<ReportsTaskFullDto>>(employeeTasks), paginationFilter));
    }

    [HttpGet("ModifiedByEmployee", Name = "GetTasksModifiedByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReportsTaskFullDto>>> GetTasksModifiedByEmployee(
        [FromQuery] Guid employeeId,
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        IReadOnlyCollection<ReportsTask> tasksModifiedByEmployee
            = await _tasksService.FindsTaskModifiedByEmployeeId(employeeId);

        if (tasksModifiedByEmployee == null || tasksModifiedByEmployee.Count == 0)
            return NotFound();

        var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

        return Ok(IndexViewModelExtensions<ReportsTaskFullDto>
            .ToIndexViewModel(_mapper.Map<List<ReportsTaskFullDto>>(tasksModifiedByEmployee), paginationFilter));
    }

    [HttpGet("CreatedByEmployeeSubordinates", Name = "GetTasksCreatedByEmployeeSubordinates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReportsTaskFullDto>>> GetTasksCreatedByEmployeeSubordinates(
        [FromQuery] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> tasksCreatedByEmployeeSubordinates
            = await _tasksService.FindTasksCreatedByEmployeeSubordinates(employeeId);

        if (tasksCreatedByEmployeeSubordinates == null || tasksCreatedByEmployeeSubordinates.Count == 0)
            return NotFound();

        return Ok(_mapper.Map<List<ReportsTaskFullDto>>(tasksCreatedByEmployeeSubordinates));
    }

    [HttpPut("{taskId:guid}/content")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetContent(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string content)
    {
        ReportsTask updatedTask
                = await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskContentCommand(content));

        return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
    }

    [HttpPut("{taskId:guid}/owner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetOwner(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] Guid ownerId)
    {
        ReportsTask updatedTask
            = await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskOwnerCommand(ownerId));

        return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
    }

    [HttpPut("{taskId:guid}/comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddComment(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string comment)
    {
        ReportsTask updatedTask
                = await _tasksService.UseChangeTaskCommand(taskId, changerId, new AddTaskCommentCommand(comment));

        return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
    }

    [HttpPut("{taskId:guid}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetTitle(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string title)
    {
        ReportsTask updatedTask
            = await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskTitleCommand(title));

        return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
    }

    [HttpPut("{taskId:guid}/state")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetState(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] ReportTaskStates state)
    {
        ReportsTask updatedTask
                = await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskStateCommand(state));

        return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
    }

    [HttpDelete("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveTask([FromRoute] Guid taskId)
    {
        await _tasksService.RemoveTaskById(taskId);

        return NoContent();
    }
}