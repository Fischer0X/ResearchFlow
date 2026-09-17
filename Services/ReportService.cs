using Microsoft.EntityFrameworkCore;
using ResearchFlow.Data;
using ResearchFlow.Models;
using ResearchFlow.ViewModels;
namespace ResearchFlow.Services;
public class ReportService(ApplicationDbContext db)
{
    public async Task<StudyReportViewModel?> Build(int id) 
    { 
        var s = await db.ResearchStudies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); 
       
        if (s is null)
            return null; 
       
        var q = db.ResearchPhases.AsNoTracking().Where(x => x.ResearchStudyId == id);
       
        var total = await q.CountAsync();
      
        var done = await q.CountAsync(x => x.Status == ResearchStatus.Completed);
      
        return new() 
        { 
            StudyTitle = s.Title, 
            Total = total,
            Planned = await q.CountAsync(x => x.Status == ResearchStatus.Planned),
            InProgress = await q.CountAsync(x => x.Status == ResearchStatus.InProgress), 
            Completed = done, Percentage = total == 0 ? 0 : Math.Round(done * 100.0 / total, 2), Researchers = await q.Where(x => x.AssignedResearcherId != null).GroupBy(x => x.AssignedResearcher!.FullName).Select(g => new ResearcherResult { Name = g.Key, Assigned = g.Count(), Completed = g.Count(x => x.Status == ResearchStatus.Completed) 
            }).ToListAsync() 
        }; 
    } 
}