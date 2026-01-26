using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Controllers;

public class SkillsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SkillsController(ApplicationDbContext context)
    {
        this._context = context;
    }

    public IActionResult Index()
    {
        IEnumerable<Skill> skills = this._context
            .Skills
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToArray();

        return this.View(skills);
    }
}
