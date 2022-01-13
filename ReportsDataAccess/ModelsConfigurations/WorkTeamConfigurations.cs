namespace ReportsDataAccess.ModelsConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using ReportsDomain.Entities;

public class WorkTeamConfigurations : IEntityTypeConfiguration<WorkTeam>
{
    public void Configure(EntityTypeBuilder<WorkTeam> builder)
    {
        builder.HasKey(workTeam => workTeam.Id);

        builder.HasMany(w => w.Sprints)
            .WithOne()
            .HasForeignKey(s => s.WorkTeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(workTeam => workTeam.Employees)
            .WithOne()
            .HasForeignKey(employee => employee.WorkTeamId)
            .OnDelete(DeleteBehavior.SetNull);

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