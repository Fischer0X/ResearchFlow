using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResearchFlow.Models;
namespace ResearchFlow.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> o) : IdentityDbContext<ApplicationUser>(o)
{ 
   
    public DbSet<ResearchStudy> ResearchStudies => 
        Set<ResearchStudy>();
   
    public DbSet<ResearchPhase> ResearchPhases =>
        Set<ResearchPhase>();
   
    public DbSet<ActivityLog> ActivityLogs =>
        Set<ActivityLog>(); 
  
    protected override void OnModelCreating(ModelBuilder b)
    { 
        base.OnModelCreating(b);
        
        b.Entity<ResearchStudy>()
            .HasOne(x => x.CreatedBy)
            .WithMany(x => x.CreatedStudies)
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
       
        b.Entity<ResearchPhase>()
            .HasOne(x => x.ResearchStudy)
            .WithMany(x => x.Phases)
            .HasForeignKey(x => x.ResearchStudyId)
            .OnDelete(DeleteBehavior.Cascade);
       
        b.Entity<ResearchPhase>()
            .HasOne(x => x.ParentPhase)
            .WithMany(x => x.ChildPhases)
            .HasForeignKey(x => x.ParentPhaseId)
            .OnDelete(DeleteBehavior.Restrict);
       
        b.Entity<ResearchPhase>()
            .HasOne(x => x.AssignedResearcher)
            .WithMany(x => x.AssignedPhases)
            .HasForeignKey(x => x.AssignedResearcherId)
            .OnDelete(DeleteBehavior.SetNull);
      
        b.Entity<ActivityLog>()
            .HasOne(x => x.ResearchPhase)
            .WithMany(x => x.ActivityLogs)
            .HasForeignKey(x => x.ResearchPhaseId)
            .OnDelete(DeleteBehavior.Cascade);
       
        b.Entity<ActivityLog>()
            .HasOne(x => x.ChangedByUser)
            .WithMany(x => x.ActivityLogs)
            .HasForeignKey(x => x.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}