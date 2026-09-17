using Microsoft.AspNetCore.Mvc;
namespace ResearchFlow.Controllers;
public class HomeController : Controller
{ 
    public IActionResult Index() =>
        View();
    
    public IActionResult Error() => 
        View();
}