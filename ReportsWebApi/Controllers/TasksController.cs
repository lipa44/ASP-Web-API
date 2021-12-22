using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Enums;
using ReportsDomain.Tasks;
using ReportsDomain.Tasks.TaskChangeCommands;
using ReportsInfrastructure.Services.Interfaces;
using ReportsWebApi.DataTransferObjects;
using ReportsWebApi.Extensions;

namespace ReportsWebApi.Controllers;

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

    // GET: Tasks
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ReportsTaskDto>>> GetTasks() =>
        _mapper.Map<List<ReportsTaskDto>>(await _tasksService.GetTasks());

    // GET: Tasks/1
    [HttpGet("{taskId}", Name = "GetTaskById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FullReportsTaskDto>> GetTask([FromRoute] Guid taskId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            ReportsTask reportsTask = await _tasksService.GetTaskById(taskId);
            return _mapper.Map<FullReportsTaskDto>(reportsTask);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask(string taskName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            ReportsTask reportsTask = await _tasksService.CreateTask(taskName);

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
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> FindTasksByCreationTime([FromRoute] DateTime creationTime)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        IReadOnlyCollection<ReportsTask> tasksByCreationTime = await _tasksService.FindTasksByCreationTime(creationTime);

        if (tasksByCreationTime == null || tasksByCreationTime.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksByCreationTime);
    }

    // GET: Tasks/1
    [HttpGet("byModificationTime/{modificationTime}", Name = "GetTasksByModificationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> FindTasksByModificationTime(
        [FromRoute] DateTime modificationTime)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        IReadOnlyCollection<ReportsTask> tasksByModificationTime =
            await _tasksService.FindTasksByModificationDate(modificationTime);

        if (tasksByModificationTime == null || tasksByModificationTime.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksByModificationTime);
    }

    // GET: Tasks/1
    [HttpGet("byEmployee/{employeeId}", Name = "GetTasksByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> GetTasksByEmployee([FromRoute] Guid employeeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        IReadOnlyCollection<ReportsTask> employeeTasks = await _tasksService.FindTasksByEmployeeId(employeeId);

        if (employeeTasks == null || employeeTasks.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(employeeTasks);
    }

    // GET: Tasks/1
    [HttpGet("ModifiedByEmployee/{employeeId}", Name = "GetTasksModifiedByEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> GetTasksModifiedByEmployee([FromRoute] Guid employeeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        IReadOnlyCollection<ReportsTask> tasksModifiedByEmployee
            = await _tasksService.FindsTaskModifiedByEmployeeId(employeeId);

        if (tasksModifiedByEmployee == null || tasksModifiedByEmployee.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksModifiedByEmployee);
    }

    // GET: Tasks/1
    [HttpGet("CreatedByEmployeeSubordinates/{employeeId}", Name = "GetTasksCreatedByEmployeeSubordinates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<FullReportsTaskDto>>> GetTasksCreatedByEmployeeSubordinates(
        [FromRoute] Guid employeeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        IReadOnlyCollection<ReportsTask> tasksCreatedByEmployeeSubordinates
            = await _tasksService.FindTasksCreatedByEmployeeSubordinates(employeeId);

        if (tasksCreatedByEmployeeSubordinates == null || tasksCreatedByEmployeeSubordinates.Count == 0) return NotFound();

        return _mapper.Map<List<FullReportsTaskDto>>(tasksCreatedByEmployeeSubordinates);
    }

    [HttpPut("{taskId}/owner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetOwner(
        [Required] Guid taskId,
        [Required] Guid changerId,
        [FromQuery] Guid ownerId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskOwnerCommand(ownerId));
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut("{taskId}/content")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetContent(
        [Required] Guid taskId,
        [Required] Guid changerId,
        [FromQuery] string content)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskContentCommand(content));
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut("{taskId}/comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddComment(
        [Required] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string comment)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            await _tasksService.UseChangeTaskCommand(taskId, changerId, new AddTaskCommentCommand(comment));
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut("{taskId}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetTitle(
        [Required] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] string title)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskTitleCommand(title));
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut("{taskId}/state")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetState(
        [Required] Guid taskId,
        [FromQuery] Guid changerId,
        [FromQuery] TaskStates state)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            await _tasksService.UseChangeTaskCommand(taskId, changerId, new SetTaskStateCommand(state));
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpDelete("{taskId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public IActionResult Delete(Guid taskId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            _tasksService.RemoveTaskById(taskId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}