using CalisthenicsSkillTracker.Services.Core.Contracts;
using CalisthenicsSkillTracker.ViewModels.Favorites;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsSkillTracker.Controllers;

public class FavoritesController : ControllerBase
{
    private readonly IFavoritesService _service;

    public FavoritesController(IFavoritesService service)
    {
        this._service = service;
    }

    public async Task<IActionResult> Index()
    {
        if (!Guid.TryParse(this.GetUserId(), out Guid userId))
            return this.Unauthorized();

        FavoritesViewModel model = await this._service.GetUserFavoritesAsync(userId);

        return this.View(model);
    }
}
