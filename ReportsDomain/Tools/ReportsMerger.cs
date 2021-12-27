namespace ReportsDomain.Tools;

using System;
using System.Linq;
using ReportsDomain.Entities;

public class ReportsMerger
{
    private readonly Report _reportToMergeFrom;
    private Report _reportToMergeIn;

    public ReportsMerger(Report reportToMergeIn, Report reportToMergeFrom)
    {
        ArgumentNullException.ThrowIfNull(reportToMergeIn);
        ArgumentNullException.ThrowIfNull(reportToMergeFrom);

        _reportToMergeIn = reportToMergeIn;
        _reportToMergeFrom = reportToMergeFrom;
    }

    public void Merge()
    {
        Report newReport = new ()
        {
            Id = _reportToMergeIn.Id,
            Owner = _reportToMergeIn.Owner,
            OwnerId = _reportToMergeIn.OwnerId,
        };

        var sortedMergedModifications = _reportToMergeIn.Modifications
            .Except(_reportToMergeFrom.Modifications)
            .OrderBy(m => m.ModificationTime)
            .ToList();

        _reportToMergeIn = newReport.CommitModifications(sortedMergedModifications);
    }
}