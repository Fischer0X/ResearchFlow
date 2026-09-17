using Microsoft.AspNetCore.Identity;
namespace ResearchFlow.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public ICollection<ResearchStudy> CreatedStudies { get; set; } = []; public ICollection<ResearchPhase> AssignedPhases { get; set; } = []; public ICollection<ActivityLog> ActivityLogs { get; set; } = [];
}