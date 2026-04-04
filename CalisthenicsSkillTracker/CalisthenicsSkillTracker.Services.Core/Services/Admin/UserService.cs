using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;
using CalisthenicsSkillTracker.Services.Core.Interfaces.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.User;


using System.Data;

namespace CalisthenicsSkillTracker.Services.Core.Services.Admin;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        this._repository = repository;
    }

    public async Task<bool> EditUserUsernameAsync(ApplicationUser user, string newUsername)
    {
        user.UserName = newUsername;
        user.NormalizedUserName = newUsername.ToUpper();

        bool result = await this._repository.UpdateApplicationUserAsync(user);

        return result;
    }

    public async Task<bool> DeleteUserAsync(ApplicationUser user)
    {
        bool result = await this._repository.DeleteApplicationUserAsync(user);

        return result;
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
        => await this._repository.GetApplicationUserAsync(userId);

    public async Task<bool> RoleAddedSuccessfullyAsync(ApplicationUser user, string role)
    {
        bool result = await this._repository.AddRoleToUserAsync(user, role);

        return result;
    }

    public async Task<bool> RoleAlreadyAssignedAsync(ApplicationUser user, string role)
        => await this._repository.RoleAlreadyAssignedAsync(user, role);

    public async Task<bool> RoleExistsAsync(string role)
        => await this._repository   .RoleExistsAsync(role);

    public async Task<bool> RoleRemovedSuccessfullyAsync(ApplicationUser user, string role)
    {
        bool result = await this._repository.RemoveRoleFromUserAsync(user, role);

        return result;
    }

    public async Task<bool> UserExistsAsync(string userId)
        => await this._repository.UserExistsAsync(userId);

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
