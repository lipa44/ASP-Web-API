using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;
using ReportsWebApiLayer.Services.Interfaces;
using ReportsTask = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ReportsTasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public ReportsTasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<List<ReportsTask>>> Get()
        {
            return _taskService.GetTasks().ToList();
        }

        // GET: api/Tasks/1
        [HttpGet("{id}", Name = "GetTask")]
        public async Task<ActionResult<ReportsTask>> Get(Guid id)
        {
            ReportsTask? task = await _taskService.FindTaskById(id);

            if (task == null) return NotFound();

            return task;
        }

        [HttpPost]
        public async Task<CreatedAtRouteResult> Create(string taskName)
        {

            ReportsTask task = await _taskService.CreateTask(taskName);

            return CreatedAtRoute("GetTask", new {id = task.Id}, task);
        }

        [HttpPut("{taskId}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            ReportsTask task = await _taskService.GetTaskById(id);

            _taskService.RemoveTaskById(task.Id);

            return NoContent();
        }
    }
}