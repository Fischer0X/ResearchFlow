using System.ComponentModel.DataAnnotations;
namespace ResearchFlow.Models;

public class ResearchPhase
{
    public int Id { get; set; }
    public int ResearchStudyId { get; set; }
    public ResearchStudy ResearchStudy { get; set; } = null!; 
    public int? ParentPhaseId { get; set; }
    public ResearchPhase? ParentPhase { get; set; }
    public ICollection<ResearchPhase> ChildPhases { get; set; } = []; 
    public string? AssignedResearcherId { get; set; }
    public ApplicationUser? AssignedResearcher { get; set; }
    [Required, StringLength(150, MinimumLength = 3)] 
    public string Title { get; set; } = string.Empty;
    [StringLength(2000)] 
    public string? Description { get; set; }
    public ResearchPriority Priority { get; set; } = ResearchPriority.Medium; public ResearchStatus Status { get; set; } = ResearchStatus.Planned; public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<ActivityLog> ActivityLogs { get; set; } = [];
}