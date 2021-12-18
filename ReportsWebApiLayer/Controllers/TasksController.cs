using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Enums;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskChangeCommands;
using ReportsWebApiLayer.DataTransferObjects;

namespace ReportsWebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public TasksController(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    // GET: Tasks
    [HttpGet]
    public Task<ActionResult<List<ReportsTaskDto>>> GetTasks() =>
        Task.FromResult<ActionResult<List<ReportsTaskDto>>>(
            _mapper.Map<List<ReportsTaskDto>>(_taskService.GetTasks().Result));

    // GET: Tasks/1
    [HttpGet("{taskId}", Name = "GetTaskById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<FullReportsTaskDto>> GetTask([FromRoute] Guid taskId)
    {
        ReportsTask reportsTask = await _taskService.GetTaskById(taskId);

        if (reportsTask == null) return NotFound();

        return _mapper.Map<FullReportsTaskDto>(reportsTask);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<CreatedAtRouteResult> CreateTask(string taskName, Guid creatorId)
    {
        ReportsTask reportsTask = await _taskService.CreateTask(taskName, creatorId);

        return CreatedAtRoute(
            "GetTaskById", new { taskId = reportsTask.Id }, _mapper.Map<ReportsTaskDto>(reportsTask));
    }

    // GET: Tasks/1
    [HttpGet("byCreationTime/{creationTime}", Name = "GetTasksByCreationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<List<FullReportsTaskDto>>> FindTasksByCreationTime([FromRoute] DateTime creationTime)
    {
        IReadOnlyCollection<ReportsTask> tasksByCreationTime = await _taskService.FindTasksByCreationTime(creationTime);

        if (tasksByCreationTime == null) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksByCreationTime);
    }

    // GET: Tasks/1
    [HttpGet("byModificationTime/{modificationTime}", Name = "GetTasksByModificationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<List<FullReportsTaskDto>>> FindTasksByModificationTime(
        [FromRoute] DateTime modificationTime)
    {
        IReadOnlyCollection<ReportsTask> tasksByModificationTime =
            await _taskService.FindTasksByModificationDate(modificationTime);

        if (tasksByModificationTime == null) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksByModificationTime);
    }

    // GET: Tasks/1
    [HttpGet("byEmployee/{employeeId}", Name = "GetTasksByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<List<FullReportsTaskDto>>> GetTasksByEmployee([FromRoute] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> employeeTasks = await _taskService.FindTasksByEmployeeId(employeeId);

        if (employeeTasks == null) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(employeeTasks);
    }

    // GET: Tasks/1
    [HttpGet("ModifiedByEmployee/{employeeId}", Name = "GetTasksModifiedByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<List<FullReportsTaskDto>>> GetTasksModifiedByEmployee([FromRoute] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> tasksModifiedByEmployee
            = await _taskService.FindsTaskModifiedByEmployeeId(employeeId);

        if (tasksModifiedByEmployee == null) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksModifiedByEmployee);
    }

    // GET: Tasks/1
    [HttpGet("CreatedByEmployeeSubordinates/{employeeId}", Name = "GetTasksCreatedByEmployeeSubordinates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<List<FullReportsTaskDto>>> GetTasksCreatedByEmployeeSubordinates(
        [FromRoute] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> tasksCreatedByEmployeeSubordinates
            = await _taskService.FindTasksCreatedByEmployeeSubordinates(employeeId);

        if (tasksCreatedByEmployeeSubordinates == null) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksCreatedByEmployeeSubordinates);
    }

    [HttpPut("{taskId}/owner")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> SetOwner(
        [Required] Guid taskId,
        [Required] Guid changerId,
        [FromQuery] Guid ownerId)
    {
        var setOwnerCommand = new SetTaskOwnerCommand(ownerId);

        _taskService.UseChangeTaskCommand(taskId, changerId, setOwnerCommand);

        return Task.FromResult<IActionResult>(NoContent());
    }

    [HttpPut("{taskId}/content")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> SetContent(
        [Required] Guid taskId,
        [Required] Guid changerId,
        [FromQuery] string content)
    {
        var setContentCommand = new SetTaskContentCommand(content);

        _taskService.UseChangeTaskCommand(taskId, changerId, setContentCommand);

        return Task.FromResult<IActionResult>(NoContent());
    }

    [HttpPut("{taskId}/comment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> AddComment(
        [Required] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string comment)
    {
        var addCommentCommand = new AddTaskCommentCommand(comment);

        _taskService.UseChangeTaskCommand(taskId, changerId, addCommentCommand);

        return Task.FromResult<IActionResult>(NoContent());
    }

    [HttpPut("{taskId}/title")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> SetTitle(
        [Required] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string title)
    {
        var setTitleCommand = new SetTaskTitleCommand(title);

        _taskService.UseChangeTaskCommand(taskId, changerId, setTitleCommand);

        return Task.FromResult<IActionResult>(NoContent());
    }

    [HttpPut("{taskId}/state")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> SetState(
        [Required] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] TaskStates state)
    {
        var setStateCommand = new SetTaskStateCommand(state);

        _taskService.UseChangeTaskCommand(taskId, changerId, setStateCommand);

        return Task.FromResult<IActionResult>(NoContent());
    }

    [HttpDelete("{taskId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> Delete(Guid taskId)
    {
        _taskService.RemoveTaskById(taskId);

        return Task.FromResult<IActionResult>(NoContent());
    }
}