using Microsoft.AspNetCore.Mvc;
using CalisthenicsSkillTracker.ViewModels.SkillProgressViewModels;
using CalisthenicsSkillTracker.Services.Core.Interfaces;

namespace CalisthenicsSkillTracker.Controllers;

public class SkillProgressController : Controller
{
    private readonly ISkillProgressService _skillProgressService;
    private readonly ILogger<SkillProgressController> _logger;

    public SkillProgressController(ISkillProgressService skillProgress, ILogger<SkillProgressController> logger)
    {
        this._skillProgressService = skillProgress;
        this._logger = logger;
    }

    [HttpGet]
    public IActionResult Create()
    {
        CreateSkillProgressViewModel model = this._skillProgressService.CreateSkillProgressViewModel();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSkillProgressViewModel model)
    {
        this._skillProgressService.PopulateSelectListItems(model);

        if (!await this._skillProgressService.UserExistsAsync(model.UserId)) 
            ModelState.AddModelError(nameof(model.UserId), "Invalid user id!");

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

        return RedirectToAction("Index", "Home");
    }
}
