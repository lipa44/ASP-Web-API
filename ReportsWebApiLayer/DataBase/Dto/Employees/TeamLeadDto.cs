using ReportsLibrary.Entities;

namespace ReportsWebApiLayer.DataBase.Dto.Employees;

public class TeamLeadDto : EmployeeDto
{
    public List<WorkTeam> WorkTeams { get; set; }
}