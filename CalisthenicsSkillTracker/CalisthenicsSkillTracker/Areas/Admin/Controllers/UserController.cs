using CalisthenicsSkillTracker.Data.Seeding;
using CalisthenicsSkillTracker.Services.Core.Interfaces.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.User;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsSkillTracker.Areas.Admin.Controllers;

public class UserController : AdminControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        this._userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        string adminUserId = this.GetAdminUserId()!;

        IEnumerable<ManageUserViewModel> models = await this._userService
            .GetAllManageableUsersAsync(adminUserId);

        ViewData["AllRoles"] = IdentitySeeder._roles;

        return this.View(models);
    }
}
