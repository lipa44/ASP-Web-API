namespace ReportsWebApi.Controllers;

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
    public async Task<ActionResult<IReadOnlyCollection<ReportsTaskDto>>> GetTasks() =>
        Ok(_mapper.Map<List<ReportsTaskDto>>(await _tasksService.GetTasks()));

    [HttpGet("{taskId}", Name = "GetTaskById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportsTaskFullDto>> GetTask([FromRoute] Guid taskId)
    {
        ReportsTask reportsTask = await _tasksService.GetTaskById(taskId);
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
    public async Task<ActionResult<List<ReportsTaskFullDto>>> FindTasksByCreationTime([FromQuery] DateTime creationTime)
    {
        IReadOnlyCollection<ReportsTask> tasksByCreationTime = await _tasksService.FindTasksByCreationTime(creationTime);

        if (tasksByCreationTime == null || tasksByCreationTime.Count == 0) return NotFound();

        return Ok(_mapper.Map<List<ReportsTaskFullDto>>(tasksByCreationTime));
    }

    [HttpGet("byModificationTime", Name = "GetTasksByModificationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReportsTaskFullDto>>> FindTasksByModificationTime(
        [FromQuery] DateTime modificationTime)
    {
        IReadOnlyCollection<ReportsTask> tasksByModificationTime =
            await _tasksService.FindTasksByModificationDate(modificationTime);

        if (tasksByModificationTime == null || tasksByModificationTime.Count == 0) return NotFound();

        return Ok(_mapper.Map<List<ReportsTaskFullDto>>(tasksByModificationTime));
    }

    [HttpGet("byEmployee", Name = "GetTasksByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReportsTaskFullDto>>> GetTasksByEmployee([FromQuery] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> employeeTasks = await _tasksService.FindTasksByEmployeeId(employeeId);

        if (employeeTasks == null || employeeTasks.Count == 0) return NotFound();

        return Ok(_mapper.Map<List<ReportsTaskFullDto>>(employeeTasks));
    }

    [HttpGet("ModifiedByEmployee", Name = "GetTasksModifiedByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReportsTaskFullDto>>> GetTasksModifiedByEmployee([FromQuery] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> tasksModifiedByEmployee
            = await _tasksService.FindsTaskModifiedByEmployeeId(employeeId);

        if (tasksModifiedByEmployee == null || tasksModifiedByEmployee.Count == 0) return NotFound();

        return Ok(_mapper.Map<List<ReportsTaskFullDto>>(tasksModifiedByEmployee));
    }

    [HttpGet("CreatedByEmployeeSubordinates", Name = "GetTasksCreatedByEmployeeSubordinates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReportsTaskFullDto>>> GetTasksCreatedByEmployeeSubordinates(
        [FromQuery] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> tasksCreatedByEmployeeSubordinates
            = await _tasksService.FindTasksCreatedByEmployeeSubordinates(employeeId);

        if (tasksCreatedByEmployeeSubordinates == null || tasksCreatedByEmployeeSubordinates.Count == 0) return NotFound();

        return Ok(_mapper.Map<List<ReportsTaskFullDto>>(tasksCreatedByEmployeeSubordinates));
    }

    [HttpPut("{taskId}/content")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetContent(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string content)
    {
        try
        {
            ReportsTask updatedTask
                = await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskContentCommand(content));

            return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut("{taskId}/owner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetOwner(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] Guid ownerId)
    {
        ReportsTask updatedTask
            = await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskOwnerCommand(ownerId));

        return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
    }

    [HttpPut("{taskId}/comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddComment(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string comment)
    {
        ReportsTask updatedTask
                = await _tasksService.UseChangeTaskCommand(taskId, changerId, new AddTaskCommentCommand(comment));

        return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
    }

    [HttpPut("{taskId}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetTitle(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string title)
    {
        ReportsTask updatedTask
            = await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskTitleCommand(title));

        return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
    }

    [HttpPut("{taskId}/state")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetState(
        [FromRoute] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] TaskStates state)
    {
        ReportsTask updatedTask
                = await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskStateCommand(state));

        return Ok(_mapper.Map<ReportsTaskFullDto>(updatedTask));
    }

    [HttpDelete("{taskId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid taskId)
    {
        ReportsTask removedTask = await _tasksService.RemoveTaskById(taskId);
        return Ok(_mapper.Map<ReportsTaskFullDto>(removedTask));
    }
}