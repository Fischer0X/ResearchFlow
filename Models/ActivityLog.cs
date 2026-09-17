using System.ComponentModel.DataAnnotations;
namespace ResearchFlow.Models;

public class ActivityLog
{
    public int Id { get; set; }
    public int ResearchPhaseId { get; set; }
    public ResearchPhase ResearchPhase { get; set; } = null!;
    public string ChangedByUserId { get; set; } = string.Empty;
    public ApplicationUser ChangedByUser { get; set; } = null!;
    public ResearchStatus OldStatus { get; set; }
    public ResearchStatus NewStatus { get; set; }
    [StringLength(500)]
    public string? Note { get; set; }
    public DateTime ChangedAt { get; set; }
}