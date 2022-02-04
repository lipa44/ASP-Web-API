using Domain.Entities;

namespace DataAccess.Repositories.Employees;

public interface IEmployeesRepository : IRepository<Employee>
{
    Task<IReadOnlyCollection<Employee>> GetEmployeeSubordinatesById(Guid employeeId);
    Task<Report> CreateEmptyReport(Guid employeeId);
}