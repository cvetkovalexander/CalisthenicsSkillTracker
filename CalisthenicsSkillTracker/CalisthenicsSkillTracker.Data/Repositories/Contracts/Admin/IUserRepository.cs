using CalisthenicsSkillTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;

public interface IUserRepository
{
    Task<IEnumerable<ApplicationUser>> GetUsersWithRolesAsync(Expression<Func<ApplicationUser, bool>>? filterQuery = null, Expression<Func<ApplicationUser, ApplicationUser>>? projectionQuery = null);

    Task<bool> UpdateApplicationUserAsync(ApplicationUser user);

    Task<bool> DeleteApplicationUserAsync(ApplicationUser user);

    Task<bool> AddRoleToUserAsync(ApplicationUser user, string role);

    Task<bool> RoleAlreadyAssignedAsync(ApplicationUser user, string role);

    Task<bool> RoleExistsAsync(string role);

    Task<bool> RemoveRoleFromUserAsync(ApplicationUser user, string role);

    Task<bool> UserExistsAsync(string userId);

    Task<ApplicationUser?> GetApplicationUserAsync(string userId);

}
