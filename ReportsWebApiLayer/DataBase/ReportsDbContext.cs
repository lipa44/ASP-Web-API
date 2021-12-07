using Microsoft.EntityFrameworkCore;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsWebApiLayer.DataBase.Dto;
using ReportsWebApiLayer.DataBase.Dto.Employees;
using Task = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.DataBase;

public class ReportsDbContext : DbContext
{
    public ReportsDbContext(DbContextOptions<ReportsDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<WorkTeam> WorkTeams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SharedTypeEntity<EmployeeDto>(
            "Employee", bb =>
            {
                bb.ToTable("Employees");
                bb.HasKey(e => e.Id);
            });

        modelBuilder.SharedTypeEntity<TaskDto>(
            "Task", bb =>
            {
                bb.ToTable("Tasks");
                bb.HasKey(e => e.Id);
            });

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // modelBuilder.Entity<Task>().ToTable("Tasks")
        //     .HasOne<Employee>()
        //     .WithMany()
        //     .HasForeignKey(p => p.Id);
        //
        // modelBuilder.Entity<WorkTeam>().ToTable("WorkTeam")
        //     .HasMany<Employee>();

        // base.OnModelCreating(modelBuilder);
    }
}
