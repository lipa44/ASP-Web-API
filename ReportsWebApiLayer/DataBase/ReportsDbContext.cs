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
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Owner)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Employee>()
            .HasMany(t => t.Subordinates)
            .WithOne()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Chief)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Employee>()
            .Property(e => e.Tasks)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<Task>>(v));

        modelBuilder.Entity<Task>()
            .HasMany(t => t.Comments)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Task>()
            .Property(e => e.Modifications)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<TaskModification>>(v));

        modelBuilder.Entity<Sprint>()
            .HasMany(s => s.Tasks)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkTeam>()
            .HasOne(t => t.TeamLead)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<TaskComment>()
            .HasOne(t => t.Commentator)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

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