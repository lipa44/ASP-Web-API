using Microsoft.EntityFrameworkCore;
using ReportsLibrary.Employees;
namespace ReportsWebApiLayer.Models.Contexts
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Employee>();
        // }

        public DbSet<Employee> Employees { get; set; }
    }
}