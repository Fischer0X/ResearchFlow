using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResearchFlow.Data;
namespace ResearchFlow.Controllers;

[Authorize]
public class ActivityLogsController(ApplicationDbContext db) : Controller {
    public async Task<IActionResult> Index(int studyId) =>

        View(
            await db.ActivityLogs.Include(x => x.ResearchPhase)
            .Include(x => x.ChangedByUser)
            .Where(x => x.ResearchPhase.ResearchStudyId == studyId)
            .OrderByDescending(x => x.ChangedAt).AsNoTracking().ToListAsync()); 
}