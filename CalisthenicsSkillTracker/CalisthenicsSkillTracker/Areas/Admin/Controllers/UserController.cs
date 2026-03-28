using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Seeding;
using CalisthenicsSkillTracker.Services.Core.Interfaces.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.User;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;
using static CalisthenicsSkillTracker.GCommon.OutputMessages.IdentityMessages;

using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsSkillTracker.Areas.Admin.Controllers;

public class UserController : AdminControllerBase
{
    private readonly IUserService _userService;

    private ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        this._userService = userService;
        this._logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        string adminUserId = this.GetAdminUserId()!;

        IEnumerable<ManageUserViewModel> models = await this._userService
            .GetAllManageableUsersAsync(adminUserId);

        ViewData["AllRoles"] = IdentitySeeder._roles;

        return this.View(models);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignRole(ManageUserRoleViewModel model) 
    {
        if (!ModelState.IsValid) 
        {
            TempData[ErrorTempDataKey] = InvalidRoleAssignmentMessage;

            this._logger.LogError(InvalidRoleAssignmentMessage);

            return this.RedirectToAction(nameof(Index));
        }

        if (!await this._userService.UserExistsAsync(model.UserId)) 
        {
            TempData[ErrorTempDataKey] = UserNotFoundMessage;

            this._logger.LogError(UserNotFoundMessage);

            return this.RedirectToAction(nameof(Index));
        }

        ApplicationUser user = await this._userService.GetUserByIdAsync(model.UserId);

        if (!await this._userService.RoleExistsAsync(model.Role)) 
        {
            TempData[ErrorTempDataKey] = RoleNotFoundMessage;

            this._logger.LogError(RoleNotFoundMessage);

            return this.RedirectToAction(nameof(Index));
        }

        if (await this._userService.RoleAlreadyAssignedAsync(user, model.Role))
        {
            TempData[ErrorTempDataKey] = string.Format(RoleAlreadyAssignedMessage,model.Role);

            this._logger.LogError(string.Format(RoleAlreadyAssignedMessage, model.Role));

            return this.RedirectToAction(nameof(Index));
        }

        if (!await this._userService.RoleAddedSuccessfullyAsync(user, model.Role))
        {
            TempData[ErrorTempDataKey] = string.Format(RoleAssignmentFailedMessage, model.Role);

            this._logger.LogError(string.Format(RoleAssignmentFailedMessage, model.Role));

            return this.RedirectToAction(nameof(Index));
        }

        TempData[SuccessTempDataKey] = string.Format(RoleSuccessfullyAssignedMessage, model.Role);

        return this.RedirectToAction(nameof(Index));
    }
}
