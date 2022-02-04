using Domain.Entities;
using Domain.Entities.Tasks;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Extensions;

public static class ModelBuilderExtensions
{
    private static Employee _mishaLibchenko;
    private static Employee _isaKudashev;
    private static Employee _isaTudashev;
    private static Employee _kirillAziat;

    private static List<ReportsTask> _mishaLibchenkoTasks;
    private static List<ReportsTask> _isaKudashevTasks;
    private static List<ReportsTask> _isaTudashevTasks;
    private static List<ReportsTask> _kirillAziatTasks;

    private static WorkTeam _itmoProgTeam;
    private static WorkTeam _makeMoneyTeam;

    private static Sprint _isProgTeamSprint;
    private static Sprint _makeMoneyTeamSprint;

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.SeedEmployees();
        modelBuilder.SeedTasks();
        modelBuilder.SeedWorkTeams();
        modelBuilder.SeedSprints();
    }

    private static void SeedEmployees(this ModelBuilder modelBuilder)
    {
        _mishaLibchenko = new Employee
        {
            Name = "Misha",
            Surname = "Libchenko",
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Role = EmployeeRoles.TeamLead,
        };

        _isaKudashev = new Employee
        {
            Name = "Isa",
            Surname = "Kudashev",
            Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
            Role = EmployeeRoles.Supervisor,
        };

        _isaTudashev = new Employee
        {
            Name = "Isa",
            Surname = "Tudashev",
            Id = Guid.Parse("11111111-1111-1111-1111-111111111113"),
            Role = EmployeeRoles.OrdinaryEmployee,
        };

        _kirillAziat = new Employee
        {
            Name = "Kirill",
            Surname = "Aziat",
            Id = Guid.Parse("11111111-1111-1111-1111-111111111114"),
            Role = EmployeeRoles.TeamLead,
        };

        _mishaLibchenko.AddSubordinate(_isaKudashev);
        _isaKudashev.AddSubordinate(_isaTudashev);

        modelBuilder.Entity<Employee>().HasData(_mishaLibchenko, _isaKudashev, _isaTudashev, _kirillAziat);
    }

    private static void SeedTasks(this ModelBuilder modelBuilder)
    {
        var seedTasks = new List<ReportsTask>();

        _mishaLibchenkoTasks = new List<ReportsTask>
        {
            new ("To do reports", _mishaLibchenko.Id),
            new ("To fix reports", _mishaLibchenko.Id),
            new ("Delete reports and rewrite again", _mishaLibchenko.Id),
        };
        seedTasks.AddRange(_mishaLibchenkoTasks);

        _isaKudashevTasks = new List<ReportsTask>
        {
            new ("To find a job", _isaKudashev.Id),
            new ("To read \"Clean Code\"", _isaKudashev.Id),
            new ("To learn TypeScript", _isaKudashev.Id),
        };
        seedTasks.AddRange(_isaKudashevTasks);

        _isaTudashevTasks = new List<ReportsTask>
        {
            new ("To find a better job", _isaTudashev.Id),
            new ("To learn JavaScript + React", _isaTudashev.Id),
        };
        seedTasks.AddRange(_isaTudashevTasks);

        _kirillAziatTasks = new List<ReportsTask>
        {
            new ("Make life in such way to have no tasks", _kirillAziat.Id),
        };
        seedTasks.AddRange(_kirillAziatTasks);

        modelBuilder.Entity<ReportsTask>().HasData(seedTasks);
    }

    private static void SeedWorkTeams(this ModelBuilder modelBuilder)
    {
        _itmoProgTeam = new WorkTeam(_mishaLibchenko, "Is Programming Team");
        _itmoProgTeam.AddEmployee(_isaKudashev, _mishaLibchenko);
        _isaKudashev.SetWorkTeam(_itmoProgTeam);

        _makeMoneyTeam = new WorkTeam(_kirillAziat, "Make Money Team");
        _makeMoneyTeam.AddEmployee(_isaTudashev, _kirillAziat);
        _isaTudashev.SetWorkTeam(_makeMoneyTeam);

        modelBuilder.Entity<WorkTeam>().HasData(_itmoProgTeam, _makeMoneyTeam);
    }

    private static void SeedSprints(this ModelBuilder modelBuilder)
    {
        _isProgTeamSprint = new Sprint(DateTime.Now.AddMonths(3), _itmoProgTeam.Id);

        _mishaLibchenkoTasks.ToList().ForEach(t =>
            t.SetSprint(_mishaLibchenko, _isProgTeamSprint));

        _isaKudashevTasks.ToList().ForEach(t =>
            t.SetSprint(_isaKudashev, _isProgTeamSprint));

        _itmoProgTeam.AddSprint(_mishaLibchenko, _isProgTeamSprint);

        _makeMoneyTeamSprint = new Sprint(DateTime.Now.AddMonths(1), _makeMoneyTeam.Id);

        _kirillAziatTasks.ToList().ForEach(t =>
            t.SetSprint(_kirillAziat, _makeMoneyTeamSprint));

        _isaTudashevTasks.ToList().ForEach(t =>
            t.SetSprint(_isaTudashev, _makeMoneyTeamSprint));

        _makeMoneyTeam.AddSprint(_kirillAziat, _makeMoneyTeamSprint);

        modelBuilder.Entity<Sprint>().HasData(_isProgTeamSprint, _makeMoneyTeamSprint);
    }
}