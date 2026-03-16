using Microsoft.AspNetCore.Mvc;
using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;
using CalisthenicsSkillTracker.Services.Core.Interfaces;

namespace CalisthenicsSkillTracker.Controllers;

public class SkillProgressController : ControllerBase
{
    private readonly ISkillProgressService _skillProgressService;
    private readonly ILogger<SkillProgressController> _logger;

    public SkillProgressController(ISkillProgressService skillProgress, ILogger<SkillProgressController> logger)
    {
        this._skillProgressService = skillProgress;
        this._logger = logger;
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
            await this._skillProgressService.CreateSkillProgress(model);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Exception occured while trying to add a skill progress to database");

            ModelState.AddModelError(string.Empty, "An error occurred while logging the skill progress. Please try again.");

            return this.View(model);
        }

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
        catch (Exception e)
        {
            this._logger.LogError(e, "Exception occured while trying to delete a skill record from database");

            ModelState.AddModelError(string.Empty, "An error occurred while deleting the skill record. Please try again.");
        }

        return RedirectToAction(nameof(Index));
    }
}
