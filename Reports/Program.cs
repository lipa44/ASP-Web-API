using System;
using System.Linq;
using System.Threading.Channels;
using Reports.Employees;
using Reports.Employees.Abstractions;
using Reports.Entities;
using Reports.Task.TaskStates;
using ReportsTask = Reports.Task.Task;

namespace Reports
{
    public class Program
    {
        public static void Main()
        {
            var reportsService = ReportsService.GetInstance();

            var mishaId = Guid.NewGuid();
            TeamLead misha = new ("Misha", "Libchenko", mishaId);
            reportsService.RegisterEmployee(misha);

            var ksuId = Guid.NewGuid();
            Supervisor ksu = new ("Ksusha", "Vasutinskaya", ksuId, misha);
            reportsService.RegisterEmployee(ksu);

            var isaId = Guid.NewGuid();
            OrdinaryEmployee isa = new ("Iskander", "Kudashev", isaId, misha);
            reportsService.RegisterEmployee(isa);
            isa.SetChief(misha);

            WorkTeam dreamTeam = new (misha, "DreamProgrammingTeam");
            reportsService.RegisterWorkTeam(dreamTeam);

            dreamTeam.AddEmployee(isa);
            dreamTeam.AddSprint(misha, new Sprint(DateTime.Today.AddMonths(1)));

            Console.WriteLine($"Isa's chief: {isa.Chief}");
            reportsService.ChangeChief(isa, ksu);
            Console.WriteLine($"Isa's chief: {isa.Chief}");
        }
    }
}