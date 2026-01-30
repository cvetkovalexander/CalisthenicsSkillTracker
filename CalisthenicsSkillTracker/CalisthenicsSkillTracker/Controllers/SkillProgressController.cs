using CalisthenicsSkillTracker.Models.Enums;
using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Models;
using CalisthenicsSkillTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsSkillTracker.Controllers;

public class SkillProgressController : Controller
{
    private readonly ApplicationDbContext _context;

    public SkillProgressController(ApplicationDbContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        CreateSkillProgressViewModel model = new CreateSkillProgressViewModel 
        {
            Users = this._context
                .Users
                .Select(u => new SelectListItem 
                { 
                    Value = u.Id.ToString(), 
                    Text = u.Username 
                })
                .ToList(),

            Skills = this._context
                .Skills
                .Select(s => new SelectListItem 
                { 
                    Value = s.Id.ToString(), 
                    Text = s.Name 
                })
                .ToList(),

            Progressions = Enum.GetValues<Progression>()
                .Select(p => new SelectListItem 
                { 
                    Value = p.ToString(), 
                    Text = p.ToString() 
                })
                .ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(CreateSkillProgressViewModel model)
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

        return RedirectToAction("Index", "Home");
    }
}
