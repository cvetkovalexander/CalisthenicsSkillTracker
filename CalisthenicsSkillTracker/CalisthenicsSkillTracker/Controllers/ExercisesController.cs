using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CalisthenicsSkillTracker.Controllers;

public class ExercisesController : Controller
{
    private readonly IExerciseInputService _inputService;
    private readonly IExerciseOutputService _outputService;
    private readonly ILogger<ExercisesController> _logger;

    public ExercisesController(IExerciseOutputService outputService, IExerciseInputService inputService, ILogger<ExercisesController> logger)
    {
        this._inputService = inputService;
        this._outputService = outputService;
        this._logger = logger;
    }
    public async Task<IActionResult> Index(string? filter)
    {
        IEnumerable<ListTableItemViewModel> exercises = await this._outputService.GetAllExercisesAsync(filter);

        ViewData["Filter"] = filter;

        return this.View(exercises);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        if (!await this._outputService.ExerciseExistsAsync(id))
            return this.NotFound();

        DetailsExerciseViewModel model = await this._outputService.GetExerciseDetailsAsync(id);

        return this.View(model);
    }

    [HttpGet]
    public IActionResult Create() 
    {
        CreateExerciseViewModel model = this._inputService.CreateExerciseViewModelWithEnums();

        ViewData["FormAction"] = "Create";

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExerciseViewModel model) 
    {
        this._inputService.FetchEnums(model);

        if (await this._inputService.ExerciseNameExistsAsync(model.Name))
            ModelState
                .AddModelError(nameof(model.Name), "A skill with this name already exists.");

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await this._inputService.CreateExerciseAsync(model);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Exception occured while trying to save a skill in database");

            ModelState.AddModelError(string.Empty, "An error occurred while adding the skill. Please try again.");

            return this.View(model);
        }

        return RedirectToAction(nameof(Index));
    }
}
