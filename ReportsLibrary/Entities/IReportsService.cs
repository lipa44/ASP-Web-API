using ReportsLibrary.Employees.Abstractions;

namespace ReportsLibrary.Entities
{
    public interface IReportsService
    {
        void RegisterEmployee(Employee employee);
        void RemoveEmployee(Employee employee);
        void RegisterWorkTeam(WorkTeam workTeam);
        void RemoveWorkTeam(WorkTeam workTeam);
    }
}