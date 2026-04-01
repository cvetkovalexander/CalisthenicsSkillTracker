using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.ExerciseViewModels;
using static CalisthenicsSkillTracker.GCommon.OutputMessages.EntityMessages;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        PaginationResultViewModel<ListTableItemViewModel> allExercises = await this._outputService.GetAllExercisesAsync(null, null, false);

        return this.View(allExercises);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? filter, string? indexName, Guid? indexId, bool isPreviousPage = false)
    {
        PaginationResultViewModel<ListTableItemViewModel> filteredExercises = await this._outputService.GetAllExercisesAsync(indexName, indexId, isPreviousPage, filter);

        return PartialView("_ExercisePaginationTablePartial", filteredExercises);
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
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Create() 
    {
        CreateExerciseViewModel model = await this._inputService.CreateExerciseViewModelWithEnumsAsync();

        ViewData["FormAction"] = "Create";

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Create(CreateExerciseViewModel model) 
    {
        this._inputService.FetchEnums(model);
        model.AvailableExercises = await this._inputService.GetAvailableSkillsAsync();

        if (await this._inputService.ExerciseNameExistsAsync(model.Name))
            ModelState
                .AddModelError(nameof(model.Name), "A exercise with this name already exists.");

        if (model.SkillId is not null)
            if (!await this._inputService.SkillExistsAsync(Guid.Parse(model.SkillId)))
                ModelState.AddModelError(nameof(model.SkillId), "Ivalid skill id.");

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await this._inputService.CreateExerciseAsync(model);
        }
        catch (EntityCreatePersistException ecpe)
        {
            this._logger.LogError(ecpe, string.Format(EntitySaveError, nameof(Exercise)));

            TempData[ErrorTempDataKey] = string.Format(EntitySaveError, nameof(Exercise));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, UnexpectedErrorMessage);

            TempData[ErrorTempDataKey] = UnexpectedErrorMessage;

            return this.View(model);
        }

        TempData[SuccessTempDataKey] = string.Format(EntitySuccessfullyCreated, nameof(Exercise));

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!await this._outputService.ExerciseExistsAsync(id))
            return this.NotFound();

        EditExerciseViewModel model = await this._inputService.CreateEditExerciseViewModelAsync(id);

        ViewData["FormAction"] = "Edit";

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Edit(EditExerciseViewModel model)
    {
        this._inputService.FetchEnums(model);
        model.AvailableExercises = await this._inputService.GetAvailableSkillsAsync();

        if (!await this._outputService.ExerciseExistsAsync(model.Id))
            return this.NotFound();

        if (await this._inputService.ExerciseNameExcludingCurrentExistsAsync(model.Id, model.Name))
            ModelState
                .AddModelError(nameof(model.Name), "A exercise with this name already exists.");

        if (model.SkillId is not null)
            if (!await this._inputService.SkillExistsAsync(Guid.Parse(model.SkillId)))
                ModelState.AddModelError(nameof(model.SkillId), "Ivalid skill id.");

        if (!ModelState.IsValid)
            return this.View(model);

        try 
        {
            await this._inputService.EditExerciseDataAsync(model);
        }
        catch (EntityEditPersistException eepe)
        {
            this._logger.LogError(eepe, string.Format(EntityEditError, nameof(Exercise)));

            TempData[ErrorTempDataKey] = string.Format(EntityEditError, nameof(Exercise));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, UnexpectedErrorMessage);

            TempData[ErrorTempDataKey] = UnexpectedErrorMessage;

            return this.View(model);
        }

        TempData[SuccessTempDataKey] = string.Format(EntitySuccessfullyEdited, nameof(Exercise));

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await this._outputService.ExerciseExistsAsync(id))
            return this.NotFound();

        try
        {
            await this._inputService.DeleteExerciseAsync(id);
        }
        catch (EntityDeleteException ede)
        {
            this._logger.LogError(ede, string.Format(EntityDeleteError, nameof(Exercise)));

            TempData[ErrorTempDataKey] = string.Format(EntityDeleteError, nameof(Exercise));

            return this.RedirectToAction("Edit");
        }
        catch (Exception e)
        {
            this._logger.LogError(e, UnexpectedErrorMessage);

            TempData[ErrorTempDataKey] = UnexpectedErrorMessage;

            return this.RedirectToAction("Edit");
        }

        TempData[SuccessTempDataKey] = string.Format(EntitySuccessfullyDeleted, nameof(Exercise));

        return RedirectToAction(nameof(Index));
    }
}
