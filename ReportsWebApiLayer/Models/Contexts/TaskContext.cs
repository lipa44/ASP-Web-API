using Microsoft.EntityFrameworkCore;

namespace ReportsWebApiLayer.Models.Contexts
{
    public class TaskContext : DbContext 
    {
        public TaskContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Task>();
        // }
    }
}