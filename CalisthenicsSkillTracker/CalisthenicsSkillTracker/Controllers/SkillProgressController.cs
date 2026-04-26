using CalisthenicsSkillTracker.GCommon.Exceptions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;
using static CalisthenicsSkillTracker.GCommon.OutputMessages.EntityMessages;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;

using Microsoft.AspNetCore.Mvc;
using CalisthenicsSkillTracker.Data.Models;

namespace CalisthenicsSkillTracker.Controllers;

public class SkillProgressController : ControllerBase
{
    private readonly ISkillProgressService _skillProgressService;

    public SkillProgressController(ISkillProgressService skillProgress)
    {
        this._skillProgressService = skillProgress;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? userId = this.GetUserId();
        if (userId is null)
            return this.Unauthorized();

        IEnumerable<ListRecordViewModel> models = await this._skillProgressService.GetRecordsAsync(userId);

        return this.View(models);
    }

    [HttpGet]
    public IActionResult Create()
    {
        string? userId = this.GetUserId();
        if (userId is null)
            return this.Unauthorized();

        CreateSkillProgressViewModel model = this._skillProgressService.CreateSkillProgressViewModel(userId);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSkillProgressViewModel model)
    {
        this._skillProgressService.PopulateSelectListItems(model);

        if (!await this._skillProgressService.SkillExistsAsync(model.SkillId))
            ModelState.AddModelError(nameof(model.SkillId), "Invalid skill id!");

        if (!ModelState.IsValid)
            return this.View(model);

        try
        {
            await this._skillProgressService.CreateSkillProgressAsync(model);
        }
        catch (EntityCreatePersistException ecpe)
        {
            this.HandleException(ecpe, string.Format(EntitySaveError, nameof(SkillProgress)));

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            this.HandleUnexpectedException(ex);

            return this.RedirectToAction(nameof(Index));
        }

        TempData[SuccessTempDataKey] = string.Format(EntitySuccessfullyCreated, "Skill record");

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id) 
    {
        if (!await this._skillProgressService.SkillRecordExistsAsync(id))
            return this.NotFound();

        try
        {
            await this._skillProgressService.DeleteSkillRecordAsync(id);
        }
        catch (EntityDeleteException ede)
        {
            this.HandleException(ede, string.Format(EntityDeleteError, nameof(SkillProgress)));

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            this.HandleUnexpectedException(ex);

            return this.RedirectToAction(nameof(Index));
        }

        TempData[SuccessTempDataKey] = string.Format(EntitySuccessfullyDeleted, "Skill record");

        return RedirectToAction(nameof(Index));
    }
}
