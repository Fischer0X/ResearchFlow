namespace ResearchFlow.ViewModels;

public class StudyReportViewModel
{
    public string StudyTitle { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Planned { get; set; }
    public int InProgress { get; set; }
    public int Completed { get; set; }
    public double Percentage { get; set; }
    public List<ResearcherResult> Researchers { get; set; } = [];
}

public class ResearcherResult
{
    public string Name { get; set; } = string.Empty;
    public int Assigned { get; set; }
    public int Completed
    {
        get; set;
    }
}