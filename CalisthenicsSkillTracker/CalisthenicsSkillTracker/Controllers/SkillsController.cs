using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.ViewModels;
using CalisthenicsSkillTracker.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsSkillTracker.Controllers;

public class SkillsController : Controller
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
    public async Task<IActionResult> Index(string? filter)
    {
        IEnumerable<ListTableItemViewModel> allSkills 
            = await this._outputService.GetAllSkillsAsync(filter);

        ViewData["Filter"] = filter;

        return this.View(allSkills);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id) 
    {
        if (!await this._outputService.SkillExistsAsync(id))
            return this.NotFound();

        DetailsSkillViewModel model = await this._outputService.GetSkillDetailsAsync(id);

        return this.View(model);
    }   

    [HttpGet]
    public IActionResult Create()
    {
        CreateSkillViewModel model = this._inputService.CreateSkillViewModelWithEnums();

        ViewData["FormAction"] = "Create";

        return this.View(model);
    }

    [HttpPost]
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
        catch (Exception e)
        {
            this._logger.LogError(e, "Exception occured while trying to save a skill in database");

            ModelState.AddModelError(string.Empty, "An error occurred while adding the skill. Please try again.");

            return this.View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id) 
    {
        if (!await this._outputService.SkillExistsAsync(id))
            return this.NotFound();

        EditSkillViewModel model = await this._inputService.CreateEditSkillViewModelAsync(id);

        ViewData["FormAction"] = "Edit";

        return this.View(model);
    }

    [HttpPost]
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

        await this._inputService.EditSkillDataAsync(model);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id) 
    {
        if (!await this._outputService.SkillExistsAsync(id))
            return this.NotFound();

        try
        {
            await this._inputService.DeleteSkillAsync(id);
        }
        catch (Exception e) 
        {
            this._logger.LogError(e, "Exception occured while trying to delete a skill from database");

            ModelState.AddModelError(string.Empty, "An error occurred while adding the skill. Please try again.");

            return this.RedirectToAction("Edit");
        }

        return RedirectToAction(nameof(Index));
    }
}
