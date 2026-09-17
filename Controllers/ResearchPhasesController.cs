using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResearchFlow.Data;
using ResearchFlow.Services;
using ResearchFlow.ViewModels;
namespace ResearchFlow.Controllers; 

[Authorize]
public class ResearchPhasesController(PhaseService service, ApplicationDbContext db) : Controller 
{
    public async Task<IActionResult> Details(int id)
    { 
        var x = await service.Get(id);
        return x is null ? NotFound() : View(x); 
    }
    
    [Authorize(Roles = "LabManager")]
    public async Task<IActionResult> Create(int studyId, int? parentId) 
    { 
        var v = new PhaseFormViewModel 
        {
            ResearchStudyId = studyId, 
            ParentPhaseId = parentId 
        };
        await service.Fill(v);
        return View(v); 
    }
    
    [HttpPost, Authorize(Roles = "LabManager")] 
    public async Task<IActionResult> Create(PhaseFormViewModel v) 
    { 
        if (!ModelState.IsValid)
        {
            await service.Fill(v);
            return View(v);
        } 
        try
        
        { 
            var id = await service.Create(v);
            return RedirectToAction(nameof(Details), new { id }); 
        } 
        catch (InvalidOperationException e) 
        { 
            ModelState.AddModelError("", e.Message); await service.Fill(v); return View(v);
        }
    }
    [Authorize(Roles = "LabManager")] 
    
    public async Task<IActionResult> Edit(int id)
    { 
        var x = await service.Get(id);
        if (x is null)
            return NotFound();
        
        var v = new PhaseFormViewModel
        {
            ResearchStudyId = x.ResearchStudyId, 
            ParentPhaseId = x.ParentPhaseId,
            AssignedResearcherId = x.AssignedResearcherId,
            Title = x.Title,
            Description = x.Description,
            Priority = x.Priority
        };
        
        await service.Fill(v, id);
        return View(v);
    } 
    [HttpPost, Authorize(Roles = "LabManager")]
    public async Task<IActionResult> Edit(int id, PhaseFormViewModel v) 
    { 
        if (!ModelState.IsValid) 
        { 
            await service.Fill(v, id);
            return View(v); 
        } 
        try 
        {
            if (!await service.Update(id, v)) 
                return NotFound();
            
            return RedirectToAction(nameof(Details), new { id }); 
        } 
        catch (InvalidOperationException e) { ModelState.AddModelError("", e.Message);
            await service.Fill(v, id); return View(v);
        }
    } 
    
    [Authorize(Roles = "LabManager")] 
    public async Task<IActionResult> Delete(int id)
    { 
        var x = await service.Get(id); 
        return x is null ? NotFound() : View(x); 
    }
    
    [HttpPost, ActionName("Delete"), Authorize(Roles = "LabManager")] 
    public async Task<IActionResult> DeleteConfirmed(int id) 
    { 
        var x = await service.Get(id);
        if (x is null) 
            return NotFound();
        var e = await service.Delete(id); 
        if (e is not null) 
        {
            TempData["Error"] = e;
            return RedirectToAction(nameof(Details), new { id }); 
        }
        return RedirectToAction("Details", "ResearchStudies", new { id = x.ResearchStudyId });
    
    }
    public async Task<IActionResult> MyWork() 
    {
        var uid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return View(await db.ResearchPhases.Include(x => x.ResearchStudy).Where(x => x.AssignedResearcherId == uid).AsNoTracking().ToListAsync()); } }