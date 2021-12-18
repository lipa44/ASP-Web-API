using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDataAccessLayer.DataTransferObjects;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Enums;

namespace ReportsWebApiLayer.Controllers;

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
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetEmployees() =>
         _mapper.Map<List<EmployeeDto>>(await _employeeService.GetEmployees());

    // GET: api/Employees/1
    [HttpGet("{employeeId}", Name = "GetEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullEmployeeDto>> GetEmployee(Guid employeeId)
    {
        try
        {
            Employee employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            return _mapper.Map<FullEmployeeDto>(employee);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    // POST: api/Employees?name=asd&surname=asd&id=52&role=1
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterEmployee(string name, string surname, Guid id, EmployeeRoles role)
    {
        try
        {
            Employee newEmployee = await _employeeService.RegisterEmployee(id, name, surname, role);

            return CreatedAtRoute(
                "GetEmployee", new { id = newEmployee.Id }, _mapper.Map<EmployeeDto>(newEmployee));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/Employees/1/chief?chiefId=5
    [HttpPut("{employeeId}/chief")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetChief([FromRoute] Guid employeeId, [FromQuery] Guid chiefId)
    {
        try
        {
            await _employeeService.SetChief(employeeId, chiefId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/Employees/1/commit
    [HttpPut("{employeeId}/commit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CommitChangesToReport([FromRoute] Guid employeeId)
    {
        try
        {
            await _employeeService.CommitChangesToReport(employeeId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/Employees/1/createReport
    [HttpPut("{employeeId}/createReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateReportForEmployee([FromRoute] Guid employeeId)
    {
        try
        {
            await _employeeService.CreateReport(employeeId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // DELETE: api/Employees/1
    [HttpDelete("{employeeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult RemoveEmployee(Guid employeeId)
    {
        try
        {
            _employeeService.RemoveEmployee(employeeId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}