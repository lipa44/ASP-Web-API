using ReportsLibrary.Employees;
using ReportsLibrary.Entities;

namespace Reports.Interfaces
{
    public interface IEmployeeService
    {
        void RegisterEmployee(Employee employee);
        void RemoveEmployee(Employee employee);
        void RegisterWorkTeam(WorkTeam workTeam);
        void RemoveWorkTeam(WorkTeam workTeam);
    }
}