using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;

namespace ReportsDataAccessLayer.ModelsConfigurations;

public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(employee => employee.Id);

        // one-to-many relationship: employee has one chief and chief has many employees
        builder
            .HasOne(employee => employee.Chief)
            .WithMany()
            .HasForeignKey(employee => employee.ChiefId)
            .OnDelete(DeleteBehavior.Cascade);

        // one-to-many relationship: employee has many tasks and task has one owner
        builder
            .HasMany(employee => employee.Tasks)
            .WithOne(task => task.Owner)
            .HasForeignKey(task => task.OwnerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(employee => employee.Report)
            .WithOne(r => r.Owner)
            .HasForeignKey<Report>(e => e.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}