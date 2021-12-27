using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Employees;
using ReportsDomain.Enums;
using ReportsInfrastructure.Services.Interfaces;
using ReportsWebApi.DataTransferObjects;
using ReportsWebApi.Extensions;

namespace ReportsWebApi.Controllers;

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

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetEmployees() =>
         Ok(_mapper.Map<List<EmployeeDto>>(await _employeesService.GetEmployeesAsync()));

    [HttpGet("{employeeId}", Name = "GetEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullEmployeeDto>> GetEmployee(Guid employeeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            Employee employee = await _employeesService.GetEmployeeByIdAsync(employeeId);
            return Ok(_mapper.Map<FullEmployeeDto>(employee));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterEmployee(string name, string surname, Guid id, EmployeeRoles role)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            Employee newEmployee = await _employeesService.RegisterEmployee(id, name, surname, role);

            return CreatedAtRoute(
                "GetEmployee", new { employeeId = newEmployee.Id }, _mapper.Map<EmployeeDto>(newEmployee));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut("{employeeId}/chief")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetChief([FromRoute] Guid employeeId, [FromQuery] Guid chiefId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            Employee updatedEmployee = await _employeesService.SetChief(employeeId, chiefId);
            return Ok(_mapper.Map<FullEmployeeDto>(updatedEmployee));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpDelete("{employeeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveEmployee(Guid employeeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            Employee removedEmployee = await _employeesService.RemoveEmployee(employeeId);
            return Ok(_mapper.Map<FullEmployeeDto>(removedEmployee));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}