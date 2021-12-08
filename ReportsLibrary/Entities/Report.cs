namespace ReportsLibrary.Entities;

public class Report
{
    public Report() { }

    public Report(string workTeamName)
    {
        WorkTeamName = workTeamName;
    }

    public string WorkTeamName { get; init; }
}