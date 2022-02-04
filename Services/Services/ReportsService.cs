using DataAccess.Repositories.Reports;
using Domain.Entities;
using Services.Services.Interfaces;

namespace Services.Services;

public class ReportsService : IReportsService
{
    private readonly IReportsRepository _reportsRepository;

    public ReportsService(IReportsRepository reportsRepository)
    {
        _reportsRepository = reportsRepository;
    }

    public async Task<IReadOnlyCollection<Report>> GetReports()
        => await _reportsRepository.GetAll();

    public async Task<Report> FindReportById(Guid reportId)
        => await _reportsRepository.FindItem(reportId);

    public async Task<Report> GetReportByEmployeeId(Guid employeeId)
        => await _reportsRepository.GetReportByEmployeeId(employeeId);

    public async Task<Report> CommitChangesToReport(Guid ownerId)
    {
        Report report = await _reportsRepository.GetReportByEmployeeId(ownerId);
        Employee owner = await _reportsRepository.GetEmployeeByReportId(report.Id);

        owner.CommitChangesToReport();

        return await _reportsRepository.Update(report);
    }

    // public async Task<Report> GenerateWorkTeamReport(Guid workTeamId, Guid changerId)
    // {
    //     await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
    //
    //     try
    //     {
    //         await transaction.CreateSavepointAsync("BeforeReportGenerated");
    //
    //         Employee changerToGenerateReport = await GetEmployeeFromDbAsync(changerId);
    //         WorkTeam teamToGenerateReport = await GetWorkTeamByIdAsync(workTeamId);
    //
    //         Report updatedReport = teamToGenerateReport.GenerateReport(changerToGenerateReport);
    //         updatedReport.SetReportAsDone(changerToGenerateReport);
    //
    //         _dbContext.Reports.Update(updatedReport);
    //         _dbContext.WorkTeams.Update(teamToGenerateReport);
    //
    //         await _dbContext.SaveChangesAsync();
    //         await transaction.CommitAsync();
    //
    //         return updatedReport;
    //     }
    //     catch (Exception)
    //     {
    //         await transaction.RollbackToSavepointAsync("BeforeReportGenerated");
    //         throw;
    //     }
    // }
    public async Task<Report> SetReportAsDone(Guid reportId, Guid changerId)
    {
        Report report = await _reportsRepository.GetItem(reportId);
        Employee owner = await _reportsRepository.GetEmployeeByReportId(changerId);

        report.SetReportAsDone(owner);

        return await _reportsRepository.Update(report);
    }
}