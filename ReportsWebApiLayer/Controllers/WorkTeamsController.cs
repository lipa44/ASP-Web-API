using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Entities;
using ReportsWebApiLayer.DataTransferObjects;

namespace ReportsWebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTeamsController : ControllerBase
    {
        private readonly IWorkTeamService _workTeamService;
        private readonly IMapper _mapper;

        public WorkTeamsController(IWorkTeamService workTeamService, IMapper mapper)
        {
            _workTeamService = workTeamService;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        public Task<ActionResult<IEnumerable<WorkTeamDto>>> Get() =>
            Task.FromResult<ActionResult<IEnumerable<WorkTeamDto>>>(
                _mapper.Map<List<WorkTeamDto>>(_workTeamService.GetWorkTeams().Result));

        // GET: api/Employees/1
        [HttpGet("{workTeamId}", Name = "GetWorkTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<FullWorkTeamDto>> GetWorkTeam(Guid workTeamId)
        {
            WorkTeam workTeam = await _workTeamService.GetWorkTeamById(workTeamId);

            if (workTeam == null) return NotFound();

            return _mapper.Map<FullWorkTeamDto>(workTeam);
        }

        // POST: api/Employees
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<WorkTeamDto> RegisterWorkTeam(Guid leadId, string workTeamName)
        {
            WorkTeam newWorkTeam = await _workTeamService.RegisterWorkTeam(leadId, workTeamName);

            return _mapper.Map<WorkTeamDto>(newWorkTeam);

            // return CreatedAtRoute(
            //     "GetWorkTeam", new { id = newWorkTeam.Id }, _mapper.Map<WorkTeamDto>(newWorkTeam));
        }

        // PUT: api/Employees/1/workTeam/add
        [HttpPut("{workTeamId}/add")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid workTeamId)
        {
            _workTeamService.AddEmployeeToTeam(employeeId, changerId, workTeamId);

            return NoContent();
        }

        // PUT: api/Employees/1/workTeam/add
        [HttpPut("{workTeamId}/remove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid workTeamId)
        {
            _workTeamService.RemoveEmployeeFromTeam(employeeId, changerId, workTeamId);

            return NoContent();
        }

        // PUT: api/Employees/1/workTeam/add
        [HttpPut("{workTeamId}/sprints/add")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult AddSprintToWorkTeam(Guid changerId, Guid workTeamId, DateTime sprintExpirationDate)
        {
            _workTeamService.AddSprintToTeam(workTeamId, changerId, sprintExpirationDate);

            return NoContent();
        }

        // PUT: api/Employees/1/workTeam/add
        [HttpPut("{workTeamId}/sprints/remove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult RemoveSprintFromWorkTeam(Guid changerId, Guid workTeamId, Guid sprintId)
        {
            _workTeamService.RemoveSprintFromTeam(workTeamId, changerId, sprintId);

            return NoContent();
        }

        // // PUT: api/Employees/1/workTeam/remove
        // [HttpPut("{employeeId}/workTeam/remove")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesDefaultResponseType]
        // public IActionResult RemoveWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId)
        // {
        //     Employee employee = _workTeamService.RemoveWorkTeam(employeeId, changerId, workTeamId).Result;
        //
        //     if (employee == null) return NotFound();
        //
        //     return NoContent();
        // }
        //
        // // PUT: api/Employees/1/chief
        // [HttpPut("{employeeId}/chief")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesDefaultResponseType]
        // public async Task<IActionResult> SetChief([FromRoute] Guid employeeId, [FromQuery] Guid chiefId)
        // {
        //     await _workTeamService.SetChief(employeeId, chiefId);
        //
        //     return NoContent();
        // }
        //
        // // PUT: api/Employees/1/chief
        // [HttpPut("{employeeId}/commit")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesDefaultResponseType]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // public Task<IActionResult> CommitChangesToReport([FromRoute] Guid employeeId)
        // {
        //     _workTeamService.CommitChangesToReport(employeeId);
        //
        //     return Task.FromResult<IActionResult>(NoContent());
        // }
        //
        // // DELETE: api/Employees/1
        // [HttpDelete("{employeeId}")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesDefaultResponseType]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // public Task<IActionResult> RemoveEmployee(Guid employeeId)
        // {
        //     _workTeamService.RemoveEmployee(employeeId);
        //
        //     return Task.FromResult<IActionResult>(NoContent());
        // }
    }
}