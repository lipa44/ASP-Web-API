using Microsoft.AspNetCore.Mvc;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Tools;

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
        public Task<ActionResult<IEnumerable<Employee>>> Get() =>
            Task.FromResult<ActionResult<IEnumerable<Employee>>>(_employeeService.GetEmployees().Result!.ToList());

        // GET: api/Employees/1
        [HttpGet("{id}", Name = "GetEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Employee>> Get(Guid id)
        {
            Employee employee = await _employeeService.GetEmployeeById(id);

            if (employee == null) return NotFound();

            return employee;
        }

        // POST: api/Employees
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<CreatedAtRouteResult> Create(string name, string surname, Guid id, EmployeeRoles role)
        {
            var newTeamLead = new Employee(name, surname, id, role);

            await _employeeService.RegisterEmployee(newTeamLead);

            return CreatedAtRoute("GetEmployee", new { id = newTeamLead.Id }, newTeamLead);
        }

        // PUT: api/Employees/1
        [HttpPut("{id}", Name = "BasicPut")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult Update(Guid id)
        {
            Employee employee = _employeeService.FindEmployeeById(id).Result;

            if (employee == null) return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/chief")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SetChief([FromRoute] Guid id, [FromQuery] Guid chiefId)
        {
            Employee employee = _employeeService.GetEmployeeById(id).Result;
            Employee chief = _employeeService.GetEmployeeById(chiefId).Result;

            await _employeeService.SetChief(employee, chief);

            return new ObjectResult(employee);
        }

        // DELETE: api/Employees/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(Guid id)
        {
            Employee employee = await _employeeService.GetEmployeeById(id);

            _employeeService.RemoveEmployee(employee);

            return NoContent();
        }
    }
}