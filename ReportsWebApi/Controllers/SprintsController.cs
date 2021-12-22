using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Entities;
using ReportsInfrastructure.Services.Interfaces;
using ReportsWebApi.DataTransferObjects;
using ReportsWebApi.Extensions;

namespace ReportsWebApi.Controllers;

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

        // GET: Sprints
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<SprintDto>>> GetSprints() =>
            _mapper.Map<List<SprintDto>>(await _sprintsService.GetSprintsAsync());

        // GET: Sprints/1
        [HttpGet("{sprintId}", Name = "GetSprintById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SprintDto>> GetSprint([FromRoute] Guid sprintId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                Sprint sprint = await _sprintsService.GetSprintByIdAsync(sprintId);
                return _mapper.Map<SprintDto>(sprint);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSprint(DateTime expirationTime, Guid workTeamId, Guid changerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                Sprint sprint = await _sprintsService.CreateSprint(expirationTime, workTeamId, changerId);

                return CreatedAtRoute(
                    "GetSprintById", new { sprintId = sprint.SprintId }, _mapper.Map<SprintDto>(sprint));
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPut("{sprintId}/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTaskToSprint([FromRoute] Guid sprintId, Guid taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                await _sprintsService.AddTaskToSprint(sprintId, taskId);
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPut("{sprintId}/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveTaskFromSprint(Guid sprintId, Guid taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                await _sprintsService.RemoveTaskFromSprint(sprintId, taskId);
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
}