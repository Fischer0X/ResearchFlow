using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResearchFlow.Models;
namespace ResearchFlow.ViewModels;
public class PhaseFormViewModel 
{ 
    public int ResearchStudyId { get; set; } 
    public int? ParentPhaseId { get; set; } 
    public string? AssignedResearcherId { get; set; }
    
    [Required, StringLength(150, MinimumLength = 3)] 
    public string Title { get; set; } = string.Empty;
    [StringLength(2000)] public string? Description { get; set; } 
    public ResearchPriority Priority { get; set; } = ResearchPriority.Medium; public IEnumerable<SelectListItem> Researchers { get; set; } = [];
    public IEnumerable<SelectListItem> ParentPhases { get; set; } = []; 
}