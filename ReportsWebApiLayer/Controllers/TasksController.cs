using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;
using ReportsLibrary.Tools;
using ReportsWebApiLayer.Services.Interfaces;
using ReportsTask = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        private readonly Employee _lipa =
            new ("Misha", "Libchenko", new Guid("11111111-1111-1111-1111-111111111111"), EmployeeRoles.TeamLead);

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/Tasks
        [HttpGet]
        public Task<ActionResult<List<ReportsTask>>> Get()
        {
            return Task.FromResult<ActionResult<List<ReportsTask>>>(_taskService.GetTasks().Result.ToList());
        }

        // GET: api/Tasks/1
        [HttpGet("{id}", Name = "GetTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ReportsTask>> Get(Guid id)
        {
            ReportsTask task = await _taskService.GetTaskById(id);

            if (task == null) return NotFound();

            return task;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<CreatedAtRouteResult> Create(string taskName)
        {
            ReportsTask task = await _taskService.CreateTask(taskName);

            return CreatedAtRoute("GetTask", new { id = task.Id }, task);
        }

        [HttpPut("{taskId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Update(
            [Required] Guid taskId,
            [FromQuery] string taskNewName,
            [FromQuery] string taskNewContent,
            [FromQuery] string taskNewComment,
            [FromQuery] Employee taskNewImplementor,
            [FromQuery] TaskState taskNewState)
        {
            ReportsTask task = await _taskService.GetTaskById(taskId);

            // _taskService.Update(id, taskIn);
            return NoContent();
        }

        [HttpPut("{taskId}/owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SetOwner(
            [Required] Guid taskId,
            [FromQuery] Guid ownerId)
        {
            await _taskService.SetOwner(taskId, ownerId, ownerId);

            return NoContent();
        }

        [HttpPut("{taskId}/content")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> SetContent(
            [Required] Guid taskId,
            [FromQuery] string content)
        {
            _taskService.SetContent(taskId, _lipa.Id, content);

            return Task.FromResult<IActionResult>(NoContent());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(Guid id)
        {
            ReportsTask task = await _taskService.GetTaskById(id);

            _taskService.RemoveTaskById(task.Id);

            return NoContent();
        }
    }
}