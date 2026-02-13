using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsSkillTracker.Controllers;

public class ExercisesController : Controller
{
    private readonly IExerciseInputService _inputService;
    private readonly IExerciseOutputService _outputService;

    public ExercisesController(IExerciseOutputService outputService, IExerciseInputService inputService)
    {
        this._inputService = inputService;
        this._outputService = outputService;
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
}
