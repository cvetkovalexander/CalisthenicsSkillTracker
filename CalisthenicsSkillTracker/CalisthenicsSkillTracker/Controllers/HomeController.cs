using System.Diagnostics;
using CalisthenicsSkillTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsSkillTracker.Controllers
{
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Home/Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == StatusCodes.Status400BadRequest)
                return this.View("BadRequest");

            if (statusCode == StatusCodes.Status404NotFound)
                return this.View("NotFound");

            if (statusCode == StatusCodes.Status500InternalServerError)
                return this.View("ServerError");

            return this.Ok(statusCode);
        }
    }
}
