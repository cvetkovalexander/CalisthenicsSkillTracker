using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Data.Repositories.Admin;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(ApplicationDbContext context) 
        : base(context)
    {
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
}
