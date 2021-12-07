using Microsoft.EntityFrameworkCore;
using ReportsLibrary.Employees;
using ReportsWebApiLayer.DataBase.Dto.Employees;

namespace ReportsWebApiLayer.Models.Contexts
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeDto>();
        }

        public DbSet<EmployeeDto> Employees { get; set; }
    }
}