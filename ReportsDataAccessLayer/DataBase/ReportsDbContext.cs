using Microsoft.EntityFrameworkCore;
using ReportsDataAccessLayer.ModelsConfigurations;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;

namespace ReportsDataAccessLayer.DataBase;

public class ReportsDbContext : DbContext
{
    public ReportsDbContext(DbContextOptions<ReportsDbContext> options)
        : base(options)
    {
        // Database.EnsureDeleted();
        Database.EnsureCreatedAsync();
    }

    public DbSet<ReportsTask> Tasks { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<WorkTeam> WorkTeams { get; set; }

    // using Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfigurations());
        modelBuilder.ApplyConfiguration(new WorkTeamConfigurations());
        modelBuilder.ApplyConfiguration(new EmployeeConfigurations());

        // modelBuilder.Entity<Employee>().HasKey(e => e.Id);
        //
        // // one-to-many relationship: employee has one chief and chief has many employees
        // modelBuilder.Entity<Employee>()
        //     .HasOne(t => t.Chief)
        //     .WithMany()
        //     .HasForeignKey(e => e.ChiefId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // // one-to-many relationship: employee has many tasks and task has one owner
        // modelBuilder.Entity<Employee>(e =>
        // {
        //     e.HasMany(x => x.Tasks)
        //         .WithOne(s => s.Owner)
        //         .HasForeignKey(s => s.OwnerId);
        // });
        // modelBuilder.Entity<Task>()
        //     .OwnsMany(
        //         task => task.Comments,
        //         comment =>
        //             comment.WithOwner());
        //
        // modelBuilder.Entity<Task>()
        //     .OwnsMany(
        //         task => task.Modifications,
        //         modification =>
        //             modification.WithOwner());

        // explicit one-to-many relationship: task has one sprint and sprint has many tasks
        // modelBuilder.Entity<Task>()
        //     .HasOne(t => t.Sprint)
        //     .WithMany(s => s.Tasks)
        //     .HasForeignKey(t => t.SprintId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<Sprint>()
        //     .HasMany(sprint => sprint.Tasks)
        //     .WithOne(s => s.Sprint)
        //     .HasForeignKey(task => task.SprintId)
        //     .OnDelete(DeleteBehavior.Cascade);
        // modelBuilder.Entity<WorkTeam>()
        //     .HasOne(team => team.TeamLead)
        //     .WithOne()
        //     .HasForeignKey<WorkTeam>(team => team.TeamLeadId)
        //     .OnDelete(DeleteBehavior.SetNull);

        /* SERIALIZATION (OPTIONAL) */

        // Set employee's tasks serialization
        // modelBuilder.Entity<Employee>()
        //     .Property(e => e.Tasks)
        //     .HasConversion(
        //         v => JsonConvert.SerializeObject(v),
        //         v => JsonConvert.DeserializeObject<List<Task>>(v));
        //
        // // Set task's modifications serialization
        // modelBuilder.Entity<Task>()
        //     .Property(e => e.Modifications)
        //     .HasConversion(
        //         v => JsonConvert.SerializeObject(v),
        //         v => JsonConvert.DeserializeObject<List<TaskModification>>(v));
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // modelBuilder.Entity<Task>().ToTable("Tasks")
        //     .HasOne<Employee>()
        //     .WithMany()
        //     .HasForeignKey(p => p.Id);
        //
        // modelBuilder.Entity<WorkTeam>().ToTable("WorkTeam")
        //     .HasMany<Employee>();
        //
        // base.OnModelCreating(modelBuilder);
    }
}