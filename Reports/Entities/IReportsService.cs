using Reports.Employees.Abstractions;

namespace Reports.Entities
{
    public interface IReportsService
    {
        void RegisterEmployee(Employee employee);
        void RemoveEmployee(Employee employee);
        void RegisterWorkTeam(WorkTeam workTeam);
        void RemoveWorkTeam(WorkTeam workTeam);
    }
}