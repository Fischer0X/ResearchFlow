using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFlow.Services;
namespace ResearchFlow.Controllers;
[Authorize]
public class ReportsController(ReportService service) : Controller 
{
    public async Task<IActionResult> StudyProgress(int studyId) 
    { 
        var x = await service.Build(studyId); return x is null ? NotFound() : 
View(x); 
    }
}