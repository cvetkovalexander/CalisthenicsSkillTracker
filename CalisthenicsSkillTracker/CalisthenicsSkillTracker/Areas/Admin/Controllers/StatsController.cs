using CalisthenicsSkillTracker.Services.Core.Contracts.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.Stats;

using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsSkillTracker.Areas.Admin.Controllers;

public class StatsController : AdminControllerBase
{
    private readonly IStatsService _service;

    public StatsController(ILogger<StatsController> logger, IStatsService service)
        : base(logger)
    {
        this._service = service;
    }
    public async Task<IActionResult> Index()
    {
        OverviewViewModel model = await this._service.CreateOverviewViewModelAsync();

        return this.View(model);
    }
}
