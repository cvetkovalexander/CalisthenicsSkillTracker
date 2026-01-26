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
}
