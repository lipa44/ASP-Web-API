using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;
using Task = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.DataBase;

public class ReportsDbContext : DbContext
{
    public ReportsDbContext(DbContextOptions<ReportsDbContext> options)
        : base(options)
    {
        // Database.EnsureDeleted();
        Database.EnsureCreatedAsync();
    }

    public DbSet<Task> Tasks { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<WorkTeam> WorkTeams { get; set; }

    // using Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One-to-many: many employees refers to one chief
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Subordinates)
            .WithOne(e => e.Chief)
            .OnDelete(DeleteBehavior.SetNull);

        // One-to-many: many tasks refers to one owner
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Owner)
            .WithMany(e => e.Tasks)
            .OnDelete(DeleteBehavior.SetNull);

        // One-to-many: many comments has refers to one task
        modelBuilder.Entity<Task>()
            .HasMany(t => t.Comments)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-many: many tasks refers to one sprint
        modelBuilder.Entity<Sprint>()
            .HasMany(s => s.Tasks)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-many: many work teams refers to one employee
        modelBuilder.Entity<WorkTeam>()
            .HasOne(t => t.TeamLead)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        // One-to-many: many comments refers to one employee
        modelBuilder.Entity<TaskComment>()
            .HasOne(t => t.Commentator)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

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