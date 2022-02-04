using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.ModelsConfigurations;

public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(employee => employee.Id);

        // one-to-many relationship: employee has many subordinates and employee has one owner
        builder
            .HasMany(employee => employee.Subordinates)
            .WithOne()
            .HasForeignKey(employee => employee.ChiefId)
            .OnDelete(DeleteBehavior.SetNull);

        // one-to-many relationship: employee has many tasks and task has one owner
        builder
            .HasMany(employee => employee.Tasks)
            .WithOne()
            .HasForeignKey(task => task.OwnerId)
            .OnDelete(DeleteBehavior.SetNull);

        // one-to-one relationship: employee has one report and report has one owner
        builder
            .HasOne(employee => employee.Report)
            .WithOne(report => report.Owner)
            .HasForeignKey<Report>(report => report.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}