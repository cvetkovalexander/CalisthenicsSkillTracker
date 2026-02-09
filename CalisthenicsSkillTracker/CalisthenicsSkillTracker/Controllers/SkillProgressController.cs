using CalisthenicsSkillTracker.Data.Models.Enums;
using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;

namespace CalisthenicsSkillTracker.Controllers;

public class SkillProgressController : Controller
{
    private readonly ApplicationDbContext _context;

    private const string UsersFetchKey = "Users";
    private const string SkillsFetchKey = "Skills";
    private const string ProgressionsFetchKey = "Progressions";

    public SkillProgressController(ApplicationDbContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        CreateSkillProgressViewModel model = new CreateSkillProgressViewModel();
        this.PopulateSelectLists(model);

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(CreateSkillProgressViewModel model)
    {
        this.PopulateSelectLists(model);
        if (!ModelState.IsValid) 
        {
            return this.View(model);
        }

        User? user = this._context.Users.Find(model.UserId);
        if (user is null) 
        {
            ModelState.AddModelError(nameof(model.UserId), "Invalid user id!");

            return this.View(model);
        }

        Skill? skill = this._context.Skills.Find(model.SkillId);
        if (skill is null) 
        {
            ModelState.AddModelError(nameof(model.SkillId), "Invalid skill id!");

            return this.View(model);
        }

       

        try 
        {
            SkillProgress record = new SkillProgress
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                SkillId = model.SkillId,
                Date = DateTime.UtcNow,
                Progression = model.Progression,
                Repetitions = model.Repetitions,
                Duration = model.Duration,
                Notes = model.Notes
            };

            this._context.SkillProgressRecords.Add(record);
            this._context.SaveChanges();
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);
            ModelState.AddModelError(string.Empty, "An error occurred while logging the skill progress. Please try again.");
            return this.View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    private void PopulateSelectLists(CreateSkillProgressViewModel model)
    {
        model.Users = this._context
            .Users
            .Select(u => new SelectListItem 
            { 
                Value = u.Id.ToString(), 
                Text = u.Username 
            })
            .ToList();
        model.Skills = this._context
            .Skills
            .Select(s => new SelectListItem 
            { 
                Value = s.Id.ToString(), 
                Text = s.Name 
            })
            .ToList();
        model.Progressions = Enum.GetValues<Progression>()
            .Select(p => new SelectListItem 
            { 
                Value = p.ToString(), 
                Text = p.ToString() 
            })
            .ToList();
    }
}
