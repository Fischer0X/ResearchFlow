using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFlow.Services;
using ResearchFlow.ViewModels;
namespace ResearchFlow.Controllers; 


[Authorize]
public class ResearchStudiesController(StudyService service) : Controller { 
    public async Task<IActionResult> Index() => 
       
        View(await service.List()); 
  
    public async Task<IActionResult> Details(int id) 
    { 
        var x = await service.Get(id); 
        return x is null ? NotFound() : View(x);
    } 
    
    
    [Authorize(Roles = "LabManager")] 
    public IActionResult Create() => 
        View(new StudyFormViewModel()); 
    
  
    [HttpPost, Authorize(Roles = "LabManager")]
    public async Task<IActionResult> Create(StudyFormViewModel v) 
    {
        if (!ModelState.IsValid) 
            return View(v);
        
        var id = await service.Create(v, User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return RedirectToAction(nameof(Details), new { id });
    } 
   
    
    [Authorize(Roles = "LabManager")] 
    public async Task<IActionResult> Edit(int id) 
    {
        var x = await service.Get(id);
        return x is null ? NotFound() : View(new StudyFormViewModel 
        {
            Title = x.Title,
            Description = x.Description,
            StartDate = x.StartDate,
            ExpectedEndDate = x.ExpectedEndDate
        }
        );
    }
    
   
    [HttpPost, Authorize(Roles = "LabManager")]
    public async Task<IActionResult> Edit(int id, StudyFormViewModel v)
    { 
        if (!ModelState.IsValid)
            return View(v);
        if (!await service.Update(id, v))
            return NotFound();
        return RedirectToAction(nameof(Details), new { id });
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
      
        var e = await service.Delete(id);
       
        if (e is not null) 
        {
            TempData["Error"] = e;
            return RedirectToAction(nameof(Details), new { id }); 
        }
      
        return RedirectToAction(nameof(Index));
    }
}