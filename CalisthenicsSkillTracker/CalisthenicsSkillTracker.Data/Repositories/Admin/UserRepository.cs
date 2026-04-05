using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Data.Repositories.Admin;

public class UserRepository : BaseRepository, IUserRepository
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(ApplicationDbContext context, RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager) 
        : base(context)
    {
        this._roleManager = roleManager;
        this._userManager = userManager;
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersWithRolesAsync(Expression<Func<ApplicationUser, bool>>? filterQuery = null, Expression<Func<ApplicationUser, ApplicationUser>>? projectionQuery = null)
    {
        IQueryable<ApplicationUser> usersQuery = this.Context
            .Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .OrderBy(u => u.UserName)
            .AsNoTracking();

        if (filterQuery is not null)
            usersQuery = usersQuery.Where(filterQuery);

        if (projectionQuery is not null)
            usersQuery = usersQuery.Select(projectionQuery);

        return await usersQuery.ToArrayAsync();
    }

    public async Task<bool> UpdateApplicationUserAsync(ApplicationUser user)
    {
        IdentityResult result = await this._userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<bool> DeleteApplicationUserAsync(ApplicationUser user)
    {
        IdentityResult result = await this._userManager.DeleteAsync(user);

        return result.Succeeded;
    }

    public async Task<bool> AddRoleToUserAsync(ApplicationUser user, string role)
    {
        IdentityResult result = await this._userManager.AddToRoleAsync(user, role);

        return result.Succeeded;
    }

    public async Task<bool> RoleAlreadyAssignedAsync(ApplicationUser user, string role)
        => await this._userManager.IsInRoleAsync(user, role);

    public async Task<bool> RoleExistsAsync(string role)
        => await this._roleManager.RoleExistsAsync(role);

    public async Task<bool> RemoveRoleFromUserAsync(ApplicationUser user, string role)
    {
        IdentityResult result = await this._userManager.RemoveFromRoleAsync(user, role);

        return result.Succeeded;
    }

    public async Task<bool> UserExistsAsync(string userId)
    {
        ApplicationUser? user = await this._userManager.FindByIdAsync(userId);
        return user is not null;
    } 

    public Task<ApplicationUser?> GetApplicationUserAsync(string userId)
        => this._userManager.FindByIdAsync(userId);
}
