using System;
using Reports.Services;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskStates;
using ReportsLibrary.Tools;

namespace Reports
{
    public class Program
    {
        public static void Main()
        {
            var reportsService = EmployeeService.GetInstance();

            Employee misha = new ("Misha", "Libchenko", Guid.NewGuid(), EmployeeRoles.TeamLead);
            reportsService.RegisterEmployee(misha);

            Employee ksu = new ("Ksusha", "Vasutinskaya", Guid.NewGuid(), EmployeeRoles.Supervisor);
            reportsService.RegisterEmployee(ksu);

            Employee isa = new ("Iskander", "Kudashev", Guid.NewGuid(), EmployeeRoles.OrdinaryEmployee);
            reportsService.RegisterEmployee(isa);

            WorkTeam dreamTeam = new (misha, "DreamProgrammingTeam");
            reportsService.RegisterWorkTeam(dreamTeam);

            dreamTeam.AddEmployee(isa);
            dreamTeam.AddEmployee(ksu);

            Sprint sprint = new (DateTime.Today.AddMonths(1));
            dreamTeam.AddSprint(misha, new Sprint(DateTime.Today.AddMonths(1)));

            Task task = new ("To write a reports");
            task.SetOwner(misha, misha);
            sprint.AddTask(task);

            task.SetOwner(misha, isa);
            task.SetState(misha, new ActiveTaskState());

            Console.WriteLine($"Ksu: {ksu.GetType()}");
            Console.WriteLine($"Isa: {isa.GetType()}");
            reportsService.SetChief(isa, ksu);
        }
    }
}