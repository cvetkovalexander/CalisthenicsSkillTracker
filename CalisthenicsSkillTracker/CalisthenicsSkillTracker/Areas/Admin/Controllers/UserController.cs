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
            return this.ErrorRedirectToIndex(InvalidRoleAssignmentMessage);

        ApplicationUser? user = await this._userService.GetUserByIdAsync(model.UserId);
        if (user is null) 
            return this.ErrorRedirectToIndex(UserNotFoundMessage);

        if (!await this._userService.RoleExistsAsync(model.Role)) 
            return this.ErrorRedirectToIndex(RoleNotFoundMessage);

        if (await this._userService.RoleAlreadyAssignedAsync(user, model.Role))
            return this.ErrorRedirectToIndex(string.Format(RoleAlreadyAssignedMessage, model.Role));

        if (!await this._userService.RoleAddedSuccessfullyAsync(user, model.Role))
            return this.ErrorRedirectToIndex(string.Format(RoleAssignmentFailedMessage, model.Role));

        return this.SuccessRedirectToIndex(string.Format(RoleSuccessfullyAssignedMessage, model.Role));
    }

    public async Task<IActionResult> RemoveRole(ManageUserRoleViewModel model) 
    {
        if (!ModelState.IsValid) 
            return this.ErrorRedirectToIndex(InvalidRoleRemovalMessage);

        ApplicationUser? user = await this._userService.GetUserByIdAsync(model.UserId);
        if (user is null)
            return this.ErrorRedirectToIndex(UserNotFoundMessage);

        if (!await this._userService.RoleExistsAsync(model.Role))
            return this.ErrorRedirectToIndex(RoleNotFoundMessage);

        if (!await this._userService.RoleAlreadyAssignedAsync(user, model.Role))
            return this.ErrorRedirectToIndex(string.Format(RoleNotAssignedMessage, model.Role));

        if (!await this._userService.RoleRemovedSuccessfullyAsync(user, model.Role))
            return this.ErrorRedirectToIndex(string.Format(RoleRemovalFailedMessage, model.Role));

        return this.SuccessRedirectToIndex(string.Format(RoleSuccessfullyRemovedMessage, model.Role));
    }



    private IActionResult SuccessRedirectToIndex(string message) 
    {
        TempData[SuccessTempDataKey] = message;

        return this.RedirectToAction(nameof(Index));
    }

    private IActionResult ErrorRedirectToIndex(string message) 
    {
        TempData[ErrorTempDataKey] = message;

        this._logger.LogError(message);

        return this.RedirectToAction(nameof(Index));
    }
}
