using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories.Contracts.Admin;
using CalisthenicsSkillTracker.Services.Core.Interfaces.Admin;
using CalisthenicsSkillTracker.ViewModels.Admin.User;

namespace CalisthenicsSkillTracker.Services.Core.Services.Admin;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        this._repository = repository;
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
}
