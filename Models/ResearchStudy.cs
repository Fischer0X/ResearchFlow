using System.ComponentModel.DataAnnotations;
namespace ResearchFlow.Models;

public class ResearchStudy
{
    public int Id { get; set; }
    [Required, StringLength(150, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;
    [StringLength(2000)] public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public string CreatedById { get; set; } = string.Empty;
    public ApplicationUser CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<ResearchPhase> Phases { get; set; } = [];
}