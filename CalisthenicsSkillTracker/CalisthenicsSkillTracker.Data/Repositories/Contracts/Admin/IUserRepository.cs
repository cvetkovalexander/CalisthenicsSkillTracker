using CalisthenicsSkillTracker.Data.Models;
using System.Linq.Expressions;

namespace CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;

public interface IUserRepository
{
    Task<IEnumerable<ApplicationUser>> GetUsersWithRolesAsync(Expression<Func<ApplicationUser, bool>>? filterQuery = null, Expression<Func<ApplicationUser, ApplicationUser>>? projectionQuery = null);
}
