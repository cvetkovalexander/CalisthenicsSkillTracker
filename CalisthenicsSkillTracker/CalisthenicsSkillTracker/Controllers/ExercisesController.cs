using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Create() 
    {
        CreateExerciseViewModel model = await this._inputService.CreateExerciseViewModelWithEnumsAsync();

        ViewData["FormAction"] = "Create";

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExerciseViewModel model) 
    {
        this._inputService.FetchEnums(model);
        model.AvailableExercises = await this._inputService.GetAvailableSkillsAsync();

        if (await this._inputService.ExerciseNameExistsAsync(model.Name))
            ModelState
                .AddModelError(nameof(model.Name), "A exercise with this name already exists.");

        if (model.SkillId is not null)
            if (!await this._inputService.SkillExistsAsync(Guid.Parse(model.SkillId)))
                ModelState.AddModelError(nameof(model.SkillId), "Ivalid skill id");

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await this._inputService.CreateExerciseAsync(model);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Exception occured while trying to save a exercise in database");

            ModelState.AddModelError(string.Empty, "An error occurred while adding the exercise. Please try again.");

            return this.View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!await this._outputService.ExerciseExistsAsync(id))
            return this.NotFound();

        EditExerciseViewModel model = await this._inputService.CreateEditExerciseViewModelAsync(id);

        ViewData["FormAction"] = "Edit";

        return this.View(model);
    }

    public async Task<IActionResult> Edit(EditExerciseViewModel model)
    {
        this._inputService.FetchEnums(model);

        if (!await this._outputService.ExerciseExistsAsync(model.Id))
            return this.NotFound();

        if (await this._inputService.ExerciseNameExcludingCurrentExistsAsync(model.Id, model.Name))
            ModelState
                .AddModelError(nameof(model.Name), "A exercise with this name already exists.");

        if (!ModelState.IsValid)
            return this.View(model);

        await this._inputService.EditExerciseDataAsync(model);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await this._outputService.ExerciseExistsAsync(id))
            return this.NotFound();

        try
        {
            await this._inputService.DeleteExerciseAsync(id);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Exception occured while trying to delete an exercise from database");

            ModelState.AddModelError(string.Empty, "An error occurred while deleting the exercise. Please try again.");

            return this.RedirectToAction("Edit");
        }

        return RedirectToAction(nameof(Index));
    }
}
