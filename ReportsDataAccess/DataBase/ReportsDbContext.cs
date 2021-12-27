namespace ReportsDataAccess.DataBase;

using Microsoft.EntityFrameworkCore;
using ModelsConfigurations;
using ReportsDomain.Employees;
using ReportsDomain.Entities;
using ReportsDomain.Tasks;

public sealed class ReportsDbContext : DbContext
{
    public ReportsDbContext(DbContextOptions<ReportsDbContext> options)
        : base(options)
        => Database.EnsureCreatedAsync();

    public DbSet<ReportsTask> Tasks { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<WorkTeam> WorkTeams { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Sprint> Sprints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfigurations());
        modelBuilder.ApplyConfiguration(new WorkTeamConfigurations());
        modelBuilder.ApplyConfiguration(new EmployeeConfigurations());
        modelBuilder.ApplyConfiguration(new ReportConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}