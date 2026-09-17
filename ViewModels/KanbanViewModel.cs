using ResearchFlow.Models;
namespace ResearchFlow.ViewModels; 
public class KanbanViewModel
{ 
    public int StudyId { get; set; } 
    public string StudyTitle { get; set; } = string.Empty;
    public List<ResearchPhase> Planned { get; set; } = []; 
    public List<ResearchPhase> InProgress { get; set; } = [];
    public List<ResearchPhase> Completed { get; set; } = []; 
}