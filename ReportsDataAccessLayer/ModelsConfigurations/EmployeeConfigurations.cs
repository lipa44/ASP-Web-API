using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsLibrary.Employees;

namespace ReportsDataAccessLayer.ModelsConfigurations;

public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        // one-to-many relationship: employee has one chief and chief has many employees
        builder
            .HasOne(t => t.Chief)
            .WithMany()
            .HasForeignKey(e => e.ChiefId)
            .OnDelete(DeleteBehavior.Cascade);

        // one-to-many relationship: employee has many tasks and task has one owner
        builder
            .HasMany(x => x.Tasks)
            .WithOne(s => s.Owner)
            .HasForeignKey(s => s.OwnerId);
    }
}