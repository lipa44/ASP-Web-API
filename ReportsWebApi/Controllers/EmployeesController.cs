namespace ReportsWebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Employees;
using ReportsDomain.Enums;
using ReportsInfrastructure.Services.Interfaces;
using DataTransferObjects;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeesService _employeesService;
    private readonly IMapper _mapper;

    public EmployeesController(IEmployeesService employeesService, IMapper mapper)
    {
        _employeesService = employeesService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetEmployees() =>
         Ok(_mapper.Map<List<EmployeeDto>>(await _employeesService.GetEmployeesAsync()));

    [HttpGet("{employeeId}", Name = "GetEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeFullDto>> GetEmployee([FromRoute] Guid employeeId)
    {
        Employee employee = await _employeesService.GetEmployeeByIdAsync(employeeId);
        return Ok(_mapper.Map<EmployeeFullDto>(employee));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterEmployee(
        [FromQuery] string name,
        [FromQuery] string surname,
        [FromQuery] Guid id,
        [FromQuery] EmployeeRoles role)
    {
        Employee newEmployee = await _employeesService.RegisterEmployee(id, name, surname, role);

        return CreatedAtRoute(
                "GetEmployee", new { employeeId = newEmployee.Id }, _mapper.Map<EmployeeDto>(newEmployee));
    }

    [HttpPut("{employeeId}/chief")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetChief([FromRoute] Guid employeeId, [FromQuery] Guid chiefId)
    {
        Employee updatedEmployee = await _employeesService.SetChief(employeeId, chiefId);
        return Ok(_mapper.Map<EmployeeFullDto>(updatedEmployee));
    }

    [HttpDelete("{employeeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveEmployee([FromRoute] Guid employeeId)
    {
        Employee removedEmployee = await _employeesService.RemoveEmployee(employeeId);
        return Ok(_mapper.Map<EmployeeFullDto>(removedEmployee));
    }
}