using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Employees;
using ReportsWebApiLayer.DataBase.Dto.Employees;
using ReportsWebApiLayer.Services.Interfaces;

namespace ReportsWebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employees
        [HttpGet]
        public ActionResult<List<EmployeeDto>> Get()
        {
            return _employeeService.GetEmployees().Value?.Select(EmployeeToDto).ToList()!;
        }

        // GET: api/Employees/1
        [HttpGet("{id}", Name = "GetEmployees")]
        public ActionResult<EmployeeDto> Get(Guid id)
        {
            Employee? employee = _employeeService.FindEmployeeById(id);

            if (employee == null) return NotFound();

            return EmployeeToDto(employee);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<CreatedAtRouteResult> Create(string name, string surname, Guid id)
        {
            // TODO: fix hard type cast
            var newTeamLead = new TeamLead(name, surname, id);

            await _employeeService.RegisterEmployee(newTeamLead);

            return CreatedAtRoute("GetEmployees", new {id = newTeamLead.Id}, EmployeeToDto(newTeamLead));
        }

        // PUT: api/Employees/1
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, EmployeeDto employeeDtoIn)
        {
            Employee? employee = _employeeService.FindEmployeeById(id);

            if (employee == null) return NotFound();

            // _employeeService.Update(id, employeeDtoIn);

            return NoContent();
        }

        // DELETE: api/Employees/1
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            Employee? employee = _employeeService.FindEmployeeById(id);

            if (employee == null) return NotFound();

            _employeeService.RemoveEmployee(employee);

            return NoContent();
        }

        private static EmployeeDto EmployeeToDto(Employee employeeToDto)
        {
            return employeeToDto switch
            {
                OrdinaryEmployee => new OrdinaryEmployeeDto
                {
                    Name = employeeToDto.Name,
                    Surname = employeeToDto.Surname,
                    Id = employeeToDto.Id,
                    Chief = employeeToDto.Chief,
                    Employees = employeeToDto.Subordinates.ToList()
                },
                Supervisor => new SupervisorDto
                {
                    Name = employeeToDto.Name,
                    Surname = employeeToDto.Surname,
                    Id = employeeToDto.Id,
                    Chief = employeeToDto.Chief,
                    Employees = employeeToDto.Subordinates.ToList()
                },
                TeamLead leadDto => new TeamLeadDto
                {
                    Name = leadDto.Name,
                    Surname = leadDto.Surname,
                    Id = leadDto.Id,
                    Chief = leadDto.Chief,
                    Employees = leadDto.Subordinates.ToList(),
                    WorkTeams = leadDto.WorkTeams.ToList()
                },
                _ => throw new Exception("Employee role unrecognized")
            };
        }
    }
}