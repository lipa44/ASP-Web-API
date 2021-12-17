using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tools;
using ReportsWebApiLayer.DataTransferObjects;

namespace ReportsWebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        public Task<ActionResult<IEnumerable<EmployeeDto>>> Get() =>
            Task.FromResult<ActionResult<IEnumerable<EmployeeDto>>>(
                _mapper.Map<List<EmployeeDto>>(_employeeService.GetEmployees().Result));

        // GET: api/Employees/1
        [HttpGet("{id}", Name = "GetEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<FullEmployeeDto>> GetEmployee(Guid id)
        {
            Employee employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null) return NotFound();

            return _mapper.Map<FullEmployeeDto>(employee);
        }

        // POST: api/Employees
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<CreatedAtRouteResult> RegisterEmployee(string name, string surname, Guid id, EmployeeRoles role)
        {
            Employee newEmployee = await _employeeService.RegisterEmployee(id, name, surname, role);

            return CreatedAtRoute(
                "GetEmployee", new { id = newEmployee.Id }, _mapper.Map<EmployeeDto>(newEmployee));
        }

        // // PUT: api/Employees/1/workTeam/add
        // [HttpPut("{employeeId}/workTeam/add")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesDefaultResponseType]
        // public IActionResult SetWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId)
        // {
        //     Employee employee = _employeeService.SetWorkTeam(employeeId, changerId, workTeamId).Result;
        //
        //     if (employee == null) return NotFound();
        //
        //     return NoContent();
        // }

        // // PUT: api/Employees/1/workTeam/remove
        // [HttpPut("{employeeId}/workTeam/remove")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesDefaultResponseType]
        // public IActionResult RemoveWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId)
        // {
        //     Employee employee = _employeeService.RemoveWorkTeam(employeeId, changerId, workTeamId).Result;
        //
        //     if (employee == null) return NotFound();
        //
        //     return NoContent();
        // }

        // PUT: api/Employees/1/chief
        [HttpPut("{employeeId}/chief")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SetChief([FromRoute] Guid employeeId, [FromQuery] Guid chiefId)
        {
            await _employeeService.SetChief(employeeId, chiefId);

            return NoContent();
        }

        // PUT: api/Employees/1/chief
        [HttpPut("{employeeId}/commit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ReportDto> CommitChangesToReport([FromRoute] Guid employeeId)
        {
            Report newReport = await _employeeService.CommitChangesToReport(employeeId);

            return _mapper.Map<ReportDto>(newReport);
        }

        // public async Task<ReportDto> GetReport(Guid reportId)
        // {
        //     return _mapper.Map<ReportDto>(await _employeeService.GetReport(reportId));
        // }

        // PUT: api/Employees/1/chief
        [HttpPut("{employeeId}/createReport")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ReportDto> CreateReportForEmployee([FromRoute] Guid employeeId)
        {
            Report newReport = await _employeeService.CreateReport(employeeId);

            return _mapper.Map<ReportDto>(newReport);
        }

        // DELETE: api/Employees/1
        [HttpDelete("{employeeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> RemoveEmployee(Guid employeeId)
        {
            _employeeService.RemoveEmployee(employeeId);

            return Task.FromResult<IActionResult>(NoContent());
        }
    }
}