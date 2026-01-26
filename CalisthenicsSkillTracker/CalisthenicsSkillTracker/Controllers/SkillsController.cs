using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

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

    public IActionResult Details(Guid id) 
    {
        Skill? skill = this._context
            .Skills
            .AsNoTracking()
            .SingleOrDefault(s => s.Id == id);

        if (skill is null)
            this.NotFound();

        return this.View(skill);
    }
}
