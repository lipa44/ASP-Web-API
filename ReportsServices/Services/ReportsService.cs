using System;
using Reports.Interfaces;

namespace Reports.Services;

public class ReportsService
{
    private readonly IEmployeeService _employeeService;
    private readonly ITaskService _taskService;

    public ReportsService(IEmployeeService employeeService, ITaskService taskService)
    {
        ArgumentNullException.ThrowIfNull(employeeService);
        ArgumentNullException.ThrowIfNull(taskService);

        _employeeService = employeeService;
        _taskService = taskService;
    }
}