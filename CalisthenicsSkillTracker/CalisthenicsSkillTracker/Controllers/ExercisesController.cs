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

public class ExercisesController : ControllerBase
{
    private readonly IExerciseInputService _inputService;
    private readonly IExerciseOutputService _outputService;

    public ExercisesController(IExerciseOutputService outputService, IExerciseInputService inputService)
    {
        this._inputService = inputService;
        this._outputService = outputService;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? sortOrder = null, string? difficultyFilter = null)
    {
        Guid? userId = Guid.TryParse(this.GetUserId(), out Guid parsedUserId) ? parsedUserId : null;

        PaginationResultViewModel<ListTableItemViewModel> allExercises = await this._outputService.GetAllExercisesAsync(null, null, false, userId, sortOrder, difficultyFilter);

        return this.View(allExercises);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Search(string? filter, string? sortOrder, string? difficultyFilter, string? indexName, Guid? indexId, bool isPreviousPage = false)
    {
        Guid? userId = Guid.TryParse(this.GetUserId(), out Guid parsedUserId) ? parsedUserId : null;

        PaginationResultViewModel<ListTableItemViewModel> filteredExercises = await this._outputService.GetAllExercisesAsync(indexName, indexId, isPreviousPage, userId, filter, sortOrder, difficultyFilter);

        return PartialView("_ExercisePaginationTablePartial", filteredExercises);
    }

    [HttpGet]
    [AllowAnonymous]
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
            this.HandleException(ecpe, string.Format(EntitySaveError, nameof(Exercise)));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this.HandleUnexpectedException(ex);

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
            this.HandleException(eepe, string.Format(EntityEditError, nameof(Exercise)));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this.HandleUnexpectedException(ex);

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
            this.HandleException(ede, string.Format(EntityDeleteError, nameof(Exercise)));

            return this.RedirectToAction("Edit");
        }
        catch (Exception ex)
        {
            this.HandleUnexpectedException(ex);

            return this.RedirectToAction("Edit");
        }

        TempData[SuccessTempDataKey] = string.Format(EntitySuccessfullyDeleted, nameof(Exercise));

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFavorite(Guid id) 
    {
        if (!await this._outputService.ExerciseExistsAsync(id))
            return this.NotFound();

        if (!Guid.TryParse(this.GetUserId(), out Guid userId))
            return this.Unauthorized();

        try
        {
            bool isFavorited = await this._inputService.ToggleFavoriteAsync(id, userId);
            return Json(new { success = true, isFavorited = isFavorited });
        }
        catch (EntityFavoritePersistException efpe)
        {
            this.HandleUnexpectedException(efpe);

            return Json(new { success = false });
        }
    }
}
