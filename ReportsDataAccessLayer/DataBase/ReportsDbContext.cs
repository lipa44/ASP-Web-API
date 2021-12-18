using Microsoft.EntityFrameworkCore;
using ReportsDataAccessLayer.ModelsConfigurations;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;

namespace ReportsDataAccessLayer.DataBase;

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