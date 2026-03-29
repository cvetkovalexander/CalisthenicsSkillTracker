using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;

namespace CalisthenicsSkillTracker.Services.Core.Interfaces.Admin;

public interface IUserService
{
    Task<IEnumerable<ManageUserViewModel>> GetAllManageableUsersAsync(string adminUserId);

    Task<ApplicationUser?> GetUserByIdAsync(string userId);

    Task<bool> UserExistsAsync(string userId);

    Task<bool> RoleExistsAsync(string role);

    Task<bool> RoleAlreadyAssignedAsync(ApplicationUser user, string role);

    Task<bool> RoleAddedSuccessfullyAsync(ApplicationUser user, string role);

    Task<bool> RoleRemovedSuccessfullyAsync(ApplicationUser user, string role);

    Task<bool> EditUserUsernameAsync(ApplicationUser user, string newUsername);

    Task<bool> DeleteUserAsync(ApplicationUser user);

    EditUsernameViewModel CreateEditUsernameViewModel(string userId, string username);

    DeleteUserViewModel CreateDeleteUserViewModel(string userId, string username);
}
