using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace DataAccess.ModelsConfigurations;

public class WorkTeamConfigurations : IEntityTypeConfiguration<WorkTeam>
{
    public void Configure(EntityTypeBuilder<WorkTeam> builder)
    {
        builder.HasKey(workTeam => workTeam.Id);

        builder.HasMany(w => w.Sprints)
            .WithOne()
            .HasForeignKey(s => s.WorkTeamId)
            .OnDelete(DeleteBehavior.Cascade);

        // one-to-many relationship: workTeam has many employee and employee has one workTeam
        builder.HasMany(workTeam => workTeam.Employees)
            .WithOne()
            .HasForeignKey(employee => employee.WorkTeamId)
            .OnDelete(DeleteBehavior.SetNull);

        // one-to-one relationship: workTeam has one teamLead and teamLead has one workTeam
        builder.HasOne(e => e.TeamLead)
            .WithOne()
            .HasForeignKey<WorkTeam>(team => team.TeamLeadId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(workTeam => workTeam.Reports)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<Report>>(v));
    }
}