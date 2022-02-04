using DataAccess.Repositories.Employees;
using Domain.Entities;
using Domain.Enums;
using Services.Services.Interfaces;

namespace Services.Services;

public class EmployeesService : IEmployeesService
{
    private readonly IEmployeesRepository _employeesRepository;

    public EmployeesService(IEmployeesRepository employeesRepository)
    {
        _employeesRepository = employeesRepository;
    }

    public async Task<IReadOnlyCollection<Employee>> GetEmployees()
        => await _employeesRepository.GetAll();

    public async Task<Employee> FindEmployeeById(Guid employeeId)
        => await _employeesRepository.FindItem(employeeId);

    public async Task<IReadOnlyCollection<Employee>> GetEmployeeSubordinatesById(Guid employeeId)
        => await _employeesRepository.GetEmployeeSubordinatesById(employeeId);

    public async Task<Employee> CreateEmployee(Guid employeeId, string name, string surname, EmployeeRoles role)
    {
        Employee employee = new (name, surname, employeeId, role);

        return await _employeesRepository.Create(employee);
    }

    public async Task<Report> CreateReport(Guid employeeId)
        => await _employeesRepository.CreateEmptyReport(employeeId);

    public async Task<Employee> SetChief(Guid employeeId, Guid chiefId)
    {
        Employee employee = await _employeesRepository.FindItem(employeeId);
        Employee chief = await _employeesRepository.FindItem(employeeId);

        chief.AddSubordinate(employee);

        return await _employeesRepository.Update(chief);
    }

    public async Task<Employee> RemoveEmployee(Guid employeeId)
        => await _employeesRepository.Delete(employeeId);
}