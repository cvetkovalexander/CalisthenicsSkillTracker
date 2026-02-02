using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.ViewModels.WorkoutViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace CalisthenicsSkillTracker.Controllers;

public class WorkoutsController : Controller
{
    private readonly ApplicationDbContext _context;

    public WorkoutsController(ApplicationDbContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public IActionResult Log()
    {
        CreateWorkoutViewModel model = new CreateWorkoutViewModel()
        {
            Users = this._context
                .Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Username
                })
                .ToList(),
            Date = DateTime.UtcNow
        };
        return this.View(model);
    }

    [HttpPost]
    public IActionResult Log(CreateWorkoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return this.View(model);
        }

        WorkoutInProgressViewModel workoutModel = new WorkoutInProgressViewModel()
        {
            Date = model.Date,
            UserId = model.UserId,
            Notes = model.Notes
        };

        TempData["WorkoutInProgress"] = JsonSerializer.Serialize(workoutModel);

        return this.RedirectToAction("AddExercises");
    }

    [HttpGet]
    public IActionResult AddExercises()
    {
        return this.View();
    }
}
