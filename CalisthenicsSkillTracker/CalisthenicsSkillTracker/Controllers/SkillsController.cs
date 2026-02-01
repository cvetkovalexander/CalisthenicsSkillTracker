using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Models;
using CalisthenicsSkillTracker.Models.Enums;
using CalisthenicsSkillTracker.ViewModels.CreateViewModels;
using CalisthenicsSkillTracker.ViewModels.DetailsViewModels;
using CalisthenicsSkillTracker.ViewModels.EditViewModels;
using CalisthenicsSkillTracker.ViewModels.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace CalisthenicsSkillTracker.Controllers;

public class SkillsController : Controller
{
    private readonly ApplicationDbContext _context;

    private const string MeasurementKey = "Measurement";

    private const string CategoryKey = "Category";

    private const string SkillTypeKey = "SkillType";

    private const string DifficultyKey = "Difficulty";

    public SkillsController(ApplicationDbContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        IEnumerable<Skill> skills = this._context
            .Skills
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToArray();

        return this.View(skills);
    }

    [HttpGet]
    public IActionResult Details(Guid id) 
    {
        Skill? skill = this._context
            .Skills
            .AsNoTracking()
            .SingleOrDefault(s => s.Id == id);

        if (skill is null)
            this.NotFound();

        DetailsSkillViewModel model = new DetailsSkillViewModel()
        {
            Name = skill.Name,
            Description = skill.Description,
            Measurement = skill.MeasurementType,
            Category = skill.Category,
            SkillType = skill.SkillType,
            Difficulty = skill.Difficulty,
            SkillRecords = this._context
                .SkillProgressRecords
                .AsNoTracking()
                .Include(r => r.PerformedBy)
                .Where(r => r.SkillId == skill.Id)
                .OrderByDescending(r => r.Date)
                .ToArray()
        };

        return this.View(model);
    }   

    [HttpGet]
    public IActionResult Create()
    {
        CreateSkillViewModel model = new CreateSkillViewModel() 
        {
            MeasurementOptions = this.FetchSelectedEnum(MeasurementKey),

            CategoryOptions = this.FetchSelectedEnum(CategoryKey),

            SkillTypeOptions = this.FetchSelectedEnum(SkillTypeKey),

            DifficultyOptions = this.FetchSelectedEnum(DifficultyKey)
        };

        return this.View(model);
    }

    [HttpPost]
    public IActionResult Create(CreateSkillViewModel model) 
    {
        if (!ModelState.IsValid) 
        {
            this.FetchViewModelEnums(model);

            return View(model);
        }

        if (this.SkillExists(model.Name)) 
        {
            ModelState
                .AddModelError(nameof(model.Name), "A skill with this name already exists.");

            this.FetchViewModelEnums(model);

            return View(model);
        }

        Skill skill = new Skill()
        {
            Name = model.Name,
            Description = model.Description,
            MeasurementType = model.Measurement,
            Category = model.Category,
            SkillType = model.SkillType,
            Difficulty = model.Difficulty
        };

        this._context.Skills.Add(skill);
        this._context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(Guid id) 
    {
        Skill? skill = this._context
            .Skills
            .AsNoTracking()
            .SingleOrDefault(s => s.Id == id)!;

        if (skill is null)
            return this.NotFound();

        EditSkillViewModel model = new EditSkillViewModel()
        {
            Id = skill.Id,
            Name = skill.Name,
            Description = skill.Description,
            Measurement = skill.MeasurementType,
            Category = skill.Category,
            SkillType = skill.SkillType,
            Difficulty = skill.Difficulty
        };

        this.FetchViewModelEnums(model);                                                    

        return this.View(model);
    }

    [HttpPost]
    public IActionResult Edit(EditSkillViewModel model) 
    {
        if (!ModelState.IsValid) 
        {
            this.FetchViewModelEnums(model);

            return this.View(model);
        }

        Skill? skill = this._context
            .Skills
            .AsNoTracking()
            .SingleOrDefault(s => s.Id == model.Id);

        if (skill is null)
            return this.NotFound();

        bool skillExistsExcludingCurrent = this._context
            .Skills
            .AsNoTracking()
            .Any(s => s.Id != model.Id && s.Name.ToLower() == this.RemoveWhitespaces(model.Name).ToLower());

        if (skillExistsExcludingCurrent) 
        {
            ModelState
                .AddModelError(nameof(model.Name), "A skill with this name already exists.");

            this.FetchViewModelEnums(model);

            return View(model);
        }

        skill.Name = model.Name;
        skill.Description = model.Description;
        skill.MeasurementType = model.Measurement;
        skill.Category = model.Category;
        skill.SkillType = model.SkillType;
        skill.Difficulty = model.Difficulty;

        this._context.Skills.Update(skill);
        this._context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(Guid id) 
    {
        Skill? skill = this._context
            .Skills
            .SingleOrDefault(s => s.Id == id);
        if (skill is null)
            return this.NotFound();

        this._context.Skills.Remove(skill);

        this._context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private List<SelectListItem> FetchSelectedEnum(string key) 
    {
        Dictionary<string, List<SelectListItem>> enums = new Dictionary<string, List<SelectListItem>>
        {
            [MeasurementKey] = Enum
                .GetValues(typeof(Measurement))
                .Cast<Measurement>()
                .Select(m => new SelectListItem
                {
                    Value = ((int)m).ToString(),
                    Text = m.ToString()
                })
                .ToList(),

            [CategoryKey] = Enum
                .GetValues(typeof(Category))
                .Cast<Category>()
                .Select(c => new SelectListItem
                {
                    Value = ((int)c).ToString(),
                    Text = c.ToString()
                })
                .ToList(),

            [SkillTypeKey] = Enum
                .GetValues(typeof(SkillType))
                .Cast<SkillType>()
                .Select(sk => new SelectListItem
                {
                    Value = ((int)sk).ToString(),
                    Text = sk.ToString()
                })
                .ToList(),

            [DifficultyKey] = Enum
                .GetValues(typeof(Difficulty))
                .Cast<Difficulty>()
                .Select(d => new SelectListItem
                {
                    Value = ((int)d).ToString(),
                    Text = d.ToString()
                })
                .ToList()
        };

        return enums[key];
    }

    private void FetchViewModelEnums(ISkillViewModel model) 
    {
        model.MeasurementOptions = this.FetchSelectedEnum(MeasurementKey);
        model.CategoryOptions = this.FetchSelectedEnum(CategoryKey);
        model.SkillTypeOptions = this.FetchSelectedEnum(SkillTypeKey);
        model.DifficultyOptions = this.FetchSelectedEnum(DifficultyKey);
    }

    private bool SkillExists(string name) 
        => this._context.Skills.Any(s => s.Name.ToLower() == this.RemoveWhitespaces(name).ToLower());

    private string RemoveWhitespaces(string input)
        => string.Concat(input.Where(c => !char.IsWhiteSpace(c)));
}
