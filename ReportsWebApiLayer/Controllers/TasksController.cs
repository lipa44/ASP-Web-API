using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDataAccessLayer.DataTransferObjects;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Enums;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskChangeCommands;

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
    public async Task<ActionResult<IReadOnlyCollection<ReportsTaskDto>>> GetTasks() =>
        _mapper.Map<List<ReportsTaskDto>>(await _taskService.GetTasks());

    // GET: Tasks/1
    [HttpGet("{taskId}", Name = "GetTaskById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullReportsTaskDto>> GetTask([FromRoute] Guid taskId)
    {
        try
        {
            ReportsTask reportsTask = await _taskService.GetTaskById(taskId);
            return _mapper.Map<FullReportsTaskDto>(reportsTask);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTask(string taskName, Guid creatorId)
    {
        try
        {
            ReportsTask reportsTask = await _taskService.CreateTask(taskName, creatorId);

            return CreatedAtRoute(
                "GetTaskById", new { taskId = reportsTask.Id }, _mapper.Map<ReportsTaskDto>(reportsTask));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // GET: Tasks/1
    [HttpGet("byCreationTime/{creationTime}", Name = "GetTasksByCreationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> FindTasksByCreationTime([FromRoute] DateTime creationTime)
    {
        IReadOnlyCollection<ReportsTask> tasksByCreationTime = await _taskService.FindTasksByCreationTime(creationTime);

        if (tasksByCreationTime == null || tasksByCreationTime.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksByCreationTime);
    }

    // GET: Tasks/1
    [HttpGet("byModificationTime/{modificationTime}", Name = "GetTasksByModificationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> FindTasksByModificationTime(
        [FromRoute] DateTime modificationTime)
    {
        IReadOnlyCollection<ReportsTask> tasksByModificationTime =
            await _taskService.FindTasksByModificationDate(modificationTime);

        if (tasksByModificationTime == null || tasksByModificationTime.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksByModificationTime);
    }

    // GET: Tasks/1
    [HttpGet("byEmployee/{employeeId}", Name = "GetTasksByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> GetTasksByEmployee([FromRoute] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> employeeTasks = await _taskService.FindTasksByEmployeeId(employeeId);

        if (employeeTasks == null || employeeTasks.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(employeeTasks);
    }

    // GET: Tasks/1
    [HttpGet("ModifiedByEmployee/{employeeId}", Name = "GetTasksModifiedByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> GetTasksModifiedByEmployee([FromRoute] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> tasksModifiedByEmployee
            = await _taskService.FindsTaskModifiedByEmployeeId(employeeId);

        if (tasksModifiedByEmployee == null || tasksModifiedByEmployee.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksModifiedByEmployee);
    }

    // GET: Tasks/1
    [HttpGet("CreatedByEmployeeSubordinates/{employeeId}", Name = "GetTasksCreatedByEmployeeSubordinates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> GetTasksCreatedByEmployeeSubordinates(
        [FromRoute] Guid employeeId)
    {
        IReadOnlyCollection<ReportsTask> tasksCreatedByEmployeeSubordinates
            = await _taskService.FindTasksCreatedByEmployeeSubordinates(employeeId);

        if (tasksCreatedByEmployeeSubordinates == null || tasksCreatedByEmployeeSubordinates.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksCreatedByEmployeeSubordinates);
    }

    [HttpPut("{taskId}/owner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> SetOwner(
        [Required] Guid taskId,
        [Required] Guid changerId,
        [FromQuery] Guid ownerId)
    {
        try
        {
            _taskService.UseChangeTaskCommand(taskId, changerId, new SetTaskOwnerCommand(ownerId));
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(Problem(e.Message));
        }
    }

    [HttpPut("{taskId}/content")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> SetContent(
        [Required] Guid taskId,
        [Required] Guid changerId,
        [FromQuery] string content)
    {
        try
        {
            _taskService.UseChangeTaskCommand(taskId, changerId, new SetTaskContentCommand(content));
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(Problem(e.Message));
        }
    }

    [HttpPut("{taskId}/comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> AddComment(
        [Required] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string comment)
    {
        try
        {
            _taskService.UseChangeTaskCommand(taskId, changerId, new AddTaskCommentCommand(comment));
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(Problem(e.Message));
        }
    }

    [HttpPut("{taskId}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> SetTitle(
        [Required] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string title)
    {
        try
        {
            _taskService.UseChangeTaskCommand(taskId, changerId, new SetTaskTitleCommand(title));
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(Problem(e.Message));
        }
    }

    [HttpPut("{taskId}/state")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> SetState(
        [Required] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] TaskStates state)
    {
        try
        {
            _taskService.UseChangeTaskCommand(taskId, changerId, new SetTaskStateCommand(state));
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(Problem(e.Message));
        }
    }

    [HttpDelete("{taskId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete(Guid taskId)
    {
        try
        {
            _taskService.RemoveTaskById(taskId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}