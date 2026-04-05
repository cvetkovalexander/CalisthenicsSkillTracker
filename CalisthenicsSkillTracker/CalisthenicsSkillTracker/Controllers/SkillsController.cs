using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;
using static CalisthenicsSkillTracker.GCommon.OutputMessages.EntityMessages;

namespace CalisthenicsSkillTracker.Controllers;

public class SkillsController : ControllerBase
{
    private readonly ISkillOutputService _outputService;
    private readonly ISkillInputService _inputService;
    private readonly ILogger<SkillsController> _logger;

    public SkillsController(ISkillOutputService outputService, ISkillInputService inputService, ILogger<SkillsController> logger)
    {
        this._outputService = outputService;
        this._inputService = inputService;
        this._logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        Guid? userId = Guid.TryParse(this.GetUserId(), out Guid parsedUserId) ? parsedUserId : null;

        PaginationResultViewModel<ListTableItemViewModel> model = await this._outputService
            .GetAllSkillsAsync(null, null, false, userId);

        return this.View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Search(string? filter, string? indexName, Guid? indexId, bool isPreviousPage = false)
    {
        Guid? userId = Guid.TryParse(this.GetUserId(), out Guid parsedUserId) ? parsedUserId : null;

        PaginationResultViewModel<ListTableItemViewModel> filteredModel = await this._outputService
             .GetAllSkillsAsync(indexName, indexId, isPreviousPage, userId, filter);

        return PartialView("_SkillPaginationTablePartial", filteredModel);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id) 
    {
        if (!await this._outputService.SkillExistsAsync(id))
            return this.NotFound();

        DetailsSkillViewModel model = await this._outputService.GetSkillDetailsAsync(id);

        return this.View(model);
    }   

    [HttpGet]
    [Authorize(Roles = "Admin, Moderator")]
    public IActionResult Create()
    {
        CreateSkillViewModel model = this._inputService.CreateSkillViewModelWithEnums();

        ViewData["FormAction"] = "Create";

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Create(CreateSkillViewModel model) 
    {
        this._inputService.FetchEnums(model);

        if (await this._inputService.SkillNameExistsAsync(model.Name)) 
            ModelState
                .AddModelError(nameof(model.Name), "A skill with this name already exists.");

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await this._inputService.CreateSkillAsync(model);
        }
        catch (EntityCreatePersistException ecpe)
        {
            this._logger.LogError(ecpe, string.Format(EntitySaveError, nameof(Skill)));

            TempData[ErrorTempDataKey] = string.Format(EntitySaveError, nameof(Skill));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, UnexpectedErrorMessage);

            TempData[ErrorTempDataKey] = UnexpectedErrorMessage;

            return this.View(model);
        }

        TempData[SuccessTempDataKey] = string.Format(EntitySuccessfullyCreated, nameof(Skill));

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Edit(Guid id) 
    {
        if (!await this._outputService.SkillExistsAsync(id))
            return this.NotFound();

        EditSkillViewModel model = await this._inputService.CreateEditSkillViewModelAsync(id);

        ViewData["FormAction"] = "Edit";

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Edit(EditSkillViewModel model) 
    {
        this._inputService.FetchEnums(model);

        if (!await this._outputService.SkillExistsAsync(model.Id))
            return this.NotFound();

        if (await this._inputService.SkillNameExcludingCurrentExistsAsync(model.Id, model.Name))
            ModelState
                .AddModelError(nameof(model.Name), "A skill with this name already exists.");

        if (!ModelState.IsValid)
            return this.View(model);

        try
        {
            await this._inputService.EditSkillDataAsync(model);
        }
        catch (EntityEditPersistException eepe) 
        {
            this._logger.LogError(eepe, string.Format(EntityEditError, nameof(Skill)));

            TempData[ErrorTempDataKey] = string.Format(EntityEditError, nameof(Skill));

            return this.View(model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, UnexpectedErrorMessage);

            TempData[ErrorTempDataKey] = UnexpectedErrorMessage;

            return this.View(model);
        }

        TempData[SuccessTempDataKey] = string.Format(EntitySuccessfullyEdited, nameof(Skill));

        return RedirectToAction(nameof(Index));
    }

    [HttpPost] 
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Delete(Guid id) 
    {
        if (!await this._outputService.SkillExistsAsync(id))
            return this.NotFound();

        try
        {
            await this._inputService.DeleteSkillAsync(id);
        }
        catch (EntityDeleteException ede) 
        {
            this._logger.LogError(ede, string.Format(EntityDeleteError, nameof(Skill)));

            TempData[ErrorTempDataKey] = string.Format(EntityDeleteError, nameof(Skill));

            return this.RedirectToAction("Edit");
        }
        catch (Exception e) 
        {
            this._logger.LogError(e, UnexpectedErrorMessage);

            TempData[ErrorTempDataKey] = UnexpectedErrorMessage;

            return this.RedirectToAction("Edit");
        }

        TempData[SuccessTempDataKey] = string.Format(EntitySuccessfullyDeleted, nameof(Skill));

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFavorite(Guid id)
    {
        if (!await this._outputService.SkillExistsAsync(id))
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
            this._logger.LogError(efpe, UnexpectedErrorMessage);

            TempData[ErrorTempDataKey] = UnexpectedErrorMessage;

            return Json(new { success = false });
        }
    }
}
