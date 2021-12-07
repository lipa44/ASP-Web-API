using Microsoft.EntityFrameworkCore;
using ReportsWebApiLayer.DataBase.Dto;

namespace ReportsWebApiLayer.Models.Contexts
{
    public class TaskContext : DbContext 
    {
        public TaskContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<TaskDto> TaskDtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDto>();
        }
    }
}