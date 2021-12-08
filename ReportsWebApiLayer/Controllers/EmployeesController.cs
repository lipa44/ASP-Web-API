using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Employees;
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
        public ActionResult<List<Employee>> Get()
        {
            return _employeeService.GetEmployees().Value?.ToList()!;
        }

        // GET: api/Employees/1
        [HttpGet("{id}", Name = "GetEmployees")]
        public ActionResult<Employee> Get(Guid id)
        {
            Employee? employee = _employeeService.FindEmployeeById(id);

            if (employee == null) return NotFound();

            return employee;
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<CreatedAtRouteResult> Create(string name, string surname, Guid id)
        {
            // TODO: fix hard type cast
            var newTeamLead = new TeamLead(name, surname, id);

            await _employeeService.RegisterEmployee(newTeamLead);

            return CreatedAtRoute("GetEmployees", new {id = newTeamLead.Id}, newTeamLead);
        }

        // PUT: api/Employees/1
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, Employee employeeIn)
        {
            Employee? employee = _employeeService.FindEmployeeById(id);

            if (employee == null) return NotFound();

            // _employeeService.Update(id, employeeIn);

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
    }
}