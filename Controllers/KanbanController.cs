using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResearchFlow.Data;
using ResearchFlow.Models;
using ResearchFlow.Services;
using ResearchFlow.ViewModels;
namespace ResearchFlow.Controllers; 
[Authorize]
public class KanbanController(ApplicationDbContext db, PhaseService service) : Controller 
{ 
    public async Task<IActionResult> Board(int studyId) 
    {
        var s = await db.ResearchStudies.FindAsync(studyId);
        
        if (s is null) 
            return NotFound();
        
        var all = 
            await db.ResearchPhases.Where(x => x.ResearchStudyId == studyId).Include(x => x.AssignedResearcher).Include(x => x.ParentPhase).AsNoTracking().ToListAsync();
       
        return
            View(
                new KanbanViewModel 
            { 
                    StudyId = studyId,
                    StudyTitle = s.Title,
                    Planned = [.. all.Where(x => x.Status == ResearchStatus.Planned)],
                    InProgress = [.. all.Where(x => x.Status == ResearchStatus.InProgress)], 
                    Completed = [.. all.Where(x => x.Status == ResearchStatus.Completed)] 
                }
                );
    }
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id, int studyId, ResearchStatus status, string? note)
    { 
        if (!Enum.IsDefined(status))
            return BadRequest();
        
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        
        if (!User.IsInRole("LabManager"))
        {
            var p = 
                await db.ResearchPhases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (p?.AssignedResearcherId != uid)
                return Forbid();
        } 
        if (!await service.Change(id, status, uid, note))
            return NotFound();
     
        return RedirectToAction(nameof(Board), new { studyId });
    }
}