using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker.Controllers;

public class ExercisesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExercisesController(ApplicationDbContext context)
    {
        this._context = context;
    }
    public IActionResult Index(string? filter)
    {
        IEnumerable<ListTableItemViewModel> exercises = this._context
            .Exercises
            .AsNoTracking()
            .OrderBy(e => e.Name)
            .Select(e => new ListTableItemViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Difficulty = e.Difficulty
            })
            .ToArray();

        if (filter is not null)
              exercises = exercises.Where(s => s.Name.ToLower().Contains(filter.ToLower()));

        ViewData["Filter"] = filter;

        return this.View(exercises);
    }
}
