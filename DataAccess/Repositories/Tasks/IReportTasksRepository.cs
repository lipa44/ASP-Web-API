using Domain.Entities;
using Domain.Entities.Tasks;

namespace DataAccess.Repositories.Tasks;

public interface IReportTasksRepository : IRepository<ReportsTask>
{
    Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime);
    Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationDate(DateTime modificationTime);
    Task<IReadOnlyCollection<ReportsTask>> FindTasksByEmployeeId(Guid employeeId);
    Task<IReadOnlyCollection<ReportsTask>> FindsTaskModifiedByEmployeeId(Guid employeeId);
    Task<IReadOnlyCollection<ReportsTask>> FindTasksCreatedByEmployeeSubordinates(Employee employee);
}