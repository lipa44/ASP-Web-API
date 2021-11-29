using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskStates;

namespace Reports
{
    public class Program
    {
        public static void Main()
        {
            var reportsService = ReportsService.GetInstance();

            TeamLead misha = new ("Misha", "Libchenko", Guid.NewGuid());
            reportsService.RegisterEmployee(misha);

            Supervisor ksu = new ("Ksusha", "Vasutinskaya", Guid.NewGuid(), misha);
            reportsService.RegisterEmployee(ksu);

            OrdinaryEmployee isa = new ("Iskander", "Kudashev", Guid.NewGuid(), misha);
            reportsService.RegisterEmployee(isa);

            WorkTeam dreamTeam = new (misha, "DreamProgrammingTeam");
            reportsService.RegisterWorkTeam(dreamTeam);

            dreamTeam.AddEmployee(isa);
            dreamTeam.AddEmployee(ksu);

            Sprint sprint = new (DateTime.Today.AddMonths(1));
            dreamTeam.AddSprint(misha, new Sprint(DateTime.Today.AddMonths(1)));

            Task task = new (misha, "To write a reports");
            sprint.AddTask(task);

            task.ChangeImplementer(misha, isa);
            task.ChangeState(misha, new ActiveTaskState());

            Console.WriteLine($"Isa's chief: {isa.Chief}");
            reportsService.ChangeChief(isa, ksu);
            Console.WriteLine($"Isa's new chief: {isa.Chief}");
        }
    }
}