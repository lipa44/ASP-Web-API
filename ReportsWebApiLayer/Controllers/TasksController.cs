using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;
using ReportsWebApiLayer.DataBase.Dto;
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
        public async Task<ActionResult<List<TaskDto>>> Get()
        {
            return _taskService.GetTasks().Result.Value?.Select(TaskToDto).ToList()!;
        }

        // GET: api/Tasks/1
        [HttpGet("{id}", Name = "GetTask")]
        public async Task<ActionResult<TaskDto>> Get(Guid id)
        {
            ReportsTask? task = await _taskService.FindTaskById(id);

            if (task == null) return NotFound();

            return TaskToDto(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create(Employee implementor, string taskName)
        {
            ReportsTask task = await _taskService.CreateTask(implementor, taskName);

            return CreatedAtRoute("GetTask", new {id = task.Id}, TaskToDto(task));
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> Update(
            [Required] Guid taskId,
            [FromQuery] string taskNewName,
            [FromQuery] string taskNewContent,
            [FromQuery] string taskNewComment,
            [FromQuery] Employee taskNewImplementor,
            [FromQuery] ITaskState taskNewState)
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

        private static TaskDto TaskToDto(ReportsTask taskToDto) => new(taskToDto);
    }
}