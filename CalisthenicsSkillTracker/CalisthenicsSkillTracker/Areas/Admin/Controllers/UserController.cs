using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Seeding;
using CalisthenicsSkillTracker.Services.Core.Interfaces.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.User;
using static CalisthenicsSkillTracker.GCommon.ApplicationConstants;
using static CalisthenicsSkillTracker.GCommon.OutputMessages.IdentityMessages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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

    [HttpGet]
    public async Task<IActionResult> EditUsername(string userId) 
    {
        ApplicationUser? user = await this._userService.GetUserByIdAsync(userId);
        if (user is null)
            return this.ErrorRedirectToIndex(UserNotFoundMessage);

        EditUsernameViewModel model = this._userService
            .CreateEditUsernameViewModel(userId, user!.UserName!);

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUsername(EditUsernameViewModel model) 
    {
        if (!ModelState.IsValid)
            return this.ErrorRedirectToIndex(string.Format(InvalidOperationMessage, "edit username"));

        ApplicationUser? user = await this._userService.GetUserByIdAsync(model.UserId);
        if (user is null) 
            return this.ErrorRedirectToIndex(UserNotFoundMessage);

        if (!await this._userService.EditUserUsernameAsync(user, model.UserName))
            return this.ErrorRedirectToIndex(UsernameEditFailedMessage);

        return this.SuccessRedirectToIndex(UsernameEditSuccessMessage);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(DeleteUserViewModel model) 
    {
        if (!ModelState.IsValid)
            return this.ErrorRedirectToIndex(string.Format(InvalidOperationMessage, "delete user"));

        ApplicationUser? user = await this._userService.GetUserByIdAsync(model.UserId);
        if (user is null)
            return this.ErrorRedirectToIndex(UserNotFoundMessage);

        if (!await this._userService.DeleteUserAsync(user))
            return this.ErrorRedirectToIndex(UserDeleteFailedMessage);

        return this.SuccessRedirectToIndex(string.Format(UserDeleteSuccessMessage, model.UserName));
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
