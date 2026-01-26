using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        this._context = context;
    }

    public IActionResult Index()
    {
        IEnumerable<User> users = this._context
            .Users
            .AsNoTracking()
            .OrderBy(u => u.Username)
            .ToArray();

        return this.View(users);
    }

    public IActionResult Records(Guid id) 
    {
        IEnumerable<SkillProgress> records = this._context
            .SkillProgressRecords
            .Include(r => r.PerformedBy)
            .Include(r => r.Skill)
            .AsNoTracking()
            .AsSplitQuery()
            .Where(r => r.UserId == id)
            .OrderByDescending(r => r.Date)
            .ToArray();

        if (records is null)
            this.NotFound();

        return this.View(records);
    }
}
