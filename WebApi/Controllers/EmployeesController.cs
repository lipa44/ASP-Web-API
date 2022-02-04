using AutoMapper;
using DataAccess.Dto;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using WebApi.Extensions;
using WebApi.Filters;

namespace WebApi.Controllers;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetEmployees(
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        IReadOnlyCollection<Employee> employees = await _employeesService.GetEmployees();

        var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

        return Ok(IndexViewModelExtensions<EmployeeDto>
            .ToIndexViewModel(_mapper.Map<List<EmployeeDto>>(employees), paginationFilter));
    }

    [HttpGet("{employeeId:guid}", Name = "GetEmployee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeFullDto>> GetEmployee([FromRoute] Guid employeeId)
    {
        Employee employee = await _employeesService.FindEmployeeById(employeeId);

        if (employee is null) return NotFound();

        return Ok(_mapper.Map<EmployeeFullDto>(employee));
    }

    [HttpGet("{employeeId:guid}/subordinates", Name = "GetEmployeeSubordinates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EmployeeFullDto>>> GetEmployeeSubordinates([FromRoute] Guid employeeId)
    {
        var subordinates = await _employeesService.GetEmployeeSubordinatesById(employeeId);

        return Ok(_mapper.Map<List<EmployeeFullDto>>(subordinates));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterEmployee(
        [FromQuery] string name,
        [FromQuery] string surname,
        [FromQuery] Guid id,
        [FromQuery] EmployeeRoles role)
    {
        Employee newEmployee = await _employeesService.CreateEmployee(id, name, surname, role);

        return CreatedAtRoute(
                "GetEmployee", new { employeeId = newEmployee.Id }, _mapper.Map<EmployeeDto>(newEmployee));
    }

    [HttpPut("{employeeId:guid}/chief")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetChief([FromRoute] Guid employeeId, [FromQuery] Guid chiefId)
    {
        Employee updatedEmployee = await _employeesService.SetChief(employeeId, chiefId);

        return Ok(_mapper.Map<EmployeeFullDto>(updatedEmployee));
    }

    [HttpDelete("{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveEmployee([FromRoute] Guid employeeId)
    {
        await _employeesService.RemoveEmployee(employeeId);

        return NoContent();
    }
}