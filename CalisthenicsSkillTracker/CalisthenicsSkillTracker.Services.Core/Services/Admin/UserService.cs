using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;
using CalisthenicsSkillTracker.Services.Core.Interfaces.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.User;

using Microsoft.AspNetCore.Identity;

using System.Data;

namespace CalisthenicsSkillTracker.Services.Core.Services.Admin;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IUserRepository repository, RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager)
    {
        this._repository = repository;
        this._roleManager = roleManager;
        this._userManager = userManager;
    }

    public async Task<bool> EditUserUsernameAsync(ApplicationUser user, string newUsername)
    {
        user.UserName = newUsername;
        user.NormalizedUserName = newUsername.ToUpper();

        IdentityResult result = await this._userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(ApplicationUser user)
    {
        IdentityResult result = await this._userManager.DeleteAsync(user);

        return result.Succeeded;
    }

    public async Task<IEnumerable<ManageUserViewModel>> GetAllManageableUsersAsync(string adminUserId)
    {
        Guid adminGuidId = Guid.Parse(adminUserId);

        IEnumerable<ApplicationUser> users = await this._repository
            .GetUsersWithRolesAsync(filterQuery: u => u.Id != adminGuidId);

        IEnumerable<ManageUserViewModel> models = users
            .Select(u => new ManageUserViewModel
            {
                Id = u.Id,
                Email = u.Email!,
                UserName = u.UserName!,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()!
            })
            .ToList();

        return models;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        => await this._userManager.FindByIdAsync(userId);

    public async Task<bool> RoleAddedSuccessfullyAsync(ApplicationUser user, string role)
    {
        IdentityResult result = await this._userManager.AddToRoleAsync(user, role);

        return result.Succeeded;
    }

    public async Task<bool> RoleAlreadyAssignedAsync(ApplicationUser user, string role)
        => await this._userManager.IsInRoleAsync(user, role);

    public async Task<bool> RoleExistsAsync(string role)
        => await this._roleManager.RoleExistsAsync(role);

    public async Task<bool> RoleRemovedSuccessfullyAsync(ApplicationUser user, string role)
    {
        IdentityResult result = await this._userManager.RemoveFromRoleAsync(user, role);

        return result.Succeeded;
    }

    public async Task<bool> UserExistsAsync(string userId)
    {
        ApplicationUser? user = await this._userManager.FindByIdAsync(userId);
        return user is not null;
    }

    public EditUsernameViewModel CreateEditUsernameViewModel(string userId, string username)
        => new EditUsernameViewModel
        {
            UserId = userId,
            UserName = username
        };

    public DeleteUserViewModel CreateDeleteUserViewModel(string userId, string username)
        => new DeleteUserViewModel
        {
            UserId = userId,
            UserName = username
        };
}
