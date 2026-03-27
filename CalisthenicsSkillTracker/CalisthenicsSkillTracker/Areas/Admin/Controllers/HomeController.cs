using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsSkillTracker.Areas.Admin.Controllers;

public class HomeController : AdminControllerBase
{
    public IActionResult Index()
    {
        return View();
    }
}
